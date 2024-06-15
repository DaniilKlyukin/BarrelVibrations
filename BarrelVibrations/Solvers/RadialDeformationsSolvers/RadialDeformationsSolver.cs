using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BasicLibraryWinForm;
using MathNet.Numerics.Interpolation;
using Environment = BarrelVibrations.ModelingObjects.EnvironmentFolder.Environment;

namespace BarrelVibrations.Solvers.RadialDeformationsSolvers
{
    public class RadialVibrationsSolver
    {
        private readonly Barrel barrel;
        private readonly Environment environment;
        private readonly double[] xs;

        public double[] InnerU => UCurrent.Select(arr => arr[0]).ToArray();
        public double[,] Radiuses { get; }
        public double[][] UCurrent { get; private set; } = new double[0][];
        public double[][] UPrevious { get; private set; } = new double[0][];
        public double[][] UPrevious2 { get; private set; } = new double[0][];

        public int PointI { get; private set; }
        public int PointJ { get; private set; }
        public double PointDeformation { get; private set; }

        public int Iteration { get; private set; }

        public LinearSpline[] TemperaturesPrevious { get; private set; } = Array.Empty<LinearSpline>();

        private double dt, dtPrevious, dt_2;
        private int R => Radiuses.GetLength(1) - 1;

        public RadialVibrationsSolverType SolverType { get; }
        private Action<int, double, double[,]> calculationMethod;
        public bool Calculate { get; }

        private readonly double rho;
        private readonly double a;
        private readonly double nu;
        private readonly double E;
        private readonly double K;

        private readonly double L;
        private readonly double M;
        private readonly double C;

        public RadialVibrationsSolver(
            Barrel barrel,
            Material barrelMaterial,
            Environment environment,
            double[] innerR,
            double[] outerR,
            double rGrowSpeed,
            int rCount,
            RadialVibrationsSolverType solverType,
            bool calculate)
        {
            this.barrel = barrel;
            this.environment = environment;
            xs = barrel.X;
            SolverType = solverType;
            Calculate = calculate;
            UCurrent = new double[xs.Length][];
            UPrevious = new double[xs.Length][];
            UPrevious2 = new double[xs.Length][];

            switch (solverType)
            {
                case RadialVibrationsSolverType.AxisymmetricFreeX:
                    {
                        calculationMethod = CalculateAxisymmetricFreeX;
                    }
                    break;
                case RadialVibrationsSolverType.AxisymmetricFixedX:
                    {
                        calculationMethod = CalculateAxisymmetricFixedX;
                    }
                    break;
                default:
                    calculationMethod = CalculateAxisymmetricFreeX; break;
            }

            E = barrelMaterial.YoungModulus;
            rho = barrelMaterial.Density;
            nu = barrelMaterial.PoissonRatio;
            a = barrelMaterial.LinearThermalExpansion;
            K = E / (1 - nu * nu);

            L = nu * E / ((1 + nu) * (1 - 2 * nu));
            M = E / (2 * (1 + nu));
            C = L + 2 * M;

            Radiuses = new double[xs.Length, rCount];

            for (int i = 0; i < xs.Length; i++)
            {
                UCurrent[i] = new double[rCount];
                UPrevious[i] = new double[rCount];
                UPrevious2[i] = new double[rCount];

                var fixedP = new Dictionary<int, (double, double)>
                {
                    { 0, (innerR[i], rGrowSpeed) },
                    { rCount-1, (outerR[i], 1) }
                };

                Mesh1DCreator.Create(fixedP, Radiuses, i);
            }
        }

        public void SolveIteration(
           double dt,
           double[] p1s,
           double[,] temperatureField)
        {
            if (!Calculate)
                return;

            dtPrevious = this.dt;
            this.dt = dt;

            if (dtPrevious == 0)
                dtPrevious = dt;

            dt_2 = dt * dtPrevious;

            if (Iteration > 1)
            {
                UPrevious2 = UPrevious.Copy();
            }

            if (Iteration > 0)
            {
                UPrevious = UCurrent.Copy();
            }

            Parallel.For(
                0,
                xs.Length,
                i => calculationMethod(i, p1s[i], temperatureField));


            PointDeformation = UCurrent[PointI][PointJ];
            Iteration++;
        }

        private void CalculateAxisymmetricFreeX(int index, double p1, double[,] temperatureField)
        {
            var A = new double[R + 1, R + 1];
            var b = new double[R + 1];

            for (var j = 0; j < R + 1; j++)
            {
                if (j == 0)
                {
                    var dr = Radiuses[index, j + 1] - Radiuses[index, j];
                    var T = 0.0;

                    if (temperatureField != null)
                        T = temperatureField[index, j] - environment.Temperature;

                    A[j, j] = -1.0 / dr + nu / Radiuses[index, j];
                    A[j, j + 1] = 1.0 / dr;

                    b[j] = (-p1 + a * E * T / (1 - nu)) / K;
                }
                else if (j == R)
                {
                    var dr = Radiuses[index, j] - Radiuses[index, j - 1];

                    A[j, j - 1] = -1.0 / dr;
                    A[j, j] = 1.0 / dr + nu / Radiuses[index, j];

                    b[j] = 0;
                }
                else
                {
                    var drR1_2 = (Radiuses[index, j + 1] - Radiuses[index, j - 1]) / 2;
                    var drR1 = Radiuses[index, j + 1] - Radiuses[index, j];
                    var dr = Radiuses[index, j] - Radiuses[index, j - 1];
                    var r = Radiuses[index, j];

                    var dT = 0.0;

                    if (temperatureField != null)
                        dT = temperatureField[index, j + 1] - temperatureField[index, j - 1];

                    A[j, j - 1] = 1.0 / (drR1_2 * dr) - 1.0 / (2 * r * drR1_2);

                    A[j, j] = -(1.0 / (drR1_2 * drR1) + 1.0 / (drR1_2 * dr)
                              + 1.0 / (r * r)
                              + rho / (K * dt_2));

                    A[j, j + 1] = 1.0 / (drR1_2 * drR1) + 1.0 / (2 * r * drR1_2);

                    b[j] = a * E * dT / (K * (1 - nu) * 2 * drR1_2) - rho * (2 * UPrevious[index][j] - UPrevious2[index][j]) / (K * dt_2);
                }
            }

            UCurrent[index] = LinearEquationSolver.SolveDiagonalSystem(A, b, 3);
        }

        private void CalculateAxisymmetricFixedX(int index, double p1, double[,] temperatureField)
        {
            var A = new double[R + 1, R + 1];
            var b = new double[R + 1];

            for (var j = 0; j < R + 1; j++)
            {
                if (j == 0)
                {
                    var dr = Radiuses[index, j + 1] - Radiuses[index, j];

                    var T = 0.0;

                    if (temperatureField != null)
                        T = temperatureField[index, j] - environment.Temperature;

                    A[j, j] = -(1.0 / dr - L / (C * Radiuses[index, j]));
                    A[j, j + 1] = 1.0 / dr;

                    b[j] = (-p1 + a * E * T / (1 - 2 * nu)) / C;
                }
                else if (j == R)
                {
                    var dr = Radiuses[index, j] - Radiuses[index, j - 1];

                    A[j, j - 1] = -1.0 / dr;
                    A[j, j] = 1.0 / dr + L / (C * Radiuses[index, j]);

                    b[j] = 0;
                }
                else
                {
                    var drR1_2 = (Radiuses[index, j + 1] - Radiuses[index, j - 1]) / 2;
                    var drR1 = Radiuses[index, j + 1] - Radiuses[index, j];
                    var dr = Radiuses[index, j] - Radiuses[index, j - 1];
                    var r = Radiuses[index, j];

                    var dT = 0.0;

                    if (temperatureField != null)
                        dT = temperatureField[index, j + 1] - temperatureField[index, j - 1];

                    A[j, j - 1] = 1.0 / (drR1_2 * dr) - 1.0 / (2 * r * drR1_2);

                    A[j, j] = -(1.0 / (drR1_2 * drR1) + 1.0 / (drR1_2 * dr)
                              + 1.0 / (r * r)
                              + rho / (C * dt_2));

                    A[j, j + 1] = 1.0 / (drR1_2 * drR1) + 1.0 / (2 * r * drR1_2);

                    b[j] = a * E * dT / (C * (1 - 2 * nu) * 2 * drR1_2) - rho * (2 * UPrevious[index][j] - UPrevious2[index][j]) / (C * dt_2);
                }
            }

            UCurrent[index] = LinearEquationSolver.SolveDiagonalSystem(A, b, 3);
        }
    }
}
