using BasicLibraryWinForm;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace InletBallisticLibrary
{
    public class GrainedPowder : Powder
    {
        private int channelsCount = 7;
        /// <summary>
        ///     Количество каналов
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Количество каналов")]
        [DataMember(Name = "Количество каналов")]
        public int ChannelsCount
        {
            get => channelsCount;
            set => channelsCount = Math.Max(1, value);
        }

        private double dInner = 1e-3;
        /// <summary>
        ///     Внутренний диаметр каналов, м
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Внутренний диаметр каналов, м")]
        [DataMember(Name = "Внутренний диаметр каналов, м")]
        public double DInner
        {
            get => dInner;
            set => dInner = Math.Max(1e-9, value);
        }

        private double dOuter = 1e-3;
        /// <summary>
        ///     Внешний диаметр элемента, м
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Внешний диаметр элемента, м")]
        [DataMember(Name = "Внешний диаметр элемента, м")]
        public double DOuter
        {
            get => dOuter;
            set => dOuter = Math.Max(1e-9, value);
        }

        private double length = 1e-3;
        /// <summary>
        ///     Длина элемента, м
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Длина элемента, м")]
        [DataMember(Name = "Длина элемента, м")]
        public double Length
        {
            get => length;
            set => length = Math.Max(1e-9, value);
        }

        public override double PowderVolume => Math.PI * Length * (FastMath.Pow2(DOuter) - ChannelsCount * FastMath.Pow2(DInner)) / 4;

        public override double PowderArea => Math.PI * (Length * DOuter + 2 * FastMath.Pow2(DOuter) / 4 + ChannelsCount * (Length * DInner - 2 * FastMath.Pow2(DInner) / 4));

        public override double Kappa => (Q_ + 2 * P) * Beta / Q_;

        public override double Lambda => 2 * (3 - P) * Beta / (Q_ + 2 * P);

        public override double Mu => -6 * FastMath.Pow2(Beta) / (Q_ + 2 * P);

        private double Beta => _2e1 / Length;

        private double P => (DOuter + ChannelsCount * DInner) / Length;
        private double Q_ => (FastMath.Pow2(DOuter) - ChannelsCount * FastMath.Pow2(DInner)) / FastMath.Pow2(Length);

        public override PowderType Type => PowderType.Grained;

        public GrainedPowder()
        {

        }

        [JsonConstructor]
        public GrainedPowder(
            string name,
            double dInner,
            double dOuter,
            double l,
            int channelsCount,
            double u1,
            double f,
            double cp,
            double cv,
            double alpha,
            double powderDensity,
            double _2e1,
            double w,
            double heatConductivity,
            double viscosity)
        {
            Name = name;
            DInner = dInner;
            DOuter = dOuter;
            Length = l;
            ChannelsCount = channelsCount;
            this.u1 = u1;
            this.f = f;
            Cp = cp;
            Cv = cv;
            Alpha = alpha;
            PowderDensity = powderDensity;
            this._2e1 = _2e1;
            this.w = w;
            HeatConductivity = heatConductivity;
            Viscosity = viscosity;
        }

        public override Powder GetCopy()
        {
            return new GrainedPowder(
                Name,
                DInner,
                DOuter,
                Length,
                ChannelsCount,
                u1,
                f,
                Cp,
                Cv,
                Alpha,
                PowderDensity,
                _2e1,
                w,
                HeatConductivity,
                Viscosity);
        }
    }
}
