#pragma warning disable CS0105 // Директива Using уже использовалась в этом пространстве имен
#pragma warning restore CS0105 // Директива Using уже использовалась в этом пространстве имен

namespace BasicLibraryWinForm.ODE
{
    public class AdamsMulton5Solver : OdeSolver
    {
        private Queue<OdeSolution> solutions = new Queue<OdeSolution>();
        private Queue<double> times = new Queue<double>();

        double[] coeffs = new[] { 251.0 / 720, 646.0 / 720, -264.0 / 720, 106.0 / 720, -19.0 / 720 };

        protected override bool SolveInnerStep(double dt)
        {
            var ys = solutions.ToArray();
            OdeSolution der = (CurrentSolution - LastSolution) / dt;

            der = EquationsSystem(CurrentTime, CurrentSolution, der);
            var k1 = der * dt;

            der = EquationsSystem(CurrentTime + 0.5 * dt, 0.5 * k1 + CurrentSolution, der);
            var k2 = der * dt;

            der = EquationsSystem(CurrentTime + 0.5 * dt, 0.5 * k2 + CurrentSolution, der);
            var k3 = der * dt;

            der = EquationsSystem(CurrentTime + dt, k3 + CurrentSolution, der);
            var k4 = der * dt;

            var y_next = new OdeSolution(CurrentSolution.Values);

            for (var i = 0; i < y_next.Length; i++)
            {
                y_next[i] = CurrentSolution[i] + (k1[i] + 2 * k2[i] + 2 * k3[i] + k4[i]) / 6.0;
            }

            solutions.Enqueue(new OdeSolution(y_next.Values.Copy()));
            times.Enqueue(CurrentTime + dt);

            if (ys.Length == 6)
            {
                solutions.Dequeue();
                times.Dequeue();
            }

            if (ys.Length != 5)
            {
                CurrentSolution = y_next;
            }
            else
            {
                var ts = times.ToArray();

                for (int s = 0; s < ys.Length; s++)
                {
                    der = EquationsSystem(ts[s], ys[s], der);
                    CurrentSolution += dt * der * coeffs[^(s + 1)];
                }
            }

            return false;
        }
    }
}
