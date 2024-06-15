using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.BarrelFolder
{
    [DataContract(Name = "Область закрепления")]
    public class FixationArea
    {
        [DisplayName("От")]
        [DataMember(Name = "От")]
        public double From { get; set; }

        [DisplayName("До")]
        [DataMember(Name = "До")]
        public double To { get; set; }

        public override string ToString()
        {
            return $"Закрепление {From} м - {To} м";
        }
    }
}
