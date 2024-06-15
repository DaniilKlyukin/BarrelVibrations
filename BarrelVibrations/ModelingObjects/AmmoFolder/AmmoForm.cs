using BarrelVibrations.Solvers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarrelVibrations.ModelingObjects.AmmoFolder
{
    public partial class AmmoForm : Form
    {
        private readonly List<Ammo> ammo = new();

        public AmmoForm()
        {
            InitializeComponent();
        }

        public AmmoForm(List<Ammo> ammo)
        {
            InitializeComponent();
            this.ammo = ammo;

            UpdateAmmoList();
        }

        private void AddAmmoButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Не задано название снаряда!", "Внимание!");
                return;
            }

            var a = new Ammo() { Name = NameTextBox.Text };
            ammo.Add(a);

            UpdateAmmoList();
        }

        private void AmmoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AmmoListBox.SelectedIndex < 0 || AmmoListBox.SelectedIndex >= ammo.Count)
                return;

            NameTextBox.Text = ammo[AmmoListBox.SelectedIndex].Name;
            MissilePropertyGrid.SelectedObject = ammo[AmmoListBox.SelectedIndex].Missile;
            PowderChargePropertyGrid.SelectedObject = ammo[AmmoListBox.SelectedIndex].PowderCharge;
        }

        private void NameTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (AmmoListBox.SelectedIndex < 0 || AmmoListBox.SelectedIndex >= ammo.Count)
                    return;

                ammo[AmmoListBox.SelectedIndex].Name = NameTextBox.Text;

                UpdateAmmoList();
            }
        }

        private void DeleteAmmoButton_Click(object sender, EventArgs e)
        {
            if (AmmoListBox.SelectedIndex < 0 || AmmoListBox.SelectedIndex >= ammo.Count)
                return;

            ammo.RemoveAt(AmmoListBox.SelectedIndex);
            UpdateAmmoList();
        }

        private void UpdateAmmoList()
        {
            AmmoListBox.Items.Clear();

            foreach (var a in ammo)
            {
                AmmoListBox.Items.Add(a);
            }
        }

        private void MeshMissileButton_Click(object sender, EventArgs e)
        {
            if (AmmoListBox.SelectedIndex < 0 || AmmoListBox.SelectedIndex >= ammo.Count)
                return;

            var missile = ammo[AmmoListBox.SelectedIndex].Missile;

            missile.InitializeMissileProperties(missile.Diameter / missile.MeshQuality);
        }

        private void AmmoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ammo.Any())
            {
                var result = MessageBox.Show("Необходимо добавить хотя бы 1 боеприпас", "Внимание!", MessageBoxButtons.OKCancel);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (ammo.Any(a => !a.Missile.Initialized))
            {
                var result = MessageBox.Show("Не для всех снарядов рассчитаны физические параметры!", "Внимание!", MessageBoxButtons.OKCancel);
                
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
