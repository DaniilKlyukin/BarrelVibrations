namespace BarrelVibrations.Solvers
{

    public partial class SolverIdentifier
    {
        public abstract class ParametricSolverCloner
        {
            public abstract MainSolver GеtClone(MainSolver solver, double parameterValue);
        }
    }
}
