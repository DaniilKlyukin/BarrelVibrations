namespace BarrelVibrations.Solvers
{

    public partial class SolverIdentifier
    {
        public class PressureTarget : TargetCalculator
        {
            public override double GеtTargerValue(MainSolver solver)
            {
                return solver.InletBallistic.Pkn.Max() * 1e-6;
            }
        }
    }
}
