using BasicLibraryWinForm;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.MaterialFolder
{
    [Serializable]
    [DataContract(Name = "Таблица параметров материала")]
    public class MaterialTable : ICloneable
    {
        /// <summary>
        /// Список параметров материала
        /// </summary>
        [DataMember(Name = "Список параметров материала")]
        public List<MaterialData> MaterialDatas { get; private set; } = new();

        /// <summary>
        /// Функция плотности от температуры
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> Density { get; private set; } = _ => 7850;

        /// <summary>
        /// Функция теплоемкости от температуры
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> HeatCapacity { get; private set; } = _ => 567;

        /// <summary>
        /// Функция теплопроводности от температуры
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> HeatConductivity { get; private set; } = _ => 32;

        /// <summary>
        /// Функция коэффициента линейного теплового расширения от температуры
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> LinearThermalExpansion { get; private set; } = _ => 1.25e-5;

        /// <summary>
        /// Функция коэффициента Пуассона от температуры
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> PoissonRatio { get; private set; } = _ => 0.3;

        /// <summary>
        /// Функция модуля Юнга от температуры
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> YoungModulus { get; private set; } = _ => 200e9;

        public void UpdateFunctions()
        {
            var ordered = MaterialDatas.OrderBy(v => v.Temperature).ToArray();

            Density = Algebra.GetFunc(
                ordered.Select(v => v.Temperature).ToArray(),
                ordered.Select(v => v.Density).ToArray(),
                false);

            HeatCapacity = Algebra.GetFunc(
                ordered.Select(v => v.Temperature).ToArray(),
                ordered.Select(v => v.HeatCapacity).ToArray(),
                false);

            HeatConductivity = Algebra.GetFunc(
                ordered.Select(v => v.Temperature).ToArray(),
                ordered.Select(v => v.HeatConductivity).ToArray(),
                false);

            LinearThermalExpansion = Algebra.GetFunc(
                ordered.Select(v => v.Temperature).ToArray(),
                ordered.Select(v => v.LinearThermalExpansion).ToArray(),
                false);

            PoissonRatio = Algebra.GetFunc(
                ordered.Select(v => v.Temperature).ToArray(),
                ordered.Select(v => v.PoissonRatio).ToArray(),
                false);

            YoungModulus = Algebra.GetFunc(
                ordered.Select(v => v.Temperature).ToArray(),
                ordered.Select(v => v.YoungModulus).ToArray(),
                false);
        }

        public override string ToString()
        {
            return $"Параметры материала, {MaterialDatas.Count} значений";
        }

        public object Clone()
        {
            var table = new MaterialTable
            {
                MaterialDatas = MaterialDatas.Select(x => x.Clone()).Cast<MaterialData>().ToList()
            };

            table.UpdateFunctions();
            return table;
        }
    }
}
