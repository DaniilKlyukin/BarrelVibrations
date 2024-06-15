using BasicLibraryWinForm;
using MathNet.Numerics;

namespace BasicLibraryWinForm
{
    public static class PolynomialExtensions
    {
        public static Polynomial Pow(this Polynomial p, int power)
        {
            var result = new Polynomial(p.Coefficients);

            for (var i = 1; i < power; i++)
            {
                result *= p;
            }

            return result;
        }

        public static Polynomial Differentiate(this Polynomial p, int degree)
        {
            var result = new Polynomial(p.Coefficients);

            for (var i = 0; i < degree; i++)
            {
                result = result.Differentiate();
            }

            return result;
        }
    }

    public class GaussLegendreIntegral
    {
        public Polynomial Polynomial { get; }

        public double[] Ksi { get; }

        public double[] W { get; }

        public static double Factorial(int num)
        {
            var f = 1;

            for (var i = 2; i <= num; i++)
            {
                f *= i;
            }

            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Степень</param>
        /// <exception cref="ArgumentException"></exception>
        public GaussLegendreIntegral(int n)
        {
            if (n == 0)
                throw new ArgumentException();

            if (n == 1)
            {
                Polynomial = new Polynomial(1, 0);
                Ksi = new double[] { 0 };
                W = new double[] { 2 };
                return;
            }

            var p0 = new Polynomial(-1, 0, 1);

            Polynomial = p0.Pow(n).Differentiate(n) / (Math.Pow(2, n) * Factorial(n));
            var pNext = p0.Pow(n + 1).Differentiate(n + 1) / (Math.Pow(2, n + 1) * Factorial(n + 1));

            Ksi = Polynomial.Roots().Select(c => c.Real).OrderBy(x => x).ToArray();
            W = new double[Ksi.Length];

            for (var i = 0; i < W.Length; i++)
            {
                var value = pNext.Evaluate(Ksi[i]);

                W[i] = 2.0 * (1 - FastMath.Pow2(Ksi[i])) / (FastMath.Pow2(n + 1) * FastMath.Pow2(value));
            }
        }

        public double Integrate(double x0, double x1, Func<double, double> f)
        {
            var detJ = Math.Abs(x1 - x0) / 2;

            var integral = 0.0;

            for (int i = 0; i < W.Length; i++)
            {
                var x = (x1 - x0) * (Ksi[i] + 1) / 2 + x0;
                integral += W[i] * detJ * f(x);
            }

            return integral;
        }
    }
}
