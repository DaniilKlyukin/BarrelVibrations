using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;


namespace BarrelVibrations.Optimization
{
    public partial class BarrelOptimizationConstraintForm : Form, IEditable<BarrelOptimizationConstraint>
    {
        public BarrelOptimizationConstraintForm()
        {
            InitializeComponent();
        }

        public BarrelOptimizationConstraint GetValues()
        {
            var barrelX = new double[dataGridView.RowCount - 1];
            var thicknessMins = new double[dataGridView.RowCount - 1];

            for (var i = 0; i < dataGridView.RowCount - 1; i++)
            {
                barrelX[i] = TextWorker.Parse(dataGridView.Rows[i].Cells[0].Value.ToString());
                thicknessMins[i] = 1e-3 * TextWorker.Parse(dataGridView.Rows[i].Cells[1].Value.ToString());
            }

            return new BarrelOptimizationConstraint(
                 barrelX,
                 thicknessMins,
                 (double)maxMassNumericUpDown.Value,
                 outerDNeedDecreaseCheckBox.Checked);
        }

        public void SetValues(BarrelOptimizationConstraint constraint)
        {
            dataGridView.Rows.Clear();

            maxMassNumericUpDown.Value = (decimal)constraint.MaxMass;
            outerDNeedDecreaseCheckBox.Checked = constraint.IsOuterDiameterNeedDecrease;

            var rowsCount = constraint.BarrelX.Length;

            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView.Rows.Add();

                var row = new List<double>
                {
                    constraint.BarrelX[i],
                    constraint.ThicknessMins[i]*1e3,
                };

                for (int j = 0; j < row.Count; j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = row[j];
                }
            }
        }
    }
}
