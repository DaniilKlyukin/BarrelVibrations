using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Identification
{
    [Serializable]
    [DataContract(Name = "Идентифицируемый параметр")]
    public enum IdentificationParameterEnum
    {
        [Description("Скорость горения, мм³/(Н·с)")]
        BurnSpeed,
        [Description("Сила пороха,  КДж")]
        PowderPower,
    }
}
