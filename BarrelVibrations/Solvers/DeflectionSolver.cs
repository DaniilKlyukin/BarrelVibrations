using BarrelVibrations.ModelingObjects;
using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BasicLibraryWinForm;

namespace BarrelVibrations.Solvers
{
    public class DeflectionSolver
    {
        private readonly double E, rho;
        private readonly bool calculateMissileGravity;
        private readonly Barrel barrel;
        private readonly Material material;
        private readonly MuzzleBreak muzzleBreak;
        private readonly ModelingObjects.EnvironmentFolder.Environment environment;
        private readonly double camoraLength;
        private readonly Missile missile;
        private readonly double shotAngle;
        private readonly double g;
        private readonly double[] F = Array.Empty<double>();
        private readonly double[] D = Array.Empty<double>();
        private readonly double[] Jy = Array.Empty<double>();
        private readonly double[] Jz = Array.Empty<double>();
        private readonly double[] Wy = Array.Empty<double>();
        private readonly double[] Wz = Array.Empty<double>();
        private readonly double[] xs = Array.Empty<double>();

        private readonly double[] d2Wy;
        private readonly double[] d2Wz;

        #region Перемещения в локальной СК 

        /// <summary>
        /// Локальные Ox перемещения, м
        /// </summary>
        public double[] XDeflection { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Локальные Oy перемещения, м
        /// </summary>
        public double[] YDeflection { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Локальные Oz перемещения, м
        /// </summary>
        public double[] ZDeflection { get; set; } = Array.Empty<double>();
        #endregion

        public bool Calculate { get; }

        private readonly int I;


        public DeflectionSolver(
            Material material,
            Barrel barrelGeometry,
            MuzzleBreak muzzleBreak,
            Missile missile,
            double shotAngle,
            ModelingObjects.EnvironmentFolder.Environment environment,
            bool calculate,
            bool calculateMissileGravity,
            double g = 9.81)
        {
            this.material = material;
            this.muzzleBreak = muzzleBreak;
            camoraLength = barrelGeometry.CamoraLength;
            this.missile = missile;
            this.shotAngle = shotAngle;
            this.barrel = barrelGeometry;
            F = barrelGeometry.F;
            D = barrelGeometry.OuterD;
            Jy = barrelGeometry.Jy;
            Jz = barrelGeometry.Jz;
            Wy = barrelGeometry.Wy;
            Wz = barrelGeometry.Wz;
            I = barrelGeometry.X.Length - 1;
            xs = barrelGeometry.X;
            E = material.YoungModulus;
            rho = material.Density;
            this.calculateMissileGravity = calculateMissileGravity;

            this.environment = environment;
            this.g = g;
            Calculate = calculate;

            d2Wy = new double[xs.Length];
            d2Wz = new double[xs.Length];

            for (var i = 0; i < d2Wy.Length; i++)
            {
                d2Wy[i] = Algebra.GetDerivative2(xs, Wy, i);
                d2Wz[i] = Algebra.GetDerivative2(xs, Wz, i);
            }

            XDeflection = new double[I + 1];
            YDeflection = new double[I + 1];
            ZDeflection = new double[I + 1];
        }


        public virtual void Solve(double tolerance = 1)
        {
            if (Calculate)
            {
                CalculateF1(out var f1);
                XDeflection = CalculateX(f1);

                CalculateF23(XDeflection, out var fx, out var f2, out var f3);
                YDeflection = CalculateY(fx, f2);
                ZDeflection = CalculateZ(fx, f3);
            }
        }

        private double[] CalculateX(double[] f1)
        {
            var A = new double[I + 1, I + 1];
            var b = new double[I + 1];

            for (var i = 0; i < I + 1; i++)
            {
                if (i == 0 || barrel.FixationsAreas.Any(area => xs[i] >= area.From && xs[i] <= area.To))
                {
                    A[i, i] = 1;

                    b[i] = 0;
                }
                else if (i == I)
                {
                    var FLeftAvg = (F[i] + F[i - 1]) / 2;
                    var dxLeft = xs[i] - xs[i - 1];

                    A[i, i - 1] = E * FLeftAvg / FastMath.Pow2(dxLeft);
                    A[i, i] = -E * FLeftAvg / FastMath.Pow2(dxLeft);

                    b[i] = (3 * f1[i] + f1[i - 1]) / 8 - muzzleBreak.Mass * g * Math.Sin(shotAngle) / dxLeft;
                }
                else
                {
                    var hR1 = xs[i + 1] - xs[i];
                    var h = xs[i] - xs[i - 1];
                    var hR1_2 = (xs[i + 1] - xs[i - 1]) / 2;
                    var FR1_2 = (F[i + 1] + F[i]) / 2;
                    var FL1_2 = (F[i - 1] + F[i]) / 2;

                    var AL = E * FL1_2 / (hR1_2 * h);
                    var AR = E * FR1_2 / (hR1_2 * hR1);

                    A[i, i - 1] = AL;
                    A[i, i] = -(AL + AR);
                    A[i, i + 1] = AR;

                    b[i] = f1[i];
                }
            }

            return LinearEquationSolver.SolveDiagonalSystem(A, b, 3);
        }

        private double[] CalculateY(double[] fx, double[] f2)
        {
            var A = new double[I + 1, I + 1];
            var b = new double[I + 1];

            for (var i = 0; i < I + 1; i++)
            {
                if (i == 0 || barrel.FixationsAreas.Any(area => xs[i] >= area.From && xs[i] <= area.To))
                {
                    A[i, i] = 1;

                    b[i] = 0;
                }
                else if (i == 1)
                {
                    A[i, i - 1] = -1;
                    A[i, i] = 1;

                    b[i] = 0;
                }
                else if (i == I - 1)
                {
                    var hR1_2 = (xs[i + 1] - xs[i - 1]) / 2;
                    var hR1 = xs[i + 1] - xs[i];
                    var h = xs[i] - xs[i - 1];
                    var hL1 = xs[i - 1] - xs[i - 2];
                    var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                    A[i, i - 2] = -E * Jz[i - 1] / (h * hL1 * hL1_2);

                    A[i, i - 1] = E * Jz[i - 1] / (h * hL1 * hL1_2) +
                                    E * Jz[i - 1] / (h * h * hL1_2) +
                                    E * Jz[i] / (h * h * hR1_2) +
                                    E * Jz[i] / (h * hR1 * hR1_2) -
                                    fx[i] / h;

                    A[i, i] = -(E * Jz[i - 1] / (h * h * hL1_2) +
                              E * Jz[i] / (h * h * hR1_2) +
                              E * Jz[i] / (h * hR1 * hR1_2) +
                              E * Jz[i] / (hR1 * h * hR1_2) +
                              E * Jz[i] / (hR1 * hR1 * hR1_2) -
                              fx[i] * (1.0 / hR1 + 1.0 / h));

                    A[i, i + 1] = E * Jz[i] / (hR1 * hR1 * hR1_2) +
                                    E * Jz[i] / (h * hR1 * hR1_2) -
                                    fx[i] / hR1;

                    b[i] = hR1_2 * f2[i];
                }
                else if (i == I)
                {
                    var h = xs[i] - xs[i - 1];
                    var hL1 = xs[i - 1] - xs[i - 2];
                    var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                    A[i, i - 2] = -E * Jz[i - 1] / (h * hL1 * hL1_2);

                    A[i, i - 1] = E * Jz[i - 1] / (h * hL1 * hL1_2) +
                                  E * Jz[i - 1] / (h * h * hL1_2);

                    A[i, i] = -(E * Jz[i - 1] / (h * h * hL1_2));

                    b[i] = 0.5 * h * f2[i];
                }
                else
                {
                    var hR2 = xs[i + 2] - xs[i + 1];
                    var hR1 = xs[i + 1] - xs[i];
                    var h = xs[i] - xs[i - 1];
                    var hL1 = xs[i - 1] - xs[i - 2];
                    var hR1_2 = (xs[i + 1] - xs[i - 1]) / 2;
                    var hR3_2 = (xs[i + 2] - xs[i]) / 2;
                    var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                    A[i, i - 2] = -E * Jz[i - 1] / (h * hL1 * hL1_2);

                    A[i, i - 1] = E * Jz[i - 1] / (h * hL1 * hL1_2) +
                                    E * Jz[i - 1] / (h * h * hL1_2) +
                                    E * Jz[i] / (h * h * hR1_2) +
                                    E * Jz[i] / (h * hR1 * hR1_2) -
                                    fx[i] / h;

                    A[i, i] = -(E * Jz[i - 1] / (h * h * hL1_2) +
                              E * Jz[i] / (h * h * hR1_2) +
                              E * Jz[i] / (h * hR1 * hR1_2) +
                              E * Jz[i] / (hR1 * h * hR1_2) +
                              E * Jz[i] / (hR1 * hR1 * hR1_2) +
                              E * Jz[i + 1] / (hR1 * hR1 * hR3_2) -
                              fx[i] * (1.0 / hR1 + 1.0 / h));

                    A[i, i + 1] = E * Jz[i + 1] / (hR1 * hR2 * hR3_2) +
                                    E * Jz[i + 1] / (hR1 * hR1 * hR3_2) +
                                    E * Jz[i] / (hR1 * hR1 * hR1_2) +
                                    E * Jz[i] / (h * hR1 * hR1_2) -
                                    fx[i] / hR1;

                    A[i, i + 2] = -E * Jz[i + 1] / (hR1 * hR2 * hR3_2);

                    b[i] = hR1_2 * f2[i];
                }
            }

            return LinearEquationSolver.SolveDiagonalSystem(A, b, 5);
        }

        private double[] CalculateZ(double[] fx, double[] f3)
        {
            var A = new double[I + 1, I + 1];
            var b = new double[I + 1];

            for (var i = 0; i < I + 1; i++)
            {
                if (i == 0 || barrel.FixationsAreas.Any(area => xs[i] >= area.From && xs[i] <= area.To))
                {
                    A[i, i] = 1;

                    b[i] = 0;
                }
                else if (i == 1)
                {
                    A[i, i - 1] = -1;
                    A[i, i] = 1;

                    b[i] = 0;
                }
                else if (i == I - 1)
                {
                    var hR1_2 = (xs[i + 1] - xs[i - 1]) / 2;
                    var hR1 = xs[i + 1] - xs[i];
                    var h = xs[i] - xs[i - 1];
                    var hL1 = xs[i - 1] - xs[i - 2];
                    var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                    A[i, i - 2] = -E * Jy[i - 1] / (h * hL1 * hL1_2);

                    A[i, i - 1] = E * Jy[i - 1] / (h * hL1 * hL1_2) +
                                    E * Jy[i - 1] / (h * h * hL1_2) +
                                    E * Jy[i] / (h * h * hR1_2) +
                                    E * Jy[i] / (h * hR1 * hR1_2) -
                                    fx[i] / h;

                    A[i, i] = -(E * Jy[i - 1] / (h * h * hL1_2) +
                              E * Jy[i] / (h * h * hR1_2) +
                              E * Jy[i] / (h * hR1 * hR1_2) +
                              E * Jy[i] / (hR1 * h * hR1_2) +
                              E * Jy[i] / (hR1 * hR1 * hR1_2) -
                              fx[i] * (1.0 / hR1 + 1.0 / h));

                    A[i, i + 1] = E * Jy[i] / (hR1 * hR1 * hR1_2) +
                                    E * Jy[i] / (h * hR1 * hR1_2) -
                                    fx[i] / hR1;

                    b[i] = hR1_2 * f3[i];
                }
                else if (i == I)
                {
                    var h = xs[i] - xs[i - 1];
                    var hL1 = xs[i - 1] - xs[i - 2];
                    var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                    A[i, i - 2] = -E * Jy[i - 1] / (h * hL1 * hL1_2);

                    A[i, i - 1] = E * Jy[i - 1] / (h * hL1 * hL1_2) +
                                    E * Jy[i - 1] / (h * h * hL1_2);

                    A[i, i] = -(E * Jy[i - 1] / (h * h * hL1_2));

                    b[i] = 0.5 * h * f3[i];
                }
                else
                {
                    var hR2 = xs[i + 2] - xs[i + 1];
                    var hR1 = xs[i + 1] - xs[i];
                    var h = xs[i] - xs[i - 1];
                    var hL1 = xs[i - 1] - xs[i - 2];
                    var hR1_2 = (xs[i + 1] - xs[i - 1]) / 2;
                    var hR3_2 = (xs[i + 2] - xs[i]) / 2;
                    var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                    A[i, i - 2] = -E * Jy[i - 1] / (h * hL1 * hL1_2);

                    A[i, i - 1] = E * Jy[i - 1] / (h * hL1 * hL1_2) +
                                    E * Jy[i - 1] / (h * h * hL1_2) +
                                    E * Jy[i] / (h * h * hR1_2) +
                                    E * Jy[i] / (h * hR1 * hR1_2) -
                                    fx[i] / h;

                    A[i, i] = -(E * Jy[i - 1] / (h * h * hL1_2) +
                              E * Jy[i] / (h * h * hR1_2) +
                              E * Jy[i] / (h * hR1 * hR1_2) +
                              E * Jy[i] / (hR1 * h * hR1_2) +
                              E * Jy[i] / (hR1 * hR1 * hR1_2) +
                              E * Jy[i + 1] / (hR1 * hR1 * hR3_2) -
                              fx[i] * (1.0 / hR1 + 1.0 / h));

                    A[i, i + 1] = E * Jy[i + 1] / (hR1 * hR2 * hR3_2) +
                                    E * Jy[i + 1] / (hR1 * hR1 * hR3_2) +
                                    E * Jy[i] / (hR1 * hR1 * hR1_2) +
                                    E * Jy[i] / (h * hR1 * hR1_2) -
                                    fx[i] / hR1;

                    A[i, i + 2] = -E * Jy[i + 1] / (hR1 * hR2 * hR3_2);

                    b[i] = hR1_2 * f3[i];
                }
            }

            return LinearEquationSolver.SolveDiagonalSystem(A, b, 5);
        }

        private void CalculateF1(out double[] f1)
        {
            f1 = new double[xs.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                f1[i] = rho * F[i] * g * Math.Sin(shotAngle);
            }

            if (calculateMissileGravity)
                for (int i = 0; i < xs.Length; i++)
                {
                    if (xs[i] >= camoraLength && xs[i] <= camoraLength + missile.BodyLength)
                    {
                        f1[i] += Math.Sin(shotAngle) * g * missile.Mass / missile.BodyLength;
                    }
                }
        }

        private void CalculateF23(double[] u, out double[] fx, out double[] f2, out double[] f3)
        {
            f2 = new double[xs.Length];
            f3 = new double[xs.Length];
            fx = new double[xs.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                var iR = i == xs.Length - 1 ? i : i + 1;
                var iL = i == 0 ? i : i - 1;

                fx[i] = F[i] * E * (u[iR] - u[iL]) / (xs[iR] - xs[iL]);

                f2[i] =
                    rho * F[i] * g * Math.Cos(shotAngle)
                    - fx[i] * d2Wy[i];

                f3[i] = -fx[i] * d2Wz[i];
            }

            if (calculateMissileGravity)
                for (int i = 0; i < xs.Length; i++)
                {
                    if (xs[i] >= camoraLength && xs[i] <= camoraLength + missile.BodyLength)
                    {
                        f2[i] += Math.Cos(shotAngle) * g * missile.Mass / missile.BodyLength;
                    }
                }

            f2[^1] += 2 * muzzleBreak.Mass * g * Math.Cos(shotAngle) / (xs[^1] - xs[^2]); // умножаем на 2 т.к. 0,5f
        }
    }
}
