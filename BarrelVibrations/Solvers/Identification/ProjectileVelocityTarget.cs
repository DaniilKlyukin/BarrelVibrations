namespace BarrelVibrations.Solvers
{

    public partial class SolverIdentifier
    {
        public class ProjectileVelocityTarget : TargetCalculator
        {
            public override double GеtTargerValue(MainSolver solver)
            {
                return solver.ShotsParameters.FirstOrDefault()?.ProjectileSpeed ?? 0.0;
            }
        }
    }
}
