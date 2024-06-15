using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BasicLibraryWinForm
{
    public class Circle
    {

        public static readonly Circle INVALID = new Circle(new Point(0, 0), -1);

        private const double MULTIPLICATIVE_EPSILON = 1 + 1e-14;


        public Point Center { get; set; }   // Center
        public double Radius { get; set; }   // Radius


        public Circle(Point c, double r)
        {
            Center = c;
            Radius = r;
        }


        public bool Contains(Point p)
        {
            return Algebra.GetDistance(Center, p) <= Radius * MULTIPLICATIVE_EPSILON;
        }


        public bool Contains(ICollection<Point> ps)
        {
            foreach (Point p in ps)
            {
                if (!Contains(p))
                    return false;
            }
            return true;
        }

    }
}
