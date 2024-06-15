using BasicLibraryWinForm.Delaunator.Interfaces;
using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BasicLibraryWinForm.Delaunator.Models
{
    public struct VoronoiCell : IVoronoiCell
    {
        public Point[] Points { get; set; }
        public int Index { get; set; }
        public VoronoiCell(int triangleIndex, Point[] points)
        {
            Points = points;
            Index = triangleIndex;
        }
    }
}
