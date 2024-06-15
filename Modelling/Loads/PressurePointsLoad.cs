using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PropertiesTemplates;

namespace Modelling.Loads
{
    [Serializable]
    [DataContract(Name = "Нагружение в точках")]
    public class PressurePointsLoad
    {
        /// <summary>
        /// Момент времени, сек
        /// </summary>
        [Browsable(true)]
        [Category("Нагружение")]
        [Description("")]
        [DisplayName("Момент времени, сек")]
        [DataMember(Name = "Момент времени, сек")]
        public double TimeMoment { get; set; }

        /// <summary>
        /// Точки нагружения и значения
        /// </summary>
        [Browsable(true)]
        [Category("Нагружение")]
        [Description("")]
        [DisplayName("Точки нагружения и значения")]
        [DataMember(Name = "Точки нагружения и значения")]
        [TypeConverter(typeof(CountArrayConverter))]
        public List<PressurePointLoad> Loads { get; set; } = new();


        public override string ToString()
        {
            return $"t={TimeMoment*1e3:0.00}, {Loads.Count} точек";
        }
    }
}
