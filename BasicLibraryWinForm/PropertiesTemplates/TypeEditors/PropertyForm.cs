namespace BasicLibraryWinForm.PropertiesTemplates.TypeEditors
{
    public partial class PropertyForm<T> : Form
    {
        public PropertyForm()
        {
            InitializeComponent();
        }

        public void SetValue(T value)
        {
            propertyGrid.SelectedObject = value;
        }

        public T GetValue()
        {
            return (T)propertyGrid.SelectedObject;
        }
    }
}
