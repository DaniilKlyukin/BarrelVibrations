using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers
{
    [Serializable]
    [DataContract(Name = "Основные результаты внешней баллистики")]
    public class InletBallisticMainResults
    {
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Давление")]
        [Description("")]
        [DisplayName("Максимальное давление на канал ствола, МПа")]
        [DataMember(Name = "Максимальное давление на канал ствола, МПа")]
        public double PknMax { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Давление")]
        [Description("")]
        [DisplayName("Максимальное давление на снаряд, МПа")]
        [DataMember(Name = "Максимальное давление на снаряд, МПа")]
        public double PsnMax { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Скорость")]
        [Description("")]
        [DisplayName("Дульная скорость снаряда, м/с")]
        [DataMember(Name = "Дульная скорость снаряда, м/с")]
        public double Vmax { get; set; }
    }
}
