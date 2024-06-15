using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.BarrelFolder
{
    [Serializable]
    [DataContract(Name = "Способ расчета напряжений")]
    public enum StressCalculationMethod
    {
        [Description("Упрощенный")]
        Simple,
        [Description("Физический")]
        Physic
    }
}
