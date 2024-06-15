using BasicLibraryWinForm;

namespace BarrelVibrations.Solvers.OutletBallisticFolder
{
    public static class FormResistanceCoefficientFinder1958
    {
        private static double[] Machs = { 0.5, 0.8, 0.9, 1.0, 1.1, 1.2, 1.5, 1.75, 2.0, 2.5, 3.0 };
        private static double[] Cx = { 0.306, 0.334, 0.372, 0.354, 0.616, 0.620, 0.558, 0.514, 0.478, 0.416, 0.369 };
        private static Func<double, double> CxFunc = Algebra.GetFunc(Machs, Cx, false);

        public static (double, double, double) GetResistanceCoefficients(double machNumber, double horizontalAngleRad = 0, double verticalAngleRad = 0)
        {
            return (CxFunc(machNumber), 0, 0);
        }
    }
}
