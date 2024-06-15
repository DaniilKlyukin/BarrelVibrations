using System.ComponentModel;

namespace Visualization
{
    public class VisualizationMenuProperties
    {
        [Browsable(true)]
        [Category("Основные параметры")]
        [Description("Если < 1, то анимация замедляется, если > 1, то анимация ускоряется в заданное число раз.")]
        [DisplayName("Скорость воспроизведения")]
        public double VideoAcceleration { get; set; } = 0.00025;

        [Browsable(true)]
        [Category("Основные параметры")]
        [Description("Позволяет зафиксировать шкалу значений для анимации и графиков.")]
        [DisplayName("Фиксированные пределы")]
        public bool FixedLimits { get; set; } = false;

        private double minValue;
        [Browsable(true)]
        [Category("Основные параметры")]
        [Description("Позволяет зафиксировать шкалу значений для анимации и графиков.")]
        [DisplayName("Нижняя граница значений")]
        public double MinValue
        {
            get => minValue;
            set
            {
                if (value < maxValue)
                    minValue = value;
            }
        }

        private double maxValue = 1.0;
        [Browsable(true)]
        [Category("Основные параметры")]
        [Description("Позволяет зафиксировать шкалу значений для анимации и графиков.")]
        [DisplayName("Верхняя граница значений")]
        public double MaxValue
        {
            get => maxValue;
            set
            {
                if (value > minValue)
                    maxValue = value;
            }
        }
        public VisualizationMenuProperties()
        {

        }
    }
}
