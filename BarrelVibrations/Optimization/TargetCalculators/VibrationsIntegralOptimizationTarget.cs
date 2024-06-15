using BarrelVibrations.Solvers;
using BarrelVibrations.Solvers.Solutions;
using BasicLibraryWinForm;
using System.Runtime.Serialization;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public class VibrationsIntegralOptimizationTarget : OptimizationTargetCalculator
    {
        public override OptimizationTarget OptimizationTarget => OptimizationTarget.VibrationsIntegral;

        public override double Calculate(MainSolver mainSolver)
        {
            if (!mainSolver.Vibrations.BarrelEndYs.Any() || !mainSolver.Vibrations.BarrelEndZs.Any())
                return 0;

            var y0 = mainSolver.Vibrations.BarrelEndYs.FirstOrDefault();
            var z0 = mainSolver.Vibrations.BarrelEndZs.FirstOrDefault();

            var dyz = new double[mainSolver.Vibrations.BarrelEndYs.Count];

            for (var i = 0; i < mainSolver.Vibrations.BarrelEndYs.Count; i++)
            {
                var vibrationA = Math.Sqrt(FastMath.Pow2(mainSolver.Vibrations.BarrelEndYs[i] - y0) + FastMath.Pow2(mainSolver.Vibrations.BarrelEndZs[i] - z0));

                dyz[i] = vibrationA * 1e6;
            }

            return Algebra.GetIntegralTrapezoid(mainSolver.TimeMoments.ToArray(), dyz) / mainSolver.TimeMoments.Last();
        }

        public override string GetTargetText()
        {
            return "Средняя интегральная амплитуда колебаний, мкм";
        }

        public override string ToString()
        {
            return "Средняя интегральная амплитуда колебаний";
        }
    }
}
