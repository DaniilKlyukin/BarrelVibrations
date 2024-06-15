using BasicLibraryWinForm;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace InletBallisticLibrary
{
    [Serializable]
    [DataContract(Name = "Трубчатый порох")]
    public class TubePowder : Powder
    {
        private double dInner = 1e-3;
        /// <summary>
        ///     Внутренний диаметр элемента, м
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Внутренний диаметр элемента, м")]
        [DataMember(Name = "Внутренний диаметр элемента, м")]
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

        public override double PowderVolume => Math.PI * Length * (FastMath.Pow2(DOuter) - FastMath.Pow2(DInner)) / 4;

        public override double PowderArea => Math.PI * (Length * DOuter + 2 * FastMath.Pow2(DOuter) / 4 + Length * DInner - 2 * FastMath.Pow2(DInner) / 4);

        public override double Kappa => 1 + Beta;

        public override double Lambda => -Beta / (1 + Beta);

        public override double Mu => 0;

        private double Beta => (DOuter - DInner) / (2 * Length);

        public override PowderType Type => PowderType.Tube;

        public TubePowder()
        {

        }

        [JsonConstructor]
        public TubePowder(
            string name,
            double dInner,
            double dOuter,
            double l,
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
            return new TubePowder(
                Name,
                DInner,
                DOuter,
                Length,
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
