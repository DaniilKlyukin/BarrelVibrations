using BarrelVibrations.Solvers;
using BasicLibraryWinForm;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public class WeightedOptimizationTarget : OptimizationTargetCalculator
    {
        /// <summary>
        ///     Вес амплитуды угла
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("Угол дульного среза измеряются в градусах")]
        [DisplayName("Вес амплитуды угла")]
        [DataMember(Name = "Вес амплитуды угла")]
        public double AngleWeight { get; set; } = 150;

        /// <summary>
        ///     Вес скорости дульного среза
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("Скорость дульного среза измеряются в м/с")]
        [DisplayName("Вес амплитуды скорости дульного среза")]
        [DataMember(Name = "Вес амплитуды скорости дульного среза")]
        public double VelocityWeight { get; set; } = 40;

        /// <summary>
        ///     Вес колебаний
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("Колебания измеряются в мкм")]
        [DisplayName("Вес амплитуды колебаний")]
        [DataMember(Name = "Вес амплитуды колебаний")]
        public double VibrationsWeight { get; set; } = 0.01;

        /// <summary>
        ///     Вес прогиба
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("Прогиб измеряется в мм")]
        [DisplayName("Вес прогиба")]
        [DataMember(Name = "Вес прогиба")]
        public double DeflectionWeight { get; set; } = 1;

        public override OptimizationTarget OptimizationTarget => OptimizationTarget.WeightedCriterion;

        public WeightedOptimizationTarget(
            double angleWeight = 150,
            double vibrationsWeight = 0.01,
            double velocityWeight = 40)
        {
            this.AngleWeight = angleWeight;
            this.VibrationsWeight = vibrationsWeight;
            VelocityWeight = velocityWeight;
        }

        public override double Calculate(MainSolver mainSolver)
        {
            var result = mainSolver.GetVibrationsMainResults();

            var velocity = new List<double>();

            for (var i = 0; i < mainSolver.Vibrations.BarrelEndVelocityXs.Count; i++)
            {
                var vx = mainSolver.Vibrations.BarrelEndVelocityXs[i];
                var vy = mainSolver.Vibrations.BarrelEndVelocityYs[i];
                var vz = mainSolver.Vibrations.BarrelEndVelocityZs[i];

                velocity.Add(Math.Sqrt(FastMath.Pow2(vx) + FastMath.Pow2(vy) + FastMath.Pow2(vz)));
            }

            if (!mainSolver.Vibrations.BarrelEndVelocityXs.Any())
                return Math.Abs(DeflectionWeight * mainSolver.Deflection.DeflectionY.LastOrDefault());

            return AngleWeight * result.AngleAmplitude
                + VibrationsWeight * result.VibrationsAmplitude
                + VelocityWeight * (velocity.Max() - velocity.Min())
                + Math.Abs(DeflectionWeight * mainSolver.Deflection.DeflectionY.LastOrDefault() * 1e3);
        }

        public override string GetTargetText()
        {
            return "Взвешенный критерий";
        }

        public override string ToString()
        {
            return "Взвешенный критерий";
        }
    }
}
