using System.Text;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms.Common
{
    public partial class BasicView : Form
    {
        public FormsPlot Plot { get; }
        protected List<ITableData> data { get; set; } = new();
        private readonly VisualizationProperties properties;

        public BasicView(string groupBoxName, FormsPlot plot, VisualizationProperties properties)
        {
            Plot = plot;
            this.properties = properties;
            InitializeComponent();
            groupBox.Text = @$"{groupBoxName}";
            StartPosition = FormStartPosition.CenterScreen;
        }

        protected void CopyButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0 && listBox.SelectedIndex < data.Count)
                Clipboard.SetText(data[listBox.SelectedIndex].GetString());
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0 && listBox.SelectedIndex >= data.Count)
                return;

            var sfd = new SaveFileDialog
            {
                FileName = $"результаты {data[listBox.SelectedIndex].Name}.txt",
                DefaultExt = ".txt"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllText(sfd.FileName, data[listBox.SelectedIndex].GetString(), Encoding.UTF8);
        }

        protected void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0 && listBox.SelectedIndex >= data.Count)
                return;

            Plot.Plot.Clear();

            var table = data[listBox.SelectedIndex];

            var array = data[listBox.SelectedIndex].GetValues();
            var matrix = array.Reshape(table.Size1, table.Size2);

            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var values = matrix.GetColumnSlice(j).ToArray();
                var arr = table.ArgumentsValues.ToArray();

                if (values.Length != arr.Length
                    || values.Any(v => !double.IsFinite(v))
                    || !arr.Any(double.IsFinite))
                    continue;

                Plot.Plot.AddScatter(
                    arr,
                    values,
                    markerShape: properties.PlotMarkers,
                    lineWidth: properties.PlotLineWidth,
                    label: $"{j + 1}");

                Plot.Plot.Legend(properties.PlotLegend);
            }

            table.AdditionalDrawMethod(0, Plot);

            Plot.Plot.XLabel(table.ArgumentsColumnName);
            Plot.Plot.YLabel(table.ValuesColumnName);
            Plot.Plot.Title(table.Name);

            Plot.Render();
        }
    }
}
