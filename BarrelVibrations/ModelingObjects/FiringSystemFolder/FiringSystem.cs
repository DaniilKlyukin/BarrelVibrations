using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BarrelVibrations.ModelingObjects.FiringSystemFolder
{
    [DataContract(Name = "Боевая система")]
    [Serializable]
    public class FiringSystem
    {
        /// <summary>
        /// Расстояние от дна канала до земли, м
        /// </summary>
        [Browsable(true)]
        [Category("Установка")]
        [Description("Расстояние от дна канала до земли, м")]
        [DisplayName("Расстояние от дна канала до земли, м")]
        [DataMember(Name = "Расстояние от дна канала до земли, м")]
        public double GroundDistance { get; set; }

        /// <summary>
        /// Масса всей установки, кг
        /// </summary>
        [Browsable(true)]
        [Category("Установка")]
        [Description("Общая масса артиллерийской установки")]
        [DisplayName("Масса установки, кг")]
        [DataMember(Name = "Масса установки, кг")]
        public double FullSystemMass { get; set; }

        /// <summary>
        /// Коэффициент трения всей установки
        /// </summary>
        [Browsable(true)]
        [Category("Установка")]
        [Description("Коэффициент трения между установкой и землей, от 0 до 1")]
        [DisplayName("Коэффициент трения установки")]
        [DataMember(Name = "Коэффициент трения установки")]
        public double FullSystemFrictionCoefficient { get; set; }

        /// <summary>
        /// Учитывать движение установки?
        /// </summary>
        [Browsable(true)]
        [Category("Установка")]
        [Description("")]
        [DisplayName("Учитывать движение установки?")]
        [DataMember(Name = "Учитывать движение установки?")]
        public bool IsFullSystemMoving { get; set; }

        /// <summary>
        /// Коэффициент упругости откатных частей, КН/м
        /// </summary>
        [Browsable(true)]
        [Category("Откатные части")]
        [Description("")]
        [DisplayName("Коэффициент упругости откатных частей, КН/м")]
        [DataMember(Name = "Коэффициент упругости откатных частей, КН/м")]
        public double MovingPartsStiffness { get; set; }

        /// <summary>
        /// Коэффициент демпфирования откатных частей, кг/с
        /// </summary>
        [Browsable(true)]
        [Category("Откатные части")]
        [Description("")]
        [DisplayName("Коэффициент демпфирования откатных частей, кг/с")]
        [DataMember(Name = "Коэффициент демпфирования откатных частей, кг/с")]
        public double MovingPartsDamping { get; set; }

        /// <summary>
        /// Учитывать откат ствола?
        /// </summary>
        [Browsable(true)]
        [Category("Откатные части")]
        [Description("")]
        [DisplayName("Учитывать откат ствола?")]
        [DataMember(Name = "Учитывать откат ствола?")]
        public bool IsMovingBarrel { get; set; }

        /// <summary>
        /// Коэффициент демпфирования колебаний ствола
        /// </summary>
        [Browsable(true)]
        [Category("Колебания")]
        [Description("")]
        [DisplayName("Коэффициент демпфирования колебаний ствола")]
        [DataMember(Name = "Коэффициент демпфирования колебаний ствола")]
        public double VibrationDampingCoefficient { get; set; }

        /// <summary>
        /// Известные данные отката ствола, м
        /// </summary>
        [Browsable(true)]
        [Category("Экспериментальные данные")]
        [Description("Известные данные отката ствола, м")]
        [DisplayName("Известные данные отката ствола, м")]
        [DataMember(Name = "Известные данные отката ствола, м")]
        public List<BarrelMovement> BarrelMovements { get; set; } = new List<BarrelMovement>();
    }
}
