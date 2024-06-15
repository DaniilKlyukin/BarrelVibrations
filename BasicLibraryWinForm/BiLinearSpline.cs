using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLibraryWinForm
{
    public class BiLinearSpline
    {
        public double[] columns;
        public double[] rows;
        public double[,] values;

        public BiLinearSpline(double[] rows, double[] columns, double[,] values)
        {
            if (columns.Length != values.GetLength(1) || rows.Length != values.GetLength(0))
                throw new ArgumentException();

            this.columns = columns;
            this.rows = rows;
            this.values = values;
        }

        public double Interpolate(double row, double column)
        {
            var (i, _) = Algebra.BinarySearch(rows, row);
            var (j, _) = Algebra.BinarySearch(columns, column);

            i = Math.Min(Math.Max(i, 0), rows.Length - 1);
            j = Math.Min(Math.Max(j, 0), columns.Length - 1);

            var iNext = Math.Min(i + 1, rows.Length - 1);
            var jNext = Math.Min(j + 1, columns.Length - 1);
            
            if (i == iNext)
                i--;

            if (j == jNext)
                j--;
            
            return (values[i, j] * (columns[jNext] - column) * (rows[iNext] - row)
                + values[iNext, j] * (columns[jNext] - column) * (row - rows[i])
                + values[i, jNext] * (column - columns[j]) * (rows[iNext] - row)
                + values[iNext, jNext] * (column - columns[j]) * (row - rows[i]))
                / ((columns[jNext] - columns[j]) * (rows[iNext] - rows[i]));
        }
    }
}
