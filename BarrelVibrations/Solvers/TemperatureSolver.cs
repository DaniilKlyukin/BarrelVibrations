using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BasicLibraryWinForm;
using MathNet.Numerics.Interpolation;

namespace BarrelVibrations.Solvers
{
    public class TemperatureSolver
    {
        public int Iteration { get; private set; }

        public int PointMaxTemperatureI { get; private set; }
        public int PointMaxTemperatureJ { get; private set; }
        public double PointMaxTemperature { get; private set; }

        public double AverageTemperatureInnerSurface { get; private set; }
        public double AverageTemperatureOuterSurface { get; private set; }

        public int PointI { get; private set; }
        public int PointJ { get; private set; }
        public double PointTemperature { get; private set; }

        public double[,] MaxTemperatures { get; private set; } = new double[0, 0];
        public double[,] RadiusesTotal { get; private set; } = new double[0, 0]; // радиусы по всей толщине ствола
        public double[,] TemperaturesCurrent { get; private set; } = new double[0, 0];
        public double[,] TemperaturesPrevious { get; private set; } = new double[0, 0];

        private readonly Barrel barrel;
        private Material material;

        private int[] RWavePrevious;
        private int[] RWaveCurrent;
        private int[] RMaxCurrent;

        public bool Calculate { get; private set; }

        private int RMax;
        private double lambda => material.HeatConductivity;
        private double c => material.HeatCapacity;
        private double rho => material.Density;

        private double k => c * rho / lambda;
        private double dt;


        private readonly double initialTemperature;

        private double RAreaSize { get; set; }
        private double[] OuterR { get; set; }
        private double[] InnerR { get; set; }

        public TemperatureSolver(
            Barrel barrel,
            Material material,
            double[] innerR,
            double[] outerR,
            double rGrowSpeed,
            int maxRCount,
            double rAreaSize,
            double initialTemperature,
            bool calculate)
        {
            this.barrel = barrel;
            this.material = material;
            RAreaSize = rAreaSize;
            RMax = maxRCount;

            PointI = 0;
            PointJ = 0;

            TemperaturesCurrent = new double[barrel.X.Length, RMax + 1];
            TemperaturesPrevious = new double[barrel.X.Length, RMax + 1];

            this.initialTemperature = initialTemperature;

            PointMaxTemperature = initialTemperature;
            PointTemperature = initialTemperature;
            AverageTemperatureInnerSurface = initialTemperature;
            AverageTemperatureOuterSurface = initialTemperature;
            Iteration = 0;
            Calculate = calculate;

            InnerR = innerR.Copy();
            OuterR = outerR.Copy();

            MaxTemperatures = new double[barrel.X.Length, RMax + 1];
            RadiusesTotal = new double[barrel.X.Length, RMax + 1];
            RWavePrevious = new int[barrel.X.Length];
            RWaveCurrent = new int[barrel.X.Length];
            RMaxCurrent = new int[barrel.X.Length];

            for (var i = 0; i < barrel.X.Length; i++)
            {
                var fixedP = new Dictionary<int, (double, double)>
                {
                    { 0, (InnerR[i], rGrowSpeed) },
                    { RadiusesTotal.GetLength(1) - 1, (OuterR[i], 1) }
                };

                Mesh1DCreator.Create(fixedP, RadiusesTotal, i);

                for (var j = 0; j < RadiusesTotal.GetLength(1); j++)
                {
                    if (RadiusesTotal[i, j] >= InnerR[i] + RAreaSize)
                    {
                        RWavePrevious[i] = RWaveCurrent[i] = RMaxCurrent[i] = j;
                        break;
                    }
                }

                if (RMaxCurrent[i] == 0)
                {
                    RWavePrevious[i] = RWaveCurrent[i] = RMaxCurrent[i] = RadiusesTotal.GetLength(1) - 1;
                }
            }

            for (var i = 0; i < MaxTemperatures.GetLength(0); i++)
            {
                for (var j = 0; j < MaxTemperatures.GetLength(1); j++)
                {
                    TemperaturesCurrent[i, j] = initialTemperature;
                    TemperaturesPrevious[i, j] = initialTemperature;
                    MaxTemperatures[i, j] = initialTemperature;
                }
            }
        }

        public void SolveIteration(
            double dt,
            double[] gasTemperatures, double[] gasHeatTransfers,
            double environmentTemperature, double environmentHeatTransfer)
        {
            this.dt = dt;

            if (material == null || !Calculate)
                return;

            if (Iteration > 0)
            {
                TemperaturesPrevious = TemperaturesCurrent.Copy();
            }
            
            Parallel.For(
            0,
            TemperaturesCurrent.GetLength(0),
            i =>
            {
                SolveIteration(i, dt, gasTemperatures[i], gasHeatTransfers[i], environmentTemperature, environmentHeatTransfer);
                SetTemperatureWaveIndex(i);
            });
            
            /*
            for (var i = 0; i < TemperaturesCurrent.GetLength(0); i++)
            {
                SolveIteration(i, dt, gasTemperatures[i], gasHeatTransfers[i], environmentTemperature, environmentHeatTransfer);
                SetTemperatureWaveIndex(i);
            }
            */
            var tasks = new Task[] {
                Task.Run(CalculateEpures),
                Task.Run(CalculateAverageSurfaceTemperature),
            };

            Task.WaitAll(tasks);

            Iteration++;
        }


        private void SolveIteration(
            int index,
            double dt,
            double gasTemperature, double gasHeatTransfer,
            double environmentTemperature, double environmentHeatTransfer)
        {
            if (material == null)
                return;

            var R = RMaxCurrent[index];

            var A = new double[R + 1, R + 1];
            var b = new double[R + 1];

            environmentHeatTransfer *= barrel.CoolingCoefficient[index];

            for (var j = 0; j < b.Length; j++)
            {
                if (j == 0)
                {
                    /*
                    var dr = RadiusesTotal[index, j + 1] - RadiusesTotal[index, j];

                    A[0, 0] = lambda / dr + gasHeatTransfer;
                    A[0, 1] = -lambda / dr;

                    b[0] = gasTemperature * gasHeatTransfer;*/

                    var r0 = RadiusesTotal[index, j];
                    var r1 = RadiusesTotal[index, j + 1];

                    A[j, j] = c * rho / dt + 4 * lambda * (r0 + r1) / (FastMath.Pow2(r1 - r0) * (3 * r0 + r1)) + 8 * gasHeatTransfer * r0 / ((r1 - r0) * (3 * r0 + r1));

                    A[j, j + 1] = -4 * lambda * (r0 + r1) / (FastMath.Pow2(r1 - r0) * (3 * r0 + r1));

                    b[j] = c * rho / dt * TemperaturesPrevious[index, j] + 8 * gasHeatTransfer * gasTemperature * r0 / ((r1 - r0) * (3 * r0 + r1));

                }
                else if (j == R)
                {
                    if (R == RMax)
                    {/*
                        var dr = RadiusesTotal[index, j] - RadiusesTotal[index, j - 1];

                        A[R, R - 1] = -lambda / dr;
                        A[R, R] = lambda / dr + environmentHeatTransfer;

                        b[R] = environmentTemperature * environmentHeatTransfer;*/

                        var r1 = RadiusesTotal[index, j];
                        var r0 = RadiusesTotal[index, j - 1];

                        A[j, j - 1] = -4 * lambda * (r0 + r1) / (FastMath.Pow2(r1 - r0) * (3 * r0 + r1));

                        A[j, j] = c * rho / dt + 4 * lambda * (r0 + r1) / (FastMath.Pow2(r1 - r0) * (3 * r0 + r1)) + 8 * environmentHeatTransfer * r1 / ((r1 - r0) * (3 * r0 + r1));


                        b[j] = c * rho / dt * TemperaturesPrevious[index, j] + 8 * environmentHeatTransfer * environmentTemperature * r1 / ((r1 - r0) * (3 * r0 + r1));
                    }
                    else
                    {
                        /*
                        A[R, R - 1] = -1;
                        A[R, R] = 1;

                        b[R] = 0;*/

                        var r1 = RadiusesTotal[index, j];
                        var r0 = RadiusesTotal[index, j - 1];

                        A[j, j - 1] = -4 * lambda * (r0 + r1) / (FastMath.Pow2(r1 - r0) * (3 * r0 + r1));

                        A[j, j] = c * rho / dt + 4 * lambda * (r0 + r1) / (FastMath.Pow2(r1 - r0) * (3 * r0 + r1));


                        b[j] = c * rho / dt * TemperaturesPrevious[index, j];
                    }
                }
                else
                {
                    var rMidAvg = (RadiusesTotal[index, j + 1] + 2 * RadiusesTotal[index, j] + RadiusesTotal[index, j - 1]) / 4;
                    var rRightAvg = (RadiusesTotal[index, j + 1] + RadiusesTotal[index, j]) / 2;
                    var rLeftAvg = (RadiusesTotal[index, j] + RadiusesTotal[index, j - 1]) / 2;

                    var drRight = RadiusesTotal[index, j + 1] - RadiusesTotal[index, j];
                    var drLeft = RadiusesTotal[index, j] - RadiusesTotal[index, j - 1];
                    var drMidAvg = (RadiusesTotal[index, j + 1] - RadiusesTotal[index, j - 1]) / 2;

                    A[j, j - 1] = rLeftAvg / (rMidAvg * drMidAvg * drLeft);

                    A[j, j] = -(k / dt + rRightAvg / (rMidAvg * drMidAvg * drRight) + rLeftAvg / (rMidAvg * drMidAvg * drLeft));

                    A[j, j + 1] = rRightAvg / (rMidAvg * drMidAvg * drRight);

                    b[j] = -TemperaturesPrevious[index, j] * k / dt;
                }
            }

            var T = LinearEquationSolver.SolveDiagonalSystem(A, b, 3);

            for (var r = 0; r < R + 1; r++)
                TemperaturesCurrent[index, r] = T[r];
        }

        private void SetTemperatureWaveIndex(int layerIndex, double tolerance = 1e-6, int stepsReserve = 2)
        {
            if (RWaveCurrent[layerIndex] == RMax)
                return;

            var rWaveNew = RWaveCurrent[layerIndex];

            for (int j = RMax; j >= RWaveCurrent[layerIndex]; j--)
            {
                var T = TemperaturesCurrent[layerIndex, j];

                if (Math.Abs(1 - initialTemperature / T) * 100 >= tolerance)
                {
                    rWaveNew = j;
                    break;
                }
            }

            var rPrevious = RadiusesTotal[layerIndex, RWavePrevious[layerIndex]];
            var rCurrent = RadiusesTotal[layerIndex, RWaveCurrent[layerIndex]];
            var rNew = RadiusesTotal[layerIndex, rWaveNew];

            var vCurrent = (rNew - rCurrent) / dt;
            var vPrevious = (rCurrent - rPrevious) / dt;

            var aCurrent = (vCurrent - vPrevious) / dt;

            var rNext = rCurrent + vCurrent * dt + aCurrent * FastMath.Pow2(dt);

            var rNextIndex = 0;

            for (var r = rWaveNew; r <= RMax; r++)
            {
                rNextIndex = r;
                if (RadiusesTotal[layerIndex, r] > rNext)
                    break;
            }

            RWavePrevious[layerIndex] = RWaveCurrent[layerIndex];
            RWaveCurrent[layerIndex] = rNextIndex;

            RMaxCurrent[layerIndex] = Math.Min(rNextIndex + stepsReserve, RMax);
        }

        private void CalculateEpures()
        {
            for (var i = 0; i < RadiusesTotal.GetLength(0); i++)
            {
                for (var j = 0; j < RadiusesTotal.GetLength(1); j++)
                {
                    var T = TemperaturesCurrent[i, j];

                    if (T > MaxTemperatures[i, j])
                    {
                        MaxTemperatures[i, j] = T;
                    }

                    if (T > PointMaxTemperature)
                    {
                        PointMaxTemperature = T;

                        PointMaxTemperatureI = i;
                        PointMaxTemperatureJ = j;
                    }
                }
            }

            PointTemperature = TemperaturesCurrent[PointI, PointJ];
        }

        private void CalculateAverageSurfaceTemperature()
        {
            var sumL = 0.0;
            AverageTemperatureInnerSurface = 0;
            AverageTemperatureOuterSurface = 0;

            for (var i = 0; i < TemperaturesCurrent.GetLength(0); i++)
            {
                var iR = Math.Min(i + 1, barrel.X.Length - 1);
                var iL = Math.Max(i - 1, 0);
                var dx = (barrel.X[iR] - barrel.X[iL]) / (iR - iL);
                sumL += dx;

                AverageTemperatureInnerSurface += TemperaturesCurrent[i, 0] * dx;
                AverageTemperatureOuterSurface += TemperaturesCurrent[i, RMax] * dx;
            }

            AverageTemperatureInnerSurface /= sumL;
            AverageTemperatureOuterSurface /= sumL;
        }
    }
}
