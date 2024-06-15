using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.ComponentModel;

namespace InletBallisticLibrary
{
    public partial class PowderChargeForm : Form, IEditable<List<Powder>>
    {
        private BindingList<Powder> Powders = new();

        public PowderChargeForm()
        {
            InitializeComponent();

            powdersListBox.DataSource = Powders;
        }

        public void SetValues(List<Powder> powders)
        {
            Powders.Clear();

            foreach (var powder in powders)
                Powders.Add(powder);

            Powders.ResetBindings();
            powdersPropertyGrid.SelectedObject = powdersListBox.SelectedItem;
        }

        public List<Powder> GetValues() => Powders.ToList();

        private void deletePowderButton_Click(object sender, EventArgs e)
        {
            Powders.Remove((Powder)powdersListBox.SelectedItem);
            Powders.ResetBindings();
        }

        private void addTubePowderButton_Click(object sender, EventArgs e)
        {
            Powders.Add(new TubePowder());
            Powders.ResetBindings();

            powdersListBox.SelectedIndex = Powders.Count - 1;
            powdersPropertyGrid.SelectedObject = powdersListBox.SelectedItem;
        }

        private void addGrainedPowderButton_Click(object sender, EventArgs e)
        {
            Powders.Add(new GrainedPowder());
            Powders.ResetBindings();

            powdersListBox.SelectedIndex = Powders.Count - 1;
            powdersPropertyGrid.SelectedObject = powdersListBox.SelectedItem;
        }

        private void powdersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            powdersPropertyGrid.SelectedObject = powdersListBox.SelectedItem;
        }

        private void powdersPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Powders.ResetBindings();
        }

        private void powdersListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Up && powdersListBox.SelectedIndex > 0)
            {
                var index = powdersListBox.SelectedIndex;

                var temp = Powders[index];
                Powders[index] = Powders[index - 1];
                Powders[index - 1] = temp;
            }
            else if (e.Control && e.KeyCode == Keys.Down && powdersListBox.SelectedIndex < Powders.Count - 1)
            {
                var index = powdersListBox.SelectedIndex;

                var temp = Powders[index];
                Powders[index] = Powders[index + 1];
                Powders[index + 1] = temp;
            }

            Powders.ResetBindings();
        }
    }
}
