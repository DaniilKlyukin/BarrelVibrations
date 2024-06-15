using BasicLibraryWinForm.Delaunator.Interfaces;
using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;
namespace BasicLibraryWinForm.Delaunator.Models
{
    public struct Edge : IEdge
    {
        public Point P { get; set; }
        public Point Q { get; set; }
        public int Index { get; set; }

        public Edge(int e, Point p, Point q)
        {
            Index = e;
            P = p;
            Q = q;
        }
    }
}
