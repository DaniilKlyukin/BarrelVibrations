using SharpGL.SceneGraph;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace Visualization.OpenGL
{
    public class GLSurface
    {
        public bool WithEdges { get; set; } = true;

        public IEnumerable<ColoredPoint> Points { get; private set; }
        public Point Normal { get; }

        public double CenterX => Points.Average(coloredPoint => coloredPoint.Point.X);
        public double CenterY => Points.Average(coloredPoint => coloredPoint.Point.Y);
        public double CenterZ => Points.Average(coloredPoint => coloredPoint.Point.Z);

        public Vertex Center => new((float)CenterX, (float)CenterY, (float)CenterZ);

        public double AverageLength
        {
            get
            {
                var minX = Points.Min(p => p.X);
                var minY = Points.Min(p => p.Y);
                var minZ = Points.Min(p => p.Z);

                var maxX = Points.Max(p => p.X);
                var maxY = Points.Max(p => p.Y);
                var maxZ = Points.Max(p => p.Z);

                var w = maxX - minX;
                var h = maxY - minY;
                var l = maxZ - minZ;

                return Math.Sqrt(w * w + h * h + l * l);
            }
        }

        public void SetColor(Color color)
        {
            Points = Points.Select(p => new ColoredPoint(p.Point, color));
        }

        public GLSurface(IEnumerable<ColoredPoint> points) : this(points, new Point())
        {
        }

        public GLSurface(params ColoredPoint[] points) : this(points, new Point())
        {
        }

        public GLSurface(Color color, params Point[] points) :
            this(points.Select(p => new ColoredPoint(p, color)), new Point())
        {
        }

        public GLSurface(params Point[] points) :
    this(points.Select(p => new ColoredPoint(p, Color.Gray)), new Point())
        {
        }

        public GLSurface(params (double, double, double)[] points) :
this(points.Select(p => new ColoredPoint(p, Color.Gray)), new Point())
        {
        }

        public GLSurface(IEnumerable<ColoredPoint> points, Point normal)
        {
            Points = points;
            Normal = normal;
        }
    }
}
