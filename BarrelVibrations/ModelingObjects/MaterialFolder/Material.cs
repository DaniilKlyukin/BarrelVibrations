using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PropertiesTemplates;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;

namespace BarrelVibrations.ModelingObjects.MaterialFolder
{
    [Serializable]
    [DataContract(Name = "Материал")]
    public class Material : ICloneable
    {
        private const ushort CATEGORIESCOUNT = 3;

        /// <summary>
        /// Название материала
        /// </summary>
        [CustomSortedCategory("Название", 1, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Название материала")]
        [DataMember(Name = "Название материала")]
        public string Name { get; set; } = "";

        [CustomSortedCategory("Таблица параметров материала", 2, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Таблица параметров материала")]
        [DataMember(Name = "Таблица параметров материала")]
        [Editor(typeof(FormTypeEditor<MaterialTableForm, MaterialTable>), typeof(UITypeEditor))]
        public MaterialTable MaterialTable { get; set; } = new();

        /// <summary>
        /// Плотность, кг/м³
        /// </summary>
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры материала", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Плотность, кг/м³")]
        public double Density => MaterialTable.Density(300);

        /// <summary>
        /// Теплоемкость, Дж/(Кг·К)
        /// </summary>
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры материала", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Теплоемкость, Дж/(Кг·К)")]
        public double HeatCapacity => MaterialTable.HeatCapacity(300);

        /// <summary>
        /// Теплопроводность, Вт/(м·К)
        /// </summary>
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры материала", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Теплопроводность, Вт/(м·К)")]
        public double HeatConductivity => MaterialTable.HeatConductivity(300);

        /// <summary>
        /// Модуль Юнга, Па
        /// </summary>
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры материала", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Модуль Юнга, Па")]
        public double YoungModulus => MaterialTable.YoungModulus(300);

        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры материала", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Коэффициент Пуассона")]
        public double PoissonRatio => MaterialTable.PoissonRatio(300);

        /// <summary>
        /// Коэффициент линейного теплового расширения, 1/К
        /// </summary>
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры материала", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Коэффициент линейного теплового расширения, 1/К")]
        public double LinearThermalExpansion => MaterialTable.LinearThermalExpansion(300);

        public object Clone()
        {
            var clone = new Material
            {
                Name = new string(Name),
                MaterialTable = (MaterialTable)MaterialTable.Clone()
            };

            return clone;
        }
    }
}
