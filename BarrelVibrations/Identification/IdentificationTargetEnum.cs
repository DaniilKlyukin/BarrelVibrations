using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Identification
{
    [Serializable]
    [DataContract(Name = "Целевой параметр идентификации")]
    public enum IdentificationTargetEnum
    {
        [Description("Максимальное давление, МПа")]
        MaxPressure,
        [Description("Скорость снаряда,  м/с")]
        ProjectileVelocity,
    }
}
