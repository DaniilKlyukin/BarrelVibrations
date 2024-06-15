namespace BasicLibraryWinForm.ODE
{
    public class RungeKutta6Solver : OdeSolver
    {
        static double[] alpha =
        {
            1.0/6,
            4.0/15,
            2.0 / 3,
            5.0/6,
            1.0,
            1.0/15,
            1.0
        };
        static double[][] beta =
        {
            new []{ 1.0/6 },
            new []{ 4.0/75, 16.0/75 },
            new []{ 5.0/6, -8.0/3, 5.0/2 },
            new []{ -165.0/64, 55.0/6, -425.0/64, 85.0/96 },
            new []{ 12.0/5, -8.0, 4015.0/612, -11.0/36, 88.0/255 },
            new []{ -8263.0/15000, 124.0/75, -643.0/680, -81.0/250, 2484.0/10625, 0 },
            new []{ 3501.0/1720, -300.0/43, 297275.0/52632, -319.0/2322, 24068.0/84065,0,3850.0/26703 },
        };

        static double[] gamma =
        {
            13.0 / 160,
            0,
            2375.0 / 5984,
            5.0 / 16,
            12.0 / 85,
            3.0 / 44,
            0,
            0
        };

        protected override bool SolveInnerStep(double dt)
        {
            OdeSolution der = (CurrentSolution - LastSolution) / dt;

            var ks = new OdeSolution[alpha.Length + 1];

            ks[0] = EquationsSystem(CurrentTime, CurrentSolution, der);

            for (int i = 1; i < ks.Length; i++)
            {
                var sum = new OdeSolution(new double[CurrentSolution.Length]);

                for (int j = 0; j < i; j++)
                {
                    sum += beta[i - 1][j] * ks[j] * dt;
                }

                ks[i] = EquationsSystem(CurrentTime + alpha[i - 1] * dt, CurrentSolution + sum, ks[i - 1]);
            }

            for (int j = 0; j < ks.Length; j++)
            {
                CurrentSolution += gamma[j] * ks[j] * dt;
            }

            return false;
        }
    }
}
