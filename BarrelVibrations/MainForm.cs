#nullable enable
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BarrelVibrations.Identification;
using BarrelVibrations.ModelingObjects;
using BarrelVibrations.ModelingObjects.AmmoFolder;
using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.FiringSystemFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BarrelVibrations.ModelingObjects.MeshFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BarrelVibrations.ModelingObjects.ShotFolder;
using BarrelVibrations.Optimization;
using BarrelVibrations.Optimization.TargetCalculators;
using BarrelVibrations.PropertyGridClasses.FormProperties;
using BarrelVibrations.Solvers;
using BarrelVibrations.ViewForms;
using BarrelVibrations.ViewForms.Common;
using BasicLibraryWinForm;
using BasicLibraryWinForm.Minimization;
using BasicLibraryWinForm.ODE;
using BasicLibraryWinForm.Optimization;
using GeneticSharp;
using InletBallisticLibrary;
using MathNet.Numerics;
using ScottPlot;
using SharpGL;
using Visualization;
using Visualization.OpenGL;
using Environment = BarrelVibrations.ModelingObjects.EnvironmentFolder.Environment;
using MyPoint = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations
{
    public partial class MainForm : Form
    {
        private const string
            imageChecked = "checked.png",
            imageNeedAction = "needAction.png",
            imageRightArrow = "rightArrow.png",
            imageDocument = "document.png";

        private const string BARREL_SURFACES = "barrel", MISSILE_SURFACES = "missile", POWDER_SURFACES = "powder", SLEEVE_SURFACES = "sleeve";

        private readonly Stopwatch calculationWatch = new();
        private double lastCalculationPcnt = 0;
        private TimeSpan lastTimePredictionToEnd = new(0);

        private BackgroundWorker calculationWorker { get; set; } = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        private BackgroundWorker optimizationWorker { get; set; } = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

        private readonly VisualizationProperties VisualizationProperties = new();
        private OpenGLDrawer OpenGlDrawer { get; }

        /// <summary>
        /// Перемещения ствола при визуализации
        /// </summary>
        private double[] deltaX = new double[1], deltaY = new double[1], deltaZ = new double[1], deltaR = new double[1];

        #region Входные данные

        private IdentificationProperties IdentificationProperties = new();

        private Optimizer currentOptimizer = new BarrelNelderMead();

        public MainSolver MainSolver { get; set; } = new();

        #endregion

        public MainForm()
        {
            InitializeComponent();

            VisualizationSplitContainer.Panel1.Controls.Add(myHeatBar);

            openglControl.GotFocus += (s, a) => { VisualizationProperties.Draw = true; };

            openglControl.LostFocus += (s, a) => { VisualizationProperties.Draw = false; };

            OpenGlDrawer = new OpenGLDrawer(openglControl, myHeatBar, VisualizationProperties);
        }

        private void openglControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (VisualizationProperties.Draw)
            {
                //myHeatBar.MeasureUnits = "";
                OpenGlDrawer.Draw();
                Invalidate();
            }
        }

        private void UpdateNodesText()
        {
            UpdateMaterialNode();
            UpdateAmmoNode();
            UpdateMuzzleBreakNode();
            UpdateBarrelGeometryNode();
            UpdateMeshNode();
            UpdateEnvironmentNode();
            UpdateModelNode();
        }

        private void UpdateMaterialNode()
        {
            var @checked = MainSolver.Material.MaterialTable.MaterialDatas.Any();

            ElementsTreeView.Nodes["MaterialNode"].ImageKey =
            ElementsTreeView.Nodes["MaterialNode"].SelectedImageKey = @checked ? imageChecked : imageNeedAction;

            ElementsTreeView.Nodes["MaterialNode"].Nodes[0].Text = $@"{MainSolver.Material.Name}";
        }

        private void UpdateAmmoNode()
        {
            ElementsTreeView.Nodes["AmmoNode"].ImageKey =
            ElementsTreeView.Nodes["AmmoNode"].SelectedImageKey =
                MainSolver.Ammo.Any() && MainSolver.Ammo.All(a => a.Initialized) ? imageChecked : imageNeedAction;

            ElementsTreeView.Nodes["AmmoNode"].Nodes.Clear();

            for (var i = 0; i < MainSolver.Ammo.Count; i++)
            {
                var node = ElementsTreeView.Nodes["AmmoNode"].Nodes.Add($"{i + 1}) {MainSolver.Ammo[i].Name}");
                node.ImageKey = node.SelectedImageKey = imageDocument;
            }
        }

        private void UpdateMuzzleBreakNode()
        {
            ElementsTreeView.Nodes["MuzzleBreakNode"].ImageKey = imageChecked;
        }

        private void UpdateBarrelGeometryNode()
        {
            ElementsTreeView.Nodes["BarrelGeometryNode"].ImageKey =
            ElementsTreeView.Nodes["BarrelGeometryNode"].SelectedImageKey =
                MainSolver.Barrel.Initialized ? imageChecked : imageNeedAction;

            ElementsTreeView.Nodes["BarrelGeometryNode"].Nodes[0].Text =
                    $@"{MainSolver.Barrel.Name}";

            if (MainSolver.Barrel.BarrelSections.Any())
                ElementsTreeView.Nodes["BarrelGeometryNode"].Nodes[0].Text +=
                    $@", {MainSolver.Barrel.BarrelSections.Last().dInner} мм";
        }


        private void UpdateMeshNode()
        {
            ElementsTreeView.Nodes["MeshNode"].ImageKey =
            ElementsTreeView.Nodes["MeshNode"].SelectedImageKey =
               MainSolver.Barrel.Meshes.Any()
                ? imageChecked : imageNeedAction;

            ElementsTreeView.Nodes["MeshNode"].Nodes[0].Text = $@"Точек по Ox: {MainSolver.MeshProperties.PointsXCount}";
        }

        private void UpdateEnvironmentNode()
        {
            ElementsTreeView.Nodes["EnvironmentNode"].ImageKey =
            ElementsTreeView.Nodes["EnvironmentNode"].SelectedImageKey =
                MainSolver.Environment.MeteoTable.MeteoDatas.Any() ? imageChecked : imageNeedAction;

            ElementsTreeView.Nodes["EnvironmentNode"].Nodes[0].Text =
                $@"Давление: {MainSolver.Environment.Pressure:0.0} Па";
            ElementsTreeView.Nodes["EnvironmentNode"].Nodes[1].Text =
                $@"Температура: {MainSolver.Environment.Temperature:0.0} К";
            ElementsTreeView.Nodes["EnvironmentNode"].Nodes[2].Text =
                $@"Плотность: {MainSolver.Environment.Density:0.0} кг/м³";
            ElementsTreeView.Nodes["EnvironmentNode"].Nodes[3].Text =
                $@"Показатель адиабаты: {MainSolver.Environment.k:0.0}";
            ElementsTreeView.Nodes["EnvironmentNode"].Nodes[4].Text =
                $@"Коэф. теплопередачи: {MainSolver.Environment.HeatTransfer:0.0} Вт/(м²∙К)";
        }

        private void UpdateModelNode()
        {
            ElementsTreeView.Nodes["ModelNode"].ImageKey =
            ElementsTreeView.Nodes["ModelNode"].SelectedImageKey =
                MainSolver.Material.MaterialTable.MaterialDatas.Any() &&
                MainSolver.Ammo.Any() &&
                MainSolver.Barrel.Initialized &&
                MainSolver.Barrel.BarrelSections.Any() &&
                MainSolver.Barrel.Meshes.Any() &&
                MainSolver.Environment.MeteoTable.MeteoDatas.Any() ? imageChecked : imageNeedAction;

            ElementsTreeView.Nodes["ModelNode"].Nodes[0].Text = @$"Окончание расчета: {MainSolver.ModelProperties.EndTime * 1e3} мс";
            ElementsTreeView.Nodes["ModelNode"].Nodes[1].Text = @$"Шаг по времени: {MainSolver.ModelProperties.MainTimeStep * 1e3} мс";
            ElementsTreeView.Nodes["ModelNode"].Nodes[2].Text = @$"Выстрелов: {MainSolver.ModelProperties.Shots.Count} шт.";
            ElementsTreeView.Nodes["ModelNode"].Nodes[3].Text =
                @$"Решать задачу начального прогиба: {(MainSolver.ModelProperties.CalculateDeformations ? "Да" : "Нет")}";
            ElementsTreeView.Nodes["ModelNode"].Nodes[4].Text =
                @$"Решать задачу теплопроводности: {(MainSolver.ModelProperties.CalculateTemperatures ? "Да" : "Нет")}";
            ElementsTreeView.Nodes["ModelNode"].Nodes[5].Text =
                @$"Решать задачу колебаний: {(MainSolver.ModelProperties.CalculateVibrations ? "Да" : "Нет")}";
        }

        private void ElementsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            try
            {
                SelectNode(e.Node.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void SelectNode(string nodeName)
        {
            propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical;

            switch (nodeName)
            {
                case "MaterialNode":
                    propertyGrid.SelectedObject = MainSolver.Material;
                    break;
                case "MuzzleBreakNode":
                    propertyGrid.SelectedObject = MainSolver.MuzzleBreak;
                    break;
                case "BarrelGeometryNode":
                    propertyGrid.SelectedObject = MainSolver.Barrel;
                    break;
                case "FiringSystemNode":
                    propertyGrid.SelectedObject = MainSolver.FiringSystem;
                    break;
                case "MeshNode":
                    propertyGrid.SelectedObject = MainSolver.MeshProperties;
                    break;
                case "EnvironmentNode":
                    propertyGrid.SelectedObject = MainSolver.Environment;
                    break;
                case "ModelNode":
                    propertyGrid.SelectedObject = MainSolver.ModelProperties;
                    break;
                case "OptimizationNode":
                    propertyGrid.SelectedObject = currentOptimizer;

                    if (currentOptimizer.History.Any())
                    {
                        formsPlot.Plot.Clear();

                        var yMultiplier = 1.0;

                        var opt = currentOptimizer as IBarrelOptimizer;

                        formsPlot.Plot.YLabel(opt.OptimizationTargetCalculator.GetTargetText());

                        formsPlot.Plot.AddScatter(
                            currentOptimizer.History.Select(v => (double)v.Item1).ToArray(),
                            currentOptimizer.History.Select(v => v.Item2).Mult(yMultiplier).ToArray(),
                            markerShape: MarkerShape.filledCircle,
                            lineWidth: VisualizationProperties.PlotLineWidth);

                        formsPlot.Plot.XLabel("Итерация");

                        formsPlot.Plot.Title("Оптимизация формы");

                        formsPlot.Refresh();
                    }
                    break;
                case "IdentificationNode":
                    propertyGrid.SelectedObject = IdentificationProperties;
                    break;
                case "GasNode":
                    propertyGrid.SelectedObject = MainSolver.GetInletBallisticMainResults();
                    break;
                case "VibrationsNode":
                    propertyGrid.SelectedObject = MainSolver.GetVibrationsMainResults();
                    break;
            }

            UpdateNodesText();
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            var form = new PropertiesForm(VisualizationProperties);
            form.Closed += (s, a) =>
            {
                SetVisualizationSurfaces();

                openglControl.FrameRate = VisualizationProperties.FPS;
                OpenGlDrawer.Render();
            };
            form.Show();
        }

        private void ViewOxyMenuItem_Click(object sender, EventArgs e)
        {
            OpenGlDrawer.SetOxyView();
        }

        private void ViewOxzMenuItem_Click(object sender, EventArgs e)
        {
            OpenGlDrawer.SetOxzView();
        }

        private void ViewOyzMenuItem_Click(object sender, EventArgs e)
        {
            OpenGlDrawer.SetOyzView();
        }

        private void SetVisualizationSurfaces()
        {
            if (!MainSolver.Barrel.Initialized)
                return;

            switch (VisualizationProperties.VisualizationType)
            {
                case GeometryVisualizationType.Section:
                    {
                        if (VisualizationProperties.SelectedSection is { Index: >= 0 }
                            && VisualizationProperties.SelectedSection.Index < MainSolver.Barrel.Meshes.Length
                            && MainSolver.Barrel.Meshes.Any())
                        {
                            var surfaces = VisualizationConverterTool.GetBarrelSectionSurfaces(
                                    MainSolver.Barrel,
                                    VisualizationProperties.SelectedSection.Index,
                                    VisualizationProperties.BarrelBasicColor).ToList();

                            OpenGlDrawer.AddObjectSurfaces(BARREL_SURFACES, surfaces);

                            var source = new Dictionary<int, double>();
                            var pressure = 1.0;
                            var sectionIndex = VisualizationProperties.SelectedSection.Index;
                            var r1 = MainSolver.Barrel.InnerD[sectionIndex] / 2;

                            foreach (var surface in surfaces)
                            {
                                foreach (var coloredPoint in surface.Points)
                                {
                                    var p = coloredPoint.Point;
                                    var a = MyPoint.GetAngle0_360(p.Z, p.Y, 1, 0);
                                    var aGrad = a * 180 / Math.PI;
                                    var boardPoint = MainSolver.Barrel.GetBoardCoordinate(sectionIndex, a);

                                    var r2 = Math.Sqrt(boardPoint.Y * boardPoint.Y + boardPoint.Z * boardPoint.Z);
                                    var r = Math.Sqrt(p.Y * p.Y + p.Z * p.Z);

                                    var srr = pressure * FastMath.Pow2(r1) * (1 - FastMath.Pow2(r2 / r)) / (FastMath.Pow2(r2) - FastMath.Pow2(r1));
                                    var stt = pressure * FastMath.Pow2(r1) * (1 + FastMath.Pow2(r2 / r)) / (FastMath.Pow2(r2) - FastMath.Pow2(r1));

                                    if (source.ContainsKey(p.Id))
                                        continue;

                                    switch (VisualizationProperties.SectionStressType)
                                    {
                                        case SectionStressType.SigmaRR:
                                            source.Add(p.Id, srr);
                                            break;
                                        case SectionStressType.SigmaTT:
                                            source.Add(p.Id, stt);
                                            break;
                                        case SectionStressType.SigmaXX:
                                            source.Add(p.Id, stt + srr);
                                            break;
                                    }
                                }
                            }

                            if (VisualizationProperties.SectionStressType != SectionStressType.None)
                                OpenGlDrawer.SetColorsToSurfaces(surfaces, source, "МПа");

                            OpenGlDrawer.RemoveObjectSurfaces(MISSILE_SURFACES);
                            OpenGlDrawer.RemoveObjectSurfaces(POWDER_SURFACES);
                            OpenGlDrawer.RemoveObjectSurfaces(SLEEVE_SURFACES);
                        }
                    }
                    break;
                case GeometryVisualizationType.Sections:
                    {
                        OpenGlDrawer.AddObjectSurfaces(BARREL_SURFACES,
                            VisualizationConverterTool.GetBarrelSectionsSurfaces(
                                MainSolver.Barrel,
                                VisualizationProperties.BarrelBasicColor).SelectMany(x => x).ToList());
                    }
                    break;
                case GeometryVisualizationType.Barrel:
                    {
                        SetOpenGLObjects();
                    }
                    break;
            }

            OpenGlDrawer.Render();
        }

        #region Материал

        private void SaveMaterialButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.material)|*.material"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.Material, sfd.FileName);
        }

        private void LoadMaterialButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.material)|*.material"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadMaterial(ofd.FileName);
        }

        public void LoadMaterial(string path)
        {
            propertyGrid.SelectedObject = MainSolver.Material = FileWorker.LoadJsonNewton<Material>(path);
            MainSolver.Material.MaterialTable.UpdateFunctions();
            UpdateNodesText();
        }

        #endregion

        #region Боеприпас
        private void SaveAmmoButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.ammo)|*.ammo"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.Ammo, sfd.FileName);
        }

        private void LoadAmmoButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.ammo)|*.ammo"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadAmmo(ofd.FileName);
        }

        public void LoadAmmo(string path)
        {
            MainSolver.Ammo = AmmoJsonSerializer.Parse(File.ReadAllText(path));

            foreach (var a in MainSolver.Ammo)
            {
                if (!a.Missile.Initialized)
                    a.Missile.InitializeMissileProperties();
            }

            ShotForm.SetAmmo(MainSolver.Ammo);

            SetOpenGLObjects();
            OpenGlDrawer.Render();
        }

        private void SelectAmmoButton_Click(object sender, EventArgs e)
        {
            new AmmoForm(MainSolver.Ammo).ShowDialog();
            ShotForm.SetAmmo(MainSolver.Ammo);

            SetOpenGLObjects();
            OpenGlDrawer.Render();
        }
        #endregion

        #region Дульный тормоз

        private void SaveMuzzleBreakButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.muzzleBreak)|*.muzzleBreak"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.MuzzleBreak, sfd.FileName);
        }

        private void LoadMuzzleBreakButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.muzzleBreak)|*.muzzleBreak"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadMuzzleBreak(ofd.FileName);
        }

        public void LoadMuzzleBreak(string path)
        {
            propertyGrid.SelectedObject = MainSolver.MuzzleBreak = FileWorker.LoadJsonNewton<MuzzleBreak>(path);
            UpdateNodesText();
        }

        #endregion

        #region Геометрия ствола

        private void SaveBarrelGeometryButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.barrel)|*.barrel"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.Barrel, sfd.FileName);
        }

        private void LoadBarrelGeometryButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.barrel)|*.barrel"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            MainSolver.Barrel = FileWorker.LoadJsonNewton<Barrel>(ofd.FileName);

            propertyGrid.SelectedObject = MainSolver.Barrel;
            UpdateNodesText();
            PostProcessBarrelGeometry();
        }

        private void ViewBarrelButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainSolver.Barrel.Initialized)
                    new BarrelGeometryView(MainSolver.Barrel.Name, MainSolver.Material.Density,
                        MainSolver.Barrel, formsPlot, VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateBarrelStiffenersDiametersButton_Click(object sender, EventArgs e)
        {
            CalculateBarrelStiffenersDiameters();
        }

        public void CalculateBarrelStiffenersDiameters()
        {
            foreach (var section in MainSolver.Barrel.BarrelSections)
            {
                var d = section.dInner;
                var D = section.DOuter;

                section.StiffenersDiameter = MainSolver.Barrel.StiffenersType switch
                {
                    StiffenersType.Outer => 0.7 * D * Math.Sin(Math.PI / MainSolver.Barrel.StiffenersCount),
                    StiffenersType.Inner => D - d,
                    _ => 0
                };
            }
        }

        private void CalculateBarrelStiffenersDistanceButton_Click(object sender, EventArgs e)
        {
            CalculateBarrelStiffenersDistance();
        }

        public void CalculateBarrelStiffenersDistance()
        {
            foreach (var section in MainSolver.Barrel.BarrelSections)
            {
                var d = section.dInner;
                var D = section.DOuter;

                section.StiffenersDistance = MainSolver.Barrel.StiffenersType switch
                {
                    StiffenersType.Outer => (d + D) / 4,
                    StiffenersType.Inner => D / (2 * Math.Cos(Math.PI / MainSolver.Barrel.StiffenersCount)),
                    _ => 0
                };
            }
        }

        private void LoadBarrelRoughsButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "(*.txt)|*.txt|(*.csv)|*.csv"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var roughs = File.ReadAllLines(ofd.FileName)
                .Select(row =>
                {
                    var vals = row.Split(new char[] { '\t', ';' });
                    var x = double.Parse(vals[0]);
                    var roughY = double.Parse(vals[1]);
                    var roughZ = double.Parse(vals[2]);

                    return (new BarrelRough() { X = x, Rough = roughY }, new BarrelRough() { X = x, Rough = roughZ });
                })
                .ToList();

            MainSolver.Barrel.RoughsY = roughs.Select(v => v.Item1).ToList();
            MainSolver.Barrel.RoughsZ = roughs.Select(v => v.Item2).ToList();
            propertyGrid.SelectedObject = MainSolver.FiringSystem;
        }

        #endregion

        #region Боевая установка

        private void SaveFiringSystemButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.firingSystem)|*.firingSystem"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.FiringSystem, sfd.FileName);
        }

        private void LoadFiringSystemButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.firingSystem)|*.firingSystem"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadFiringSystem(ofd.FileName);
        }

        public void LoadFiringSystem(string path)
        {
            propertyGrid.SelectedObject = MainSolver.FiringSystem = FileWorker.LoadJsonNewton<FiringSystem>(path);
            UpdateNodesText();
        }

        private void LoadMovementsFiringSystemButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "(*.txt)|*.txt|(*.csv)|*.csv"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var data = File.ReadAllLines(ofd.FileName)
                .Select(row =>
                {
                    var vals = row.Split(new char[] { '\t', ';' });
                    var t = double.Parse(vals[0]);
                    var u = double.Parse(vals[1]);

                    return new BarrelMovement() { Time = t, Displacement = u };
                })
                .ToList();

            MainSolver.FiringSystem.BarrelMovements = data;
            propertyGrid.SelectedObject = MainSolver.FiringSystem;
        }

        #endregion

        #region Сетка

        private void SaveMeshButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.mesh)|*.mesh"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.MeshProperties, sfd.FileName);
        }

        private void LoadMeshButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.mesh)|*.mesh"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadMesh(ofd.FileName);
        }

        public void LoadMesh(string path)
        {
            propertyGrid.SelectedObject = MainSolver.MeshProperties = FileWorker.LoadJsonNewton<MeshProperties>(path);
            UpdateNodesText();
        }

        private async void CalculateGeometryButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Расчет может занять много времени, продолжить?",
                    "Продолжить?",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information)
                != DialogResult.OK)
                return;

            await CalculateGeometryAsync();
        }

        /// <summary>
        ///     Построить геометрию на основе заполненных данных
        /// </summary>
        public async Task CalculateGeometryAsync()
        {
            var observer = (int pcnt, string msg) =>
            {
                if (Math.Abs(pcnt - lastCalculationPcnt) < 1)
                    return;

                progressBar.Value = pcnt;
                statusLabel.Text = $"{msg}, до завершения расчета {CalculateTimeToEnd(pcnt, out _, out _, out _, out _)}";

                calculationWatch.Restart();
                lastCalculationPcnt = pcnt;
            };

            var invokedObserver = (int pcnt, string msg) => Invoke(() => observer(pcnt, msg));

            await Task.Run(() =>
            {
                MainSolver.Barrel.CalculateBarrelPhysics(
                    MainSolver.MeshProperties.PointsXCount,
                    MainSolver.MeshProperties.ElementSizeInSections,
                    invokedObserver);
            });

            Invoke(PostProcessBarrelGeometry);
        }

        private void PostProcessBarrelGeometry()
        {
            VisualizationProperties.SetSections(MainSolver.Barrel.X);

            SetVisualizationSurfaces();

            progressBar.Value = 100;
            statusLabel.Text = "Геометрия построена";
        }

        #endregion

        #region Параметры окружающей среды

        private void SaveEnvironmentButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.environment)|*.environment"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.Environment, sfd.FileName);
        }

        private void LoadEnvironmentButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.environment)|*.environment"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadEnvironment(ofd.FileName);
        }

        public void LoadEnvironment(string path)
        {
            propertyGrid.SelectedObject =
                MainSolver.Environment = FileWorker.LoadJsonNewton<Environment>(path);
            MainSolver.Environment.MeteoTable.UpdateFunctions();
            UpdateNodesText();
        }

        #endregion

        #region Параметры модели

        private void SaveModelButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.model)|*.model"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(MainSolver.ModelProperties, sfd.FileName);
        }

        private void LoadModelButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.model)|*.model"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadModel(ofd.FileName);
        }

        public void LoadModel(string path)
        {
            propertyGrid.SelectedObject = MainSolver.ModelProperties = FileWorker.LoadJsonNewton<ModelProperties>(path);
            UpdateNodesText();
        }

        #endregion

        #region Расчет

        private void StartCalculationButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
            {
                MessageBox.Show($"Ствол не инициализирован", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!MainSolver.Ammo.Any())
            {
                MessageBox.Show($"Боеприпасы не инициализированы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (calculationWorker.IsBusy)
            {
                MessageBox.Show($"Решатель занят", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            calculationWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };

            calculationWorker.DoWork += (o, args) =>
            {
                MainSolver.IsStop = (time, missileInBarrel) => time >= MainSolver.ModelProperties.EndTime;
                MainSolver.Initialize();
                MainSolver.Solve();
            };

            calculationWorker.ProgressChanged += CalculationWorkerProgressChanged;

            calculationWorker.RunWorkerCompleted += (o, args) =>
            {
                statusLabel.Text = "Расчет выполнен!";
                calculationWatch.Stop();
            };

            MainSolver.Worker = calculationWorker;

            calculationWatch.Restart();
            calculationWorker.RunWorkerAsync();
        }

        private void CalculationWorkerProgressChanged(object? sender, ProgressChangedEventArgs args)
        {
            Invoke(() =>
            {
                progressBar.Value = Math.Min(args.ProgressPercentage, 100);

                if (args.UserState is SolverStatus status)
                {
                    statusLabel.Text =
                        $"t={status.Time * 1e3:0.000} мс, {status.Percentage:0.000}%, до завершения расчета " +
                        $"{CalculateTimeToEnd(status.Percentage, out _, out _, out _, out _)}";

                    lastCalculationPcnt = status.Percentage;
                    calculationWatch.Restart();
                }
            });
        }

        private void StopCalculationButton_Click(object sender, EventArgs e)
        {
            calculationWatch.Stop();
            calculationWorker.CancelAsync();
        }

        private string CalculateTimeToEnd(double pcnt, out int days, out int hours, out int minutes, out int seconds)
        {
            var deltaTime = new TimeSpan(calculationWatch.ElapsedTicks);
            var deltaPcnts = pcnt - lastCalculationPcnt;

            var timeRemain = deltaPcnts == 0
                ? new TimeSpan(0)
                : deltaTime * Math.Max(0, (100.0 - pcnt) / deltaPcnts);

            timeRemain = (timeRemain + lastTimePredictionToEnd) / 2;

            var timeRemainStringBuilder = new StringBuilder();

            if (timeRemain.Days != 0)
                timeRemainStringBuilder.Append($"{timeRemain.Days} дн. ");

            if (timeRemain.Hours != 0)
                timeRemainStringBuilder.Append($"{timeRemain.Hours} ч. ");

            if (timeRemain.Minutes != 0)
                timeRemainStringBuilder.Append($"{timeRemain.Minutes} мин. ");

            if (timeRemain.Seconds != 0)
                timeRemainStringBuilder.Append($"{timeRemain.Seconds} сек.");

            days = timeRemain.Days;
            hours = timeRemain.Hours;
            minutes = timeRemain.Minutes;
            seconds = timeRemain.Seconds;

            lastTimePredictionToEnd = timeRemain;

            return timeRemainStringBuilder.ToString();
        }

        #endregion

        #region Оптимизация

        private void SaveOptimizationButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.opt)|*.opt"
            };

            if (sfd.ShowDialog() != DialogResult.OK || currentOptimizer == null)
                return;

            FileWorker.SaveJsonNewton(currentOptimizer, sfd.FileName);
        }

        private void LoadOptimizationButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.opt)|*.opt"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            currentOptimizer = OptimizerJSONConverter.ParseJson(File.ReadAllText(ofd.FileName));

            propertyGrid.SelectedObject = currentOptimizer;
            UpdateNodesText();
        }

        private (bool, double) CallOptimization(double[] x)
        {
            calculationWatch.Restart();
            var n = MainSolver.Barrel.BarrelSections.Length;

            for (var i = 0; i < MainSolver.Barrel.BarrelSections.Length; i++)
            {
                MainSolver.Barrel.BarrelSections[i].DOuter = x[i] * 1000;

                if (MainSolver.Barrel.StiffenersType == StiffenersType.None)
                    continue;

                MainSolver.Barrel.BarrelSections[i].StiffenersDiameter = x[n + i] * 1000;
                MainSolver.Barrel.BarrelSections[i].StiffenersDistance = x[2 * n + i] * 1000;
            }

            CalculateGeometryAsync().Wait();

            var radiiDiffFunc = Algebra.GetFunc(MainSolver.Barrel.X, MainSolver.Barrel.RadiiDifference);
            var outerDFunc = Algebra.GetFunc(MainSolver.Barrel.X, MainSolver.Barrel.OuterD);

            var constraint = currentOptimizer.Constraint as BarrelOptimizationConstraint;
            var barrelX = MainSolver.Barrel.BarrelSections.Select(v => v.X * 1e-3).ToArray();
            var thickness = barrelX.Select(x => radiiDiffFunc(x)).ToArray();
            var outerD = barrelX.Select(x => outerDFunc(x)).ToArray();
            var m = MainSolver.Barrel.GetMass(MainSolver.Material.Density);

            var isValid = constraint?.IsValid(barrelX, thickness, outerD, m) ?? true;

            MainSolver.Worker = calculationWorker;
            MainSolver.Initialize();
            MainSolver.Solve();

            var opt = currentOptimizer as IBarrelOptimizer;

            var optF = opt.OptimizationTargetCalculator.Calculate(MainSolver);

            return (isValid, optF);
        }

        private void SetOptimizationWorker(Optimizer optimizer, bool resume = false)
        {
            optimizationWorker.DoWork += (o, args) =>
            {
                optimizer.Optimize(resume);
            };

            optimizationWorker.ProgressChanged += (o, args) => OptimizationProgressChanged(optimizer, args);

            optimizationWorker.RunWorkerCompleted += (o, args) =>
            {
                calculationWatch.Stop();
                statusLabel.Text = "Оптимизация выполнена!";

                var xBest = optimizer.BestSolution.Item1;
                var n = MainSolver.Barrel.BarrelSections.Length;

                if (MainSolver.Barrel.StiffenersType == StiffenersType.None)
                {
                    for (var i = 0; i < MainSolver.Barrel.BarrelSections.Length; i++)
                    {
                        MainSolver.Barrel.BarrelSections[i].DOuter = xBest[i] * 1e3;
                    }
                }
                else
                {
                    for (var i = 0; i < MainSolver.Barrel.BarrelSections.Length; i++)
                    {
                        MainSolver.Barrel.BarrelSections[i].DOuter = xBest[i] * 1e3;
                        MainSolver.Barrel.BarrelSections[i].StiffenersDiameter = xBest[n + i] * 1e3;
                        MainSolver.Barrel.BarrelSections[i].StiffenersDistance = xBest[2 * n + i] * 1e3;
                    }
                }
            };
        }

        private void OptimizationProgressChanged(Optimizer optimizer, ProgressChangedEventArgs args)
        {
            Invoke(() =>
            {
                progressBar.Value = Math.Min(args.ProgressPercentage, 100);

                if (args.UserState is SolverStatus status)
                {
                    var opt = optimizer as IBarrelOptimizer;
                    var funcStr = opt.OptimizationTargetCalculator.GetTargetText();
                    var funcValue = optimizer.BestSolution.Item2;
                    var sBuilder = new StringBuilder();

                    sBuilder.Append($"Итерация: {optimizer.CurrentIteration}, ");

                    if (funcValue < optimizer.Penalty)
                        sBuilder.Append($"{funcStr} = {funcValue:0.000}, ");

                    sBuilder.Append($"t={status.Time * 1e3:0.000} мс, {status.Percentage:0.000}%, до завершения итерации ");
                    sBuilder.Append($"{CalculateTimeToEnd(status.Percentage, out _, out _, out _, out _)}");

                    if (optimizationWorker.CancellationPending)
                        sBuilder.Append($" Расчет будет остановлен...");

                    statusLabel.Text = sBuilder.ToString();

                    lastCalculationPcnt = status.Percentage;
                    calculationWatch.Restart();
                }
            });
        }

        private void StartOptimization(bool resume)
        {
            if (!MainSolver.Barrel.Initialized || !MainSolver.Ammo.Any())
            {
                MessageBox.Show("Объекты моделирования не инециализированы!", "Не удается начать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentOptimizer is not IBarrelOptimizer opt)
            {
                MessageBox.Show("Неверный алгоритм оптимизации", "Не удается начать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            optimizationWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            calculationWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };

            calculationWorker.ProgressChanged += (o, args) => OptimizationProgressChanged(currentOptimizer, args);

            SetOptimizationWorker(currentOptimizer, resume);

            var constraint = currentOptimizer.Constraint as BarrelOptimizationConstraint;

            var normalizer = new BarrelNormalizer(
                MainSolver.Barrel.BarrelSections.Select(s => s.dInner).ToArray(),
                MainSolver.Barrel.StiffenersType,
                Algebra.InterpolateArray(
                    constraint.BarrelX,
                    constraint.ThicknessMins,
                    MainSolver.Barrel.BarrelSections.Select(s => s.X * 1e-3).ToArray()));

            //currentOptimizer.Normalizer = normalizer;
            currentOptimizer.OptimizationFunction = CallOptimization;
            currentOptimizer.Worker = optimizationWorker;

            if (MainSolver.Barrel.StiffenersType == StiffenersType.None)
                currentOptimizer.x0 = MainSolver.Barrel.BarrelSections.Select(s => s.DOuter / 1000).ToArray();
            else
            {
                var x0 = new List<double>();

                x0.AddRange(MainSolver.Barrel.BarrelSections.Select(s => s.DOuter / 1000));
                x0.AddRange(MainSolver.Barrel.BarrelSections.Select(s => s.StiffenersDiameter / 1000));
                x0.AddRange(MainSolver.Barrel.BarrelSections.Select(s => s.StiffenersDistance / 1000));

                currentOptimizer.x0 = x0.ToArray();
            }

            optimizationWorker.RunWorkerAsync();
        }

        private void StartOptimizationButton_Click(object sender, EventArgs e)
        {
            StartOptimization(false);
        }

        private void ResumeOptimizationButton_Click(object sender, EventArgs e)
        {
            StartOptimization(true);
        }

        private void StopOptimizationButton_Click(object sender, EventArgs e)
        {
            calculationWatch.Stop();
            optimizationWorker.CancelAsync();
        }

        private void CopyOptimizerData(Optimizer from, Optimizer to)
        {
            to.Constraint = from.Constraint;
            to.MaxIterations = from.MaxIterations;
            to.Penalty = from.Penalty;
            to.SaveResultsInFile = from.SaveResultsInFile;
            to.StableIterationsToStop = from.StableIterationsToStop;
            to.Tolerance = from.Tolerance;
        }

        private void NelderMeadButton_Click(object sender, EventArgs e)
        {
            var newOpt = new BarrelNelderMead();

            CopyOptimizerData(currentOptimizer, newOpt);
            currentOptimizer = newOpt;
            propertyGrid.SelectedObject = currentOptimizer;
        }

        private void HookeJeevesButton_Click(object sender, EventArgs e)
        {
            var newOpt = new BarrelHookeJeeves();

            CopyOptimizerData(currentOptimizer, newOpt);
            currentOptimizer = newOpt;
            propertyGrid.SelectedObject = currentOptimizer;
        }

        private void RandomDescendButton_Click(object sender, EventArgs e)
        {
            var newOpt = new BarrelRandomDescend();

            CopyOptimizerData(currentOptimizer, newOpt);
            currentOptimizer = newOpt;
            propertyGrid.SelectedObject = currentOptimizer;
        }

        #endregion

        #region Идентификация скорости горения

        private void IdentifyBurnSpeedButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;

            calculationWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };

            SolverIdentifier.TargetCalculator target = IdentificationProperties.IdentificationTarget switch
            {
                IdentificationTargetEnum.MaxPressure => new SolverIdentifier.PressureTarget(),
                IdentificationTargetEnum.ProjectileVelocity => new SolverIdentifier.ProjectileVelocityTarget(),
                _ => throw new ArgumentException()
            };

            SolverIdentifier.ParametricSolverCloner cloner = IdentificationProperties.IdentificationParameter switch
            {
                IdentificationParameterEnum.BurnSpeed => new SolverIdentifier.BurnSpeedCloner(),
                IdentificationParameterEnum.PowderPower => new SolverIdentifier.PowderPowerCloner(),
                _ => throw new ArgumentException()
            };

            var identifier = new SolverIdentifier(
                MainSolver,
                target,
                cloner,
                calculationWorker);

            var resultParameter = 0.0;

            calculationWorker.DoWork += (o, args) =>
            {
                resultParameter = identifier.Identify(
                    out var mainSolver,
                    IdentificationProperties.ExpectedTarget,
                    IdentificationProperties.ParameterMin,
                    IdentificationProperties.ParameterMax,
                    IdentificationProperties.Tolerance);

                MainSolver = mainSolver;
            };

            calculationWorker.ProgressChanged += (o, args) =>
            {
                progressBar.Value = Math.Min(args.ProgressPercentage, 100);

                if (args.UserState is not string text) return;

                statusLabel.Text = text;
            };

            calculationWorker.RunWorkerCompleted += (o, args) =>
            {
                MainSolver.Worker = calculationWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };

                IdentificationProperties.ResultParameter = resultParameter;
                IdentificationProperties.ResultTarget = target.GеtTargerValue(MainSolver);

                statusLabel.Text = $"Идентификация выполнена!";
            };

            statusLabel.Text = $"Идентификация начата!";
            calculationWorker.RunWorkerAsync();
        }

        private void StopIdentificationBurnSpeedButton_Click(object sender, EventArgs e)
        {
            calculationWorker.CancelAsync();
        }

        private void SaveIdentificationBurnSpeedButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.ident)|*.ident"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            FileWorker.SaveJsonNewton(IdentificationProperties, sfd.FileName);
        }

        private void LoadIdentificationBurnSpeedButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.ident)|*.ident"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            IdentificationProperties = FileWorker.LoadJsonNewton<IdentificationProperties>(ofd.FileName);

            UpdateNodesText();
        }

        #endregion

        #region Результаты



        private void SetColorsToBarrel(TrackData trackData)
        {
            var deltaXPath = Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsXFile + ".txt");
            var deltaYPath = Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsYFile + ".txt");
            var deltaZPath = Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsZFile + ".txt");
            var deltaRPath = Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsRInner + ".txt");

            if (File.Exists(deltaXPath))
                deltaX = FileWorker.ReadFileRow(deltaXPath, trackData.TimeIndex).Mult(VisualizationProperties.DisplacementScale).ToArray();

            if (File.Exists(deltaYPath))
                deltaY = FileWorker.ReadFileRow(deltaYPath, trackData.TimeIndex).Mult(VisualizationProperties.DisplacementScale).ToArray();

            if (File.Exists(deltaZPath))
                deltaZ = FileWorker.ReadFileRow(deltaZPath, trackData.TimeIndex).Mult(VisualizationProperties.DisplacementScale).ToArray();

            if (File.Exists(deltaRPath))
                deltaR = FileWorker.ReadFileRow(deltaRPath, trackData.TimeIndex).Mult(VisualizationProperties.DisplacementScale).ToArray();

            var xsn = 0.0;
            var powderColorAlpha = 0.0;

            if (MainSolver.InletBallistic.Xsn.Count > trackData.TimeIndex)
            {
                xsn = MainSolver.InletBallistic.Xsn[trackData.TimeIndex];

                powderColorAlpha = MainSolver.InletBallistic.Psi[trackData.TimeIndex].Average();
            }

            SetOpenGLObjects(xsn, powderColorAlpha, valueToColor: trackData.ValuesDestribution, measureUnit: trackData.MeasureUnit);
        }

        private void SetOpenGLObjects(
            double? xsn = null,
            double? powderColorAlpha = null,
            double[]? valueToColor = null,
            string? measureUnit = null,
            bool barrel = true,
            bool projectile = true,
            bool powder = true,
            bool sleeve = true)
        {
            xsn = xsn ?? MainSolver.Barrel.CamoraLength;
            powderColorAlpha = powderColorAlpha ?? 0;
            /*
            OpenGlDrawer.RemoveObjectSurfaces(MISSILE_SURFACES);
            OpenGlDrawer.RemoveObjectSurfaces(POWDER_SURFACES);
            OpenGlDrawer.RemoveObjectSurfaces(SLEEVE_SURFACES);*/

            if (barrel)
                OpenGlDrawer.AddObjectSurfaces(
                    BARREL_SURFACES,
                    VisualizationConverterTool.GetBarrelSurfaces(
                        MainSolver.Barrel,
                        valueToColor,
                        deltaX,
                        deltaY,
                        deltaZ,
                        deltaR,
                        VisualizationProperties.AngleSegments,
                        VisualizationProperties.SliceDraw ? Algebra.ConvertGradToRad(VisualizationProperties.SliceMinAngle) : 0,
                        VisualizationProperties.SliceDraw ? Algebra.ConvertGradToRad(VisualizationProperties.SliceMaxAngle) : 2 * Math.PI,
                        VisualizationProperties.UseHeatBarColor ? null : VisualizationProperties.BarrelBasicColor));

            if (xsn < MainSolver.Barrel.X.LastOrDefault())
            {
                var (xsnIndex, _) = Algebra.BinarySearch(MainSolver.Barrel.X, xsn.Value);

                if (projectile)
                    OpenGlDrawer.AddObjectSurfaces(MISSILE_SURFACES,
                        VisualizationConverterTool.GetMissileSurfaces(
                            MainSolver.Barrel,
                            MainSolver.CurrentAmmo.Missile,
                            xsn.Value,
                            deltaX,
                            deltaY,
                            deltaZ,
                            color: VisualizationProperties.MissileBasicColor,
                            angleSegments: VisualizationProperties.AngleSegments));

                if (powder)
                    OpenGlDrawer.AddObjectSurfaces(POWDER_SURFACES,
                        VisualizationConverterTool.GetPowderSurfaces(
                            xsn.Value,
                            MainSolver.Barrel,
                            powderColorAlpha.Value,
                            MainSolver.GetSleeveThickness(),
                            deltaX,
                            deltaY,
                            deltaZ,
                            deltaR,
                            VisualizationProperties.PowderBasicColor,
                            angleSegments: VisualizationProperties.AngleSegments));

                if (sleeve)
                    OpenGlDrawer.AddObjectSurfaces(SLEEVE_SURFACES,
                        VisualizationConverterTool.GetSleeveSurfaces(
                            MainSolver.Barrel,
                            MainSolver.GetSleeveThickness(),
                            deltaX,
                            deltaY,
                            deltaZ,
                            deltaR,
                            VisualizationProperties.SleeveBasicColor,
                            angleSegments: VisualizationProperties.AngleSegments));
            }
            else
            {
                OpenGlDrawer.RemoveObjectSurfaces(MISSILE_SURFACES);
            }


            Invoke(() => myHeatBar.SetLimits(valueToColor?.Min() ?? 0, valueToColor?.Max() ?? 1));
            myHeatBar.MeasureUnits = measureUnit ?? "";

            Invoke(OpenGlDrawer.Render);
            Invoke(OpenGlDrawer.Draw);
            Invoke(openglControl.Refresh);
        }


        private void ViewBarrelStrengthButton_Click(object sender, EventArgs e)
        {
            try
            {
                new BarrelStrengthForm(
                MainSolver.Barrel,
                MainSolver.GasEpures.Pressures,
                MainSolver.InletBallistic.Psn.ToArray(),
                MainSolver.InletBallistic.Xsn.ToArray()).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewInletBallisticButton_Click(object sender, EventArgs e)
        {
            try
            {
                new InletBallisticView($"{MainSolver.Barrel.Name}",
                MainSolver.InletBallistic,
                formsPlot,
                VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewGasDistributionButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;
            try
            {
                new GasDistributionsView($"{MainSolver.Barrel.Name}",
                MainSolver.TimeMoments.ToArray(),
                MainSolver.Barrel.X,
                formsPlot,
                VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewGasEpuresButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;
            try
            {
                new GasEpuresView($"{MainSolver.Barrel.Name}",
                    MainSolver.Barrel.X,
                    MainSolver.GasEpures,
                    formsPlot,
                    VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewTemperatureButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;
            try
            {
                new TemperatureView($"{MainSolver.Barrel.Name}",
                    MainSolver.TimeMoments.ToArray(),
                    MainSolver.TemperatureField,
                    formsPlot,
                    VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewTemperatureDistributionsButtonButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;

            try
            {
                new TemperatureDistributionsView($"{MainSolver.Barrel.Name}",
                    MainSolver.TimeMoments.ToArray(),
                    MainSolver.Barrel.X,
                    formsPlot,
                    VisualizationProperties)
                { TrackingAction = SetColorsToBarrel }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewDeflectionButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;

            try
            {
                new DeflectionView($"{MainSolver.Barrel.Name}",
                    MainSolver.Barrel.X,
                    MainSolver.Deflection,
                    formsPlot,
                    VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ViewVibrationsButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;

            var shotsTimes = new List<double>();

            for (int i = 1; i < MainSolver.InletBallistic.Xsn.Count; i++)
            {
                if (MainSolver.InletBallistic.Xsn[i - 1] < MainSolver.Barrel.X.Last() &&
                    MainSolver.InletBallistic.Xsn[i] >= MainSolver.Barrel.X.Last())
                {
                    shotsTimes.Add(MainSolver.InletBallistic.TimeMoments[i]);
                }
            }

            try
            {
                new VibrationsView($"{MainSolver.Barrel.Name}",
                    MainSolver.Barrel.X,
                    MainSolver.TimeMoments.ToArray(),
                    MainSolver.Vibrations,
                    shotsTimes.ToArray(),
                    formsPlot,
                    VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ViewVibrationsDistributionsButton_Click(object sender, EventArgs e)
        {
            if (!MainSolver.Barrel.Initialized)
                return;

            try
            {
                new VibrationsDistributionsView($"{MainSolver.Barrel.Name}",
                    MainSolver.TimeMoments.ToArray(),
                    MainSolver.Barrel.X,
                    formsPlot,
                    VisualizationProperties)
                { TrackingAction = SetColorsToBarrel }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewOutletBallisticButton_Click(object sender, EventArgs e)
        {
            try
            {
                new OutletBallisticView($"{MainSolver.Barrel.Name}",
                MainSolver.OutletBallistic,
                Algebra.ConvertGradToRad(MainSolver.ModelProperties.FiringAngle),
                formsPlot,
                VisualizationProperties).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewShotsParametersButton_Click(object sender, EventArgs e)
        {
            if (MainSolver == null)
                return;

            try
            {
                new ShotsParametersView(MainSolver.ShotsParameters).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DrawPlot(
            IEnumerable<double> xs,
            IEnumerable<double> ys,
            string? xLabel = null,
            string? yLabel = null,
            string? title = null,
            bool clearPrevious = true)
        {
            if (clearPrevious)
                formsPlot.Plot.Clear();

            formsPlot.Plot.AddScatter(
                xs.ToArray(),
                ys.ToArray(),
                markerShape: VisualizationProperties.PlotMarkers,
                lineWidth: VisualizationProperties.PlotLineWidth);

            formsPlot.Plot.XLabel(xLabel ?? "");
            formsPlot.Plot.YLabel(yLabel ?? "");
            formsPlot.Plot.Title(title ?? "");

            formsPlot.Render();
        }

        private void DrawShotsPlot(IEnumerable<double> shotTimes)
        {
            if (!VisualizationProperties.PlotShots)
                return;

            var shotsArr = shotTimes.ToArray();

            for (int i = 0; i < shotsArr.Length; i++)
            {
                formsPlot.Plot.AddVerticalLine(shotsArr[i] * 1000, label: $"выстрел {i + 1}");
            }

            formsPlot.Plot.Legend();
            formsPlot.Render();
        }

        #endregion

        #region Параметры проекта

        private async void SaveProjectMenuItem_Click(object sender, EventArgs e)
        {
            if (MainSolver == null)
            {
                MessageBox.Show("Решатель не инициализирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.project)|*.project",
                Title = "Сохранение проекта"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            var project = new ProjectDataJson(
                MainSolver,
                currentOptimizer
            );

            statusLabel.Text = "Сохранение проекта...";
            await Task.Run(() => FileWorker.SaveJsonNewton(project, sfd.FileName));
            statusLabel.Text = "Проект сохранен!";
        }

        private async void LoadProjectMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "(*.project)|*.project",
                Title = "Загрузка проекта"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            progressBar.Value = 0;
            statusLabel.Text = "Загружаем проект...";

            await Task.Run(() =>
            {
                var projectDataJson = ProjectDataJson.LoadJson(ofd.FileName);

                MainSolver = projectDataJson.MainSolver;
                MainSolver.Material.MaterialTable.UpdateFunctions();

                foreach (var a in MainSolver.Ammo)
                {
                    if (!a.Missile.Initialized)
                        a.Missile.InitializeMissileProperties();
                }
                ShotForm.SetAmmo(MainSolver.Ammo);

                MainSolver.Environment.MeteoTable.UpdateFunctions();
                currentOptimizer = projectDataJson.Optimizer;

                for (int i = 0; i < MainSolver.Barrel.X.Length; i++)
                {
                    foreach (var element in MainSolver.Barrel.Meshes[i].Elements)
                    {
                        foreach (var pId in element)
                        {
                            MainSolver.Barrel.Meshes[i].Nodes[pId].SetId(pId);
                        }
                    }
                }

                calculationWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };

                calculationWorker.DoWork += (o, args) =>
                {
                    MainSolver.Initialize();
                    MainSolver.Solve();
                };

                MainSolver.Worker = calculationWorker;

                SetOpenGLObjects();
            });

            UpdateNodesText();
            PostProcessBarrelGeometry();
            OpenGlDrawer.Render();

            statusLabel.Text = "Проект загружен";
            progressBar.Value = 100;
        }

        #endregion

        private void AboutProgramButton_Click(object sender, EventArgs e)
        {
            new AboutProgramForm().Show();
        }

        private void GuideButton_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Моделирование напряженно-деформированного состояния и колебаний ствола.chm");
        }
    }
}