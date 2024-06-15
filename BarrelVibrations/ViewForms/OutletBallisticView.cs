using System.Text;
using BarrelVibrations.Solvers.OutletBallisticFolder;
using BarrelVibrations.Solvers.Solutions;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations.ViewForms;

public partial class OutletBallisticView : Form
{
    private readonly List<OutletBallistic> outletBallistic;
    private readonly double fireAngle;
    private readonly VisualizationProperties visualizationProperties;
    private readonly List<ITableData> data = new();
    private readonly List<double> distances = new();
    private readonly List<Point> missilesOnTargetCoordinates = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resultName">Имя расчета для отображения</param>
    /// <param name="outletBallistic">результаты решения задачи внешеней баллистики</param>
    /// <param name="fireAngle">исходный угол стрельбы</param>
    /// <param name="plot">объект для отрисовки графиков</param>
    /// <param name="withMarkers">нужны ли точки на графике</param>
    public OutletBallisticView(
        string resultName,
        List<OutletBallistic> outletBallistic,
        double fireAngle,
        FormsPlot plot,
        VisualizationProperties visualizationProperties)
    {
        this.outletBallistic = outletBallistic;
        this.fireAngle = fireAngle;
        Plot = plot;
        this.visualizationProperties = visualizationProperties;
        InitializeComponent();

        shotComboBox.Items.AddRange(Enumerable.Range(1, outletBallistic.Count)
            .Select(i => $"{i}-й выстрел").ToArray());
        shotComboBox.SelectedIndex = 0;
        SelectOutletBallisticResult(0);
        listBox.Items.Clear();

        foreach (var table in data) listBox.Items.Add(table.Name);

        groupBox.Text = @$"{resultName}";
        listBox.SelectedIndex = 0;
    }

    public FormsPlot Plot { get; }

    private void SelectOutletBallisticResult(int shotIndex)
    {
        if (shotIndex >= outletBallistic.Count || shotIndex < 0)
            return;

        var result = outletBallistic[shotIndex];

        data.Clear();

        data.Add(new SingleArrayTable(
            "X снаряда",
            "t, с",
            result.TimeMoments,
            "X, м",
            result.Xs.ToArray()));

        data.Add(new SingleArrayTable(
            "Y снаряда",
            "t, с",
            result.TimeMoments,
            "Y, м",
            result.Ys.ToArray()));

        data.Add(new SingleArrayTable(
            "Z снаряда",
            "t, с",
            result.TimeMoments,
            "Z, м",
            result.Zs.ToArray()));

        data.Add(new SingleArrayTable(
            "Траектория XY",
            "X, м",
            result.Xs.ToArray(),
            "Y, м",
            result.Ys.ToArray()));

        data.Add(new SingleArrayTable(
            "Траектория XZ",
            "X, м",
            result.Xs.ToArray(),
            "Z, м",
            result.Zs.ToArray()));

        data.Add(new SingleArrayTable(
            "Скорость снаряда",
            "t, с",
            result.TimeMoments,
            "v, м/с",
            result.Velocities.ToArray()));

        data.Add(new SingleArrayTable(
            "Горизонтальный угол наклона снаряда",
            "t, с",
            result.TimeMoments,
            "Горизонтальный угол наклона траектории движения, °",
            result.HorizontalMovementAngles.ToArray()));

        data.Add(new SingleArrayTable(
            "Вертикальный угол наклона снаряда",
            "t, с",
            result.TimeMoments,
            "Вертикальный угол наклона траектории движения, °",
            result.VerticalMovementAngles.ToArray()));

        data.Add(new SingleArrayTable(
            "Отклонение угла наклона снаряда от траектории по горизонтали",
            "t, с",
            result.TimeMoments,
            "Отклонение угла наклона снаряда от траектории по горизонтали, °",
            result.HorizontalDeltaAngles.ToArray()));

        data.Add(new SingleArrayTable(
            "Отклонение угла наклона снаряда от траектории по вертикали",
            "t, с",
            result.TimeMoments,
            "Отклонение угла наклона снаряда от траектории по вертикали, °",
            result.VerticalDeltaAngles.ToArray()));

        data.Add(new SingleArrayTable(
            "Вертикальная угловая скорость снаряда",
            "t, с",
            result.TimeMoments,
            "Вертикальная угловая скорость снаряда, рад/с",
            result.VerticalAngularVelocities.ToArray()));

        data.Add(new SingleArrayTable(
            "Горизонтальная угловая скорость снаряда",
            "t, с",
            result.TimeMoments,
            "Горизонтальная угловая скорость снаряда, рад/с",
            result.HorizontalAngularVelocities.ToArray()));

        data.Add(new SingleArrayTable(
            "Угловая скорость",
            "t, с",
            result.TimeMoments,
            "Угловая скорость, рад/с",
            result.AngularVelocities.ToArray()));

        data.Add(new SingleArrayTable(
            "Критерий устойчивости",
            "t, с",
            result.TimeMoments,
            "Критерий устойчивости",
            result.StabilityCriterion));
    }

    private void Draw()
    {
        if (listBox.SelectedIndex < 0 || listBox.SelectedIndex >= data.Count)
            return;

        var table = data[listBox.SelectedIndex];

        var values = table.GetValues();

        if (!table.ArgumentsValues.Any()
            || !values.Any()
            || table.ArgumentsValues.Any(v => !double.IsFinite(v))
            || values.Any(v => !double.IsFinite(v)))
            return;

        Plot.Plot.Clear();

        Plot.Plot.AddScatter(
            table.ArgumentsValues.ToArray(),
            values.ToArray(),
            markerShape: visualizationProperties.PlotMarkers,
            lineWidth: visualizationProperties.PlotLineWidth);

        Plot.Plot.Title("Внешняя баллистика");
        Plot.Plot.XLabel(table.ArgumentsColumnName);
        Plot.Plot.YLabel(table.ValuesColumnName);

        Plot.Render();
    }



    private void DrawSpread(double[] zs, double[] ys, double spreadCenterZ, double spreadCenterY, double spread)
    {
        Plot.Plot.Clear();

        const int areaPoints = 300;

        var az = new double[areaPoints];
        var ay = new double[areaPoints];

        for (int i = 0; i < az.Length; i++)
        {
            var angle = 2 * Math.PI * i / az.Length;

            az[i] = spreadCenterZ + 0.5 * spread * Math.Cos(angle);
            ay[i] = spreadCenterY + 0.5 * spread * Math.Sin(angle);
        }

        Plot.Plot.AddPolygon(
            az,
            ay,
            lineColor: Color.Black,
            fillColor: Color.FromArgb(25, 255, 0, 0),
            lineWidth: 1);

        for (int i = 0; i < zs.Length; i++)
        {
            Plot.Plot.AddMarker(
                zs[i],
                ys[i],
                size: 20,
                label: $"Выстрел {i + 1}");
        }

        for (int i = 0; i < zs.Length - 1; i++)
        {
            Plot.Plot.AddArrow(
                zs[i + 1],
                ys[i + 1],
                zs[i],
                ys[i],
                lineWidth: 2,
                color: Color.Black);
        }

        Plot.Plot.Title("Внешняя баллистика");
        Plot.Plot.Legend();
        Plot.Plot.XLabel("z, м");
        Plot.Plot.YLabel("y, м");

        Plot.Render();
    }


    private void CopyButton_Click(object sender, EventArgs e)
    {
        if (listBox.SelectedIndex >= 0 && listBox.SelectedIndex < data.Count)
            Clipboard.SetText(data[listBox.SelectedIndex].GetString());
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (listBox.SelectedIndex < 0 && listBox.SelectedIndex >= data.Count)
            return;

        var sfd = new SaveFileDialog
        {
            FileName = "результаты.txt",
            DefaultExt = ".txt"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        File.WriteAllText(sfd.FileName, data[listBox.SelectedIndex].GetString(), Encoding.UTF8);
    }

    private void shotComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectOutletBallisticResult(shotComboBox.SelectedIndex);
        Draw();
    }

    private void distanceNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
        if (!outletBallistic.Any() || 
            !outletBallistic.First().Xs.Any() || 
            outletBallistic
                .Select(outlet => outlet.Ys)
                .Any(ys => ys.Any(y => !double.IsFinite(y))))
        {
            MessageBox.Show("Некорректные результаты, проверьте модель", "Ошибка");
            return;
        }

        missilesOnTargetCoordinates.Clear();
        distances.Clear();

        var targetDistance = Convert.ToDouble(distanceNumericUpDown.Value);

        var hitpoints = OutletBallisticSolver.CalculateLineFireHitpoints(
            outletBallistic,
            targetDistance,
            fireAngle).ToArray();

        var zs = hitpoints.Select(p => p.Z).ToArray();
        var ys = hitpoints.Select(p => p.Y).ToArray();

        var spread = OutletBallisticSolver.CalculatePlaneSpread(zs, ys, out var spreadCenterZ, out var spreadCenterY);

        FillHitPointsDataGrid(hitpoints, spread);
        DrawSpread(zs, ys, spreadCenterZ, spreadCenterY, spread);
    }

    private void FillHitPointsDataGrid(IEnumerable<Point> hitpoints, double spread)
    {
        hitpointsDataGridView.Rows.Clear();
        var i = 0;

        hitpointsDataGridView.Rows.Add(
            "Выстрел",
            "X, м",
            "Y, м",
            "Z, м");

        foreach (var hitpoint in hitpoints)
        {
            hitpointsDataGridView.Rows.Add(
                i + 1,
                hitpoint.X.ToString("0.000"),
                hitpoint.Y.ToString("0.000"),
                hitpoint.Z.ToString("0.000"));
            i++;
        }

        hitpointsDataGridView.Rows.Add(
            "Разброс, м:",
            spread.ToString("0.000"));
    }

    private void vibrationsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        SelectOutletBallisticResult(shotComboBox.SelectedIndex);
        Draw();
    }

    private void CopyDistancesButton_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(
            missilesOnTargetCoordinates.Select((p, i) => new[] { p.X, p.Y, p.Z, distances[i] })
                .ToArray()
                .Convert()
                .GetString(new[] { "X, м", "Y, м", "Z, м", "Расстояние до цели, м" }));
    }

    private void SaveDistancesButton_Click(object sender, EventArgs e)
    {
        var sfd = new SaveFileDialog
        {
            FileName = "результаты.txt",
            DefaultExt = ".txt"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        File.WriteAllText(
            sfd.FileName,
            missilesOnTargetCoordinates
                .Select((p, i) => new[] { p.X, p.Y, p.Z, distances[i] })
                .ToArray()
                .Convert()
                .GetString(new[] { "X, м", "Y, м", "Z, м", "Расстояние до цели, м" }),
            Encoding.UTF8);
    }

    private void listBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Draw();
    }
}