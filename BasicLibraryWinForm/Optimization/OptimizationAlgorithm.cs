using System.ComponentModel;
using System.Runtime.Serialization;

namespace BasicLibraryWinForm.Optimization
{
    [Serializable]
    [DataContract(Name = "Алгоритм оптимизации")]
    public enum OptimizationAlgorithm
    {
        [Description("Нелдера-Мида")]
        NelderMead,
        [Description("Хука-Дживса")]
        HookeJeeves,
        [Description("Случайный поиск")]
        RandomDescend,
    }
}
