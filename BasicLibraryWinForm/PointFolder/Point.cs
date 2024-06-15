using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BasicLibraryWinForm.PointFolder
{
    [Serializable]
    [DataContract(Name = "Точка")]
    public class Point
    {
        [JsonConstructor]
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public Point()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public double[] ToArray()
        {
            return new[] { X, Y, Z };
        }

        public Point Normalize()
        {
            return this / Length();
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static double GetScalarDot(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        public static double GetAngle(Point p1, Point p2)
        {
            return Math.Acos((p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z) / (p1.Length() * p2.Length()));
        }

        public static double GetAngle0_360(Point p1, Point p2)
        {
            return Math.Atan2(p1.Y, p1.Z) - Math.Atan2(p2.Y, p2.Z);
        }

        public static double GetAngle0_360(double v1x, double v1y, double v2x, double v2y)
        {
            return Math.Atan2(v1y, v1x) - Math.Atan2(v2y, v2x);
        }

        public static double GetAngle(double v1x, double v1y, double v2x, double v2y)
        {
            var l1 = Math.Sqrt(v1x * v1x + v1y * v1y);
            var l2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            return Math.Acos((v1x * v2x + v1y * v2y) / (l1 * l2));
        }

        public static double GetDistance(Point p1, Point p2)
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            var dz = p1.Z - p2.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public double GetDistance(Point p)
        {
            var dx = X - p.X;
            var dy = Y - p.Y;
            var dz = Z - p.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if (p1 is null || p2 is null)
                return false;

            return !(Math.Abs(p1.X - p2.X) > 1e-8) && !(Math.Abs(p1.Y - p2.Y) > 1e-8) && !(Math.Abs(p1.Z - p2.Z) > 1e-8);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point operator *(double scalar, Point p)
        {
            return new(p.X * scalar, p.Y * scalar, p.Z * scalar);
        }

        public static Point operator *(Point p, double scalar)
        {
            return scalar * p;
        }

        public static Point operator /(Point p, double scalar)
        {
            return new(p.X / scalar, p.Y / scalar, p.Z / scalar);
        }

        public static Point operator /(double scalar, Point p)
        {
            return p / scalar;
        }

        public static double Scalar(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        [DataMember] public double X { get; set; }
        [DataMember] public double Y { get; set; }
        [DataMember] public double Z { get; set; }
        public int Id { get; private set; } = -1;

        public void SetId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id} | X:{X:0.000} Y:{Y:0.000} Z:{Z:0.000}";
        }
    }
}
