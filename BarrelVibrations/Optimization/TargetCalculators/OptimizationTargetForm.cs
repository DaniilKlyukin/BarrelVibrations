using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    public partial class OptimizationTargetForm : Form, IEditable<OptimizationTargetCalculator>
    {
        public OptimizationTargetForm()
        {
            InitializeComponent();

            comboBox.DataSource = new OptimizationTargetCalculator[]
            {
                new VibrationsAmplitudeOptimizationTarget(),
                new WeightedOptimizationTarget(),
                new ShotsVibrationsSpreadOptimizationTarget(),
                new ShotsWeightedOptimizationTarget(),
                new SpreadOptimizationTarget(),
                new VibrationsIntegralOptimizationTarget()
            };

            propertyGrid.SelectedObject = comboBox.Items[0];
        }

        public OptimizationTargetCalculator GetValues()
        {
            return propertyGrid.SelectedObject as OptimizationTargetCalculator;
        }

        public void SetValues(OptimizationTargetCalculator values)
        {
            var arr = comboBox.DataSource as OptimizationTargetCalculator[];

            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i].GetType().ToString() == values.GetType().ToString())
                {
                    arr[i] = values;
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem is not OptimizationTargetCalculator calculator)
                return;

            propertyGrid.SelectedObject = calculator;
        }
    }
}
