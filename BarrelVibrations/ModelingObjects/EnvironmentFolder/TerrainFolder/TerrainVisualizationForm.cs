using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.ColorMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    public partial class TerrainVisualizationForm : Form
    {
        private readonly Terrain terrain;
        private readonly double altitudeMin;
        private readonly double altitudeMax;

        public TerrainVisualizationForm()
        {
            InitializeComponent();
        }

        public TerrainVisualizationForm(Terrain terrain)
        {
            InitializeComponent();
            this.terrain = terrain;
            altitudeMin = terrain.AltitudeMap.Min();
            altitudeMax = terrain.AltitudeMap.Max();
        }

        private void TerrainVisualizationForm_Load(object sender, EventArgs e)
        {
            Draw();
            heatBar.SetLimits(altitudeMin, altitudeMax);
        }

        private void Draw()
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);

            var bmp = (Bitmap)pictureBox.Image;

            for (var x = 0; x < bmp.Width; x++)
            {
                for (var z = 0; z < bmp.Height; z++)
                {
                    var xTerrain = terrain.Xs[0] + x * (terrain.Xs[^1] - terrain.Xs[0]) / (bmp.Width - 1);
                    var zTerrain = terrain.Zs[0] + z * (terrain.Zs[^1] - terrain.Zs[0]) / (bmp.Height - 1);

                    var y = terrain.GetAltitude(xTerrain, zTerrain);

                    bmp.SetPixel(x, z, Rainbow.Map(y, altitudeMin, altitudeMax));
                }
            }
        }

        private void TerrainVisualizationForm_ResizeEnd(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
