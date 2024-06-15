using System.ComponentModel;
using System.Diagnostics;
using Visualization.OpenGL;

namespace Visualization
{
    public partial class VisualizationMenu : Form
    {
        public double AccelerationVideo => Properties.VideoAcceleration;

        public int TimeIndex => timeMomentTrackBar.Value;

        private double[] TimeMoments { get; }
        private List<Dictionary<int, double>> VisualizationData { get; }
        private string MeasureUnits { get; }
        public double ValueMultiplier { get; }

        private readonly BackgroundWorker visualizationWorker;
        private readonly OpenGLDrawer _glDrawer;
        private VisualizationMenuProperties Properties;
        public VisualizationMenu(OpenGLDrawer glDrawer, string name, double[] timeMoments, List<Dictionary<int, double>> visualizationData, string measureUnits, double valueMultiplier)
        {
            InitializeComponent();
            VisualizationData = visualizationData;
            TimeMoments = timeMoments;
            MeasureUnits = measureUnits;
            ValueMultiplier = valueMultiplier;
            timeMomentTrackBar.Maximum = VisualizationData.Count - 1;
            Text = name;

            Properties = new VisualizationMenuProperties();
            propertyGrid.SelectedObject = Properties;

            _glDrawer = glDrawer;
            visualizationWorker = new BackgroundWorker
            { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

            var watcher = new Stopwatch();

            visualizationWorker.DoWork += (_, a) =>
            {
                var delay = (TimeMoments.Last() - TimeMoments.First()) / (AccelerationVideo * timeMomentTrackBar.Maximum) * 1000;

                var startValue = (int)(a.Argument ?? 0);

                for (var time = startValue; time < VisualizationData.Count; time++)
                {
                    watcher.Reset();
                    watcher.Start();

                    var time1 = time;
                    Invoke(new Action(() => timeMomentTrackBar.Value = time1));

                    if (timeMomentTrackBar.Maximum == 0)
                        visualizationWorker.ReportProgress(100, (time, VisualizationData));
                    else
                        visualizationWorker.ReportProgress(time * 100 / timeMomentTrackBar.Maximum, (time, VisualizationData));

                    watcher.Stop();
                    if (visualizationWorker.CancellationPending)
                    {
                        a.Cancel = true;
                        return;
                    }

                    if (watcher.ElapsedMilliseconds < delay)
                        Thread.Sleep((int)(delay - watcher.ElapsedMilliseconds));
                }
            };

            visualizationWorker.ProgressChanged += (s, a) =>
            {
                if (a.UserState is not (int time, List<Dictionary<long, double>> values))
                    return;

                Draw();
            };

            visualizationWorker.RunWorkerCompleted += (s, a) =>
            {
                startVisualizationButton.Enabled = true;
            };

            Draw();
        }

        private void startVisualizationButton_Click(object sender, EventArgs e)
        {
            if (!visualizationWorker.IsBusy)
                visualizationWorker.RunWorkerAsync(TimeIndex);
        }

        private void stopVisualizationButton_Click(object sender, EventArgs e)
        {
            visualizationWorker.CancelAsync();
        }

        private void MoveToStartAnimation_Click(object sender, EventArgs e)
        {
            if (!visualizationWorker.IsBusy)
                visualizationWorker.RunWorkerAsync(0);
        }

        private void FirstTimeMomentButton_Click(object sender, EventArgs e)
        {
            timeMomentTrackBar.Value = timeMomentTrackBar.Minimum;
        }

        private void LastTimeMomentButton_Click(object sender, EventArgs e)
        {
            timeMomentTrackBar.Value = timeMomentTrackBar.Maximum;
        }

        private void timeMomentTrackBar_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            double? min = null;
            double? max = null;

            if (Properties.FixedLimits)
            {
                min = Properties.MinValue;
                max = Properties.MaxValue;
            }

            timeMomentLabel.Text = $@"Момент времени t = {1000 * TimeMoments[TimeIndex]:0.0} мс";

            _glDrawer.SetColorsToSurfaces(VisualizationData[TimeIndex], MeasureUnits, min, max, ValueMultiplier);
        }
    }
}
