using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;
namespace BasicLibraryWinForm.Delaunator.Interfaces
{
    public interface IEdge
    {
        Point P { get; }
        Point Q { get; }
        int Index { get; }
    }
}
