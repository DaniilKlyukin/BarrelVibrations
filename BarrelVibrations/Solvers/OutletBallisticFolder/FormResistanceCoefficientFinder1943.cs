using BasicLibraryWinForm;

namespace BarrelVibrations.Solvers.OutletBallisticFolder
{
    public static class FormResistanceCoefficientFinder1943
    {
        private static double[] Machs = { 0.5, 0.8, 0.9, 1.0, 1.1, 1.2, 1.5, 1.75, 2.0, 2.5, 3.0 };
        private static double[] Cx = { 0.157, 0.159, 0.183, 0.326, 0.378, 0.384, 0.361, 0.337, 0.317, 0.288, 0.27 };
        private static Func<double, double> CxFunc = Algebra.GetFunc(Machs, Cx, false);

        public static (double, double, double) GetResistanceCoefficients(double machNumber, double horizontalAngleRad = 0, double verticalAngleRad = 0)
        {
            return (CxFunc(machNumber), 0, 0);
        }
    }
}
