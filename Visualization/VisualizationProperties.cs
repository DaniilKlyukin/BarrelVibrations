using System.ComponentModel;
using System.Runtime.Serialization;
using ScottPlot;
using BasicLibraryWinForm.PropertiesTemplates;
using BasicLibraryWinForm;
using Point = BasicLibraryWinForm.PointFolder.Point;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.Drawing.Design;
using BasicLibraryWinForm.PointFolder;

namespace Visualization
{
    [Serializable]
    [DataContract(Name = "Настройки визуализации")]
    public class VisualizationProperties
    {
        public VisualizationProperties()
        {
            ExpandableSectionsConverter.Objects = new List<IndexValue>();
            SelectedSection = ExpandableSectionsConverter.Objects.FirstOrDefault();
        }

        public void SetSections(double[] sectionsX)
        {
            ExpandableSectionsConverter.Objects = sectionsX.Select((v, i) => new IndexValue(i, v)).ToList();
            SelectedSection = ExpandableSectionsConverter.Objects.FirstOrDefault();
        }

        [Browsable(true)]
        [Category("Графики")]
        [Description("Отображать выстрелы")]
        [DisplayName("Отображать выстрелы")]
        [DataMember(Name = "Отображать выстрелы")]
        public bool PlotShots { get; set; }

        [Browsable(true)]
        [Category("Графики")]
        [Description("Отображать легенду")]
        [DisplayName("Отображать легенду")]
        [DataMember(Name = "Отображать легенду")]
        public bool PlotLegend { get; set; } = true;

        [Browsable(true)]
        [Category("Графики")]
        [Description("Маркеры в известных точках")]
        [DisplayName("Маркеры в точках")]
        [DataMember(Name = "Маркеры в точках")]
        public MarkerShape PlotMarkers { get; set; } = MarkerShape.none;

        [Browsable(true)]
        [Category("Графики")]
        [Description("")]
        [DisplayName("Ширина линий графика")]
        [DataMember(Name = "Ширина линий графика")]
        public float PlotLineWidth { get; set; } = 1;

        [Browsable(true)]
        [Category("Основные параметры")]
        [Description("")]
        [DisplayName("Отрисовка")]
        public bool Draw { get; set; } = true;

        [Browsable(true)]
        [Category("Параметры цвета")]
        [Description("")]
        [DisplayName("Цвет ствола")]
        public Color BarrelBasicColor { get; set; } = Color.Gray;

        [Browsable(true)]
        [Category("Параметры цвета")]
        [Description("")]
        [DisplayName("Цвет снаряда")]
        public Color MissileBasicColor { get; set; } = Color.FromArgb(127, 114, 117);

        [Browsable(true)]
        [Category("Параметры цвета")]
        [Description("")]
        [DisplayName("Цвет пороха")]
        public Color PowderBasicColor { get; set; } = Color.FromArgb(117, 96, 89);

        [Browsable(true)]
        [Category("Параметры цвета")]
        [Description("")]
        [DisplayName("Цвет гильзы")]
        public Color SleeveBasicColor { get; set; } = Color.FromArgb(146, 122, 55);

        [Browsable(true)]
        [Category("Параметры цвета")]
        [Description("")]
        [DisplayName("Использовать цвет тепловой шкалы")]
        public bool UseHeatBarColor { get; set; } = false;

        [Browsable(true)]
        [Category("Параметры мыши")]
        [Description("")]
        [DisplayName("Скорость перемещения")]
        public float MoveSpeed { get; set; } = 0.02f;

        [Browsable(true)]
        [Category("Параметры мыши")]
        [Description("")]
        [DisplayName("Скорость вращения")]
        public float RotateSpeed { get; set; } = 0.065f;

        [Browsable(true)]
        [Category("Параметры мыши")]
        [Description("")]
        [DisplayName("Скорость приближения")]
        public float ScaleSpeed { get; set; } = 4f;

        [Browsable(true)]
        [Category("Параметры координатной сетки")]
        [Description("Размер ячейки координатной сетки, м")]
        [DisplayName("Размер ячейки, м")]
        public double CellSize { get; set; } = 0.1;

        [Browsable(true)]
        [Category("Параметры координатной сетки")]
        [Description("Размер координатной области, м")]
        [DisplayName("Размер координатной области, м")]
        public double GridSize { get; set; } = 5;

        [Browsable(true)]
        [Category("Параметры координатной сетки")]
        [Description("Координатные оси Ox, Oy, Oz.")]
        [DisplayName("Отображать оси")]
        public bool ShowAxis { get; set; } = true;

        [Browsable(true)]
        [Category("Параметры координатной сетки")]
        [Description("")]
        [DisplayName("Отображать координатную сетку")]
        public bool ShowGrid { get; set; } = true;

        [Browsable(true)]
        [Category("Параметры координатной сетки")]
        [Description("")]
        [DisplayName("Центр координатной сетки")]
        [Editor(typeof(FormTypeEditor<PointFormEditor, Point>), typeof(UITypeEditor))]
        public Point GridCenter { get; set; } = new Point();

        [Browsable(true)]
        [Category("Параметры координатной сетки")]
        [Description("")]
        [DisplayName("Плоскость координатной сетки")]
        [TypeConverter(typeof(MyEnumConverter))]
        public CoordinateGridSurfaceType CoordinateGridSurfaceType { get; set; } = CoordinateGridSurfaceType.Auto;


        [Browsable(true)]
        [Category("Параметры отображения сетки")]
        [Description("При выборе типа \"Ствол\" отображается поверхность объекта (граница);\nПри выборе типа \"Сечения\" отображается конечно-элементная сетка в поперечных сечениях.")]
        [DisplayName("Способ отображения объекта")]
        [TypeConverter(typeof(MyEnumConverter))]
        public GeometryVisualizationType VisualizationType { get; set; } = GeometryVisualizationType.Barrel;

        [Browsable(true)]
        [Category("Параметры отображения сетки")]
        [Description("")]
        [DisplayName("Отображение напряжений в сечении")]
        [TypeConverter(typeof(MyEnumConverter))]
        public SectionStressType SectionStressType { get; set; } = SectionStressType.None;

        /// <summary>
        /// Выбранное сечение
        /// </summary>
        [Browsable(true)]
        [Category("Параметры отображения сетки")]
        [Description("")]
        [DisplayName("Выбранное сечение")]
        [DataMember(Name = "Выбранное сечение")]
        [TypeConverter(typeof(ExpandableSectionsConverter))]
        public IndexValue? SelectedSection { get; set; } = ExpandableSectionsConverter.Objects.FirstOrDefault();


        private float _lineWidth = 2f;
        [Browsable(true)]
        [Category("Параметры отображения сетки")]
        [Description("2 по умолчанию")]
        [DisplayName("Толщина расчетной сетки")]
        public float SurfacesEdges
        {
            get => _lineWidth;
            set
            {
                if (value < 0)
                    _lineWidth = 1;
                else
                {
                    _lineWidth = value;
                }
            }
        }

        [Browsable(true)]
        [Category("Параметры отображения сетки")]
        [Description("")]
        [DisplayName("Отображать границы поверхностей")]
        public bool ShowSurfacesEdges { get; set; } = true;


        private int fps = 30;
        [Browsable(true)]
        [Category("Параметры визуализации")]
        [Description("Сколько кадров необходимо отрисовывать для визуализации")]
        [DisplayName("Кадров в секунду")]
        public int FPS
        {
            get => fps;
            set => fps = value is < 1 or > 1000 ? 30 : value;
        }

        [Browsable(true)]
        [Category("Параметры визуализации")]
        [Description("")]
        [DisplayName("Отображение в разрезе")]
        public bool SliceDraw { get; set; } = false;

        [Browsable(true)]
        [Category("Параметры визуализации")]
        [Description("")]
        [DisplayName("Разрез от, град")]
        public double SliceMinAngle { get; set; } = 180;

        [Browsable(true)]
        [Category("Параметры визуализации")]
        [Description("")]
        [DisplayName("Разрез до, град")]
        public double SliceMaxAngle { get; set; } = 360;

        [Browsable(true)]
        [Category("Параметры визуализации")]
        [Description("")]
        [DisplayName("Сегментов по углу")]
        public int AngleSegments { get; set; } = 32;

        [Browsable(true)]
        [Category("Параметры отображения результатов")]
        [Description("Отображние 2D визуализации со сглаженными значениями")]
        [DisplayName("Сглаживание")]
        public bool SmoothDrawing { get; set; } = true;

        [Browsable(true)]
        [Category("Параметры отображения результатов")]
        [Description("Множитель перемещений узлов при отрисовке на экран")]
        [DisplayName("Множитель перемещений узлов")]
        public float DisplacementScale { get; set; } = 1;


    }
}
