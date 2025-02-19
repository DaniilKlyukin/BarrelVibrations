using BarrelVibrations.ModelingObjects.AmmoFolder;
using BasicLibraryWinForm;
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

namespace BarrelVibrations.ModelingObjects.ShotFolder
{
    public partial class ShotForm : Form, IEditable<List<Shot>>
    {
        private class AmmoItem
        {
            public int Id { get; set; }
            public Ammo Ammo { get; set; }

            public string Name => Ammo.ToString();
        }

        private static List<AmmoItem> ammo = new();

        public static void SetAmmo(List<Ammo> ammo)
        {
            ShotForm.ammo.Clear();

            for (var i = 0; i < ammo.Count; i++)
            {
                ShotForm.ammo.Add(new AmmoItem { Id = i, Ammo = ammo[i] });
            }
        }

        public ShotForm()
        {
            InitializeComponent();
        }

        public List<Shot> GetValues()
        {
            var shots = new List<Shot>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value is null)
                    break;

                var time = double.Parse(row.Cells[0].Value.ToString());

                var id = (int)row.Cells[1].Value;

                var ammo = ShotForm.ammo.Single(a => a.Id == id);

                shots.Add(new Shot
                {
                    ShotTimeMoment = time,
                    AmmoIndex = ShotForm.ammo.IndexOf(ammo)
                });
            }

            return shots;
        }

        public void SetValues(List<Shot> values)
        {
            dataGridView.Rows.Clear();

            for (var i = 0; i < values.Count; i++)
            {
                dataGridView.Rows.Add(values[i].ShotTimeMoment, ammo[values[i].AmmoIndex].Id);
            }
        }

        private void ShotForm_Load(object sender, EventArgs e)
        {
            var comboBox = (DataGridViewComboBoxColumn)dataGridView.Columns["AmmoColumn"];

            comboBox.Items.Clear();

            foreach (var a in ammo)
            {
                comboBox.Items.Add(a);
            }

            comboBox.ValueMember = "Id";
            comboBox.DisplayMember = "Name";
        }

        private void SetShotsButton_Click(object sender, EventArgs e)
        {
            var shotsCount = (int)shotsCountNumericUpDown.Value;
            var seriesCount = (int)seriesCountNumericUpDown.Value;

            var shotsInterval = (double)shotsIntervalNumericUpDown.Value;
            var seriesInterval = (double)seriesIntervalNumericUpDown.Value;

            var spreadPcnt = (double)shotTimeSpreadNumericUpDown.Value;

            var t0 = 0.0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value != null)
                    t0 = double.Parse(row.Cells[0].Value.ToString()) + seriesInterval;
            }

            var spreadRange = shotsInterval * spreadPcnt / 100;

            for (int i = 0; i < seriesCount; i++)
            {
                for (var j = 0; j < shotsCount; j++)
                {
                    var spreadTime = j == 0 ? 0 : Algebra.GetRandomValue(-spreadRange, spreadRange);

                    var shotTime = t0 + (i * (seriesInterval + shotsInterval * (shotsCount-1))) + j * shotsInterval + spreadTime;

                    dataGridView.Rows.Add(shotTime.ToString("0.########"), ammo[0].Id);
                }
            }
        }
    }
}
