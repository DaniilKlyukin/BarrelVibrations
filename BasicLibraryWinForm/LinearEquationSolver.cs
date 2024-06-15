using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Solvers;

namespace BasicLibraryWinForm
{
    public static class LinearEquationSolver
    {
        //ax=b
        public static double[] SolveDiagonalSystem(double[,] a, double[] b, int diagonalsCount = 0)
        {
            if (b.Length != a.GetLength(0))
            {
                Logger.Log($"Число строк матрицы не равно размеру вектора решений {a.GetLength(0)}!={b.Length}");

                return new double[b.Length];
            }

            if (a.GetLength(0) != a.GetLength(1))
            {
                Logger.Log($"Матрица не квадратная {a.GetLength(0)}!={a.GetLength(1)}");
                return new double[b.Length];
            }

            diagonalsCount = diagonalsCount == 0 ? GetDiagonalsCount(a) : diagonalsCount;

            var x = new double[b.Length];
            var steps = (diagonalsCount - 1) / 2;

            for (var i = 0; i < x.Length; i++)
            {
                var limit = Math.Min(i + diagonalsCount - steps, a.GetLength(1));
                for (var s = 1; s <= steps; s++)
                {
                    if (i + s >= x.Length)
                        break;

                    var coef = -a[i + s, i] / a[i, i]; //TODO зачем sign abs

                    for (var j = i; j < limit; j++)
                        a[i + s, j] += coef * a[i, j];

                    b[i + s] += coef * b[i];
                }
            }

            var n = x.Length - 1;
            x[n] = b[n] / a[n, n];

            for (var i = n - 1; i >= 0; i--)
            {
                var sum = 0.0;

                for (var s = 1; s <= steps; s++)
                {
                    if (i + s >= a.GetLength(0))
                        break;

                    sum += a[i, i + s] * x[i + s];
                }

                x[i] = (b[i] - sum) / a[i, i];
            }

            return x;
        }



        private static int GetDiagonalsCount(double[,] a)
        {
            var maxNumbersInLine = 0;

            for (var i = 0; i < a.GetLength(0); i++)
            {
                var numbersInLine = 0;
                for (var j = 0; j < a.GetLength(1); j++)
                    if (Math.Abs(a[i, j]) > 1e-14)
                        numbersInLine++;

                if (numbersInLine > maxNumbersInLine)
                    maxNumbersInLine = numbersInLine;
            }

            return maxNumbersInLine;
        }

        public static double[] SolveLinearEquationsSystem(double[,] A, double[] b)
        {
            var MA = Matrix<double>.Build.DenseOfArray(A);
            var vb = Vector<double>.Build.Dense(b);

            return MA.Solve(vb).ToArray();
        }

        public static double[] SolveIterative(SparseMatrix A, SparseVector b)
        {
            var solver = new GpBiCg();
            var precondition = new DiagonalPreconditioner();

            return A.SolveIterative(b, solver, precondition).ToArray();
        }
    }
}
