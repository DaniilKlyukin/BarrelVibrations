using BasicLibraryWinForm.PropertiesTemplates;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Identification
{

    [Serializable]
    [DataContract(Name = "Параметры идентификации")]
    public class IdentificationProperties
    {
        public const int CATEGORIES_COUNT = 3;

        /// <summary>
        /// Идентифицируемый параметр
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Параметры", 1, CATEGORIES_COUNT)]
        [DisplayName("Идентифицируемый параметр")]
        [DataMember(Name = "Идентифицируемый параметр")]
        [TypeConverter(typeof(MyEnumConverter))]
        public IdentificationParameterEnum IdentificationParameter { get; set; }

        /// <summary>
        /// Минимальное значение параметра
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Параметры", 1, CATEGORIES_COUNT)]
        [DisplayName("Минимальное значение параметра")]
        [DataMember(Name = "Минимальное значение параметра")]
        public double ParameterMin { get; set; } = 0;

        /// <summary>
        /// Максимальное значение параметра
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Параметры", 1, CATEGORIES_COUNT)]
        [DisplayName("Максимальное значение параметра")]
        [DataMember(Name = "Максимальное значение параметра")]
        public double ParameterMax { get; set; } = 0;

        private double tolerance = 1e-2;
        /// <summary>
        /// Требуемая точность
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Параметры", 1, CATEGORIES_COUNT)]
        [DisplayName("Требуемая точность")]
        [DataMember(Name = "Требуемая точность")]
        public double Tolerance
        {
            get => tolerance;
            set => tolerance = Math.Max(1e-13, value);
        }

        /// <summary>
        /// Целевой параметр идентификации
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Цель", 2, CATEGORIES_COUNT)]
        [DisplayName("Целевой параметр идентификации")]
        [DataMember(Name = "Целевой параметр идентификации")]
        [TypeConverter(typeof(MyEnumConverter))]
        public IdentificationTargetEnum IdentificationTarget { get; set; }
       
        /// <summary>
        /// Требуемое целевое значение
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Цель", 2, CATEGORIES_COUNT)]
        [DisplayName("Требуемое целевое значение")]
        [DataMember(Name = "Требуемое целевое значение")]
        public double ExpectedTarget { get; set; }

        /// <summary>
        /// Найденное значение параметра
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Результаты", 3, CATEGORIES_COUNT)]
        [DisplayName("Найденное значение параметра")]
        [DataMember(Name = "Найденное значение параметра")]
        public double ResultParameter { get; set; }

        /// <summary>
        /// Полученное целевое значение
        /// </summary>
        [Browsable(true)]
        [Description("")]
        [CustomSortedCategory("Результаты", 3, CATEGORIES_COUNT)]
        [DisplayName("Полученное целевое значение")]
        [DataMember(Name = "Полученное целевое значение")]
        public double ResultTarget { get; set; }
    }
}
