using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace InletBallisticLibrary
{
    [DataContract(Name = "Пороховой заряд")]
    [Serializable]
    public class PowderCharge
    {
        /// <summary>
        /// Пороха
        /// </summary>
        [Browsable(true)]
        [Category("Пороха")]
        [Description("")]
        [DisplayName("Список порохов")]
        [DataMember(Name = "Список порохов")]
        [Editor(typeof(FormTypeEditor<PowderChargeForm, List<Powder>>), typeof(UITypeEditor))]
        public List<Powder> Powders { get; set; } = new List<Powder>();

        private double igniterMass = 1e-3;
        /// <summary>
        /// Масса воспламенителя, кг
        /// </summary>
        [Browsable(true)]
        [Category("Характеристики заряда")]
        [Description("")]
        [DisplayName("Масса воспламенителя, кг")]
        [DataMember(Name = "Масса воспламенителя, кг")]
        public double IgniterMass
        {
            get => igniterMass;
            set => igniterMass = Math.Max(1e-9, value);
        }

        private double sleeveMass = 0;
        /// <summary>
        /// Масса гильзы, кг
        /// </summary>
        [Browsable(true)]
        [Category("Гильза")]
        [Description("")]
        [DisplayName("Масса гильзы, кг")]
        [DataMember(Name = "Масса гильзы, кг")]
        public double SleeveMass
        {
            get => sleeveMass;
            set => sleeveMass = Math.Max(0, value);
        }

        private double sleeveDensity = 8730;
        /// <summary>
        /// Плотность гильзы, кг/м³
        /// </summary>
        [Browsable(true)]
        [Category("Гильза")]
        [Description("")]
        [DisplayName("Плотность гильзы, кг/м³")]
        [DataMember(Name = "Плотность гильзы, кг/м³")]
        public double SleeveDensity
        {
            get => sleeveDensity;
            set => sleeveDensity = Math.Max(1, value);
        }

        /// <summary>
        /// Объём гильзы, м³
        /// </summary>
        [Browsable(true)]
        [Category("Гильза")]
        [Description("")]
        [DisplayName("Объём гильзы, м³")]
        [IgnoreDataMember]
        public double SleeveVolume => sleeveMass / sleeveDensity;

        /// <summary>
        /// Количество порохов
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [Category("Характеристики заряда")]
        [Description("")]
        [DisplayName("Количество порохов")]
        public int PowdersCount => Powders.Count;

        /// <summary>
        /// Масса заряда
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [Category("Характеристики заряда")]
        [Description("")]
        [DisplayName("Масса заряда")]
        public double Mass => IgniterMass + Powders.Sum(v => v.w);

        [Browsable(false)]
        [IgnoreDataMember]
        public bool Initialized =>
            Powders.Any() &&
            Powders.All(p =>
                double.IsFinite(p.Kappa) &&
                double.IsFinite(p.Lambda) &&
                double.IsFinite(p.Mu) &&
                double.IsFinite(p.PowderVolume) &&
                double.IsFinite(p.PowderArea) &&
                double.IsFinite(p.Teta) && p.Teta > 0 &&
                double.IsFinite(p.Q) &&
                double.IsFinite(p.Prandtl) &&
                double.IsFinite(p.w) && p.w > 0 &&
                double.IsFinite(p.R) && p.R > 0);

        public override string ToString()
        {
            return $"Пороховой заряд, {Mass:0.000} кг";
        }
    }
}
