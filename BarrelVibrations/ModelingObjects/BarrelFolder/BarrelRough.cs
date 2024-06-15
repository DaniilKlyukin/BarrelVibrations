using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.BarrelFolder
{
    public class BarrelRough
    {
        /// <summary>
        /// Координата X, мм
        /// </summary>
        [DisplayName("Координата X, мм")]
        [DataMember(Name = "Координата X, мм")]
        public double X { get; set; }

        /// <summary>
        /// Неровность, мм
        /// </summary>
        [DisplayName("Неровность, мм")]
        [DataMember(Name = "Неровность, мм")]
        public double Rough { get; set; }

        public override string ToString()
        {
            return $"X={X} мм; Δ={Rough} мм";
        }
    }
}
