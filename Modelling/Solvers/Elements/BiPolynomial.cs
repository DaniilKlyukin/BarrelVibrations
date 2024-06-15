using System;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Modelling.Solvers.Elements
{
    public class BiPolynomial
    {
        public SparseMatrix Coefficients { get; set; }

        public double Evaluate(double x, double y)
        {
            var sum = 0.0;

            foreach (var (i, j, value) in Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                sum += value * Math.Pow(x, i) * Math.Pow(y, j);
            }

            return sum;
        }

        /// <summary>
        /// Строки - степени x, колонки - степени y, 
        /// </summary>
        /// <param name="c"></param>
        public BiPolynomial(double[,] c)
        {
            Coefficients = SparseMatrix.OfArray(c);
        }

        public BiPolynomial(SparseMatrix sm)
        {
            Coefficients = SparseMatrix.OfMatrix(sm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable">1 - first; 2 - second</param>
        /// <returns></returns>
        public BiPolynomial Differentiate(int variable)
        {
            if (variable == 1)
                return DifferentiateX();
            if (variable == 2)
                return DifferentiateY();
            throw new ArgumentException();
        }

        private BiPolynomial DifferentiateX()
        {
            var matrix = new SparseMatrix(Coefficients.RowCount - 1, Coefficients.ColumnCount);

            foreach (var (i, j, value) in Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                if (i == 0)
                    continue;

                matrix[i - 1, j] = value * i;
            }

            return new BiPolynomial(matrix);
        }

        private BiPolynomial DifferentiateY()
        {
            var matrix = new SparseMatrix(Coefficients.RowCount, Coefficients.ColumnCount - 1);

            foreach (var (i, j, value) in Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                if (j == 0)
                    continue;

                matrix[i, j - 1] = value * j;
            }

            return new BiPolynomial(matrix);
        }

        public static BiPolynomial operator *(BiPolynomial bp1, BiPolynomial bp2)
        {
            var matrix = new SparseMatrix(bp1.Coefficients.RowCount * bp2.Coefficients.RowCount, bp1.Coefficients.ColumnCount * bp2.Coefficients.ColumnCount);

            foreach (var (i1, j1, value1) in bp1.Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                foreach (var (i2, j2, value2) in bp2.Coefficients.Storage.EnumerateNonZeroIndexed())
                {
                    matrix[i1 + i2, j1 + j2] += value1 * value2;
                }
            }

            return new BiPolynomial(matrix);
        }

        public static BiPolynomial operator +(BiPolynomial bp1, BiPolynomial bp2)
        {
            var matrix = new SparseMatrix(Math.Max(bp1.Coefficients.RowCount, bp2.Coefficients.RowCount), Math.Max(bp1.Coefficients.ColumnCount, bp2.Coefficients.ColumnCount));

            foreach (var (i, j, value) in bp1.Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                matrix[i, j] += value;
            }

            foreach (var (i, j, value) in bp2.Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                matrix[i, j] += value;
            }

            return new BiPolynomial(matrix).UpdateSize();
        }

        public static BiPolynomial operator -(BiPolynomial bp1, BiPolynomial bp2)
        {
            var matrix = new SparseMatrix(Math.Max(bp1.Coefficients.RowCount, bp2.Coefficients.RowCount), Math.Max(bp1.Coefficients.ColumnCount, bp2.Coefficients.ColumnCount));

            foreach (var (i, j, value) in bp1.Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                matrix[i, j] += value;
            }

            foreach (var (i, j, value) in bp2.Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                matrix[i, j] -= value;
            }

            return new BiPolynomial(matrix).UpdateSize();
        }

        private BiPolynomial UpdateSize()
        {
            var rows = 1;
            var columns = 1;

            foreach (var (i, j, value) in Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                if (i >= rows)
                    rows = i + 1;
                if (j >= columns)
                    columns = j + 1;
            }

            var matrix = new SparseMatrix(rows, columns);

            foreach (var (i, j, value) in Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                matrix[i, j] = value;
            }

            return new BiPolynomial(matrix);
        }

        public static BiPolynomial operator *(BiPolynomial bp, double value)
        {
            return new BiPolynomial(bp.Coefficients * value).UpdateSize();
        }

        public static BiPolynomial operator *(double value, BiPolynomial bp)
        {
            return bp * value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var (i, j, value) in Coefficients.Storage.EnumerateNonZeroIndexed())
            {
                var sign = value switch
                {
                    < 0 => "-",
                    > 0 when sb.Length != 0 => "+",
                    _ => string.Empty
                };

                var xi = "";
                var yj = "";

                if (i == 1)
                {
                    xi = "x";
                }
                else if (i != 0)
                {
                    xi = $"x^{i}";
                }

                if (j == 1)
                {
                    yj = "y";
                }
                else if (j != 0)
                {
                    yj = $"y^{j}";
                }

                sb.Append($"{sign}{Math.Abs(value):0.00}{xi}{yj}");
            }

            return sb.ToString();
        }
    }
}
