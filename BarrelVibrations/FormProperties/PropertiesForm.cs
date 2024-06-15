namespace BarrelVibrations.PropertyGridClasses.FormProperties
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm(object properties)
        {
            InitializeComponent();

            propertyGrid.SelectedObject = properties;
        }
    }
}
