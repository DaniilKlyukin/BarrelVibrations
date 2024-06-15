using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Modelling.Material
{
    [Serializable]
    [DataContract(Name = "Свойства материала")]
    [JsonObject(MemberSerialization.OptIn)]
    public class MaterialProperties
    {
        public static MaterialProperties GetBasicMaterial()
        {
            return new MaterialProperties(
                "Сталь",
                new MaterialProperty("Плотность, кг/м³", new[] { 300.0 }, new[] { 7900.0 }),
                new MaterialProperty("Теплоемкость, Дж/(Кг·К)", new[] { 300.0 }, new[] { 567.0 }),
                new MaterialProperty("Теплопроводность, Вт/(м·К)", new[] { 300.0 }, new[] { 32.0 }),
                new MaterialProperty("Модуль Юнга, ГПа", new[] { 300.0 }, new[] { 200.0 }),
                new MaterialProperty("Коэффициент Пуассона", new[] { 300.0 }, new[] { 0.3 }),
                new MaterialProperty("Коэффициент линейного теплового расширения, К^−1", new[] { 300.0 }, new[] { 12.6e-6 }));
        }


        [JsonConstructor]
        public MaterialProperties(string materialName,
            MaterialProperty density,
            MaterialProperty heatCapacity,
            MaterialProperty heatConductivity,
            MaterialProperty youngModulus,
            MaterialProperty poissonRatio,
            MaterialProperty linearThermalExpansion)
        {
            MaterialName = materialName;
            Density = density;
            HeatCapacity = heatCapacity;
            HeatConductivity = heatConductivity;
            YoungModulus = youngModulus;
            PoissonRatio = poissonRatio;
            LinearThermalExpansion = linearThermalExpansion;
        }

        [DataMember] public string MaterialName { get; }
        /// ρ, кг/м^3
        [DataMember] public MaterialProperty Density { get; set; }
        /// <summary>
        /// c, Дж/(Кг·К)
        /// </summary>
        [DataMember] public MaterialProperty HeatCapacity { get; set; }
        /// <summary>
        /// λ, Вт/(м·К)
        /// </summary>
        [DataMember] public MaterialProperty HeatConductivity { get; set; }
        /// <summary>
        /// E, ГПа
        /// </summary>
        [DataMember] public MaterialProperty YoungModulus { get; set; }
        /// <summary>
        /// nu
        /// </summary>
        [DataMember] public MaterialProperty PoissonRatio { get; set; }
        /// <summary>
        /// alpha
        /// </summary>
        [DataMember] public MaterialProperty LinearThermalExpansion { get; set; }

        public override string ToString()
        {
            return MaterialName;
        }
    }
}
