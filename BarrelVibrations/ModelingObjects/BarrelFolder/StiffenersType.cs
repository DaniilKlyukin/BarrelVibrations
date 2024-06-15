using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.BarrelFolder
{
    [Serializable]
    [DataContract(Name = "Тип ребер жесткости")]
    public enum StiffenersType
    {
        [Description("Без ребер жесткости")]
        None,
        [Description("Вырезы внутрь")]
        Inner,
        [Description("Внешние ребра")]
        Outer
    }
}
