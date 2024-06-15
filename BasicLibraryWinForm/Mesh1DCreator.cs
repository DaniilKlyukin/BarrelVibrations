namespace BasicLibraryWinForm
{
    public static class Mesh1DCreator
    {
        public static double[] Create(double[] sectionsX, int pointsCount)
        {
            var totalSize = sectionsX.Last();
            var avgStep = totalSize / (pointsCount - 1);

            var intervalsCounts = new int[sectionsX.Length - 1];

            for (var i = 0; i < sectionsX.Length - 1; i++)
            {
                var sizeCurrent = sectionsX[i + 1] - sectionsX[i];
                var stepsCurrent = (int)Math.Round(sizeCurrent / avgStep);

                if (i == sectionsX.Length - 2)
                {
                    stepsCurrent = pointsCount - intervalsCounts.Sum() - 1;
                }

                intervalsCounts[i] = stepsCurrent;
            }

            return Create(sectionsX, intervalsCounts);
        }

        public static double[] Create(double[] sectionsX, int[] intervalsCounts)
        {
            var fixedIndecies = new List<int>();
            var xTotal = new List<double>();

            for (var i = 0; i < sectionsX.Length - 1; i++)
            {
                fixedIndecies.Add(xTotal.Count);
                xTotal.Add(sectionsX[i]);

                var sizeCurrent = sectionsX[i + 1] - sectionsX[i];
                var avgStepCurrent = sizeCurrent / intervalsCounts[i];

                for (int j = 1; j < intervalsCounts[i]; j++)
                {
                    xTotal.Add(xTotal.Last() + avgStepCurrent);
                }
            }

            fixedIndecies.Add(xTotal.Count);
            xTotal.Add(sectionsX.Last());

            var I = xTotal.Count - 1;
            var A = new double[xTotal.Count, xTotal.Count];
            var b = new double[xTotal.Count];

            for (int i = 0; i < b.Length; i++)
            {
                if (fixedIndecies.Contains(i))
                {
                    A[i, i] = 1;

                    b[i] = xTotal[i];
                }
                else if (i == 1)
                {
                    A[i, i - 1] = 1;
                    A[i, i] = -2;
                    A[i, i + 1] = 1;

                    b[i] = 0;
                }
                else if (i == I - 1)
                {
                    A[i, i - 1] = 1;
                    A[i, i] = -2;
                    A[i, i + 1] = 1;

                    b[i] = 0;
                }
                else
                {
                    A[i, i - 2] = 1;
                    A[i, i - 1] = -4;
                    A[i, i] = 6;
                    A[i, i + 1] = -4;
                    A[i, i + 2] = 1;

                    b[i] = 0;
                }
            }

            return LinearEquationSolver.SolveDiagonalSystem(A, b, 5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedPoints">упорядоченный словарь: индекс точки (начинается с 0), координата x, сила сгущения</param>
        /// <returns></returns>
        public static double[] Create(Dictionary<int, (double, double)> fixedPoints)
        {
            var I = fixedPoints.Last().Key;

            var ks = fixedPoints.Keys.ToArray();

            var xs = new double[I + 1];

            for (int i = 0; i < fixedPoints.Count - 1; i++)
            {
                var t1 = fixedPoints[ks[i]];
                var t2 = fixedPoints[ks[i + 1]];

                var (x1, w1) = t1;
                var (x2, w2) = t2;

                var k = w1 - w2 + 1;
                var range = x2 - x1;

                var pointsCount = ks[i + 1] - ks[i] + 1;

                var dr0 = Math.Abs(k - 1) < 1e-8
                    ? range / (pointsCount - 1)
                    : range * (k - 1) / (Math.Pow(k, pointsCount - 1) - 1);

                var j0 = ks.Skip(1).Take(i).Sum();

                xs[j0] = x1;
                var k_current = 1.0;

                for (var j = 1; j < pointsCount; j++)
                {
                    xs[j + j0] = xs[j + j0 - 1] + dr0 * k_current;
                    k_current *= k;
                }
            }

            return xs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedPoints">упорядоченный словарь: индекс точки (начинается с 0), координата x, сила сгущения</param>
        /// <returns></returns>
        public static void Create(Dictionary<int, (double, double)> fixedPoints, double[,] insertionMatrix, int row)
        {
            var ks = fixedPoints.Keys.ToArray();

            for (int i = 0; i < fixedPoints.Count - 1; i++)
            {
                var t1 = fixedPoints[ks[i]];
                var t2 = fixedPoints[ks[i + 1]];

                var (x1, w1) = t1;
                var (x2, w2) = t2;

                var k = w1 - w2 + 1;
                var range = x2 - x1;

                var pointsCount = ks[i + 1] - ks[i] + 1;

                var dr0 = Math.Abs(k - 1) < 1e-8
                    ? range / (pointsCount - 1)
                    : range * (k - 1) / (Math.Pow(k, pointsCount - 1) - 1);

                var j0 = ks.Skip(1).Take(i).Sum();

                insertionMatrix[row, j0] = x1;
                var k_current = 1.0;

                for (var j = 1; j < pointsCount; j++)
                {
                    insertionMatrix[row, j + j0] = insertionMatrix[row, j + j0 - 1] + dr0 * k_current;
                    k_current *= k;
                }
            }
        }
    }
}
