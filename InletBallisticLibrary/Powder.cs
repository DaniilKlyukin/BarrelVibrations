using System.ComponentModel;
using System.Runtime.Serialization;

namespace InletBallisticLibrary
{
    [Serializable]
    [DataContract(Name = "Порох")]
    public abstract class Powder
    {

        public abstract Powder GetCopy();

        [Browsable(true)]
        [Category("Название")]
        [Description("")]
        [DisplayName("Название")]
        [DataMember(Name = "Название")]
        public string Name { get; set; }

        /// <summary>
        ///     Единичная скорость горения пороха, м^3/(Н * с)
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Единичная скорость горения, м³/(Н·с)")]
        [DataMember(Name = "Единичная скорость горения, м³/(Н·с)")]
        public double u1 { get; set; }

        /// <summary>
        ///     Объём порохового элемента, м^3
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Объём порохового элемента, м³")]
        public abstract double PowderVolume { get; }

        /// <summary>
        ///     Площадь горения порохового элемента, м^2
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Площадь горения порохового элемента, м²")]
        [DataMember(Name = "Площадь горения порохового элемента, м²")]
        public abstract double PowderArea { get; }

        /// <summary>
        ///     Сила пороха, Дж/кг
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Сила пороха, Дж/кг")]
        [DataMember(Name = "Сила пороха, Дж/кг")]
        public double f { get; set; }

        /// <summary>
        ///     Параметр Каппа
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Параметр каппа")]
        public abstract double Kappa { get; }


        /// <summary>
        ///     Параметр Лямда
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Параметр лямбда")]
        public abstract double Lambda { get; }

        /// <summary>
        ///     Параметр Мю
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Параметр мю")]
        public abstract double Mu { get; }

        /// <summary>
        ///     Теплоёмкость ПГП припостоянном давлении, Дж/(кг·К)
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Теплоёмкость ПГП при постоянном давлении, Дж/(кг·К)")]
        [DataMember(Name = "Теплоёмкость ПГП при постоянном давлении, Дж/(кг·К)")]
        public double Cp { get; set; }

        /// <summary>
        ///     Теплоёмкость ПГП припостоянном объёме, Дж/(кг·К)
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Теплоёмкость ПГП при постоянном объёме, Дж/(кг·К)")]
        [DataMember(Name = "Теплоёмкость ПГП при постоянном объёме, Дж/(кг·К)")]
        public double Cv { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Тетта")]
        public double Teta => Cp / Cv - 1.0;

        /// <summary>
        ///     Коволюм
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Коволюм, м³/кг")]
        [DataMember(Name = "Коволюм, м³/кг")]
        public double Alpha { get; set; }

        /// <summary>
        ///     Плотность пороха, кг/м^3
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Плотность пороха, кг/м³")]
        [DataMember(Name = "Плотность пороха, кг/м³")]
        public double PowderDensity { get; set; }

        /// <summary>
        ///     Толщина горящего свода, м
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Толщина горящего свода, м")]
        [DataMember(Name = "Толщина горящего свода, м")]
        public double _2e1 { get; set; }

        /// <summary>
        ///     Массса пороха, кг
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Масса пороха, кг")]
        [DataMember(Name = "Масса пороха, кг")]
        public double w { get; set; }

        /// <summary>
        ///     Теплопроводность, Вт/(м·К)
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Теплопроводность, Вт/(м·К)")]
        [DataMember(Name = "Теплопроводность, Вт/(м·К)")]
        public double HeatConductivity { get; set; }

        /// <summary>
        ///    Вязкость, кг/(м·с)
        /// </summary>
        [Browsable(true)]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Вязкость, кг/(м·с)")]
        [DataMember(Name = "Вязкость, кг/(м·с)")]
        public double Viscosity { get; set; }

        /// <summary>
        ///     Удельная газовая постоянная ПГП
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Удельная газовая постоянная ПГП")]
        public double R => Cp - Cv;

        /// <summary>
        ///     Теплотворная способность, Дж
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Теплотворная способность")]
        public double Q => f / Teta;

        /// <summary>
        ///     Пси распада
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Пси распада")]
        public double Psi_beforeDecay => Kappa * (1 + Lambda + Mu);

        /// <summary>
        ///     Число Прандтля
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [Category("Параметры пороха")]
        [Description("")]
        [DisplayName("Число Прандтля")]
        public double Prandtl => Cp * Viscosity / HeatConductivity;

        /// <summary>
        ///     Тип пороха
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Тип пороха")]
        public abstract PowderType Type { get; }

        public double GetSigma(double z, double psi)
        {
            if (psi < 0 || psi >= 1)
                return 0;

            if (psi <= Psi_beforeDecay)
                return 1.0 + 2.0 * Lambda * z + 3.0 * Mu * z * z;

            if (psi <= 1)
                return (1 + 2 * Lambda + 3 * Mu) * Math.Sqrt((1.0 - psi) / (1.0 - Psi_beforeDecay));

            return 0;
        }

        public double GetBurnSpeed(double boostPressure, double p)
        {
            var u23 = u1 * Math.Pow(2 * boostPressure, 1.0 / 3);
            var u13 = u23 * Math.Pow(boostPressure, 1.0 / 3);

            if (p <= boostPressure)
                return u13 * Math.Pow(p, 1.0 / 3);
            if (p <= 2 * boostPressure)
                return u23 * Math.Pow(p, 2.0 / 3);

            return u1 * p;
        }

        public override string ToString()
        {
            return $"Порох {Name}; m={w} кг";
        }
    }
}