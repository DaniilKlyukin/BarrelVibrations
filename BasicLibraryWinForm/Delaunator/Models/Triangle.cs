using BasicLibraryWinForm.Delaunator.Interfaces;
using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BasicLibraryWinForm.Delaunator.Models
{
    public struct Triangle : ITriangle
    {
        public int Index { get; set; }

        public IEnumerable<Point> Points { get; set; }

        public Triangle(int t, IEnumerable<Point> points)
        {
            Points = points;
            Index = t;
        }
    }
}
