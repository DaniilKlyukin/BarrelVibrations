using BasicLibraryWinForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls
{
    public partial class HeatBar : UserControl
    {
        #region Variables

        private double min = 0;
        private double max = 1;

        #endregion

        private double pictureBoxLabelHeight => HeatMapPictureBox.Height * 0.9;
        private double pictureBoxLabelMargin => (HeatMapPictureBox.Height - pictureBoxLabelHeight) / 2;

        private readonly Label[] _colorMapValuesLabels = new Label[11];

        #region Properties
        [Description("Measure units"), Category("Appearance")]
        public string MeasureUnits
        {
            get => MeasurementUnitsLabel.Text;
            set
            {
                MeasurementUnitsLabel.Text = value;
                Invalidate();
            }
        }

        [Description("Min"), Category("Appearance")]
        public double Min
        {
            get => min;
            set
            {
                min = value;
                SetValuesLabels();
                Invalidate();
            }
        }

        [Description("Max"), Category("Appearance")]
        public double Max
        {
            get => max;
            set
            {
                max = value;
                SetValuesLabels();
                Invalidate();
            }
        }
        #endregion

        public void SetLimits(double min, double max)
        {
            if (max < min)
            {
                this.min = 0;
                this.max = 1;
            }

            this.min = min;
            this.max = max;
            SetValuesLabels();
        }

        public HeatBar()
        {
            _colorMapValuesLabels = new Label[11];

            for (var i = 0; i < _colorMapValuesLabels.Length; i++)
            {
                var label = new Label { Text = "", Location = new System.Drawing.Point(5, 0), Margin = Padding.Empty, Padding = Padding.Empty };

                _colorMapValuesLabels[i] = label;
            }

            InitializeComponent();

            foreach (var label in _colorMapValuesLabels)
            {
                HeatMapLabelsPanel.Controls.Add(label);
            }

            SetValuesLabels();
            DrawHeatMapBitmap();
        }

        private void SetValuesLabels()
        {
            var dy = pictureBoxLabelHeight / (_colorMapValuesLabels.Length - 1);

            for (var i = 0; i < _colorMapValuesLabels.Length; i++)
            {
                var y = pictureBoxLabelHeight - i * dy;
                var value = Max - (Max - Min) * y / pictureBoxLabelHeight;
                _colorMapValuesLabels[i].Text = $@"{value:E3}";
            }
        }

        private void SetLocationsLabels()
        {
            var dy = pictureBoxLabelHeight / (_colorMapValuesLabels.Length - 1);

            for (var i = 0; i < _colorMapValuesLabels.Length; i++)
            {
                var y = pictureBoxLabelMargin + pictureBoxLabelHeight - i * dy - _colorMapValuesLabels[i].Height / 2.0;
                _colorMapValuesLabels[i].Location = new System.Drawing.Point(5, (int)y);
            }
        }

        private void DrawHeatMapBitmap()
        {
            if (HeatMapPictureBox.Height <= 0 || HeatMapPictureBox.Width <= 0)
                return;

            HeatMapPictureBox.Image = new Bitmap(HeatMapPictureBox.Width, HeatMapPictureBox.Height);

            for (var y = 0; y < pictureBoxLabelHeight; y++)
            {
                for (var x = 0; x < HeatMapPictureBox.Width; x++)
                {
                    ((Bitmap)HeatMapPictureBox.Image).SetPixel(x, (int)(y + pictureBoxLabelMargin),
                        Algebra.GetHeatColor(pictureBoxLabelHeight - 1 - y + pictureBoxLabelMargin, 0, pictureBoxLabelHeight + pictureBoxLabelMargin - 1));
                }
            }
        }

        private void HeatBar_SizeChanged(object sender, EventArgs e)
        {
            SetLocationsLabels();
            DrawHeatMapBitmap();
            Invalidate();
        }

        private void HeatBar_DockChanged(object sender, EventArgs e)
        {
            SetLocationsLabels();
            DrawHeatMapBitmap();
            Invalidate();
        }
    }
}
