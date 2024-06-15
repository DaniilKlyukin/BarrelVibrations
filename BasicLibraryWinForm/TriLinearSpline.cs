using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLibraryWinForm
{
    public class TriLinearSpline
    {
        public double[] xs;
        public double[] ys;
        public double[] zs;
        public double[,,] values;

        public TriLinearSpline(double[] xs, double[] ys, double[] zs, double[,,] values)
        {
            if (xs.Length != values.GetLength(0) || ys.Length != values.GetLength(1) || zs.Length != values.GetLength(2))
                throw new ArgumentException();

            this.xs = xs;
            this.ys = ys;
            this.zs = zs;
            this.values = values;
        }

        public double Interpolate(double x, double y, double z)
        {
            var (i, _) = Algebra.BinarySearch(xs, x);
            var (j, _) = Algebra.BinarySearch(ys, y);
            var (k, _) = Algebra.BinarySearch(zs, z);

            i = Math.Min(Math.Max(i, 0), xs.Length - 1);
            j = Math.Min(Math.Max(j, 0), ys.Length - 1);
            k = Math.Min(Math.Max(k, 0), zs.Length - 1);

            var iNext = Math.Min(i + 1, xs.Length - 1);
            var jNext = Math.Min(j + 1, ys.Length - 1);
            var kNext = Math.Min(k + 1, zs.Length - 1);

            if (i == iNext)
                i--;

            if (j == jNext)
                j--;

            if (k == kNext)
                k--;

            var xd = (x - xs[i]) / (xs[iNext] - xs[i]);
            var yd = (y - ys[j]) / (ys[jNext] - ys[j]);
            var zd = (z - zs[k]) / (zs[kNext] - zs[k]);

            var c00 = values[i, j, k] * (1 - xd) + values[iNext, j, k] * xd;
            var c01 = values[i, j, kNext] * (1 - xd) + values[iNext, j, kNext] * xd;
            var c10 = values[i, jNext, k] * (1 - xd) + values[iNext, jNext, k] * xd;
            var c11 = values[i, jNext, kNext] * (1 - xd) + values[iNext, jNext, kNext] * xd;

            var c0 = c00 * (1 - yd) + c10 * yd;
            var c1 = c01 * (1 - yd) + c11 * yd;

            return c0 * (1 - zd) + c1 * zd;
        }
    }
}
