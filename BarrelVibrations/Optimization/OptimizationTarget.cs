using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Optimization
{
    [Serializable]
    [DataContract(Name = "Целевая переменная оптимизации")]
    public enum OptimizationTarget
    {
        [Description("Амплитуда колебаний")]
        VibrationsAmplitude,
        [Description("Амплитуда колебаний при выстрелах")]
        ShotsVibrationsSpread,
        [Description("Взешенный критерий")]
        WeightedCriterion,
        [Description("Взешенный критерий при выстрелах")]
        ShotsWeightedCriterion,
        [Description("Разброс снарядов")]
        ProjectilesSpread,
        [Description("Интегральное значение колебаний")]
        VibrationsIntegral
    }
}
