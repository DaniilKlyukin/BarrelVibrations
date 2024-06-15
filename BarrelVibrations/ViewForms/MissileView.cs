using BarrelVibrations.ModelingObjects.MissileFolder;
using BasicLibraryWinForm;
using ScottPlot;

namespace BarrelVibrations.ViewForms
{
    public partial class MissileView : Form
    {
        public Missile Missile { get; }
        public FormsPlot Plot { get; }

        public MissileView(Missile missile, FormsPlot plot)
        {
            InitializeComponent();

            GroupBox.Text = missile.Name;
            Missile = missile;
            Plot = plot;
        }

        private void DrawMissileButton_Click(object sender, EventArgs e)
        {
            Plot.Plot.Clear();

            var xs = new[]
            {
                0.0,
                Missile.BodyLength,
                Missile.BodyLength + Missile.HeadLength,
                Missile.BodyLength,
                0.0,
                0.0
            }.Mult(1e3).ToArray();

            var ys = new[]
            {
                Missile.Diameter / 2,
                Missile.Diameter / 2,
                0,
                -Missile.Diameter / 2,
                -Missile.Diameter / 2,
                Missile.Diameter / 2
            }.Mult(1e3).ToArray();

            var length = Missile.BodyLength + Missile.HeadLength;
            var diameter = Missile.Diameter;

            Plot.Plot.AddScatter(xs, ys);

            Plot.Plot.XLabel("x, мм");
            Plot.Plot.YLabel("y, мм");

            var dx = Math.Max(length, diameter) * 1e3;
            var dy = Math.Max(length, diameter) * 1e3;

            Plot.Plot.SetAxisLimits(-0.05 * dx, 1.05 * dx, -1.05 * dy / 2, 1.05 * dy / 2);
            Plot.Plot.Title("Снаряд");

            Plot.Refresh();
        }
    }
}
