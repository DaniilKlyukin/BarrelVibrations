using BarrelVibrations.Solvers.Solutions;
using BasicLibraryWinForm;

namespace BarrelVibrations.ViewForms
{
    public partial class ShotsParametersView : Form
    {
        public ShotsParametersView()
        {
            InitializeComponent();
        }

        public ShotsParametersView(List<ShotParameters> shotParameters)
        {
            InitializeComponent();
            dataGridView.Rows.Clear();
            dataGridView.Rows.Add(
                "Выстрел",
                "Время, мс",
                "Pсн_max, МПа",
                "Pкн_max, МПа",
                "Vсн, м/с",
                "Горизонтальный угол наклона, град",
                "Вертикальный угол наклона, град",
                "Горизонтальный угол нутации, град",
                "Вертикальный угол нутации, град");

            for (int i = 0; i < shotParameters.Count; i++)
            {
                dataGridView.Rows.Add(
                    i + 1,
                    (shotParameters[i].Time * 1e3).ToString("0.000"),
                    (shotParameters[i].ProjetileMaxPressure / 1e6).ToString("0.000"),
                    (shotParameters[i].MaxPressure / 1e6).ToString("0.000"),
                    shotParameters[i].ProjectileSpeed.ToString("0.000"),
                    Algebra.ConvertRadToGrad(shotParameters[i].HorizontalAngle).ToString("0.000000"),
                    Algebra.ConvertRadToGrad(shotParameters[i].VerticalAngle).ToString("0.000000"),
                    Algebra.ConvertRadToGrad(shotParameters[i].HorizontalRotationAngle).ToString("0.000000"),
                    Algebra.ConvertRadToGrad(shotParameters[i].VerticalRotationAngle).ToString("0.000000"));
            }
        }
    }
}
