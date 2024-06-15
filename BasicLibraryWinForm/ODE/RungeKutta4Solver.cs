namespace BasicLibraryWinForm.ODE
{
    public class RungeKutta4Solver : OdeSolver
    {
        protected override bool SolveInnerStep(double dt)
        {
            OdeSolution der = (CurrentSolution - LastSolution) / dt;

            der = EquationsSystem(CurrentTime, CurrentSolution, der);
            var k1 = der * dt;

            der = EquationsSystem(CurrentTime + 0.5 * dt, 0.5 * k1 + CurrentSolution, der);
            var k2 = der * dt;

            der = EquationsSystem(CurrentTime + 0.5 * dt, 0.5 * k2 + CurrentSolution, der);
            var k3 = der * dt;

            der = EquationsSystem(CurrentTime + dt, k3 + CurrentSolution, der);
            var k4 = der * dt;

            for (var i = 0; i < CurrentSolution.Length; i++)
            {
                CurrentSolution[i] += (k1[i] + 2 * k2[i] + 2 * k3[i] + k4[i]) / 6.0;
            }

            return false;
        }
    }
}
