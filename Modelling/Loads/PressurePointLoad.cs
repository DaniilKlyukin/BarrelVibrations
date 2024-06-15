using System.ComponentModel;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PointFolder;

namespace Modelling.Loads
{
    public class PressurePointLoad
    {
        /// <summary>
        /// Координата нагружения
        /// </summary>
        [Browsable(true)]
        [Category("Нагружение")]
        [Description("")]
        [DisplayName("Координата нагружения")]
        [DataMember(Name = "Координата нагружения")]
        public Point Point { get; set; }

        /// <summary>
        /// Давление в точке нагружения, Па
        /// </summary>
        [Browsable(true)]
        [Category("Нагружение")]
        [Description("")]
        [DisplayName("Давление в точке нагружения, Па")]
        [DataMember(Name = "Давление в точке нагружения, Па")]
        public double Pressure { get; set; }

        public override string ToString()
        {
            return $"X={Point.X:0.000} м, Y={Point.Y:0.000} м, Z={Point.Z:0.000} м, P={Pressure*1e-6} МПа";
        }
    }
}
