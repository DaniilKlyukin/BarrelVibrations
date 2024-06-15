using Point = BasicLibraryWinForm.PointFolder.Point;

namespace Visualization.OpenGL
{
    public class ColoredPoint
    {
        public double X => Point.X;
        public double Y => Point.Y;
        public double Z => Point.Z;

        public Point Point { get; }

        public Color Color { get; set; }

        public ColoredPoint(Point point, Color color)
        {
            Point = point;
            Color = color;
        }


        public ColoredPoint(double x, double y, double z, Color color) : this(new Point(x, y, z), color)
        {

        }

        public ColoredPoint((double, double, double) xyz, Color color) : this(new Point(xyz.Item1, xyz.Item2, xyz.Item3), color)
        {

        }

        public static ColoredPoint operator +(ColoredPoint p1, Point p2) => new(p1.Point + p2, p1.Color);

        public static ColoredPoint operator -(ColoredPoint p1, Point p2) => new(p1.Point - p2, p1.Color);

        public static ColoredPoint operator +(Point p1, ColoredPoint p2) => new(p1 + p2.Point, p2.Color);

        public static ColoredPoint operator -(Point p1, ColoredPoint p2) => new(p1 - p2.Point, p2.Color);
    }
}
