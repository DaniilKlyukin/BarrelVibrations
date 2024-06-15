using System.Diagnostics;
using System.Text;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms.Common
{
    public partial class TrackBarFileView : Form
    {
        private readonly double[] trackBarValues;
        private readonly double[] argumentsValues;
        private readonly VisualizationProperties visualizationProperties;

        private FormsPlot Plot { get; }
        private List<FileData> fileDatas = new();

        private FileData? selectedData;
        private double[] selectedValues = new double[1];

        public Action<TrackData> TrackingAction { get; set; } = (_) => { };

        public TrackBarFileView(
            string groupBoxName,
            double[] trackBarValues,
            double[] argumentsValues,
            FormsPlot plot,
            VisualizationProperties visualizationProperties)
        {
            Plot = plot;
            this.visualizationProperties = visualizationProperties;
            InitializeComponent();
            groupBox.Text = @$"{groupBoxName}";
            this.trackBarValues = trackBarValues;
            this.argumentsValues = argumentsValues;
            timeTrackBar.Maximum = trackBarValues.Length - 1;
        }

        public void SetData(params FileData[] fileDatas)
        {
            this.fileDatas = fileDatas.ToList();
            selectedData = this.fileDatas.First();
            selectedValues = selectedData.GetFileDataArray(0);
        }

        private void DrawPlot()
        {
            if (selectedData == null)
                return;

            Plot.Plot.Clear();

            Plot.Plot.AddScatter(
                argumentsValues,
                selectedValues,
                markerShape: visualizationProperties.PlotMarkers,
                label: selectedData.Header,
                lineWidth: visualizationProperties.PlotLineWidth);

            Plot.Plot.XLabel(selectedData.ArgumentsHeader);
            Plot.Plot.YLabel(selectedData.DataHeader);

            Plot.Plot.Legend(visualizationProperties.PlotLegend);

            Plot.Render();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            var fileData = fileDatas[listBox.SelectedIndex];

            Clipboard.SetText(TextWorker.GetTableData(
                  (fileData.ArgumentsHeader, argumentsValues),
                  (fileData.DataHeader, selectedValues)));
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var fileData = fileDatas[listBox.SelectedIndex];

            var sfd = new SaveFileDialog
            {
                FileName = $"результаты {fileData.Header}.txt",
                DefaultExt = ".txt"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllText(
                sfd.FileName,
                TextWorker.GetTableData(
                    (fileData.ArgumentsHeader, argumentsValues),
                    (fileData.DataHeader, selectedValues)),
                Encoding.UTF8);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0 || listBox.SelectedIndex >= fileDatas.Count)
                return;

            selectedData = fileDatas[listBox.SelectedIndex];
            selectedValues = selectedData.GetFileDataArray(timeTrackBar.Value);

            if (timeTrackBar.Value < 0 || timeTrackBar.Value >= selectedValues.Length)
                return;

            try
            {
                DrawPlot();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        private void RunAnimationButton_Click(object sender, EventArgs e)
        {
            Task.Run(RunAnimation);
        }

        private void RunAnimation()
        {
            var sw = new Stopwatch();

            var duration = (double)AnimationDurationNumeric.Value;
            var intervals = (int)AnimationIntervalsNumeric.Value;

            var animationDt = (int)Math.Round(1e3 * duration / intervals);

            for (var i = 0; i <= intervals; i++)
            {
                sw.Restart();

                var trackBarIndex = (int)Math.Round((double)i * (trackBarValues.Length - 1) / intervals);

                Invoke(() =>
                {
                    timeTrackBar.Value = trackBarIndex;
                });

                sw.Stop();
                var sleep = animationDt - (int)sw.ElapsedMilliseconds;

                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        private void timeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (timeTrackBar.Value < 0 && timeTrackBar.Value >= trackBarValues.Length || selectedData == null)
                return;

            try
            {
                selectedValues = selectedData.GetFileDataArray(timeTrackBar.Value);

                DrawPlot();

                TrackingAction.Invoke(new TrackData(
                    timeTrackBar.Value,
                    trackBarValues[timeTrackBar.Value],
                    selectedValues,
                    selectedData.DataHeader));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }

            timeLabel.Text = $"t: {trackBarValues[timeTrackBar.Value] * 1e3:0.000} мс";
        }
    }


}
