using BarrelVibrations.PropertyGridClasses.MissileFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using GmshNet;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.MissileFolder
{
    using occ = Gmsh.Model.Occ;
    using Point = BasicLibraryWinForm.PointFolder.Point;

    [Serializable]
    [DataContract(Name = "Снаряд")]
    public class Missile : ICloneable
    {
        public Missile()
        {

        }

        public Missile(
            double bodyLength,
            double headLength,
            double diameter,
            double mass,
            MissileRegressions missileRegressions)
        {
            BodyLength = bodyLength;
            HeadLength = headLength;
            Diameter = diameter;
            Mass = mass;
            Regressions = missileRegressions;
        }

        [JsonConstructor]
        public Missile(
            double bodyLength,
            double headLength,
            double diameter,
            double mass,
            double massAfterShot,
            bool isSubcaliber,
            Point massCenter,
            double[,] inertiaMatrix,
            MissileRegressions missileRegressions)
        {
            BodyLength = bodyLength;
            HeadLength = headLength;
            Diameter = diameter;
            Mass = mass;
            MassAfterShot = massAfterShot;
            IsSubcaliber = isSubcaliber;
            MassCenter = massCenter;
            Regressions = missileRegressions;

            for (var i = 0; i < InertiaMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < InertiaMatrix.GetLength(1); j++)
                {
                    InertiaMatrix[i, j] = inertiaMatrix[i, j];
                }
            }

            Initialized = true;
        }

        public void InitializeMissileProperties(double meshSize = 0)
        {
            if (meshSize <= 0)
                meshSize = Diameter / 10;

            Gmsh.Initialize();
            Gmsh.Option.SetNumber("General.Terminal", 1);
            Gmsh.Model.Add("m1");


            var points = new int[4];

            points[0] = occ.AddPoint(0, 0, 0, meshSize);
            points[1] = occ.AddPoint(0, Diameter / 2, 0, meshSize);
            points[2] = occ.AddPoint(BodyLength, Diameter / 2, 0, meshSize);
            points[3] = occ.AddPoint(BodyLength + HeadLength, 0, 0, meshSize);

            var lines = new int[4];
            lines[0] = occ.AddLine(points[0], points[1]);
            lines[1] = occ.AddLine(points[1], points[2]);
            lines[2] = occ.AddLine(points[2], points[3]);
            lines[3] = occ.AddLine(points[3], points[0]);

            var loop = occ.AddCurveLoop(lines);

            var surface = occ.AddPlaneSurface(new[] { loop });
            occ.Revolve(new[] { (2, surface) }, 0, 0, 0, 1, 0, 0, 2 * Math.PI);

            occ.Synchronize();

            var entities = Gmsh.Model.GetEntities(0);
            Gmsh.Model.Mesh.SetSize(entities, meshSize);

            Gmsh.Model.Mesh.Generate(3);
            Gmsh.Model.Mesh.SetOrder(2);

            var (_, missileTag) = Gmsh.Model.GetEntities(3).First();

            occ.GetCenterOfMass(3, missileTag, out var x, out var y, out var z);
            MassCenter = new Point(x, y, z);

            var volume = occ.GetMass(3, missileTag);
            var rho = Mass / volume;
            var inertia = occ.GetMatrixOfInertia(3, missileTag);

            for (int i = 0; i < InertiaMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < InertiaMatrix.GetLength(1); j++)
                {
                    InertiaMatrix[i, j] = rho * inertia[i * InertiaMatrix.GetLength(0) + j];
                }
            }

            var dx = XDeviation * 1e-3;
            var dy = YDeviation * 1e-3;
            var dz = ZDeviation * 1e-3;

            InertiaMatrix[0, 0] += Mass * (FastMath.Pow2(dy) + FastMath.Pow2(dz));
            InertiaMatrix[1, 1] += Mass * (FastMath.Pow2(dx) + FastMath.Pow2(dz));
            InertiaMatrix[2, 2] += Mass * (FastMath.Pow2(dx) + FastMath.Pow2(dy));

            //Gmsh.Write("missile.msh");
            Gmsh.Finalize();

            Initialized = true;
        }

        public object Clone()
        {
            return new Missile(
                BodyLength, HeadLength, Diameter, Mass, MassAfterShot, IsSubcaliber, new Point(MassCenter.X, MassCenter.Y, MassCenter.Z),
                InertiaMatrix.Copy(), (MissileRegressions)Regressions.Clone());
        }

        /// <summary>
        /// Название снаряда
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Название", 1, 5)]
        [Description("")]
        [DisplayName("Название снаряда")]
        [DataMember(Name = "Название снаряда")]
        public string Name { get; set; } = "Снаряд";

        private double _mass = 1;

        /// <summary>
        /// Масса снаряда, кг
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Масса снаряда, кг")]
        [DataMember(Name = "Масса снаряда, кг")]
        public double Mass
        {
            get => _mass;
            set => _mass = Math.Max(1e-6, value);
        }

        private double _massAfterShot = 1;
        /// <summary>
        /// Масса снаряда после вылета, кг
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Масса снаряда после вылета, кг")]
        [DataMember(Name = "Масса снаряда после вылета, кг")]
        public double MassAfterShot
        {
            get => _massAfterShot;
            set => _massAfterShot = Math.Max(1e-6, value);
        }

        private double _bodyLength = 0.2;

        /// <summary>
        /// Длина цилиндрической части снаряда, м
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Длина цилиндрической части снаряда, м")]
        [DataMember(Name = "Длина цилиндрической части снаряда, м")]
        public double BodyLength
        {
            get => _bodyLength;
            set => _bodyLength = Math.Max(1e-6, value);
        }

        private double _headLength = 0.1;
        /// <summary>
        /// Длина головной части снаряда, м
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Длина головной части снаряда, м")]
        [DataMember(Name = "Длина головной части снаряда, м")]
        public double HeadLength
        {
            get => _headLength;
            set => _headLength = Math.Max(1e-6, value);
        }

        private double _diameter = 0.05;
        /// <summary>
        /// Диаметр снаряда, м
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Диаметр снаряда, м")]
        [DataMember(Name = "Диаметр снаряда, м")]
        public double Diameter
        {
            get => _diameter;
            set => _diameter = Math.Max(1e-6, value);
        }

        /// <summary>
        /// Подкалиберный снаряд
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Подкалиберный снаряд")]
        [DataMember(Name = "Подкалиберный снаряд")]
        public bool IsSubcaliber { get; set; }

        /// <summary>
        /// Отклонение центра масс по Ox, мм
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Отклонение центра масс по Ox, мм")]
        [DataMember(Name = "Отклонение центра масс по Ox, мм")]
        public double XDeviation { get; set; }

        /// <summary>
        /// Отклонение центра масс по Oy, мм
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Отклонение центра масс по Oy, мм")]
        [DataMember(Name = "Отклонение центра масс по Oy, мм")]
        public double YDeviation { get; set; }

        /// <summary>
        /// Отклонение центра масс по Oz, мм
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры", 2, 5)]
        [Description("")]
        [DisplayName("Отклонение центра масс по Oz, мм")]
        [DataMember(Name = "Отклонение центра масс по Oz, мм")]
        public double ZDeviation { get; set; }


        /// <summary>
        /// Коэффициенты регрессии
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Коэффициенты", 3, 5)]
        [Description("")]
        [DisplayName("Коэффициенты регрессии")]
        [DataMember(Name = "Коэффициенты регрессии")]
        [Editor(typeof(FormTypeEditor<MissileRegressionsForm, MissileRegressions>), typeof(UITypeEditor))]
        public MissileRegressions Regressions { get; set; } = new MissileRegressions();

        /// <summary>
        /// Коэффициент ix аэродинамической формы снаряда
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Коэффициенты", 3, 5)]
        [Description("")]
        [DisplayName("Коэффициент ix аэродинамической формы снаряда")]
        [DataMember(Name = "Коэффициент ix аэродинамической формы снаряда")]
        public double ix { get; set; } = 1;

        /// <summary>
        /// iz Коэффициент согласования бокового отклонения снаряда
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Коэффициенты", 3, 5)]
        [Description("")]
        [DisplayName("Коэффициент iz согласования бокового отклонения снаряда")]
        [DataMember(Name = "Коэффициент iz согласования бокового отклонения снаряда")]
        public double iz { get; set; } = 1;

        /// <summary>
        /// Координаты центра масс снаряда от дна
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [CustomSortedCategory("Характеристики", 4, 5)]
        [Description("Для идеального снаряда")]
        [DisplayName("Координаты центра масс снаряда, м")]
        [DataMember(Name = "Координаты центра масс снаряда, м")]
        public Point MassCenter { get; set; } = new Point(0, 0, 0);

        /// <summary>
        /// Длина снаряда, м
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [CustomSortedCategory("Характеристики", 4, 5)]
        [Description("")]
        [DisplayName("Длина снаряда, м")]
        public double Length => BodyLength + HeadLength;


        /// <summary>
        /// Площадь Миделевого сечения снаряда, м²
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [CustomSortedCategory("Характеристики", 4, 5)]
        [Description("")]
        [DisplayName("Площадь Миделевого сечения снаряда, м²")]
        public double SectionArea => Algebra.GetCircleArea(Diameter / 2);

        /// <summary>
        /// Матрица моментов инерции, кг∙м²
        /// </summary>        
        [Browsable(false)]
        [DataMember(Name = "Матрица моментов инерции, кг∙м²")]
        public double[,] InertiaMatrix { get; } = new double[3, 3];

        /// <summary>
        /// Момент инерции по Ox, кг∙м²
        /// </summary>   
        [Browsable(true)]
        [IgnoreDataMember]
        [CustomSortedCategory("Характеристики", 4, 5)]
        [Description("")]
        [DisplayName("Момент инерции по Ox")]
        public double Ix => InertiaMatrix[0, 0];

        /// <summary>
        /// Момент инерции по Oy, кг∙м²
        /// </summary>   
        [Browsable(true)]
        [IgnoreDataMember]
        [CustomSortedCategory("Характеристики", 4, 5)]
        [Description("")]
        [DisplayName("Момент инерции по Oy")]
        public double Iy => InertiaMatrix[1, 1];

        /// <summary>
        /// Момент инерции по Oz, кг∙м²
        /// </summary>   
        [Browsable(true)]
        [IgnoreDataMember]
        [CustomSortedCategory("Характеристики", 4, 5)]
        [Description("")]
        [DisplayName("Момент инерции по Oz")]
        public double Iz => InertiaMatrix[2, 2];

        /// <summary>
        /// Инициализированы ли характеристики снаряда
        /// </summary>
        [IgnoreDataMember]
        [Browsable(true)]
        [CustomSortedCategory("Инициализация", 5, 5)]
        [Description("")]
        [DisplayName("Инициализированы характеристики снаряда")]
        public bool Initialized { get; private set; }

        /// <summary>
        /// Качество сетки (чем больше, тем выше качество)
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Инициализация", 5, 5)]
        [Description("Чем больше, тем выше качество")]
        [DisplayName("Качество сетки")]
        [DataMember(Name = "Качество сетки")]
        public int MeshQuality { get; set; } = 10;
    }
}
