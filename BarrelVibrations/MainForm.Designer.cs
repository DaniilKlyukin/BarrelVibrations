using CustomControls;

namespace BarrelVibrations
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            TreeNode treeNode1 = new TreeNode("Материал не выбран");
            TreeNode treeNode2 = new TreeNode("Материал", new TreeNode[] { treeNode1 });
            TreeNode treeNode3 = new TreeNode("Боеприпасы");
            TreeNode treeNode4 = new TreeNode("Дульный тормоз");
            TreeNode treeNode5 = new TreeNode("Ствол не задан");
            TreeNode treeNode6 = new TreeNode("Геометрия ствола", new TreeNode[] { treeNode5 });
            TreeNode treeNode7 = new TreeNode("Сетка по пространству не задана");
            TreeNode treeNode8 = new TreeNode("Расчётная сетка", new TreeNode[] { treeNode7 });
            TreeNode treeNode9 = new TreeNode("Боевая установка");
            TreeNode treeNode10 = new TreeNode("Давление не задано");
            TreeNode treeNode11 = new TreeNode("Температура не задана");
            TreeNode treeNode12 = new TreeNode("Плотность не задана");
            TreeNode treeNode13 = new TreeNode("Показатель адиабаты не задан");
            TreeNode treeNode14 = new TreeNode("Коэф. теплопередачи не задан");
            TreeNode treeNode15 = new TreeNode("Окружение", new TreeNode[] { treeNode10, treeNode11, treeNode12, treeNode13, treeNode14 });
            TreeNode treeNode16 = new TreeNode("Узел");
            TreeNode treeNode17 = new TreeNode("Узел");
            TreeNode treeNode18 = new TreeNode("Узел");
            TreeNode treeNode19 = new TreeNode("Узел");
            TreeNode treeNode20 = new TreeNode("Узел");
            TreeNode treeNode21 = new TreeNode("Узел");
            TreeNode treeNode22 = new TreeNode("Модель", new TreeNode[] { treeNode16, treeNode17, treeNode18, treeNode19, treeNode20, treeNode21 });
            TreeNode treeNode23 = new TreeNode("Расчёт");
            TreeNode treeNode24 = new TreeNode("Оптимизация формы");
            TreeNode treeNode25 = new TreeNode("Идентификация");
            TreeNode treeNode26 = new TreeNode("Дополнительные возможности", new TreeNode[] { treeNode24, treeNode25 });
            TreeNode treeNode27 = new TreeNode("Прочность ствола");
            TreeNode treeNode28 = new TreeNode("Газопороховые параметры");
            TreeNode treeNode29 = new TreeNode("Тепловое нагружение");
            TreeNode treeNode30 = new TreeNode("Начальный прогиб");
            TreeNode treeNode31 = new TreeNode("Колебания");
            TreeNode treeNode32 = new TreeNode("Внешняя баллистика");
            TreeNode treeNode33 = new TreeNode("Дульные параметры");
            TreeNode treeNode34 = new TreeNode("Результаты", new TreeNode[] { treeNode27, treeNode28, treeNode29, treeNode30, treeNode31, treeNode32, treeNode33 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MaterialContextMenu = new ContextMenuStrip(components);
            SaveMaterialButton = new ToolStripMenuItem();
            LoadMaterialButton = new ToolStripMenuItem();
            AmmoContextMenu = new ContextMenuStrip(components);
            SaveAmmoButton = new ToolStripMenuItem();
            LoadAmmoButton = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            SelectAmmoButton = new ToolStripMenuItem();
            MuzzleBreakContextMenu = new ContextMenuStrip(components);
            SaveMuzzleBreakButton = new ToolStripMenuItem();
            LoadMuzzleBreakButton = new ToolStripMenuItem();
            BarrelGeometryContextMenu = new ContextMenuStrip(components);
            SaveBarrelGeometryButton = new ToolStripMenuItem();
            LoadBarrelGeometryButton = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            ViewBarrelButton = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            CalculateBarrelStiffenersDiametersButton = new ToolStripMenuItem();
            CalculateBarrelStiffenersDistanceButton = new ToolStripMenuItem();
            MeshContextMenu = new ContextMenuStrip(components);
            SaveMeshButton = new ToolStripMenuItem();
            LoadMeshButton = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            CalculateGeometryButton = new ToolStripMenuItem();
            FiringSystemContextMenu = new ContextMenuStrip(components);
            SaveFiringSystemButton = new ToolStripMenuItem();
            LoadFiringSystemButton = new ToolStripMenuItem();
            LoadMovementsFiringSystemButton = new ToolStripMenuItem();
            EnvironmentContextMenu = new ContextMenuStrip(components);
            SaveEnvironmentButton = new ToolStripMenuItem();
            LoadEnvironmentButton = new ToolStripMenuItem();
            ModelContextMenu = new ContextMenuStrip(components);
            SaveModelButton = new ToolStripMenuItem();
            LoadModelButton = new ToolStripMenuItem();
            CalculateContextMenu = new ContextMenuStrip(components);
            StartCalculationButton = new ToolStripMenuItem();
            StopCalculationButton = new ToolStripMenuItem();
            OptimizationContextMenu = new ContextMenuStrip(components);
            StartOptimizationButton = new ToolStripMenuItem();
            ResumeOptimizationButton = new ToolStripMenuItem();
            StopOptimizationButton = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            SaveOptimizationButton = new ToolStripMenuItem();
            LoadOptimizationButton = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            SelectOptimizationAlgorithm = new ToolStripMenuItem();
            NelderMeadButton = new ToolStripMenuItem();
            HookeJeevesButton = new ToolStripMenuItem();
            RandomDescendButton = new ToolStripMenuItem();
            IdentificationContextMenu = new ContextMenuStrip(components);
            IdentifyBurnSpeedButton = new ToolStripMenuItem();
            StopIdentificationBurnSpeedButton = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            SaveIdentificationBurnSpeedButton = new ToolStripMenuItem();
            LoadIdentificationBurnSpeedButton = new ToolStripMenuItem();
            BarrelStrengthContextMenu = new ContextMenuStrip(components);
            ViewBarrelStrengthButton = new ToolStripMenuItem();
            GasParametersContextMenu = new ContextMenuStrip(components);
            ViewInletBallisticButton = new ToolStripMenuItem();
            ViewGasDistributionButton = new ToolStripMenuItem();
            ViewGasEpuresButton = new ToolStripMenuItem();
            TemperatureContextMenu = new ContextMenuStrip(components);
            ViewTemperatureButton = new ToolStripMenuItem();
            ViewTemperatureDistributionsButton = new ToolStripMenuItem();
            DeflectionContextMenu = new ContextMenuStrip(components);
            ViewDeflectionButton = new ToolStripMenuItem();
            VibrationsContextMenu = new ContextMenuStrip(components);
            ViewVibrationsButton = new ToolStripMenuItem();
            ViewVibrationsDistributionsButton = new ToolStripMenuItem();
            OutletBallisticContextMenu = new ContextMenuStrip(components);
            ViewOutletBallisticButton = new ToolStripMenuItem();
            ShotsParametersContextMenu = new ContextMenuStrip(components);
            ViewShotsParametersButton = new ToolStripMenuItem();
            MissileContextMenu = new ContextMenuStrip(components);
            SaveMissileGeometryButton = new ToolStripMenuItem();
            LoadMissileGeometryButton = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            ViewMissileButton = new ToolStripMenuItem();
            CalculateMissilePhysicsButton = new ToolStripMenuItem();
            PowderChargeContextMenu = new ContextMenuStrip(components);
            SavePowdersButton = new ToolStripMenuItem();
            LoadPowdersButton = new ToolStripMenuItem();
            MainMenuStrip = new MenuStrip();
            FileMenuItem = new ToolStripMenuItem();
            SaveProjectMenuItem = new ToolStripMenuItem();
            LoadProjectMenuItem = new ToolStripMenuItem();
            SettingsMenuItem = new ToolStripMenuItem();
            ViewMenuItem = new ToolStripMenuItem();
            ViewOxyMenuItem = new ToolStripMenuItem();
            ViewOxzMenuItem = new ToolStripMenuItem();
            ViewOyzMenuItem = new ToolStripMenuItem();
            HelpMenuItem = new ToolStripMenuItem();
            GuideButton = new ToolStripMenuItem();
            AboutProgramButton = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            progressBar = new ToolStripProgressBar();
            statusLabel = new ToolStripStatusLabel();
            splitContainer = new SplitContainer();
            splitContainer1 = new SplitContainer();
            problemGroupBox = new GroupBox();
            ElementsTreeView = new TreeView();
            treeViewImageList = new ImageList(components);
            propertyGrid = new PropertyGrid();
            TabControl = new TabControl();
            GraphicsPage = new TabPage();
            formsPlot = new ScottPlot.FormsPlot();
            MeshPage = new TabPage();
            VisualizationSplitContainer = new SplitContainer();
            openglControl = new SharpGL.OpenGLControl();
            toolStripSeparator9 = new ToolStripSeparator();
            LoadBarrelRoughsButton = new ToolStripMenuItem();
            MaterialContextMenu.SuspendLayout();
            AmmoContextMenu.SuspendLayout();
            MuzzleBreakContextMenu.SuspendLayout();
            BarrelGeometryContextMenu.SuspendLayout();
            MeshContextMenu.SuspendLayout();
            FiringSystemContextMenu.SuspendLayout();
            EnvironmentContextMenu.SuspendLayout();
            ModelContextMenu.SuspendLayout();
            CalculateContextMenu.SuspendLayout();
            OptimizationContextMenu.SuspendLayout();
            IdentificationContextMenu.SuspendLayout();
            BarrelStrengthContextMenu.SuspendLayout();
            GasParametersContextMenu.SuspendLayout();
            TemperatureContextMenu.SuspendLayout();
            DeflectionContextMenu.SuspendLayout();
            VibrationsContextMenu.SuspendLayout();
            OutletBallisticContextMenu.SuspendLayout();
            ShotsParametersContextMenu.SuspendLayout();
            MissileContextMenu.SuspendLayout();
            PowderChargeContextMenu.SuspendLayout();
            MainMenuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            problemGroupBox.SuspendLayout();
            TabControl.SuspendLayout();
            GraphicsPage.SuspendLayout();
            MeshPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)VisualizationSplitContainer).BeginInit();
            VisualizationSplitContainer.Panel2.SuspendLayout();
            VisualizationSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)openglControl).BeginInit();
            SuspendLayout();
            // 
            // MaterialContextMenu
            // 
            MaterialContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MaterialContextMenu.Items.AddRange(new ToolStripItem[] { SaveMaterialButton, LoadMaterialButton });
            MaterialContextMenu.Name = "GeometryContextMenu";
            MaterialContextMenu.Size = new Size(152, 52);
            // 
            // SaveMaterialButton
            // 
            SaveMaterialButton.Name = "SaveMaterialButton";
            SaveMaterialButton.Size = new Size(151, 24);
            SaveMaterialButton.Text = "Сохранить";
            SaveMaterialButton.Click += SaveMaterialButton_Click;
            // 
            // LoadMaterialButton
            // 
            LoadMaterialButton.Name = "LoadMaterialButton";
            LoadMaterialButton.Size = new Size(151, 24);
            LoadMaterialButton.Text = "Загрузить";
            LoadMaterialButton.Click += LoadMaterialButton_Click;
            // 
            // AmmoContextMenu
            // 
            AmmoContextMenu.Items.AddRange(new ToolStripItem[] { SaveAmmoButton, LoadAmmoButton, toolStripSeparator1, SelectAmmoButton });
            AmmoContextMenu.Name = "AmmoContextMenu";
            AmmoContextMenu.Size = new Size(150, 82);
            // 
            // SaveAmmoButton
            // 
            SaveAmmoButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SaveAmmoButton.Name = "SaveAmmoButton";
            SaveAmmoButton.Size = new Size(149, 24);
            SaveAmmoButton.Text = "Сохранить";
            SaveAmmoButton.Click += SaveAmmoButton_Click;
            // 
            // LoadAmmoButton
            // 
            LoadAmmoButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LoadAmmoButton.Name = "LoadAmmoButton";
            LoadAmmoButton.Size = new Size(149, 24);
            LoadAmmoButton.Text = "Загрузить";
            LoadAmmoButton.Click += LoadAmmoButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(146, 6);
            // 
            // SelectAmmoButton
            // 
            SelectAmmoButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SelectAmmoButton.Name = "SelectAmmoButton";
            SelectAmmoButton.Size = new Size(149, 24);
            SelectAmmoButton.Text = "Выбрать";
            SelectAmmoButton.Click += SelectAmmoButton_Click;
            // 
            // MuzzleBreakContextMenu
            // 
            MuzzleBreakContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MuzzleBreakContextMenu.Items.AddRange(new ToolStripItem[] { SaveMuzzleBreakButton, LoadMuzzleBreakButton });
            MuzzleBreakContextMenu.Name = "PowderContextMenu";
            MuzzleBreakContextMenu.Size = new Size(152, 52);
            // 
            // SaveMuzzleBreakButton
            // 
            SaveMuzzleBreakButton.Name = "SaveMuzzleBreakButton";
            SaveMuzzleBreakButton.Size = new Size(151, 24);
            SaveMuzzleBreakButton.Text = "Сохранить";
            SaveMuzzleBreakButton.Click += SaveMuzzleBreakButton_Click;
            // 
            // LoadMuzzleBreakButton
            // 
            LoadMuzzleBreakButton.Name = "LoadMuzzleBreakButton";
            LoadMuzzleBreakButton.Size = new Size(151, 24);
            LoadMuzzleBreakButton.Text = "Загрузить";
            LoadMuzzleBreakButton.Click += LoadMuzzleBreakButton_Click;
            // 
            // BarrelGeometryContextMenu
            // 
            BarrelGeometryContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BarrelGeometryContextMenu.Items.AddRange(new ToolStripItem[] { SaveBarrelGeometryButton, LoadBarrelGeometryButton, toolStripSeparator3, ViewBarrelButton, toolStripSeparator4, CalculateBarrelStiffenersDiametersButton, CalculateBarrelStiffenersDistanceButton, toolStripSeparator9, LoadBarrelRoughsButton });
            BarrelGeometryContextMenu.Name = "GeometryContextMenu";
            BarrelGeometryContextMenu.Size = new Size(374, 188);
            // 
            // SaveBarrelGeometryButton
            // 
            SaveBarrelGeometryButton.Name = "SaveBarrelGeometryButton";
            SaveBarrelGeometryButton.Size = new Size(373, 24);
            SaveBarrelGeometryButton.Text = "Сохранить";
            SaveBarrelGeometryButton.Click += SaveBarrelGeometryButton_Click;
            // 
            // LoadBarrelGeometryButton
            // 
            LoadBarrelGeometryButton.Name = "LoadBarrelGeometryButton";
            LoadBarrelGeometryButton.Size = new Size(373, 24);
            LoadBarrelGeometryButton.Text = "Загрузить";
            LoadBarrelGeometryButton.Click += LoadBarrelGeometryButton_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(370, 6);
            // 
            // ViewBarrelButton
            // 
            ViewBarrelButton.Name = "ViewBarrelButton";
            ViewBarrelButton.Size = new Size(373, 24);
            ViewBarrelButton.Text = "Просмотр";
            ViewBarrelButton.Click += ViewBarrelButton_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(370, 6);
            // 
            // CalculateBarrelStiffenersDiametersButton
            // 
            CalculateBarrelStiffenersDiametersButton.Name = "CalculateBarrelStiffenersDiametersButton";
            CalculateBarrelStiffenersDiametersButton.Size = new Size(373, 24);
            CalculateBarrelStiffenersDiametersButton.Text = "Авторасчет диаметров ребер жесткости";
            CalculateBarrelStiffenersDiametersButton.Click += CalculateBarrelStiffenersDiametersButton_Click;
            // 
            // CalculateBarrelStiffenersDistanceButton
            // 
            CalculateBarrelStiffenersDistanceButton.Name = "CalculateBarrelStiffenersDistanceButton";
            CalculateBarrelStiffenersDistanceButton.Size = new Size(373, 24);
            CalculateBarrelStiffenersDistanceButton.Text = "Авторасчет расстояний до ребер жесткости";
            CalculateBarrelStiffenersDistanceButton.Click += CalculateBarrelStiffenersDistanceButton_Click;
            // 
            // MeshContextMenu
            // 
            MeshContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MeshContextMenu.Items.AddRange(new ToolStripItem[] { SaveMeshButton, LoadMeshButton, toolStripSeparator5, CalculateGeometryButton });
            MeshContextMenu.Name = "GeometryContextMenu";
            MeshContextMenu.Size = new Size(152, 82);
            // 
            // SaveMeshButton
            // 
            SaveMeshButton.Name = "SaveMeshButton";
            SaveMeshButton.Size = new Size(151, 24);
            SaveMeshButton.Text = "Сохранить";
            SaveMeshButton.Click += SaveMeshButton_Click;
            // 
            // LoadMeshButton
            // 
            LoadMeshButton.Name = "LoadMeshButton";
            LoadMeshButton.Size = new Size(151, 24);
            LoadMeshButton.Text = "Загрузить";
            LoadMeshButton.Click += LoadMeshButton_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(148, 6);
            // 
            // CalculateGeometryButton
            // 
            CalculateGeometryButton.Name = "CalculateGeometryButton";
            CalculateGeometryButton.Size = new Size(151, 24);
            CalculateGeometryButton.Text = "Построить";
            CalculateGeometryButton.Click += CalculateGeometryButton_Click;
            // 
            // FiringSystemContextMenu
            // 
            FiringSystemContextMenu.Items.AddRange(new ToolStripItem[] { SaveFiringSystemButton, LoadFiringSystemButton, LoadMovementsFiringSystemButton });
            FiringSystemContextMenu.Name = "FiringSystemContextMenu";
            FiringSystemContextMenu.Size = new Size(300, 76);
            // 
            // SaveFiringSystemButton
            // 
            SaveFiringSystemButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SaveFiringSystemButton.Name = "SaveFiringSystemButton";
            SaveFiringSystemButton.Size = new Size(299, 24);
            SaveFiringSystemButton.Text = "Сохранить";
            SaveFiringSystemButton.Click += SaveFiringSystemButton_Click;
            // 
            // LoadFiringSystemButton
            // 
            LoadFiringSystemButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LoadFiringSystemButton.Name = "LoadFiringSystemButton";
            LoadFiringSystemButton.Size = new Size(299, 24);
            LoadFiringSystemButton.Text = "Загрузить";
            LoadFiringSystemButton.Click += LoadFiringSystemButton_Click;
            // 
            // LoadMovementsFiringSystemButton
            // 
            LoadMovementsFiringSystemButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LoadMovementsFiringSystemButton.Name = "LoadMovementsFiringSystemButton";
            LoadMovementsFiringSystemButton.Size = new Size(299, 24);
            LoadMovementsFiringSystemButton.Text = "Загрузить перемещения из файла";
            LoadMovementsFiringSystemButton.Click += LoadMovementsFiringSystemButton_Click;
            // 
            // EnvironmentContextMenu
            // 
            EnvironmentContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            EnvironmentContextMenu.Items.AddRange(new ToolStripItem[] { SaveEnvironmentButton, LoadEnvironmentButton });
            EnvironmentContextMenu.Name = "GeometryContextMenu";
            EnvironmentContextMenu.Size = new Size(152, 52);
            // 
            // SaveEnvironmentButton
            // 
            SaveEnvironmentButton.Name = "SaveEnvironmentButton";
            SaveEnvironmentButton.Size = new Size(151, 24);
            SaveEnvironmentButton.Text = "Сохранить";
            SaveEnvironmentButton.Click += SaveEnvironmentButton_Click;
            // 
            // LoadEnvironmentButton
            // 
            LoadEnvironmentButton.Name = "LoadEnvironmentButton";
            LoadEnvironmentButton.Size = new Size(151, 24);
            LoadEnvironmentButton.Text = "Загрузить";
            LoadEnvironmentButton.Click += LoadEnvironmentButton_Click;
            // 
            // ModelContextMenu
            // 
            ModelContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ModelContextMenu.Items.AddRange(new ToolStripItem[] { SaveModelButton, LoadModelButton });
            ModelContextMenu.Name = "GeometryContextMenu";
            ModelContextMenu.Size = new Size(152, 52);
            // 
            // SaveModelButton
            // 
            SaveModelButton.Name = "SaveModelButton";
            SaveModelButton.Size = new Size(151, 24);
            SaveModelButton.Text = "Сохранить";
            SaveModelButton.Click += SaveModelButton_Click;
            // 
            // LoadModelButton
            // 
            LoadModelButton.Name = "LoadModelButton";
            LoadModelButton.Size = new Size(151, 24);
            LoadModelButton.Text = "Загрузить";
            LoadModelButton.Click += LoadModelButton_Click;
            // 
            // CalculateContextMenu
            // 
            CalculateContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            CalculateContextMenu.Items.AddRange(new ToolStripItem[] { StartCalculationButton, StopCalculationButton });
            CalculateContextMenu.Name = "CalculateContextMenu";
            CalculateContextMenu.Size = new Size(160, 52);
            // 
            // StartCalculationButton
            // 
            StartCalculationButton.Name = "StartCalculationButton";
            StartCalculationButton.Size = new Size(159, 24);
            StartCalculationButton.Text = "Начать";
            StartCalculationButton.Click += StartCalculationButton_Click;
            // 
            // StopCalculationButton
            // 
            StopCalculationButton.Name = "StopCalculationButton";
            StopCalculationButton.Size = new Size(159, 24);
            StopCalculationButton.Text = "Остановить";
            StopCalculationButton.Click += StopCalculationButton_Click;
            // 
            // OptimizationContextMenu
            // 
            OptimizationContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            OptimizationContextMenu.Items.AddRange(new ToolStripItem[] { StartOptimizationButton, ResumeOptimizationButton, StopOptimizationButton, toolStripSeparator6, SaveOptimizationButton, LoadOptimizationButton, toolStripSeparator8, SelectOptimizationAlgorithm });
            OptimizationContextMenu.Name = "CalculateContextMenu";
            OptimizationContextMenu.Size = new Size(271, 160);
            // 
            // StartOptimizationButton
            // 
            StartOptimizationButton.Name = "StartOptimizationButton";
            StartOptimizationButton.Size = new Size(270, 24);
            StartOptimizationButton.Text = "Начать";
            StartOptimizationButton.Click += StartOptimizationButton_Click;
            // 
            // ResumeOptimizationButton
            // 
            ResumeOptimizationButton.Name = "ResumeOptimizationButton";
            ResumeOptimizationButton.Size = new Size(270, 24);
            ResumeOptimizationButton.Text = "Возобновить";
            ResumeOptimizationButton.Click += ResumeOptimizationButton_Click;
            // 
            // StopOptimizationButton
            // 
            StopOptimizationButton.Name = "StopOptimizationButton";
            StopOptimizationButton.Size = new Size(270, 24);
            StopOptimizationButton.Text = "Остановить";
            StopOptimizationButton.Click += StopOptimizationButton_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(267, 6);
            // 
            // SaveOptimizationButton
            // 
            SaveOptimizationButton.Name = "SaveOptimizationButton";
            SaveOptimizationButton.Size = new Size(270, 24);
            SaveOptimizationButton.Text = "Сохранить";
            SaveOptimizationButton.Click += SaveOptimizationButton_Click;
            // 
            // LoadOptimizationButton
            // 
            LoadOptimizationButton.Name = "LoadOptimizationButton";
            LoadOptimizationButton.Size = new Size(270, 24);
            LoadOptimizationButton.Text = "Загрузить";
            LoadOptimizationButton.Click += LoadOptimizationButton_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(267, 6);
            // 
            // SelectOptimizationAlgorithm
            // 
            SelectOptimizationAlgorithm.DropDownItems.AddRange(new ToolStripItem[] { NelderMeadButton, HookeJeevesButton, RandomDescendButton });
            SelectOptimizationAlgorithm.Name = "SelectOptimizationAlgorithm";
            SelectOptimizationAlgorithm.Size = new Size(270, 24);
            SelectOptimizationAlgorithm.Text = "Выбор метода оптимизации";
            // 
            // NelderMeadButton
            // 
            NelderMeadButton.Name = "NelderMeadButton";
            NelderMeadButton.Size = new Size(226, 24);
            NelderMeadButton.Text = "Метод Нелдера-Мида";
            NelderMeadButton.Click += NelderMeadButton_Click;
            // 
            // HookeJeevesButton
            // 
            HookeJeevesButton.Name = "HookeJeevesButton";
            HookeJeevesButton.Size = new Size(226, 24);
            HookeJeevesButton.Text = "Метод Хука-Дживса";
            HookeJeevesButton.Click += HookeJeevesButton_Click;
            // 
            // RandomDescendButton
            // 
            RandomDescendButton.Name = "RandomDescendButton";
            RandomDescendButton.Size = new Size(226, 24);
            RandomDescendButton.Text = "Случайный поиск";
            RandomDescendButton.Click += RandomDescendButton_Click;
            // 
            // IdentificationContextMenu
            // 
            IdentificationContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            IdentificationContextMenu.Items.AddRange(new ToolStripItem[] { IdentifyBurnSpeedButton, StopIdentificationBurnSpeedButton, toolStripSeparator7, SaveIdentificationBurnSpeedButton, LoadIdentificationBurnSpeedButton });
            IdentificationContextMenu.Name = "CalculateContextMenu";
            IdentificationContextMenu.Size = new Size(160, 106);
            // 
            // IdentifyBurnSpeedButton
            // 
            IdentifyBurnSpeedButton.Name = "IdentifyBurnSpeedButton";
            IdentifyBurnSpeedButton.Size = new Size(159, 24);
            IdentifyBurnSpeedButton.Text = "Начать";
            IdentifyBurnSpeedButton.Click += IdentifyBurnSpeedButton_Click;
            // 
            // StopIdentificationBurnSpeedButton
            // 
            StopIdentificationBurnSpeedButton.Name = "StopIdentificationBurnSpeedButton";
            StopIdentificationBurnSpeedButton.Size = new Size(159, 24);
            StopIdentificationBurnSpeedButton.Text = "Остановить";
            StopIdentificationBurnSpeedButton.Click += StopIdentificationBurnSpeedButton_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(156, 6);
            // 
            // SaveIdentificationBurnSpeedButton
            // 
            SaveIdentificationBurnSpeedButton.Name = "SaveIdentificationBurnSpeedButton";
            SaveIdentificationBurnSpeedButton.Size = new Size(159, 24);
            SaveIdentificationBurnSpeedButton.Text = "Сохранить";
            SaveIdentificationBurnSpeedButton.Click += SaveIdentificationBurnSpeedButton_Click;
            // 
            // LoadIdentificationBurnSpeedButton
            // 
            LoadIdentificationBurnSpeedButton.Name = "LoadIdentificationBurnSpeedButton";
            LoadIdentificationBurnSpeedButton.Size = new Size(159, 24);
            LoadIdentificationBurnSpeedButton.Text = "Загрузить";
            LoadIdentificationBurnSpeedButton.Click += LoadIdentificationBurnSpeedButton_Click;
            // 
            // BarrelStrengthContextMenu
            // 
            BarrelStrengthContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BarrelStrengthContextMenu.Items.AddRange(new ToolStripItem[] { ViewBarrelStrengthButton });
            BarrelStrengthContextMenu.Name = "TemperatureContextMenu";
            BarrelStrengthContextMenu.Size = new Size(146, 28);
            // 
            // ViewBarrelStrengthButton
            // 
            ViewBarrelStrengthButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ViewBarrelStrengthButton.Name = "ViewBarrelStrengthButton";
            ViewBarrelStrengthButton.Size = new Size(145, 24);
            ViewBarrelStrengthButton.Text = "Просмотр";
            ViewBarrelStrengthButton.Click += ViewBarrelStrengthButton_Click;
            // 
            // GasParametersContextMenu
            // 
            GasParametersContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            GasParametersContextMenu.Items.AddRange(new ToolStripItem[] { ViewInletBallisticButton, ViewGasDistributionButton, ViewGasEpuresButton });
            GasParametersContextMenu.Name = "GeometryContextMenu";
            GasParametersContextMenu.Size = new Size(294, 76);
            // 
            // ViewInletBallisticButton
            // 
            ViewInletBallisticButton.Name = "ViewInletBallisticButton";
            ViewInletBallisticButton.Size = new Size(293, 24);
            ViewInletBallisticButton.Text = "Внутренняя баллистика";
            ViewInletBallisticButton.Click += ViewInletBallisticButton_Click;
            // 
            // ViewGasDistributionButton
            // 
            ViewGasDistributionButton.Name = "ViewGasDistributionButton";
            ViewGasDistributionButton.Size = new Size(293, 24);
            ViewGasDistributionButton.Text = "Распределение параметров газа";
            ViewGasDistributionButton.Click += ViewGasDistributionButton_Click;
            // 
            // ViewGasEpuresButton
            // 
            ViewGasEpuresButton.Name = "ViewGasEpuresButton";
            ViewGasEpuresButton.Size = new Size(293, 24);
            ViewGasEpuresButton.Text = "Эпюры параметров газа";
            ViewGasEpuresButton.Click += ViewGasEpuresButton_Click;
            // 
            // TemperatureContextMenu
            // 
            TemperatureContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TemperatureContextMenu.Items.AddRange(new ToolStripItem[] { ViewTemperatureButton, ViewTemperatureDistributionsButton });
            TemperatureContextMenu.Name = "TemperatureContextMenu";
            TemperatureContextMenu.Size = new Size(273, 52);
            // 
            // ViewTemperatureButton
            // 
            ViewTemperatureButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ViewTemperatureButton.Name = "ViewTemperatureButton";
            ViewTemperatureButton.Size = new Size(272, 24);
            ViewTemperatureButton.Text = "Температура";
            ViewTemperatureButton.Click += ViewTemperatureButton_Click;
            // 
            // ViewTemperatureDistributionsButton
            // 
            ViewTemperatureDistributionsButton.Name = "ViewTemperatureDistributionsButton";
            ViewTemperatureDistributionsButton.Size = new Size(272, 24);
            ViewTemperatureDistributionsButton.Text = "Распределение температуры";
            ViewTemperatureDistributionsButton.Click += ViewTemperatureDistributionsButtonButton_Click;
            // 
            // DeflectionContextMenu
            // 
            DeflectionContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            DeflectionContextMenu.Items.AddRange(new ToolStripItem[] { ViewDeflectionButton });
            DeflectionContextMenu.Name = "TemperatureContextMenu";
            DeflectionContextMenu.Size = new Size(146, 28);
            // 
            // ViewDeflectionButton
            // 
            ViewDeflectionButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ViewDeflectionButton.Name = "ViewDeflectionButton";
            ViewDeflectionButton.Size = new Size(145, 24);
            ViewDeflectionButton.Text = "Просмотр";
            ViewDeflectionButton.Click += ViewDeflectionButton_Click;
            // 
            // VibrationsContextMenu
            // 
            VibrationsContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            VibrationsContextMenu.Items.AddRange(new ToolStripItem[] { ViewVibrationsButton, ViewVibrationsDistributionsButton });
            VibrationsContextMenu.Name = "TemperatureContextMenu";
            VibrationsContextMenu.Size = new Size(278, 52);
            // 
            // ViewVibrationsButton
            // 
            ViewVibrationsButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ViewVibrationsButton.Name = "ViewVibrationsButton";
            ViewVibrationsButton.Size = new Size(277, 24);
            ViewVibrationsButton.Text = "Перемещения дульного среза";
            ViewVibrationsButton.Click += ViewVibrationsButton_Click;
            // 
            // ViewVibrationsDistributionsButton
            // 
            ViewVibrationsDistributionsButton.Name = "ViewVibrationsDistributionsButton";
            ViewVibrationsDistributionsButton.Size = new Size(277, 24);
            ViewVibrationsDistributionsButton.Text = "Распределения перемещений";
            ViewVibrationsDistributionsButton.Click += ViewVibrationsDistributionsButton_Click;
            // 
            // OutletBallisticContextMenu
            // 
            OutletBallisticContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            OutletBallisticContextMenu.Items.AddRange(new ToolStripItem[] { ViewOutletBallisticButton });
            OutletBallisticContextMenu.Name = "TemperatureContextMenu";
            OutletBallisticContextMenu.Size = new Size(146, 28);
            // 
            // ViewOutletBallisticButton
            // 
            ViewOutletBallisticButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ViewOutletBallisticButton.Name = "ViewOutletBallisticButton";
            ViewOutletBallisticButton.Size = new Size(145, 24);
            ViewOutletBallisticButton.Text = "Просмотр";
            ViewOutletBallisticButton.Click += ViewOutletBallisticButton_Click;
            // 
            // ShotsParametersContextMenu
            // 
            ShotsParametersContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ShotsParametersContextMenu.Items.AddRange(new ToolStripItem[] { ViewShotsParametersButton });
            ShotsParametersContextMenu.Name = "TemperatureContextMenu";
            ShotsParametersContextMenu.Size = new Size(146, 28);
            // 
            // ViewShotsParametersButton
            // 
            ViewShotsParametersButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ViewShotsParametersButton.Name = "ViewShotsParametersButton";
            ViewShotsParametersButton.Size = new Size(145, 24);
            ViewShotsParametersButton.Text = "Просмотр";
            ViewShotsParametersButton.Click += ViewShotsParametersButton_Click;
            // 
            // MissileContextMenu
            // 
            MissileContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MissileContextMenu.Items.AddRange(new ToolStripItem[] { SaveMissileGeometryButton, LoadMissileGeometryButton, toolStripSeparator2, ViewMissileButton, CalculateMissilePhysicsButton });
            MissileContextMenu.Name = "GeometryContextMenu";
            MissileContextMenu.Size = new Size(314, 106);
            // 
            // SaveMissileGeometryButton
            // 
            SaveMissileGeometryButton.Name = "SaveMissileGeometryButton";
            SaveMissileGeometryButton.Size = new Size(313, 24);
            SaveMissileGeometryButton.Text = "Сохранить";
            // 
            // LoadMissileGeometryButton
            // 
            LoadMissileGeometryButton.Name = "LoadMissileGeometryButton";
            LoadMissileGeometryButton.Size = new Size(313, 24);
            LoadMissileGeometryButton.Text = "Загрузить";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(310, 6);
            // 
            // ViewMissileButton
            // 
            ViewMissileButton.Name = "ViewMissileButton";
            ViewMissileButton.Size = new Size(313, 24);
            ViewMissileButton.Text = "Просмотр";
            // 
            // CalculateMissilePhysicsButton
            // 
            CalculateMissilePhysicsButton.Name = "CalculateMissilePhysicsButton";
            CalculateMissilePhysicsButton.Size = new Size(313, 24);
            CalculateMissilePhysicsButton.Text = "Рассчитать физические параметры";
            // 
            // PowderChargeContextMenu
            // 
            PowderChargeContextMenu.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PowderChargeContextMenu.Items.AddRange(new ToolStripItem[] { SavePowdersButton, LoadPowdersButton });
            PowderChargeContextMenu.Name = "PowderContextMenu";
            PowderChargeContextMenu.Size = new Size(152, 52);
            // 
            // SavePowdersButton
            // 
            SavePowdersButton.Name = "SavePowdersButton";
            SavePowdersButton.Size = new Size(151, 24);
            SavePowdersButton.Text = "Сохранить";
            // 
            // LoadPowdersButton
            // 
            LoadPowdersButton.Name = "LoadPowdersButton";
            LoadPowdersButton.Size = new Size(151, 24);
            LoadPowdersButton.Text = "Загрузить";
            // 
            // MainMenuStrip
            // 
            MainMenuStrip.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MainMenuStrip.Items.AddRange(new ToolStripItem[] { FileMenuItem, SettingsMenuItem, ViewMenuItem, HelpMenuItem });
            MainMenuStrip.Location = new Point(0, 0);
            MainMenuStrip.Name = "MainMenuStrip";
            MainMenuStrip.Padding = new Padding(8, 3, 0, 3);
            MainMenuStrip.Size = new Size(1430, 29);
            MainMenuStrip.TabIndex = 8;
            MainMenuStrip.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            FileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SaveProjectMenuItem, LoadProjectMenuItem });
            FileMenuItem.Name = "FileMenuItem";
            FileMenuItem.Size = new Size(58, 23);
            FileMenuItem.Text = "Файл";
            // 
            // SaveProjectMenuItem
            // 
            SaveProjectMenuItem.Name = "SaveProjectMenuItem";
            SaveProjectMenuItem.Size = new Size(151, 24);
            SaveProjectMenuItem.Text = "Сохранить";
            SaveProjectMenuItem.Click += SaveProjectMenuItem_Click;
            // 
            // LoadProjectMenuItem
            // 
            LoadProjectMenuItem.Name = "LoadProjectMenuItem";
            LoadProjectMenuItem.Size = new Size(151, 24);
            LoadProjectMenuItem.Text = "Загрузить";
            LoadProjectMenuItem.Click += LoadProjectMenuItem_Click;
            // 
            // SettingsMenuItem
            // 
            SettingsMenuItem.Name = "SettingsMenuItem";
            SettingsMenuItem.Size = new Size(94, 23);
            SettingsMenuItem.Text = "Настройки";
            SettingsMenuItem.Click += SettingsMenuItem_Click;
            // 
            // ViewMenuItem
            // 
            ViewMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ViewOxyMenuItem, ViewOxzMenuItem, ViewOyzMenuItem });
            ViewMenuItem.Name = "ViewMenuItem";
            ViewMenuItem.Size = new Size(48, 23);
            ViewMenuItem.Text = "Вид";
            // 
            // ViewOxyMenuItem
            // 
            ViewOxyMenuItem.Name = "ViewOxyMenuItem";
            ViewOxyMenuItem.Size = new Size(177, 24);
            ViewOxyMenuItem.Text = "Плоскость Oxy";
            ViewOxyMenuItem.Click += ViewOxyMenuItem_Click;
            // 
            // ViewOxzMenuItem
            // 
            ViewOxzMenuItem.Name = "ViewOxzMenuItem";
            ViewOxzMenuItem.Size = new Size(177, 24);
            ViewOxzMenuItem.Text = "Плоскость Oxz";
            ViewOxzMenuItem.Click += ViewOxzMenuItem_Click;
            // 
            // ViewOyzMenuItem
            // 
            ViewOyzMenuItem.Name = "ViewOyzMenuItem";
            ViewOyzMenuItem.Size = new Size(177, 24);
            ViewOyzMenuItem.Text = "Плоскость Oyz";
            ViewOyzMenuItem.Click += ViewOyzMenuItem_Click;
            // 
            // HelpMenuItem
            // 
            HelpMenuItem.DropDownItems.AddRange(new ToolStripItem[] { GuideButton, AboutProgramButton });
            HelpMenuItem.Name = "HelpMenuItem";
            HelpMenuItem.Size = new Size(76, 23);
            HelpMenuItem.Text = "Помощь";
            // 
            // GuideButton
            // 
            GuideButton.Name = "GuideButton";
            GuideButton.Size = new Size(167, 24);
            GuideButton.Text = "Справка";
            GuideButton.Click += GuideButton_Click;
            // 
            // AboutProgramButton
            // 
            AboutProgramButton.Name = "AboutProgramButton";
            AboutProgramButton.Size = new Size(167, 24);
            AboutProgramButton.Text = "О программе";
            AboutProgramButton.Click += AboutProgramButton_Click;
            // 
            // statusStrip
            // 
            statusStrip.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            statusStrip.Items.AddRange(new ToolStripItem[] { progressBar, statusLabel });
            statusStrip.Location = new Point(0, 821);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1430, 22);
            statusStrip.TabIndex = 10;
            statusStrip.Text = "statusStrip1";
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(100, 16);
            // 
            // statusLabel
            // 
            statusLabel.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(98, 17);
            statusLabel.Text = "Ожидание работы";
            // 
            // splitContainer
            // 
            splitContainer.BackColor = SystemColors.Control;
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 29);
            splitContainer.Margin = new Padding(4);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(TabControl);
            splitContainer.Size = new Size(1430, 792);
            splitContainer.SplitterDistance = 457;
            splitContainer.SplitterWidth = 12;
            splitContainer.TabIndex = 11;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(problemGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(propertyGrid);
            splitContainer1.Panel2MinSize = 120;
            splitContainer1.Size = new Size(457, 792);
            splitContainer1.SplitterDistance = 393;
            splitContainer1.SplitterWidth = 12;
            splitContainer1.TabIndex = 2;
            // 
            // problemGroupBox
            // 
            problemGroupBox.BackColor = SystemColors.ControlLightLight;
            problemGroupBox.Controls.Add(ElementsTreeView);
            problemGroupBox.Dock = DockStyle.Fill;
            problemGroupBox.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            problemGroupBox.Location = new Point(0, 0);
            problemGroupBox.Margin = new Padding(4);
            problemGroupBox.Name = "problemGroupBox";
            problemGroupBox.Padding = new Padding(4);
            problemGroupBox.Size = new Size(457, 393);
            problemGroupBox.TabIndex = 3;
            problemGroupBox.TabStop = false;
            problemGroupBox.Text = "Задача";
            // 
            // ElementsTreeView
            // 
            ElementsTreeView.Dock = DockStyle.Fill;
            ElementsTreeView.ImageIndex = 0;
            ElementsTreeView.ImageList = treeViewImageList;
            ElementsTreeView.Location = new Point(4, 23);
            ElementsTreeView.Margin = new Padding(4);
            ElementsTreeView.Name = "ElementsTreeView";
            treeNode1.ImageKey = "document.png";
            treeNode1.Name = "MaterialInfoNode";
            treeNode1.SelectedImageKey = "rightArrow.png";
            treeNode1.Text = "Материал не выбран";
            treeNode2.ContextMenuStrip = MaterialContextMenu;
            treeNode2.ImageKey = "checked.png";
            treeNode2.Name = "MaterialNode";
            treeNode2.SelectedImageKey = "checked.png";
            treeNode2.Text = "Материал";
            treeNode3.ContextMenuStrip = AmmoContextMenu;
            treeNode3.Name = "AmmoNode";
            treeNode3.Text = "Боеприпасы";
            treeNode4.ContextMenuStrip = MuzzleBreakContextMenu;
            treeNode4.Name = "MuzzleBreakNode";
            treeNode4.Text = "Дульный тормоз";
            treeNode5.ImageKey = "document.png";
            treeNode5.Name = "BarrelGeometryInfoNode";
            treeNode5.SelectedImageKey = "rightArrow.png";
            treeNode5.Text = "Ствол не задан";
            treeNode6.ContextMenuStrip = BarrelGeometryContextMenu;
            treeNode6.Name = "BarrelGeometryNode";
            treeNode6.Text = "Геометрия ствола";
            treeNode7.ImageKey = "document.png";
            treeNode7.Name = "MeshXInfoNode";
            treeNode7.SelectedImageKey = "rightArrow.png";
            treeNode7.Text = "Сетка по пространству не задана";
            treeNode8.ContextMenuStrip = MeshContextMenu;
            treeNode8.Name = "MeshNode";
            treeNode8.Text = "Расчётная сетка";
            treeNode9.ContextMenuStrip = FiringSystemContextMenu;
            treeNode9.Name = "FiringSystemNode";
            treeNode9.Text = "Боевая установка";
            treeNode10.ImageKey = "document.png";
            treeNode10.Name = "EnvironmentPInfoNode";
            treeNode10.SelectedImageKey = "rightArrow.png";
            treeNode10.Text = "Давление не задано";
            treeNode11.ImageKey = "document.png";
            treeNode11.Name = "EnvironmentTNodeNode";
            treeNode11.SelectedImageKey = "rightArrow.png";
            treeNode11.Text = "Температура не задана";
            treeNode12.ImageKey = "document.png";
            treeNode12.Name = "EnvironmentRhoNodeNode";
            treeNode12.SelectedImageKey = "rightArrow.png";
            treeNode12.Text = "Плотность не задана";
            treeNode13.ImageKey = "document.png";
            treeNode13.Name = "EnvironmentKInfoNode";
            treeNode13.SelectedImageKey = "rightArrow.png";
            treeNode13.Text = "Показатель адиабаты не задан";
            treeNode14.ImageKey = "document.png";
            treeNode14.Name = "EnvironmentAInfoNode";
            treeNode14.SelectedImageKey = "rightArrow.png";
            treeNode14.Text = "Коэф. теплопередачи не задан";
            treeNode15.ContextMenuStrip = EnvironmentContextMenu;
            treeNode15.Name = "EnvironmentNode";
            treeNode15.Text = "Окружение";
            treeNode16.ImageKey = "document.png";
            treeNode16.Name = "ModelInfo1Node";
            treeNode16.SelectedImageKey = "rightArrow.png";
            treeNode16.Text = "Узел";
            treeNode17.ImageKey = "document.png";
            treeNode17.Name = "ModelInfo2Node";
            treeNode17.SelectedImageKey = "rightArrow.png";
            treeNode17.Text = "Узел";
            treeNode18.ImageKey = "document.png";
            treeNode18.Name = "ModelInfo3Node";
            treeNode18.SelectedImageKey = "rightArrow.png";
            treeNode18.Text = "Узел";
            treeNode19.ImageKey = "document.png";
            treeNode19.Name = "ModelInfo4Node";
            treeNode19.SelectedImageKey = "rightArrow.png";
            treeNode19.Text = "Узел";
            treeNode20.ImageKey = "document.png";
            treeNode20.Name = "ModelInfo5Node";
            treeNode20.SelectedImageKey = "rightArrow.png";
            treeNode20.Text = "Узел";
            treeNode21.ImageKey = "document.png";
            treeNode21.Name = "ModelInfo6Node";
            treeNode21.SelectedImageKey = "rightArrow.png";
            treeNode21.Text = "Узел";
            treeNode22.ContextMenuStrip = ModelContextMenu;
            treeNode22.Name = "ModelNode";
            treeNode22.Text = "Модель";
            treeNode23.ContextMenuStrip = CalculateContextMenu;
            treeNode23.ImageKey = "lighting.png";
            treeNode23.Name = "CalculationNode";
            treeNode23.SelectedImageKey = "lighting.png";
            treeNode23.Text = "Расчёт";
            treeNode24.ContextMenuStrip = OptimizationContextMenu;
            treeNode24.ImageKey = "lighting.png";
            treeNode24.Name = "OptimizationNode";
            treeNode24.SelectedImageKey = "lighting.png";
            treeNode24.Text = "Оптимизация формы";
            treeNode25.ContextMenuStrip = IdentificationContextMenu;
            treeNode25.ImageKey = "lighting.png";
            treeNode25.Name = "IdentificationNode";
            treeNode25.SelectedImageKey = "lighting.png";
            treeNode25.Text = "Идентификация";
            treeNode26.ImageKey = "lighting.png";
            treeNode26.Name = "ToolsNode";
            treeNode26.SelectedImageKey = "lighting.png";
            treeNode26.Text = "Дополнительные возможности";
            treeNode27.ContextMenuStrip = BarrelStrengthContextMenu;
            treeNode27.ImageKey = "document.png";
            treeNode27.Name = "BarrelStrengthNode";
            treeNode27.SelectedImageKey = "rightArrow.png";
            treeNode27.Text = "Прочность ствола";
            treeNode28.ContextMenuStrip = GasParametersContextMenu;
            treeNode28.ImageKey = "document.png";
            treeNode28.Name = "GasNode";
            treeNode28.SelectedImageKey = "rightArrow.png";
            treeNode28.Text = "Газопороховые параметры";
            treeNode29.ContextMenuStrip = TemperatureContextMenu;
            treeNode29.ImageKey = "document.png";
            treeNode29.Name = "TemperatureNode";
            treeNode29.SelectedImageKey = "rightArrow.png";
            treeNode29.Text = "Тепловое нагружение";
            treeNode30.ContextMenuStrip = DeflectionContextMenu;
            treeNode30.ImageKey = "document.png";
            treeNode30.Name = "DeflectionNode";
            treeNode30.SelectedImageKey = "rightArrow.png";
            treeNode30.Text = "Начальный прогиб";
            treeNode31.ContextMenuStrip = VibrationsContextMenu;
            treeNode31.ImageKey = "document.png";
            treeNode31.Name = "VibrationsNode";
            treeNode31.SelectedImageKey = "rightArrow.png";
            treeNode31.Text = "Колебания";
            treeNode32.ContextMenuStrip = OutletBallisticContextMenu;
            treeNode32.ImageKey = "document.png";
            treeNode32.Name = "OutletBallisticNode";
            treeNode32.SelectedImageKey = "rightArrow.png";
            treeNode32.Text = "Внешняя баллистика";
            treeNode33.ContextMenuStrip = ShotsParametersContextMenu;
            treeNode33.ImageKey = "document.png";
            treeNode33.Name = "ShotsParametersNode";
            treeNode33.SelectedImageKey = "rightArrow.png";
            treeNode33.Text = "Дульные параметры";
            treeNode34.ImageKey = "results.png";
            treeNode34.Name = "ResultsNode";
            treeNode34.SelectedImageKey = "results.png";
            treeNode34.Text = "Результаты";
            ElementsTreeView.Nodes.AddRange(new TreeNode[] { treeNode2, treeNode3, treeNode4, treeNode6, treeNode8, treeNode9, treeNode15, treeNode22, treeNode23, treeNode26, treeNode34 });
            ElementsTreeView.SelectedImageIndex = 0;
            ElementsTreeView.Size = new Size(449, 366);
            ElementsTreeView.TabIndex = 0;
            ElementsTreeView.NodeMouseClick += ElementsTreeView_NodeMouseClick;
            // 
            // treeViewImageList
            // 
            treeViewImageList.ColorDepth = ColorDepth.Depth8Bit;
            treeViewImageList.ImageStream = (ImageListStreamer)resources.GetObject("treeViewImageList.ImageStream");
            treeViewImageList.TransparentColor = Color.Transparent;
            treeViewImageList.Images.SetKeyName(0, "checked.png");
            treeViewImageList.Images.SetKeyName(1, "needAction.png");
            treeViewImageList.Images.SetKeyName(2, "rightArrow.png");
            treeViewImageList.Images.SetKeyName(3, "document.png");
            treeViewImageList.Images.SetKeyName(4, "lighting.png");
            treeViewImageList.Images.SetKeyName(5, "results.png");
            // 
            // propertyGrid
            // 
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            propertyGrid.Location = new Point(0, 0);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(457, 387);
            propertyGrid.TabIndex = 1;
            // 
            // TabControl
            // 
            TabControl.Controls.Add(GraphicsPage);
            TabControl.Controls.Add(MeshPage);
            TabControl.Dock = DockStyle.Fill;
            TabControl.Location = new Point(0, 0);
            TabControl.Margin = new Padding(4);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new Size(961, 792);
            TabControl.TabIndex = 0;
            // 
            // GraphicsPage
            // 
            GraphicsPage.Controls.Add(formsPlot);
            GraphicsPage.Location = new Point(4, 24);
            GraphicsPage.Margin = new Padding(4);
            GraphicsPage.Name = "GraphicsPage";
            GraphicsPage.Padding = new Padding(4);
            GraphicsPage.Size = new Size(953, 764);
            GraphicsPage.TabIndex = 1;
            GraphicsPage.Text = "Графики";
            GraphicsPage.UseVisualStyleBackColor = true;
            // 
            // formsPlot
            // 
            formsPlot.Dock = DockStyle.Fill;
            formsPlot.Location = new Point(4, 4);
            formsPlot.Margin = new Padding(5, 4, 5, 4);
            formsPlot.Name = "formsPlot";
            formsPlot.Size = new Size(945, 756);
            formsPlot.TabIndex = 0;
            // 
            // MeshPage
            // 
            MeshPage.Controls.Add(VisualizationSplitContainer);
            MeshPage.Location = new Point(4, 24);
            MeshPage.Margin = new Padding(4);
            MeshPage.Name = "MeshPage";
            MeshPage.Padding = new Padding(4);
            MeshPage.Size = new Size(953, 764);
            MeshPage.TabIndex = 4;
            MeshPage.Text = "Визуализация";
            MeshPage.UseVisualStyleBackColor = true;
            // 
            // VisualizationSplitContainer
            // 
            VisualizationSplitContainer.Dock = DockStyle.Fill;
            VisualizationSplitContainer.Location = new Point(4, 4);
            VisualizationSplitContainer.Name = "VisualizationSplitContainer";
            VisualizationSplitContainer.Panel1MinSize = 50;
            // 
            // VisualizationSplitContainer.Panel2
            // 
            VisualizationSplitContainer.Panel2.Controls.Add(openglControl);
            VisualizationSplitContainer.Panel2MinSize = 50;
            VisualizationSplitContainer.Size = new Size(945, 756);
            VisualizationSplitContainer.SplitterDistance = 185;
            VisualizationSplitContainer.SplitterWidth = 12;
            VisualizationSplitContainer.TabIndex = 2;
            // 
            // openglControl
            // 
            openglControl.Dock = DockStyle.Fill;
            openglControl.DrawFPS = false;
            openglControl.FrameRate = 30;
            openglControl.Location = new Point(0, 0);
            openglControl.Margin = new Padding(5, 4, 5, 4);
            openglControl.Name = "openglControl";
            openglControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL4_4;
            openglControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            openglControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            openglControl.Size = new Size(748, 756);
            openglControl.TabIndex = 0;
            openglControl.OpenGLDraw += openglControl_OpenGLDraw;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(370, 6);
            // 
            // LoadBarrelRoughsButton
            // 
            LoadBarrelRoughsButton.Name = "LoadBarrelRoughsButton";
            LoadBarrelRoughsButton.Size = new Size(373, 24);
            LoadBarrelRoughsButton.Text = "Загрузить неровности";
            LoadBarrelRoughsButton.Click += LoadBarrelRoughsButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1430, 843);
            Controls.Add(splitContainer);
            Controls.Add(statusStrip);
            Controls.Add(MainMenuStrip);
            Name = "MainForm";
            Text = "Моделирование напряженно-деформированного состояния и колебаний ствола";
            MaterialContextMenu.ResumeLayout(false);
            AmmoContextMenu.ResumeLayout(false);
            MuzzleBreakContextMenu.ResumeLayout(false);
            BarrelGeometryContextMenu.ResumeLayout(false);
            MeshContextMenu.ResumeLayout(false);
            FiringSystemContextMenu.ResumeLayout(false);
            EnvironmentContextMenu.ResumeLayout(false);
            ModelContextMenu.ResumeLayout(false);
            CalculateContextMenu.ResumeLayout(false);
            OptimizationContextMenu.ResumeLayout(false);
            IdentificationContextMenu.ResumeLayout(false);
            BarrelStrengthContextMenu.ResumeLayout(false);
            GasParametersContextMenu.ResumeLayout(false);
            TemperatureContextMenu.ResumeLayout(false);
            DeflectionContextMenu.ResumeLayout(false);
            VibrationsContextMenu.ResumeLayout(false);
            OutletBallisticContextMenu.ResumeLayout(false);
            ShotsParametersContextMenu.ResumeLayout(false);
            MissileContextMenu.ResumeLayout(false);
            PowderChargeContextMenu.ResumeLayout(false);
            MainMenuStrip.ResumeLayout(false);
            MainMenuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            problemGroupBox.ResumeLayout(false);
            TabControl.ResumeLayout(false);
            GraphicsPage.ResumeLayout(false);
            MeshPage.ResumeLayout(false);
            VisualizationSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)VisualizationSplitContainer).EndInit();
            VisualizationSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)openglControl).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private new MenuStrip MainMenuStrip;
        private CustomControls.HeatBar myHeatBar = new() { Dock = DockStyle.Fill, MeasureUnits = "" };
        private ToolStripMenuItem FileMenuItem;
        private ToolStripMenuItem SaveProjectMenuItem;
        private ToolStripMenuItem LoadProjectMenuItem;
        private ToolStripMenuItem SettingsMenuItem;
        private ToolStripMenuItem HelpMenuItem;
        private ToolStripMenuItem GuideButton;
        private ToolStripMenuItem AboutProgramButton;
        private StatusStrip statusStrip;
        private ToolStripProgressBar progressBar;
        private ToolStripStatusLabel statusLabel;
        private SplitContainer splitContainer;
        private GroupBox problemGroupBox;
        private TreeView ElementsTreeView;
        private PropertyGrid propertyGrid;
        private TabControl TabControl;
        private TabPage GraphicsPage;
        private ScottPlot.FormsPlot formsPlot;
        private TabPage MeshPage;
        private SharpGL.OpenGLControl openglControl;
        private ContextMenuStrip BarrelGeometryContextMenu;
        private ToolStripMenuItem SaveBarrelGeometryButton;
        private ToolStripMenuItem LoadBarrelGeometryButton;
        private ContextMenuStrip MeshContextMenu;
        private ToolStripMenuItem SaveMeshButton;
        private ToolStripMenuItem LoadMeshButton;
        private ContextMenuStrip EnvironmentContextMenu;
        private ToolStripMenuItem SaveEnvironmentButton;
        private ToolStripMenuItem LoadEnvironmentButton;
        private ContextMenuStrip MaterialContextMenu;
        private ToolStripMenuItem SaveMaterialButton;
        private ToolStripMenuItem LoadMaterialButton;
        private SplitContainer splitContainer1;
        private ContextMenuStrip ModelContextMenu;
        private ToolStripMenuItem SaveModelButton;
        private ToolStripMenuItem LoadModelButton;
        private ContextMenuStrip CalculateContextMenu;
        private ToolStripMenuItem StartCalculationButton;
        private ToolStripMenuItem StopCalculationButton;
        private ContextMenuStrip MissileContextMenu;
        private ToolStripMenuItem SaveMissileGeometryButton;
        private ToolStripMenuItem LoadMissileGeometryButton;
        private ContextMenuStrip PowderChargeContextMenu;
        private ToolStripMenuItem SavePowdersButton;
        private ToolStripMenuItem LoadPowdersButton;
        private SplitContainer VisualizationSplitContainer;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem ViewMissileButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem ViewBarrelButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem CalculateBarrelStiffenersDiametersButton;
        private ToolStripMenuItem CalculateBarrelStiffenersDistanceButton;
        private ContextMenuStrip GasParametersContextMenu;
        private ToolStripMenuItem ViewInletBallisticButton;
        private ToolStripMenuItem ViewGasDistributionButton;
        private ToolStripMenuItem ViewGasEpuresButton;
        private ContextMenuStrip TemperatureContextMenu;
        private ToolStripMenuItem ViewTemperatureButton;
        private ToolStripMenuItem ViewTemperatureDistributionsButton;
        private ContextMenuStrip DeflectionContextMenu;
        private ToolStripMenuItem ViewDeflectionButton;
        private ContextMenuStrip VibrationsContextMenu;
        private ToolStripMenuItem ViewVibrationsButton;
        private ContextMenuStrip OutletBallisticContextMenu;
        private ToolStripMenuItem ViewOutletBallisticButton;
        private ToolStripMenuItem ViewVibrationsDistributionsButton;
        private ContextMenuStrip OptimizationContextMenu;
        private ToolStripMenuItem StartOptimizationButton;
        private ToolStripMenuItem StopOptimizationButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem SaveOptimizationButton;
        private ToolStripMenuItem LoadOptimizationButton;
        private ContextMenuStrip IdentificationContextMenu;
        private ToolStripMenuItem IdentifyBurnSpeedButton;
        private ToolStripMenuItem StopIdentificationBurnSpeedButton;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem SaveIdentificationBurnSpeedButton;
        private ToolStripMenuItem LoadIdentificationBurnSpeedButton;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem SelectOptimizationAlgorithm;
        private ToolStripMenuItem NelderMeadButton;
        private ToolStripMenuItem HookeJeevesButton;
        private ToolStripMenuItem ViewMenuItem;
        private ToolStripMenuItem ViewOxyMenuItem;
        private ToolStripMenuItem ViewOxzMenuItem;
        private ToolStripMenuItem ViewOyzMenuItem;
        private ToolStripMenuItem ResumeOptimizationButton;
        private ToolStripMenuItem CalculateMissilePhysicsButton;
        private ContextMenuStrip MuzzleBreakContextMenu;
        private ToolStripMenuItem SaveMuzzleBreakButton;
        private ToolStripMenuItem LoadMuzzleBreakButton;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem CalculateGeometryButton;
        private ContextMenuStrip ShotsParametersContextMenu;
        private ToolStripMenuItem ViewShotsParametersButton;
        private ToolStripMenuItem RandomDescendButton;
        private ImageList treeViewImageList;
        private ContextMenuStrip FiringSystemContextMenu;
        private ToolStripMenuItem SaveFiringSystemButton;
        private ToolStripMenuItem LoadFiringSystemButton;
        private ContextMenuStrip BarrelStrengthContextMenu;
        private ToolStripMenuItem ViewBarrelStrengthButton;
        private ContextMenuStrip AmmoContextMenu;
        private ToolStripMenuItem SaveAmmoButton;
        private ToolStripMenuItem LoadAmmoButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem SelectAmmoButton;
        private ToolStripMenuItem LoadMovementsFiringSystemButton;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem LoadBarrelRoughsButton;
    }
}