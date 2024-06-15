using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.FiringSystemFolder
{
    public class BarrelMovement
    {
        [DisplayName("Момент времени, сек")]
        [DataMember(Name = "Момент времени, сек")]
        public double Time { get; set; }

        [DisplayName("Перемещение, м")]
        [DataMember(Name = "Перемещение, м")]
        public double Displacement { get; set; }

        public override string ToString()
        {
            return $"t={Time} с; u={Displacement} м";
        }
    }
}
