using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using BarrelVibrations.ModelingObjects.MeshFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates;
using Flee.PublicTypes;
using MathNet.Numerics.Interpolation;
using Modelling.Geometry;
using Modelling.Loads;
using Modelling.MeshFolder;
using Modelling.Solvers;
using Modelling.Solvers.Problems;
using Newtonsoft.Json;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations.ModelingObjects.BarrelFolder
{

    [Serializable]
    [DataContract(Name = "Ствол")]
    public class Barrel
    {
        private const ushort CATEGORIESCOUNT = 8;

        public Barrel()
        {

        }

        [JsonConstructor]
        public Barrel(string name,
                      BarrelSection[] barrelSections,
                      double camoraLength,
                      double addCamoraVolume,
                      int groovesCount,
                      double groovesSlope,
                      double groovesDepth,
                      double groovesWidth,
                      List<BarrelRough> roughsY,
                      List<BarrelRough> roughsZ,
                      StiffenersType stiffenersType,
                      int stiffenersCount,
                      double stiffenersAngleShift,
                      StressCalculationMethod stressCalculationMethod,
                      bool initialized,
                      Mesh[] meshes,
                      double[] x,
                      double[] innerD,
                      double[] outerD,
                      double[] stiffenersDiameters,
                      double[] stiffenersDistances,
                      Point[][] stiffenersLocationsTotal,
                      double[] f,
                      double[] s,
                      double[] jy,
                      double[] jz,
                      double[] wy,
                      double[] wz,
                      double[] longitudinalUnitForce,
                      double[] unitTorqueY,
                      double[] unitTorqueZ,
                      double[] radiiDifference,
                      double[] coolingCoefficient)
        {
            Name = name;
            BarrelSections = barrelSections;
            CamoraLength = camoraLength;
            AddCamoraVolume = addCamoraVolume;
            GroovesCount = groovesCount;
            GroovesSlope = groovesSlope;
            GroovesDepth = groovesDepth;
            GroovesWidth = groovesWidth;
            if (roughsY != null)
                RoughsY = roughsY;
            if (roughsZ != null)
                RoughsZ = roughsZ;
            StiffenersType = stiffenersType;
            StiffenersCount = stiffenersCount;
            StiffenersAngleShift = stiffenersAngleShift;
            StressCalculationMethod = stressCalculationMethod;
            Initialized = initialized;
            Meshes = meshes;
            X = x;
            InnerD = innerD;
            OuterD = outerD;
            StiffenersDiameters = stiffenersDiameters;
            StiffenersDistances = stiffenersDistances;
            StiffenersLocationsTotal = stiffenersLocationsTotal;
            F = f;
            S = s;
            Jy = jy;
            Jz = jz;
            Wy = wy;
            Wz = wz;
            LongitudinalUnitForce = longitudinalUnitForce;
            UnitTorqueY = unitTorqueY;
            UnitTorqueZ = unitTorqueZ;
            RadiiDifference = radiiDifference;
            CoolingCoefficient = coolingCoefficient;
        }

        private double[] generateXs(BarrelSection[] barrelSections, int xCount)
        {
            return Mesh1DCreator.Create(barrelSections.Select(s => s.X * 1e-3).ToArray(), xCount);
        }

        #region Отображаемое в propertyGrid

        /// <summary>
        /// Название ствола
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Название", 1, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Название ствола")]
        [DataMember(Name = "Название ствола")]
        public string Name { get; set; } = "";

        /// <summary>
        /// Сечения ствола
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Основные", 2, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Сечения ствола")]
        [DataMember(Name = "Сечения ствола")]
        [TypeConverter(typeof(CountArrayConverter))]
        public BarrelSection[] BarrelSections { get; set; } = Array.Empty<BarrelSection>();

        private double camoraLength;
        /// <summary>
        /// Длина каморы, м
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Основные", 2, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Длина каморы, м")]
        [DataMember(Name = "Длина каморы, м")]
        public double CamoraLength
        {
            get => camoraLength;
            set => camoraLength = Math.Max(0, value);
        }

        /// <summary>
        /// Фактический объём каморы, м³
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Основные", 2, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Фактический объём каморы, м³")]
        [DataMember(Name = "Фактический объём каморы, м³")]
        public double CamoraVolume
        {
            get
            {
                if (!BarrelSections.Any() || CamoraLength == 0)
                    return 0;

                var dfunc = Algebra.GetFunc(
                    BarrelSections.Select(s => s.X * 1e-3).ToArray(),
                    BarrelSections.Select(s => s.dInner * 1e-3).ToArray());

                var N = 100;
                var dx = CamoraLength / N;

                var volume = 0.0;

                for (int i = 0; i < N; i++)
                {
                    var x = i * dx;
                    var s1 = Algebra.GetCircleArea(dfunc(x) / 2);
                    var s2 = Algebra.GetCircleArea(dfunc(x + dx) / 2);

                    volume += dx * (s1 + s2 + Math.Sqrt(s1 * s2)) / 3;
                }

                return volume + AddCamoraVolume;
            }
        }

        /// <summary>
        /// Добавочный объём каморы, м³
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Основные", 2, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Добавочный объём каморы, м³")]
        [DataMember(Name = "Добавочный объём каморы, м³")]
        public double AddCamoraVolume { get; set; }

        /// <summary>
        /// Области закрепления
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Крепление", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Области закрепления")]
        [DataMember(Name = "Области закрепления")]
        public List<FixationArea> FixationsAreas { get; private set; } = new List<FixationArea>();

        private int groovesCount;
        /// <summary>
        /// Количество нарезов
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Нарезы", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Количество нарезов")]
        [DataMember(Name = "Количество нарезов")]
        public int GroovesCount
        {
            get => groovesCount;
            set => groovesCount = Math.Max(0, value);
        }

        private double groovesSlope;
        /// <summary>
        /// Крутизна нарезов, клб
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Нарезы", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Крутизна нарезов, клб")]
        [DataMember(Name = "Крутизна нарезов, клб")]
        public double GroovesSlope
        {
            get => groovesSlope;
            set => groovesSlope = Math.Max(0, value);
        }

        private double groovesDepth;
        /// <summary>
        /// Глубина нарезов, мм
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Нарезы", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Глубина нарезов, мм")]
        [DataMember(Name = "Глубина нарезов, мм")]
        public double GroovesDepth
        {
            get => groovesDepth;
            set => groovesDepth = Math.Max(0, value);
        }

        private double groovesWidth;
        /// <summary>
        /// Ширина нарезов, мм
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Нарезы", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Ширина нарезов, мм")]
        [DataMember(Name = "Ширина нарезов, мм")]
        public double GroovesWidth
        {
            get => groovesWidth;
            set => groovesWidth = Math.Max(0, value);
        }

        /// <summary>
        /// Неровности по oY
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Неровности", 5, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Неровности по oY")]
        [DataMember(Name = "Неровности по oY")]
        public List<BarrelRough> RoughsY { get; set; } = new List<BarrelRough>();

        /// <summary>
        /// Неровности по oZ
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Неровности", 5, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Неровности по oZ")]
        [DataMember(Name = "Неровности по oZ")]
        public List<BarrelRough> RoughsZ { get; set; } = new List<BarrelRough>();

        /// <summary>
        /// Тип ребер жесткости
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Ребра жесткости", 6, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Тип ребер жесткости")]
        [DataMember(Name = "Тип ребер жесткости")]
        [TypeConverter(typeof(MyEnumConverter))]
        public StiffenersType StiffenersType { get; set; }

        private int stiffenersCount;
        /// <summary>
        /// Количество ребер жесткости
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Ребра жесткости", 6, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Количество ребер жесткости")]
        [DataMember(Name = "Количество ребер жесткости")]
        public int StiffenersCount
        {
            get => stiffenersCount;
            set => stiffenersCount = Math.Max(0, value);
        }

        /// <summary>
        /// Смещение ребер жесткости по углу, град 
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Ребра жесткости", 6, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Смещение ребер жесткости по углу")]
        [DataMember(Name = "Смещение ребер жесткости по углу")]
        public double StiffenersAngleShift { get; set; }

        /// <summary>
        /// Способ расчета напряжений в сечении
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Дополнительно", 7, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Способ расчета напряжений в сечении")]
        [DataMember(Name = "Способ расчета напряжений в сечении")]
        [TypeConverter(typeof(MyEnumConverter))]
        public StressCalculationMethod StressCalculationMethod { get; set; }

        /// <summary>
        /// Инициализированы ли характеристики ствола
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Инициализация", 8, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Инициализированы характеристики ствола")]
        [DataMember(Name = "Инициализированы характеристики ствола")]
        public bool Initialized { get; private set; }



        #endregion

        /// <summary>
        /// Сетки в сечениях. Сетки строятся в плоскости Oxy
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Расчетные сетки")]
        public Mesh[] Meshes { get; private set; } = Array.Empty<Mesh>();

        /// <summary>
        /// Координаты вдоль ствола, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Координаты X сечений ствола, м")]
        public double[] X { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Внутренние диаметры ствола, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Внутренние диаметры сечений ствола, м")]
        public double[] InnerD { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Внешние диаметры ствола, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Внешние диаметры сечений ствола, м")]
        public double[] OuterD { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Диаметры ребер жесткости, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Диаметры ребер жесткости, м")]
        public double[] StiffenersDiameters { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Расстояния до центров ребер жесткости, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Расстояния до центров ребер жесткости, м")]
        public double[] StiffenersDistances { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Координаты центров ребер жесткости, м: первое измерение - индекс точки, 2-е измерение - индекс ребра жёсткости
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Координаты ребер жесткости, м")]
        public Point[][] StiffenersLocationsTotal { get; private set; } = Array.Empty<Point[]>();

        /// <summary>
        /// Площади сечений, м²
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Площади поперечных сечений, м²")]
        public double[] F { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Площади канала, м²
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Площади канала, м²")]
        public double[] S { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Моменты инерции сечения Oy, м⁴
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Моменты инерции сечения Oy, м⁴")]
        public double[] Jy { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Моменты инерции сечения Oz, м⁴
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Моменты инерции сечения Oz, м⁴")]
        public double[] Jz { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Центры канала по Oy, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Центры канала по Oy, м")]
        public double[] Wy { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Центры канала по Oz, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Центры канала по Oz, м")]
        public double[] Wz { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Продольная сила на единицу давления, Н
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Продольная сила на единицу давления, Н")]
        public double[] LongitudinalUnitForce { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Момент силы по Oy на единицу давления, Н⋅м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Момент силы по Oy на единицу давления, Н⋅м")]
        public double[] UnitTorqueY { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Момент силы по Oz на единицу давления, Н⋅м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Момент силы по Oz на единицу давления, Н⋅м")]
        public double[] UnitTorqueZ { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Разность внешнего и внутреннего радиусов, м
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Разность внешнего и внутреннего радиусов, м")]
        public double[] RadiiDifference { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Коэффициент охлаждения
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Коэффициент охлаждения")]
        public double[] CoolingCoefficient { get; private set; } = Array.Empty<double>();

        public double GetMass(double density)
        {
            var m = 0.0;

            for (var i = 0; i < F.Length - 1; i++)
            {
                var h = X[i + 1] - X[i];
                m += density * h * (F[i + 1] + F[i]) / 2;
            }

            return m;
        }

        private void calculateBarrelTotalGeometry(int xCount)
        {
            X = generateXs(BarrelSections, xCount);

            var dFunc = Algebra.GetFunc(
                BarrelSections.Select(v => v.X * 1e-3).ToArray(),
                BarrelSections.Select(v => v.dInner * 1e-3).ToArray());

            var DFunc = Algebra.GetFunc(
                BarrelSections.Select(v => v.X * 1e-3).ToArray(),
                BarrelSections.Select(v => v.DOuter * 1e-3).ToArray());

            var stiffenersDiametersFunc = Algebra.GetFunc(
                BarrelSections.Select(v => v.X * 1e-3).ToArray(),
                BarrelSections.Select(v => v.StiffenersDiameter * 1e-3).ToArray());

            var stiffenersDistancesFunc = Algebra.GetFunc(
                BarrelSections.Select(v => v.X * 1e-3).ToArray(),
                BarrelSections.Select(v => v.StiffenersDistance * 1e-3).ToArray());

            InnerD = new double[X.Length];
            OuterD = new double[X.Length];
            StiffenersDiameters = new double[X.Length];
            StiffenersDistances = new double[X.Length];
            StiffenersLocationsTotal = new Point[X.Length][];
            Wy = new double[X.Length];
            Wz = new double[X.Length];

            var yRoughSpline = Algebra.GetFunc(RoughsY.Select(v => v.X * 1e-3), RoughsY.Select(v => v.Rough * 1e-3), false);
            var zRoughSpline = Algebra.GetFunc(RoughsZ.Select(v => v.X * 1e-3), RoughsZ.Select(v => v.Rough * 1e-3), false);

            var stiffenersAngleShift = Algebra.ConvertGradToRad(StiffenersAngleShift);

            for (var i = 0; i < X.Length; i++)
            {
                InnerD[i] = dFunc(X[i]);
                OuterD[i] = DFunc(X[i]);
                StiffenersDiameters[i] = stiffenersDiametersFunc(X[i]);
                StiffenersDistances[i] = stiffenersDistancesFunc(X[i]);

                var dsa = 2 * Math.PI / StiffenersCount;

                StiffenersLocationsTotal[i] = new Point[StiffenersCount];

                for (var j = 0; j < StiffenersCount; j++)
                {
                    var z0 = StiffenersDistances[i] * Math.Cos(stiffenersAngleShift + j * dsa);
                    var y0 = StiffenersDistances[i] * Math.Sin(stiffenersAngleShift + j * dsa);

                    StiffenersLocationsTotal[i][j] = new Point(X[i], y0, z0);
                }

                Wy[i] = yRoughSpline(X[i]);
                Wz[i] = zRoughSpline(X[i]);
            }
        }

        public void CalculateBarrelPhysics(
            int xCount,
            ElementSizeInSection[] elementSizeInSections,
            Action<int, string>? observer = null)
        {
            calculateBarrelTotalGeometry(xCount);

            var elementSizesFunc = Algebra.GetFunc(
    elementSizeInSections.Select(v => v.X * 1e-3).ToArray(),
    elementSizeInSections.Select(v => v.ElementSize * 1e-3).ToArray());

            F = new double[X.Length];
            S = new double[X.Length];
            Jy = new double[X.Length];
            Jz = new double[X.Length];
            RadiiDifference = new double[X.Length];
            CoolingCoefficient = new double[X.Length];

            LongitudinalUnitForce = new double[X.Length];
            UnitTorqueY = new double[X.Length];
            UnitTorqueZ = new double[X.Length];

            Meshes = new Mesh[X.Length];
            for (var i = 0; i < X.Length; i++)
            {
                Meshes[i] = new Mesh();
            }

            for (var i = 0; i < X.Length; i++)
            {
                Meshes[i] = calculateMesh(i, elementSizesFunc(X[i]));

                var pcnt = 100.0 * i / (X.Length - 1);
                observer?.Invoke((int)Math.Round(pcnt), $"Построение сетки {pcnt:0.00}%");
            }

            for (var i = 0; i < X.Length; i++)
            {
                var mesh = Meshes[i];

                F[i] = mesh.Elements.Sum(e =>
                {
                    var points = e.Select(pId => mesh.Nodes[pId]).ToArray();

                    return Algebra.GetArea(points.Select(p => p.Z).ToArray(), points.Select(p => p.Y).ToArray());
                });

                S[i] = Math.PI * FastMath.Pow2(InnerD[i] / 2);

                calculateInertias(i);
                calculateRadiiDifference(i);

                var borderLength = mesh.BorderEdgesGroups
                    .Select(b => new { MaxR = b.Max(x => Math.Sqrt(FastMath.Pow2(mesh.Nodes[x.Item1].Y) + FastMath.Pow2(mesh.Nodes[x.Item2].Z))), Border = b })
                    .OrderByDescending(t => t.MaxR)
                    .First()
                    .Border.Sum(x => Algebra.GetDistance(mesh.Nodes[x.Item1], mesh.Nodes[x.Item2]));

                var equalOuterRadius = Math.Sqrt(F[i] / Math.PI + FastMath.Pow2(InnerD[i] / 2));

                CoolingCoefficient[i] = StiffenersType switch
                {
                    StiffenersType.None => 1,
                    _ => borderLength / (2 * Math.PI * equalOuterRadius)
                };

                switch (StressCalculationMethod)
                {
                    case StressCalculationMethod.Physic: calculateForces(i); break;
                    case StressCalculationMethod.Simple:
                    default: calculateForcesSimple(i); break;
                }

                var pcnt = 100.0 * i / (X.Length - 1);
                observer?.Invoke((int)Math.Round(pcnt), $"Расчет физико-геометрических величин {pcnt:0.00}% ...");
            }

            Initialized = true;
        }


        private void calculateRadiiDifference(int i)
        {
            var mesh = Meshes[i];

            if (mesh.BorderEdgesGroups.Count != 2)
            {
                RadiiDifference[i] = 0;
                return;
            }

            RadiiDifference[i] = double.MaxValue;

            for (var g1 = 0; g1 < mesh.BorderEdgesGroups.Count; g1++)
            {
                for (var g2 = g1 + 1; g2 < mesh.BorderEdgesGroups.Count; g2++)
                {
                    for (var j1 = 0; j1 < mesh.BorderEdgesGroups[g1].Count; j1++)
                    {
                        for (var j2 = 0; j2 < mesh.BorderEdgesGroups[g2].Count; j2++)
                        {
                            var (pg11, pg12) = mesh.BorderEdgesGroups[g1][j1];
                            var (pg21, pg22) = mesh.BorderEdgesGroups[g2][j2];

                            var p1 = (mesh.Nodes[pg11] + mesh.Nodes[pg12]) / 2;
                            var p2 = (mesh.Nodes[pg21] + mesh.Nodes[pg22]) / 2;

                            RadiiDifference[i] = Math.Min(Point.GetDistance(p1, p2), RadiiDifference[i]);
                        }
                    }
                }
            }
        }

        private void calculateForces(int i)
        {
            var mesh = Meshes[i];

            LongitudinalUnitForce[i] = 0.0;
            UnitTorqueY[i] = 0.0;
            UnitTorqueZ[i] = 0.0;

            var loads = mesh.BorderEdges.Where(t =>
            {
                var (i1, i2) = t;
                var p1 = mesh.Nodes[i1];
                var p2 = mesh.Nodes[i2];

                var y = (p1.Y + p2.Y) / 2;
                var z = (p1.Z + p2.Z) / 2;

                return Algebra.GetDistance(y, z, 0, 0) <= GroovesDepth * 1e-3 + 1.05 * InnerD[i] / 2;
            })
            .ToDictionary(t => (t.Item1, t.Item2), t => new LoadFunc(true, v => 1.0));


            var converter = new CoordinatesConverter(2, 1, 0);
            var problem = new FlatProblem(converter);

            var solver = new StrainedStateStaticSolver(
                problem,
                Modelling.Material.MaterialProperties.GetBasicMaterial(),
                new Dictionary<int, Func<double, double>>(),
                new Dictionary<int, Func<double, double>>(),
                loads,
                new PressureField(),
                mesh,
                converter);

            solver.Solve();

            foreach (var element in mesh.Elements)
            {
                var points = element.Select(id => mesh.Nodes[id]).ToArray();
                var ys = points.Select(p => p.Y).ToArray();
                var zs = points.Select(p => p.Z).ToArray();

                var area = Algebra.GetArea(ys, zs);
                var sigmaSumma = element.Average(id => problem.StressXX.First()[id] + problem.StressYY.First()[id]);

                LongitudinalUnitForce[i] += sigmaSumma * area;
                UnitTorqueY[i] += sigmaSumma * area * Wy[i];
                UnitTorqueZ[i] += sigmaSumma * area * Wz[i];
            }

        }

        private void calculateForcesSimple(int i)
        {
            var mesh = Meshes[i];

            LongitudinalUnitForce[i] = 0.0;
            UnitTorqueY[i] = 0.0;
            UnitTorqueZ[i] = 0.0;

            var S = X[i] <= CamoraLength
                ? Math.PI * FastMath.Pow2(InnerD[i] / 2)
                : Math.PI * FastMath.Pow2(InnerD[i] / 2) + GroovesWidth * 1e-3 * GroovesDepth * 1e-3 * GroovesCount;

            var r1 = Math.Sqrt(S / Math.PI);
            var r2 = Math.Sqrt((F[i] + S) / Math.PI);

            var sigmaSumma = 2 * FastMath.Pow2(r1) / (FastMath.Pow2(r2) - FastMath.Pow2(r1));

            foreach (var element in mesh.Elements)
            {
                var points = element.Select(id => mesh.Nodes[id]).ToArray();
                var ys = points.Select(p => p.Y).ToArray();
                var zs = points.Select(p => p.Z).ToArray();

                var area = Algebra.GetArea(ys, zs);
                /*var fij = area / points.Length;

                 foreach (var p in points)
                 {
                     var a = Point.GetAngle0_360(p.Z, p.Y, 1, 0);
                     var aGrad = a * 180 / Math.PI;
                     var boardPoint = GetBoardCoordinate(i, a);

                     var r = Math.Sqrt(boardPoint.Y * boardPoint.Y + boardPoint.Z * boardPoint.Z);

                     LongitudinalUnitForce[i] += 2 * FastMath.Pow2(r1) * fij / (FastMath.Pow2(r) - FastMath.Pow2(r1));
                 }*/

                LongitudinalUnitForce[i] += sigmaSumma * area;
                UnitTorqueY[i] += sigmaSumma * area * Wy[i];
                UnitTorqueZ[i] += sigmaSumma * area * Wz[i];
            }
        }

        private void calculateInertias(int i)
        {
            var mesh = Meshes[i];

            Jy[i] = 0;
            Jz[i] = 0;

            foreach (var triangle in mesh.Elements)
            {
                var points = triangle.Select(pId => mesh.Nodes[pId]).ToArray();

                var df = Algebra.GetArea(points.Select(p => p.Y).ToArray(), points.Select(p => p.Z).ToArray());

                foreach (var point in points)
                {
                    Jy[i] += FastMath.Pow2(point.Z + Wz[i]) * df / points.Length; // Jy - при вращении вокруг оси Oz (влево вправо)
                    Jz[i] += FastMath.Pow2(point.Y + Wy[i]) * df / points.Length; // Jz - при вращении вокруг оси Oy (вверх вниз)
                }
            }
        }

        private Mesh calculateMesh(int i, double elementsSize)
        {
            var geometries = new List<IGeometryObject>();

            var r0 = InnerD[i] / 2;
            var r1 = OuterD[i] / 2;

            geometries.Add(new CircleObject(new Point(), r1, Operation.Add));
            geometries.Add(new CircleObject(new Point(), r0, Operation.Subtract));

            for (var j = 0; j < StiffenersCount; j++)
            {
                var px = StiffenersLocationsTotal[i][j].Z;
                var py = StiffenersLocationsTotal[i][j].Y;

                switch (StiffenersType)
                {
                    case StiffenersType.Inner:
                        geometries.Add(new CircleObject(new Point(px, py), StiffenersDiameters[i] / 2, Operation.Subtract));
                        break;
                    case StiffenersType.Outer:
                        geometries.Add(new CircleObject(new Point(px, py), StiffenersDiameters[i] / 2, Operation.Add));
                        break;
                }
            }

            var d = InnerD.Last();

            var groovesLength = d * GroovesSlope;
            var groovesAngle = Algebra.GetValueAtLine(X[i], 0, 0, groovesLength, 2 * Math.PI);

            return Mesh.GenerateGMSH(
                X[i],
                geometries,
                d,
                elementsSize,
                X[i] <= CamoraLength ? 0 : GroovesCount,
                groovesAngle,
                GroovesDepth * 1e-3,
                GroovesWidth * 1e-3
            );
        }

        public Point GetBoardCoordinate(int xIndex, double a)
        {
            var x = X[xIndex];
            var R = OuterD[xIndex] / 2;

            var py = R * Math.Sin(a);
            var pz = R * Math.Cos(a);

            if (StiffenersType == StiffenersType.None || StiffenersLocationsTotal.First().Length == 0)
                return new Point(x, py, pz);

            var rs = StiffenersDiameters[xIndex] / 2;

            var minStiffenersAngle = double.MaxValue;
            var minS = 0;

            for (var s = 0; s < StiffenersLocationsTotal.First().Length; s++)
            {
                var angle = Math.Atan2(StiffenersLocationsTotal[xIndex][s].Y, StiffenersLocationsTotal[xIndex][s].Z);
                var da = Algebra.AnglesDistance(angle, a);

                if (da < minStiffenersAngle)
                {
                    minStiffenersAngle = da;
                    minS = s;
                }
            }

            var ys = StiffenersLocationsTotal[xIndex][minS].Y;
            var zs = StiffenersLocationsTotal[xIndex][minS].Z;

            var intersect = Algebra.CircleLineSegmentIntersection(0, 0, pz, py, zs, ys, rs,
                out var z1, out var y1, out var z2, out var y2);

            if (!intersect)
                return new Point(x, py, pz);

            var d1 = Math.Sqrt(FastMath.Pow2(z1) + FastMath.Pow2(y1));
            var d2 = Math.Sqrt(FastMath.Pow2(z2) + FastMath.Pow2(y2));

            switch (StiffenersType)
            {
                case StiffenersType.Inner when d1 < d2:
                    return d1 < R ? new Point(x, y1, z1) : new Point(x, py, pz);
                case StiffenersType.Inner:
                    return d2 < R ? new Point(x, y2, z2) : new Point(x, py, pz);

                case StiffenersType.Outer when d1 > d2:
                    return d1 > R ? new Point(x, y1, z1) : new Point(x, py, pz);
                case StiffenersType.Outer:
                    return d2 > R ? new Point(x, y2, z2) : new Point(x, py, pz);
                default:
                    return new Point(x, py, pz);
            }
        }


    }
}
