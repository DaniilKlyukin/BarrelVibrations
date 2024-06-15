namespace BasicLibraryWinForm.ODE
{
    public class EulerSolver : OdeSolver
    {
        protected override bool SolveInnerStep(double dt)
        {
            OdeSolution der = (CurrentSolution - LastSolution) / dt;

            var k1 = EquationsSystem(CurrentTime, CurrentSolution, der) * dt;

            for (var i = 0; i < CurrentSolution.Length; i++)
            {
                CurrentSolution[i] += k1[i];
            }

            return false;
        }
    }
}
