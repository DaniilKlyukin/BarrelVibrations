using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.BarrelFolder
{
    [Serializable]
    [DataContract(Name = "Сечение ствола")]
    public class BarrelSection
    {
        /// <summary>
        /// Координата x сечения ствола, м
        /// </summary>
        [Browsable(true)]
        [Category("Основные")]
        [Description("")]
        [DisplayName("Координата x сечения ствола, мм")]
        [DataMember(Name = "Координата x сечения ствола, мм")]
        public double X { get; set; }

        private double d_inner = 1;
        /// <summary>
        /// Внутренний диаметр d, мм
        /// </summary>
        [Browsable(true)]
        [Category("Основные")]
        [Description("")]
        [DisplayName("Внутренний диаметр d, мм")]
        [DataMember(Name = "Внутренний диаметр d, мм")]
        public double dInner
        {
            get => d_inner;
            set => d_inner = Math.Max(1, value);
        }

        private double D_outer = 1;
        /// <summary>
        /// Внешний диаметр D, мм
        /// </summary>
        [Browsable(true)]
        [Category("Основные")]
        [Description("")]
        [DisplayName("Внешний диаметр D, мм")]
        [DataMember(Name = "Внешний диаметр D, мм")]
        public double DOuter
        {
            get => D_outer;
            set => D_outer = Math.Max(1, value);
        }

        private double stiffenersDiameter;
        /// <summary>
        /// Диаметр ребер жесткости, мм
        /// </summary>
        [Browsable(true)]
        [Category("Ребра жесткости")]
        [Description("")]
        [DisplayName("Диаметр ребер жесткости, мм")]
        [DataMember(Name = "Диаметр ребер жесткости, мм")]
        public double StiffenersDiameter
        {
            get => stiffenersDiameter;
            set => stiffenersDiameter = Math.Max(0, value);
        }

        private double stiffenersDistance;
        /// <summary>
        /// Расстояние от центра канала до центра ребер жесткости, мм
        /// </summary>
        [Browsable(true)]
        [Category("Ребра жесткости")]
        [Description("")]
        [DisplayName("Расстояние от центра канала до центра ребер жесткости, мм")]
        [DataMember(Name = "Расстояние от центра канала до центра ребер жесткости, мм")]
        public double StiffenersDistance
        {
            get => stiffenersDistance;
            set => stiffenersDistance = Math.Max(0, value);
        }

        public override string ToString()
        {
            return $"x: {X} мм, d: {d_inner} мм, D: {D_outer} мм";
        }
    }
}
