using BasicLibraryWinForm;
using BasicLibraryWinForm.PointFolder;
using System.Text;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BasicLibraryWinForm
{
    public static class ArrayExtensions
    {
        public static double GetDistance(this IList<double> list1, IList<double> list2)
        {
            var dist = 0.0;

            for (var i = 0; i < list1.Count; i++)
            {
                dist += 1 - Math.Abs(list1[i]) / (1 + Math.Abs(list2[i]));
            }

            return dist / list1.Count;
        }

        public static void PasteRowToMatrix(this double[,] A, double[] x, int rowIndex)
        {
            if (rowIndex > A.GetLength(0))
                throw new ArgumentException("Индекс строки больше размерности матрицы");

            for (var i = 0; i < A.GetLength(1); i++) A[rowIndex, i] = x[i];
        }
        public static IEnumerable<T> GetCopy<T>(this IEnumerable<T> arr)
        {
            foreach (var item in arr)
                yield return item;
        }

        public static IList<T> GetRowSlice<T>(this T[,] values, int i)
        {
            var result = new List<T>();

            for (var j = 0; j < values.GetLength(1); j++)
                result.Add(values[i, j]);

            return result;
        }

        public static IList<T> GetColumnSlice<T>(this T[,] values, int j)
        {
            var result = new List<T>();

            for (var i = 0; i < values.GetLength(0); i++)
                result.Add(values[i, j]);

            return result;
        }

        public static double? Median<TColl, TValue>(
            this IEnumerable<TColl> source,
            Func<TColl, TValue> selector)
        {
            return source.Select(selector).Median();
        }

        public static double? Median<T>(
            this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null);

            var data = source.ToArray();

            var count = data.Length;
            if (count == 0)
                return null;

            source = data.OrderBy(n => n);

            var midpoint = count / 2;
            if (count % 2 == 0)
                return (System.Convert.ToDouble(data.ElementAt(midpoint - 1)) + System.Convert.ToDouble(data.ElementAt(midpoint))) /
                       2.0;
            return System.Convert.ToDouble(source.ElementAt(midpoint));
        }

        public static IEnumerable<double> FiltrateExisted(this IEnumerable<double> input, double tolerance)
        {
            var filtered = new List<double>();

            foreach (var data in input)
                if (!filtered.Any(x => Math.Abs(x - data) <= tolerance))
                    filtered.Add(data);

            return filtered;
        }

        public static double Prod(this IEnumerable<double> arr)
        {
            var product = 1.0;

            foreach (var d in arr)
            {
                product *= d;
            }

            return product;
        }

        public static Point Average(this IEnumerable<Point> arr)
        {
            var x = 0.0;
            var y = 0.0;
            var z = 0.0;

            var points = arr as Point[] ?? arr.ToArray();

            foreach (var point in points)
            {
                x += point.X;
                y += point.Y;
                z += point.Z;
            }

            var count = points.Length;

            return new Point(x / count, y / count, z / count);
        }

        public static IEnumerable<double> Mult(this IEnumerable<double> input, double value)
        {
            foreach (var item in input)
                yield return item * value;
        }

        public static double[,] Mult(this double[,] arr, double value)
        {
            var result = Algebra.Ones(arr.GetLength(0), arr.GetLength(1), 0.0);

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    result[i, j] = arr[i, j] * value;
                }
            }

            return result;
        }

        public static double[,] Add(this double[,] arr, double value)
        {
            var result = Algebra.Ones(arr.GetLength(0), arr.GetLength(1), 0.0);

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    result[i, j] = arr[i, j] + value;
                }
            }

            return result;
        }

        public static double Min(this double[,] arr)
        {
            var min = double.MaxValue;

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    min = Math.Min(arr[i, j], min);
                }
            }

            return min;
        }

        public static double Max(this double[,] arr)
        {
            var max = double.MinValue;

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    max = Math.Max(arr[i, j], max);
                }
            }

            return max;
        }

        public static double Min(this double[,,] arr)
        {
            var min = double.MaxValue;

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    for (var k = 0; k < arr.GetLength(2); k++)
                    {
                        min = Math.Min(arr[i, j, k], min);
                    }
                }
            }

            return min;
        }

        public static double Max(this double[,,] arr)
        {
            var max = double.MinValue;

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    for (var k = 0; k < arr.GetLength(2); k++)
                    {
                        max = Math.Max(arr[i, j, k], max);
                    }
                }
            }

            return max;
        }

        public static TV[,] Select<T, TV>(this T[,] arr, Func<T, TV> selector)
        {
            var result = Algebra.Ones(arr.GetLength(0), arr.GetLength(1), default(TV));

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    result[i, j] = selector(arr[i, j]);
                }
            }

#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
            return result;
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        }

        public static T[,] Convert<T>(this T[][] arr)
        {
            var result = new T[arr.Length, arr[0].Length];

            for (var i = 0; i < result.GetLength(0); i++)
            {
                if (result.GetLength(1) != arr[i].Length)
                    throw new ArgumentException();

                for (var j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = arr[i][j];
                }
            }

            return result;
        }

        public static T[,] Reshape<T>(this IEnumerable<T> input, int rows, int columns)
        {
            var result = new T[rows, columns];

            var arr = input.ToArray();

            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = arr[i + j * result.GetLength(0)];
                }
            }

            return result;
        }


        public static T[,] Transpose<T>(this T[,] arr)
        {
            var result = new T[arr.GetLength(1), arr.GetLength(0)];

            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = arr[j, i];
                }
            }

            return result;
        }

        public static List<T> Get<T>(this T[,] arr, Func<int, int, bool> getFunc)
        {
            var result = new List<T>();

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    if (getFunc(i, j))
                        result.Add(arr[i, j]);
                }
            }

            return result;
        }

        public static double GetAbsoluteMax(this double[,] arr)
        {
            var max = double.MinValue;

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    var v = Math.Abs(arr[i, j]);
                    if (v > max)
                        max = v;
                }
            }

            return max;
        }
        public static T[] Copy<T>(this IList<T> from)
        {
            var result = new T[from.Count];

            for (var i = 0; i < from.Count; i++)
            {
                result[i] = from[i];
            }

            return result;
        }
        public static void Copy(this double[,] from, double[,] to)
        {
            Parallel.For(0, from.GetLength(0), i =>
            {
                for (var j = 0; j < from.GetLength(1); j++)
                {
                    to[i, j] = from[i, j];
                }
            });
        }

        public static double[,] Copy(this double[,] data)
        {
            var copy = new double[data.GetLength(0), data.GetLength(1)];

            Parallel.For(0, data.GetLength(0), i =>
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    copy[i, j] = data[i, j];
                }
            });

            return copy;
        }

        public static Point[,] Copy(this Point[,] data)
        {
            var copy = new Point[data.GetLength(0), data.GetLength(1)];

            Parallel.For(0, data.GetLength(0), i =>
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    copy[i, j] = new Point(data[i, j].X, data[i, j].Y, data[i, j].Z);
                }
            });

            return copy;
        }

        public static string String(this Point[,] arr)
        {
            var sb = new StringBuilder();

            sb.Append("Координаты X");
            sb.Append(Environment.NewLine);

            for (var j = arr.GetLength(1) - 1; j >= 0; j--)
            {
                for (var i = 0; i < arr.GetLength(0); i++)
                {
                    sb.Append($"{arr[i, j].X}\t");
                }
                sb.Append(Environment.NewLine);
            }

            sb.Append(Environment.NewLine);
            sb.Append("Координаты Y");
            sb.Append(Environment.NewLine);

            for (var j = arr.GetLength(1) - 1; j >= 0; j--)
            {
                for (var i = 0; i < arr.GetLength(0); i++)
                {
                    sb.Append($"{arr[i, j].Y}\t");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static string GetString<T>(this T[,] arr, string[] header, string separator = "\t")
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(separator, header));

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    sb.Append($"{arr[i, j]}");
                    if (j != arr.GetLength(1) - 1)
                        sb.Append($"{separator}");
                }
                if (i != arr.GetLength(0) - 1)
                    sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static string GetString<T>(this T[] arr, string separator = "\t")
        {
            var sb = new StringBuilder();

            for (var i = 0; i < arr.Length; i++)
            {
                sb.Append($"{arr[i]}");

                if (i != arr.Length - 1)
                    sb.Append($"{separator}");
            }

            return sb.ToString();
        }

        public static string GetString<T>(this T[] arr, string header, string separator = "\t")
        {
            var sb = new StringBuilder();

            sb.Append(header);
            sb.Append(separator);

            for (var i = 0; i < arr.Length; i++)
            {
                sb.Append($"{arr[i]}");

                if (i != arr.Length - 1)
                    sb.Append($"{separator}");
            }

            return sb.ToString();
        }

        public static string GetString<T>(this T[][] arr, string[] header, string separator = "\t")
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(separator, header));

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    sb.Append($"{arr[i][j]}");
                    if (j != arr.GetLength(1) - 1)
                        sb.Append($"{separator}");
                }
                if (i != arr.GetLength(0) - 1)
                    sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
