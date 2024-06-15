using BasicLibraryWinForm.PointFolder;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Point = BasicLibraryWinForm.PointFolder.Point;
namespace BasicLibraryWinForm
{
    public static class Algebra
    {
        static Random rnd = new Random();

        public static double GetRandomValue(double min, double max)
        {
            return (max - min) * rnd.NextDouble() + min;
        }

        public static double GetIntegralTrapezoid(double[] xs, double[] ys)
        {
            var integral = 0.0;

            for (var i = 0; i < xs.Length - 1; i++)
            {
                integral += (xs[i + 1] - xs[i]) * (ys[i + 1] + ys[i]) / 2;
            }

            return integral;
        }

        public static Matrix<double> Integrate(Func<double, double, Matrix<double>> intFunc, int rows, int columns, double[] weights, double[] integration_x, double[] integration_y)
        {
            var sum = new DenseMatrix(rows, columns);

            for (var i = 0; i < weights.Length; i++)
            {
                sum += -weights[i] * DenseMatrix.OfMatrix(intFunc(integration_x[i], integration_y[i]));
            }

            return sum;
        }


        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        public static double FindDistanceToSegment(Point pt, Point p1, Point p2)
        {
            return FindDistanceToSegment(pt.X, pt.Y, p1.X, p1.Y, p2.X, p2.Y);
        }

        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        public static double FindDistanceToSegment(double ptX, double ptY, double p1X, double p1Y, double p2X, double p2Y)
        {
            var dx = p2X - p1X;
            var dy = p2Y - p1Y;
            if (dx == 0 && dy == 0)
            {
                // It's a point not a line segment.
                dx = ptX - p1X;
                dy = ptY - p1Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            var t = ((ptX - p1X) * dx + (ptY - p1Y) * dy) /
                    (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                dx = ptX - p1X;
                dy = ptY - p1Y;
            }
            else if (t > 1)
            {
                dx = ptX - p2X;
                dy = ptY - p2Y;
            }
            else
            {
                dx = ptX - (p1X + t * dx);
                dy = ptY - (p1Y + t * dy);
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static (bool, double, double) GetLinesCross(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {

            var maxx1 = Math.Max(x1, x2);
            var maxy1 = Math.Max(y1, y2);

            var minx1 = Math.Min(x1, x2);
            var miny1 = Math.Min(y1, y2);

            var maxx2 = Math.Max(x3, x4);
            var maxy2 = Math.Max(y3, y4);
            var minx2 = Math.Min(x3, x4);
            var miny2 = Math.Min(y3, y4);

            if (minx1 > maxx2 || maxx1 < minx2 || miny1 > maxy2 || maxy1 < miny2)
                return (false, 0, 0);  // Момент, када линии имеют одну общую вершину...


            double dx1 = x2 - x1, dy1 = y2 - y1; // Длина проекций первой линии на ось x и y
            double dx2 = x4 - x3, dy2 = y4 - y3; // Длина проекций второй линии на ось x и y
            double dxx = x1 - x3, dyy = y1 - y3;
            double div = dy2 * dx1 - dx2 * dy1;
            var mul1 = dx1 * dyy - dy1 * dxx;
            var mul2 = dx2 * dyy - dy2 * dxx;

            if (div == 0)
                return (false, 0, 0); // Линии параллельны...
            if (div > 0)
            {
                if (mul1 < 0 || mul1 > div)
                    return (false, 0, 0); // Первый отрезок пересекается за своими границами...
                if (mul2 < 0 || mul2 > div)
                    return (false, 0, 0); // Второй отрезок пересекается за своими границами...
            }

            if (-mul1 < 0 || -mul1 > -div)
                return (false, 0, 0); // Первый отрезок пересекается за своими границами...
            if (-mul2 < 0 || -mul2 > -div)
                return (false, 0, 0); // Второй отрезок пересекается за своими границами...

            var ua = mul2 / div;
            //var ub = mul1 / div;


            return (true, x1 + ua * (x2 - x1), y1 + ua * (y2 - y1));
        }

        public static double GetInscribedPolygonEdgeLength(int edgesCount, double circleRadius)
        {
            return 2 * circleRadius * Math.Tan(Math.PI / edgesCount);
        }

        public static double GetInscribedPolygonEdgeCount(double edgeLength, double circleRadius)
        {
            return Math.PI / Math.Atan2(edgeLength, 2 * circleRadius);
        }

        public static double GetFiniteValue(double value)
        {
            return double.IsFinite(value) ? value : 0.0;
        }

        /// <summary>
        ///     Объём цилиндра
        /// </summary>
        /// <param name="r">Радиус</param>
        /// <param name="h">Высота</param>
        /// <returns></returns>
        public static double GetCylinderVolume(double r, double h)
        {
            return Math.PI * r * r * h;
        }



        /// <summary>
        ///     Вычисляет значение в нужной точке при условии, что значение находится на одной линии с двумя известными
        /// </summary>
        /// <param name="x">Координата x неизвестной точки</param>
        /// <param name="x0">Координата x известной точки 1</param>
        /// <param name="y0">Координата y известной точки 1</param>
        /// <param name="x1">Координата x известной точки 2</param>
        /// <param name="y1">Координата y известной точки 2</param>
        /// <returns>Значение координаты y неизвестной точки</returns>
        public static double GetValueAtLine(double x, double x0, double y0, double x1, double y1)
        {
            if (Math.Abs(x1 - x0) > 1e-14)
                return (x - x0) / (x1 - x0) * (y1 - y0) + y0;

            return y0;
        }

        public static double GetValueAtLine(double y0, double y1)
        {
            return 2 * (y1 - y0) + y0;
        }

        /// <summary>
        ///     Возвращает массив с равномерно распределёнными значениями
        /// </summary>
        /// <param name="x0">Начальное значение</param>
        /// <param name="xn">Конечное значение</param>
        /// <param name="n">Число элементов</param>
        /// <returns></returns>
        public static double[] Linspace(double x0, double xn, int n)
        {
            var arr = new double[n];
            var h = (xn - x0) / (n - 1);

            for (var i = 0; i < n; i++)
                arr[i] = x0 + i * h;

            return arr;
        }

        /// <summary>
        ///     Значение функции Хэвисайда
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double H(double x)
        {
            return x <= 0 ? 0 : 1;
        }

        public static double Sgn_star(double x)
        {
            return x < 0 ? -1 : 1;
        }

        public static double Hx(double x)
        {
            return x <= 0 ? 0 : x;
        }

        public static T[,] Ones<T>(int length1, int length2, T value)
        {
            var values = new T[length1, length2];

            for (var i = 0; i < values.GetLength(0); i++)
            {
                for (var j = 0; j < values.GetLength(1); j++) values[i, j] = value;
            }

            return values;
        }

        public static T[] Ones<T>(int length1, T value)
        {
            var values = new T[length1];

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = value;
            }

            return values;
        }

        public static Func<double, double> GetFunc(IEnumerable<double> xs, IEnumerable<double> ys, bool linear = true)
        {
            if (ys.Count() == 0)
            {
                return _ => 0;
            }

            if (ys.Count() == 1)
            {
                return _ => ys.First();
            }

            if(!linear && ys.Count() > 2)
            {
                var spline = CubicSpline.InterpolatePchip(xs, ys);
                return x => spline.Interpolate(x);
            }
            else
            {
                var spline = LinearSpline.Interpolate(xs, ys);
                return x => spline.Interpolate(x);
            }
        }

        public static Func<double, double> GetDer(double[] xs, double[] ys, bool linear = true)
        {
            if (ys.Length == 0)
            {
                return _ => 0;
            }

            if (ys.Length == 1)
            {
                return _ => ys.First();
            }

            if (!linear && ys.Length > 2)
            {
                var spline = CubicSpline.InterpolatePchip(xs, ys);
                return x => spline.Differentiate(x);
            }
            else
            {
                var spline = LinearSpline.Interpolate(xs, ys);

                return x => spline.Differentiate(x);
            }
        }

        public static (double, double) GetClockwisedNormal(Point p0, Point p1)
        {
            var dx = p1.X - p0.X;
            var dy = p1.Y - p0.Y;
            var length = Math.Sqrt(dx * dx + dy * dy);

            return (dy / length, -dx / length);
        }

        public static (double, double) GetClockwisedNormal(double x0, double y0, double x1, double y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var length = Math.Sqrt(dx * dx + dy * dy);

            return (dy / length, -dx / length);
        }

        public static bool CircleLineSegmentIntersection(double px1,
                                                         double py1,
                                                         double px2,
                                                         double py2,
                                                         double cx,
                                                         double cy,
                                                         double r,
                                                         out double x1,
                                                         out double y1,
                                                         out double x2,
                                                         out double y2)
        {
            px1 -= cx;
            py1 -= cy;
            px2 -= cx;
            py2 -= cy;

            var dx = px2 - px1;
            var dy = py2 - py1;
            var dr = Math.Sqrt(dx * dx + dy * dy);

            var D = px1 * py2 - px2 * py1;

            var delta = r * r * dr * dr - D * D;

            x1 = (D * dy + Sgn_star(dy) * dx * Math.Sqrt(delta)) / FastMath.Pow2(dr) + cx;
            x2 = (D * dy - Sgn_star(dy) * dx * Math.Sqrt(delta)) / FastMath.Pow2(dr) + cx;

            y1 = (-D * dx + Math.Abs(dy) * Math.Sqrt(delta)) / FastMath.Pow2(dr) + cy;
            y2 = (-D * dx - Math.Abs(dy) * Math.Sqrt(delta)) / FastMath.Pow2(dr) + cy;

            return delta >= 0;
        }

        public static double GetDeterminant(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }

        public static double GetArea(IList<double> xs, IList<double> ys)
        {
            if (xs.Count < 3 || xs.Count != ys.Count)
            {
                return 0;
            }

            var n = xs.Count - 1;
            var s1 = 0.0;
            var s2 = 0.0;

            for (var i = 0; i <= n - 1; i++)
            {
                s1 += xs[i] * ys[i + 1];
                s2 += xs[i + 1] * ys[i];
            }

            return Math.Abs(s1 + xs[n] * ys[0] - s2 - xs[0] * ys[n]) / 2;
        }

        public static double AnglesDistance(double a1, double a2)
        {
            return Math.Abs(Math.Min(2 * Math.PI - Math.Abs(a1 - a2), Math.Abs(a1 - a2)));
        }

        public static double GetDistanceToSurface(double Ax, double By, double Cz, double D, Point p, bool isSigned)
        {
            var d = (Ax * p.X + By * p.Y + Cz * p.Z + D) / Math.Sqrt(Ax * Ax + By * By + Cz * Cz);

            return isSigned ? d : Math.Abs(d);
        }

        public static double ConvertGradToRad(double grad)
        {
            return Math.PI * grad / 180;
        }

        public static double ConvertRadToGrad(double rad)
        {
            return rad * 180 / Math.PI;
        }


        public static double GetCircleArea(double r)
        {
            return Math.PI * r * r;
        }

        public static bool InPolygon(List<double> xs, List<double> ys, double x, double y)
        {
            var c = false;
            for (int i = 0, j = xs.Count - 1; i < xs.Count; j = i++)
            {
                if ((ys[i] <= y && y <= ys[j] || ys[j] <= y && y <= ys[i]) && ys[j] - ys[i] != 0 && x >= (xs[j] - xs[i]) * (y - ys[i]) / (ys[j] - ys[i]) + xs[i])
                    c = !c;
            }
            return c;
        }

        public static double GetDistance(Point p1, Point p2)
        {
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;
            var dz = p2.Z - p1.Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double GetDistance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var dz = z2 - z1;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static Color GetHeatColor(
           double value, double blueValue, double redValue)
        {
            if (value >= redValue)
                value = redValue;

            if (value <= blueValue)
                value = blueValue;

            if (Math.Abs(blueValue - redValue) < 1e-50)
            {
                blueValue -= 1;
                redValue += 1;
            }

            if (!double.IsFinite(value) || !double.IsFinite(blueValue) || !double.IsFinite(redValue))
            {
                return Color.Black;
            }

            // Convert into a value between 0 and 1023.
            var intValue = (int)(1023 * (value - redValue) /
                                 (blueValue - redValue));

            // Map different color bands.
            if (intValue < 256)
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                return Color.FromArgb(255, intValue, 0);

            if (intValue < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                intValue -= 256;
                return Color.FromArgb(255 - intValue, 255, 0);
            }

            if (intValue < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                intValue -= 512;
                return Color.FromArgb(0, 255, intValue);
            }

            // Aqua to blue. (0, 255, 255) to (0, 0, 255).
            intValue -= 768;
            return Color.FromArgb(0, 255 - intValue, 255);
        }

        public static (int, int) BinarySearch<T>(IList<T> values, T v) where T : IComparable
        {
            if (v.CompareTo(values.First()) < 0 || v.CompareTo(values.Last()) > 0)
                return (-1, -1);

            var iL = 0;
            var iR = values.Count - 1;

            while (true)
            {
                var ic = (iL + iR) / 2;
                var vc = values[ic];
                var cmp = vc.CompareTo(v);

                if (cmp == 0)
                    return (ic, ic);

                if (cmp > 0)
                {
                    iR = ic;
                }
                else if (cmp < 0)
                {
                    iL = ic;
                }

                if (iR - iL == 1)
                {
                    if (values[iL].CompareTo(v) == 0)
                        return (iL, iL);
                    else if (values[iR].CompareTo(v) == 0)
                        return (iR, iR);
                    return (iL, iR);
                }
            }
        }



        public static (double, double, double) Rotate(double x, double y, double z, double vx, double vy, double vz, double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var m = DenseMatrix.OfArray(new double[,]
            {
                {cos+(1-cos)*vx*vx, (1-cos)*vx*vy-sin*vz, (1-cos)*vx*vz+sin*vy},
                {(1-cos)*vy*vz+sin*vz, cos+(1-cos)*vy*vy, (1-cos)*vy*vz-sin*vx},
                {(1-cos)*vz*vx-sin*vy, (1-cos)*vz*vy+sin*vx, cos+(1-cos)*vz*vz},
            });

            var p = DenseVector.OfArray(new double[] { x, y, z });

            var res = m * p;

            return (res[0], res[1], res[2]);
        }

        public static double GetCurvature(double[] xs, double[] curve, int i)
        {
            double dy;
            double d2y;

            if (i == 0)
            {
                dy = (curve[1] - curve[0]) / (xs[1] - xs[0]);
                d2y =
                    ((curve[2] - curve[1]) / (xs[2] - xs[1])
                    - (curve[1] - curve[0]) / (xs[1] - xs[0]))
                            / (0.5 * (xs[2] - xs[0]));
            }
            else if (i == curve.Length - 1)
            {
                dy = (curve[^1] - curve[^2]) / (xs[^1] - xs[^2]);
                d2y =
                    ((curve[^1] - curve[^2]) / (xs[^1] - xs[^2])
                    - (curve[^2] - curve[^3]) / (xs[^2] - xs[^3]))
                            / (0.5 * (xs[^1] - xs[^3]));
            }
            else
            {
                dy = (curve[i + 1] - curve[i - 1]) / (xs[i + 1] - xs[i - 1]);
                d2y =
                    ((curve[i + 1] - curve[i]) / (xs[i + 1] - xs[i])
                    - (curve[i] - curve[i - 1]) / (xs[i] - xs[i - 1]))
                            / (0.5 * (xs[i + 1] - xs[i - 1]));
            }
            return Math.Abs(d2y) / Math.Pow(1 + FastMath.Pow2(dy), 1.5);
        }

        public static double GetCurvature(double[] xs, double[] curve, double[] delta, int i)
        {
            double dy;
            double d2y;

            if (i == 0)
            {
                dy = (curve[1] - curve[0] + delta[1] - delta[0]) / (xs[1] - xs[0]);
                d2y =
                    ((curve[2] - curve[1] + delta[2] - delta[1]) / (xs[2] - xs[1])
                    - (curve[1] - curve[0] + delta[1] - delta[0]) / (xs[1] - xs[0]))
                            / (0.5 * (xs[2] - xs[0]));
            }
            else if (i == curve.Length - 1)
            {
                dy = (curve[^1] - curve[^2] + delta[^1] - delta[^2]) / (xs[^1] - xs[^2]);
                d2y =
                    ((curve[^1] - curve[^2] + delta[^1] - delta[^2]) / (xs[^1] - xs[^2])
                    - (curve[^2] - curve[^3] + delta[^2] - delta[^3]) / (xs[^2] - xs[^3]))
                            / (0.5 * (xs[^1] - xs[^3]));
            }
            else
            {
                dy = (curve[i + 1] - curve[i - 1] + delta[i + 1] - delta[i - 1]) / (xs[i + 1] - xs[i - 1]);
                d2y =
                    ((curve[i + 1] - curve[i] + delta[i + 1] - delta[i]) / (xs[i + 1] - xs[i])
                    - (curve[i] - curve[i - 1] + delta[i] - delta[i - 1]) / (xs[i] - xs[i - 1]))
                            / (0.5 * (xs[i + 1] - xs[i - 1]));
            }
            return Math.Abs(d2y) / Math.Pow(1 + FastMath.Pow2(dy), 1.5);
        }

        public static double GetDerivative2(double[] xs, double[] curve, double[] delta, int i)
        {
            if (i == 0)
            {
                return GetDerivative2(xs, curve, delta, 1);
            }
            else if (i == curve.Length - 1)
            {
                return GetDerivative2(xs, curve, delta, i - 1);
            }
            else
            {
                return ((curve[i + 1] - curve[i] + delta[i + 1] - delta[i]) / (xs[i + 1] - xs[i]) - (curve[i] - curve[i - 1] + delta[i] - delta[i - 1]) / (xs[i] - xs[i - 1])) / (0.5 * (xs[i + 1] - xs[i - 1]));
            }
        }

        public static double GetDerivative2(double[] xs, double[] f, int i)
        {
            if (i == 0)
            {
                return GetDerivative2(xs, f, 1);
            }
            else if (i == xs.Length - 1)
            {
                return GetDerivative2(xs, f, i - 1);
            }
            else
            {
                return ((f[i + 1] - f[i]) / (xs[i + 1] - xs[i]) - (f[i] - f[i - 1]) / (xs[i] - xs[i - 1])) / (0.5 * (xs[i + 1] - xs[i - 1]));
            }
        }

        public static double GetDerivative(double[] xs, double[] curve, double[] delta, int i)
        {
            var iR = i + 1 < xs.Length ? i + 1 : i;
            var iL = i - 1 >= 0 ? i - 1 : i;

            return (curve[iR] - curve[iL] + delta[iR] - delta[iL]) / (xs[iR] - xs[iL]);
        }

        public static double GetDerivative(double[] xs, double[] curve, int i)
        {
            var iR = i + 1 < xs.Length ? i + 1 : i;
            var iL = i - 1 >= 0 ? i - 1 : i;

            return (curve[iR] - curve[iL]) / (xs[iR] - xs[iL]);
        }

        public static double[] InterpolateArray(double[] xs, double[] ys, double[] xsResult)
        {
            var f = GetFunc(xs, ys, false);

            return xsResult.Select(f).ToArray();
        }

        public static double GetEquivalentCircleR(double s)
        {
            return Math.Sqrt(s / Math.PI);
        }
    }
}