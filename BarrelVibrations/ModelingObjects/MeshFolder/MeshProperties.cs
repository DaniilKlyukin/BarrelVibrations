using System.ComponentModel;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PropertiesTemplates;

namespace BarrelVibrations.ModelingObjects.MeshFolder
{
    [Serializable]
    [DataContract(Name = "Сетка")]
    public class MeshProperties
    {
        private int pointsXCount = 300;

        /// <summary>
        /// Узлов сетки по Ox
        /// </summary>
        [Browsable(true)]
        [Category("Сетка по длине")]
        [Description("")]
        [DisplayName("Узлов сетки по Ox")]
        [DataMember(Name = "Узлов сетки по Ox")]
        public int PointsXCount
        {
            get => pointsXCount;
            set => pointsXCount = Math.Max(10, value);
        }

        private int maxPointsRCount = 40;
        /// <summary>
        /// Максимальное количество узлов сетки по радиусу
        /// </summary>
        [Browsable(true)]
        [Category("Сетка по радиусу")]
        [Description("")]
        [DisplayName("Максимальное количество узлов сетки по радиусу")]
        [DataMember(Name = "Максимальное количество узлов сетки по радиусу")]
        public int MaxPointsRCount
        {
            get => maxPointsRCount;
            set => maxPointsRCount = Math.Max(4, value);
        }

        private double initialRAreaSize = 0.2;
        /// <summary>
        /// Начальная ширина области сетки по радиусу, мм
        /// </summary>
        [Browsable(true)]
        [Category("Сетка по радиусу")]
        [Description("")]
        [DisplayName("Начальная ширина области сетки по радиусу, мм")]
        [DataMember(Name = "Начальная ширина области сетки по радиусу, мм")]
        public double InitialRAreaSize
        {
            get => initialRAreaSize;
            set => initialRAreaSize = Math.Max(0, value);
        }

        private double rGrowSpeed = 1.4;
        /// <summary>
        /// Скорость роста по радиусу
        /// </summary>
        [Browsable(true)]
        [Category("Сетка по радиусу")]
        [Description("")]
        [DisplayName("Скорость роста по радиусу")]
        [DataMember(Name = "Скорость роста по радиусу")]
        public double RGrowSpeed
        {
            get => rGrowSpeed;
            set => rGrowSpeed = Math.Max(1, value);
        }

        /// <summary>
        /// Размер элементов в сечениях
        /// </summary>
        [Browsable(true)]
        [Category("Сетка в сечении")]
        [Description("")]
        [DisplayName("Размер элементов в сечениях")]
        [DataMember(Name = "Размер элементов в сечениях")]
        [TypeConverter(typeof(CountArrayConverter))]
        public ElementSizeInSection[] ElementSizeInSections { get; set; } = new[] 
        {
            new ElementSizeInSection { X = 0, ElementSize = 5 },
            new ElementSizeInSection { X = 10000, ElementSize = 5 } 
        };
    }
}
