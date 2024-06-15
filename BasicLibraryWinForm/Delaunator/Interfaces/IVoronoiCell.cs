using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;
namespace BasicLibraryWinForm.Delaunator.Interfaces
{
    public interface IVoronoiCell
    {
        Point[] Points { get; }
        int Index { get; }
    }
}
