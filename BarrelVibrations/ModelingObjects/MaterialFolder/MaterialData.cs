using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.MaterialFolder
{
    [Serializable]
    [DataContract(Name = "Параметры материала")]
    public class MaterialData : ICloneable
    {
        /// <summary>
        /// Температура, К
        /// </summary>
        [DataMember(Name = "Температура, К")]
        public double Temperature { get; set; } = 300;

        /// <summary>
        /// Плотность, кг/м³
        /// </summary>
        [DataMember(Name = "Плотность, кг/м³")]
        public double Density { get; set; } = 7850;

        /// <summary>
        /// Теплоемкость, Дж/(Кг·К)
        /// </summary>
        [DataMember(Name = "Теплоемкость, Дж/(Кг·К)")]
        public double HeatCapacity { get; set; } = 567;

        /// <summary>
        /// Теплопроводность, Вт/(м·К)
        /// </summary>
        [DataMember(Name = "Теплопроводность, Вт/(м·К)")]
        public double HeatConductivity { get; set; } = 32;

        /// <summary>
        /// Коэффициент линейного теплового расширения, 1/К
        /// </summary>
        [DataMember(Name = "Коэффициент линейного теплового расширения, 1/К")]
        public double LinearThermalExpansion { get; set; } = 1.25e-5;

        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        [DataMember(Name = "Коэффициент Пуассона")]
        public double PoissonRatio { get; set; } = 0.3;

        /// <summary>
        /// Модуль Юнга, Па
        /// </summary>
        [DataMember(Name = "Модуль Юнга, Па")]
        public double YoungModulus { get; set; } = 200e9;

        [JsonConstructor]
        public MaterialData(
            double temperature,
            double density,
            double heatCapacity,
            double heatConductivity,
            double linearThermalExpansion,
            double poissonRatio,
            double youngModulus)
        {
            Temperature = temperature;
            Density = density;
            HeatCapacity = heatCapacity;
            HeatConductivity = heatConductivity;
            LinearThermalExpansion = linearThermalExpansion;
            PoissonRatio = poissonRatio;
            YoungModulus = youngModulus;
        }

        public object Clone()
        {
            return new MaterialData(Temperature, Density, HeatCapacity, HeatConductivity, LinearThermalExpansion, PoissonRatio, YoungModulus);
        }
    }
}
