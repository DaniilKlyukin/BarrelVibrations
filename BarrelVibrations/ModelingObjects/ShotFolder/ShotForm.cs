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
            var interval = (double)shotsIntervalNumericUpDown.Value;
            var spreadPcnt = (double)shotTimeSpreadNumericUpDown.Value;

            var t0 = -interval;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value != null)
                    t0 = double.Parse(row.Cells[0].Value.ToString());
            }

            for (var i = 0; i < shotsCount; i++)
            {
                var dt = interval * spreadPcnt / 100;
                var spreadTime = i == 0 ? 0 : Algebra.GetRandomValue(-dt, dt);
                dataGridView.Rows.Add((t0 + (i + 1) * interval + spreadTime).ToString(), ammo[0].Id);
            }
        }
    }
}
