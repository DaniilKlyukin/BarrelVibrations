using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.MeshFolder
{
    [Serializable]
    [DataContract(Name = "Размер элементов в сечении")]
    public class ElementSizeInSection
    {
        /// <summary>
        /// Координата сечения, м
        /// </summary>
        [Browsable(true)]
        [Category("Сетка в сечении")]
        [Description("")]
        [DisplayName("Координата сечения, мм")]
        [DataMember(Name = "Координата сечения, мм")]
        public double X { get; set; }

        private double elementSize = 2;

        /// <summary>
        /// Размер элементов, мм
        /// </summary>
        [Browsable(true)]
        [Category("Сетка в сечении")]
        [Description("")]
        [DisplayName("Размер элементов, мм")]
        [DataMember(Name = "Размер элементов, мм")]
        public double ElementSize
        {
            get => elementSize;
            set => elementSize = Math.Max(1e-3, value);
        }

        public override string ToString()
        {
            return $"X: {X} мм; Размер: {ElementSize} мм";
        }
    }
}
