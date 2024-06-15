using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;

namespace BasicLibraryWinForm.PointFolder
{
    public partial class PointFormEditor : Form, IEditable<Point>
    {
        Point p = new();

        public PointFormEditor()
        {
            InitializeComponent();

            propertyGrid.SelectedObject = p;
        }

        public Point GetValues()
        {
            return p;
        }

        public void SetValues(Point values)
        {
            p.X = values.X;
            p.Y = values.Y;
            p.Z= values.Z;
        }
    }
}
