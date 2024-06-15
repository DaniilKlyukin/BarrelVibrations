using BarrelVibrations.Optimization.TargetCalculators;

namespace BarrelVibrations.Optimization
{
    public interface IBarrelOptimizer
    {
        public OptimizationTargetCalculator OptimizationTargetCalculator { get; set; }
    }
}
