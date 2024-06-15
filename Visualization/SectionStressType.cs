using System.ComponentModel;

namespace Visualization
{
    public enum SectionStressType
    {
        [Description("Нет")]
        None,
        [Description("Радиальные напряжения")]
        SigmaRR,
        [Description("Тангенциальные напряжения")]
        SigmaTT,
        [Description("Продольные напряжения")]
        SigmaXX
    }
}