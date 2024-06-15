using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers
{
    [Serializable]
    [DataContract(Name = "Основные результаты колебаний")]
    public class VibrationsMainResults
    {
        /// <summary>
        /// Амплитуда колебаний по Oy, мкм
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Колебания")]
        [Description("")]
        [DisplayName("Амплитуда колебаний по Oy, мкм")]
        [DataMember(Name = "Амплитуда колебаний по Oy, мкм")]
        public double VibrationsYAmplitude { get; set; }

        /// <summary>
        /// Амплитуда колебаний по Oz, мкм
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Колебания")]
        [Description("")]
        [DisplayName("Амплитуда колебаний по Oz, мкм")]
        [DataMember(Name = "Амплитуда колебаний по Oz, мкм")]
        public double VibrationsZAmplitude { get; set; }

        /// <summary>
        /// Амплитуда колебаний, мкм
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Колебания")]
        [Description("")]
        [DisplayName("Амплитуда колебаний, мкм")]
        [DataMember(Name = "Амплитуда колебаний, мкм")]
        public double VibrationsAmplitude { get; set; }

        /// <summary>
        /// Амплитуда угла, град
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Колебания")]
        [Description("")]
        [DisplayName("Амплитуда угла, град")]
        [DataMember(Name = "Амплитуда угла, град")]
        public double AngleAmplitude { get; set; }
    }
}
