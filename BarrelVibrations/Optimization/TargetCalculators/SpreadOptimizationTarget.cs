using BarrelVibrations.Solvers.OutletBallisticFolder;
using BarrelVibrations.Solvers;
using BasicLibraryWinForm;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public class SpreadOptimizationTarget : OptimizationTargetCalculator
    {
        /// <summary>
        ///     Расстояние до цели, м
        /// </summary>
        [Browsable(true)]
        [Category("Параметры")]
        [Description("")]
        [DisplayName("Расстояние до цели, м")]
        [DataMember(Name = "Расстояние до цели, м")]
        public double Distance { get; set; }

        public override OptimizationTarget OptimizationTarget => OptimizationTarget.ProjectilesSpread;

        public SpreadOptimizationTarget(double distance = 1000)
        {
            this.Distance = distance;
        }

        public override double Calculate(MainSolver mainSolver)
        {
            var hitpoints = OutletBallisticSolver.CalculateLineFireHitpoints(
                mainSolver.OutletBallistic,
                Distance,
                Algebra.ConvertGradToRad(mainSolver.ModelProperties.FiringAngle));

            var zs = hitpoints.Select(p => p.Z).ToArray();
            var ys = hitpoints.Select(p => p.Y).ToArray();

            var spread = OutletBallisticSolver.CalculatePlaneSpread(zs, ys, out var spreadCenterZ, out var spreadCenterY);

            return spread;
        }

        public override string GetTargetText()
        {
            return "Разброс, м";
        }

        public override string ToString()
        {
            return "Разброс";
        }
    }
}
