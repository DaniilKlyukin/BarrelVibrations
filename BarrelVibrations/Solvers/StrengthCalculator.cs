using BasicLibraryWinForm;

namespace BarrelVibrations.Solvers
{
    public static class StrengthCalculator
    {
        public static double MaxPressure(double innerD, double thickness, double n = 1, double sigma_e = 784e6)
        {
            var outerD = innerD + 2 * thickness;
            var k = FastMath.Pow2(outerD / innerD);

            return 3 * sigma_e / (2 * n) * (k - 1) / (2 * k + 1);
        }

        public static double[] MaxPressures(double[] innerD, double[] thickness, double[] n, double sigma_e = 784e6)
        {
            var pressures = new double[innerD.Length];

            for (int i = 0; i < pressures.Length; i++)
                pressures[i] = MaxPressure(innerD[i], thickness[i], n[i], sigma_e);

            return pressures;
        }

        public static double MinThickness(double innerD, double pressure, double n = 1, double sigma_e = 784e6)
        {
            return innerD / 2 * (Math.Sqrt((1.5 * sigma_e + n * pressure) / (1.5 * sigma_e - 2 * n * pressure)) - 1);
        }

        public static double[] MinThicknesses(
            double[] x, double[] innerD, double[] maxPressures, double[] snPressures, double[] snX, double camoraLength, double sigma_e = 784e6)
        {
            var d = innerD.Last();
            var pMax = snPressures.Max();
            var iMax = Array.IndexOf(snPressures, pMax);
            var xMax = snX[iMax];
            var m = xMax + 1.5 * d;
            var l = x.Last() - 2 * d;

            var thicknesses = new double[innerD.Length];

            for (int i = 0; i < thicknesses.Length; i++)
            {
                double n;
                if (x[i] <= camoraLength)
                    n = 1;
                else if (x[i] <= m)
                    n = 1.3;
                else if (x[i] <= l)
                    n = Algebra.GetValueAtLine(x[i], m, 1.3, l, 1.9);
                else
                    n = 1.9;

                thicknesses[i] = MinThickness(innerD[i], maxPressures[i], n, sigma_e);
            }

            return thicknesses;
        }

        public static double[] MinThicknesses(double[] innerD, double[] pressures, double[] n, double sigma_e = 784e6)
        {
            var thicknesses = new double[innerD.Length];

            for (int i = 0; i < thicknesses.Length; i++)
                thicknesses[i] = MinThickness(innerD[i], pressures[i], n[i], sigma_e);

            return thicknesses;
        }
    }
}
