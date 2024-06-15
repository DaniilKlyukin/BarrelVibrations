using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;
namespace BasicLibraryWinForm.Delaunator.Interfaces
{
    public interface ITriangle
    {
        IEnumerable<Point> Points { get; }
        int Index { get; }
    }
}
