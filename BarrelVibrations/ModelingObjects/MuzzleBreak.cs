using BasicLibraryWinForm;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects
{
    [DataContract(Name = "Дульный тормоз")]
    [Serializable]
    public class MuzzleBreak
    {
        private double mass = 0;
        /// <summary>
        /// Масса дульного тормоза, кг
        /// </summary>
        [Browsable(true)]
        [Category("Параметры")]
        [Description("")]
        [DisplayName("Масса дульного тормоза, кг")]
        [DataMember(Name = "Масса дульного тормоза, кг")]
        public double Mass
        {
            get => mass;
            set => mass = Math.Max(0, value);
        }

        private double efficiency = 0;
        /// <summary>
        /// Коэффициент эффективности дульного тормоза, от 0 до 1
        /// </summary>
        [Browsable(true)]
        [Category("Параметры")]
        [Description("от 0 до 1")]
        [DisplayName("Коэффициент эффективности дульного тормоза")]
        [DataMember(Name = "Коэффициент эффективности дульного тормоза")]
        public double Efficiency
        {
            get => efficiency;
            set => efficiency = FastMath.MinMax(value, 0, 1);
        }
    }
}
