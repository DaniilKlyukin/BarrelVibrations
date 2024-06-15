using System.ComponentModel;
using System.Runtime.Serialization;

namespace InletBallisticLibrary
{
    [Serializable]
    [DataContract(Name = "Тип пороха")]
    public enum PowderType
    {
        [Description("Трубчатый")]
        Tube,
        [Description("Зерненый")]
        Grained
    }
}
