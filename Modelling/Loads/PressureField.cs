using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PropertiesTemplates;

namespace Modelling.Loads
{
    [Serializable]
    [DataContract(Name = "Поле давления")]
    public class PressureField
    {
        /// <summary>
        /// Поля давления в моменты времени
        /// </summary>
        [Browsable(true)]
        [Category("Нагружение")]
        [Description("")]
        [DisplayName("Поля давления в моменты времени")]
        [DataMember(Name = "Поля давления в моменты времени")]
        [TypeConverter(typeof(CountArrayConverter))]
        public List<PressurePointsLoad> PressureLoads { get; } = new();

    }
}
