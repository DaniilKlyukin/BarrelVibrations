using System.ComponentModel;
using BasicLibraryWinForm;
using MathNet.Numerics.Interpolation;

namespace InletBallisticLibrary
{
    public class InletBallisticSolver
    {
        private double elementIntegrationSize;
        private const double StableGasVelocity = 1;
        public double AgainstK { get; set; }
        public double AgainstPressure { get; set; }
        public double AgainstDensity { get; set; }

        public Environment Environment { get; }

        public double Error
        {
            get
            {
                var rho = Input.Powders.Sum(p => p.PowderDensity);

                var cv = Input.Powders.Sum(p => p.Cv * p.PowderDensity) / rho;
                var w = Input.Powders.Sum(powder => powder.w) + Input.wIgniter;

                var fact = cv * Temperature * (Input.Powders.Select((powder, i) => powder.w * PsiArr[i]).Sum() + Input.wIgniter)
                           + (1 + J0 * w / Input.q) * Input.q * FastMath.Pow2(Vsn) / 2
                           + Input.Powders.Select((powder, i) => powder.w * (1 - PsiArr[i]) * powder.Q).Sum();

                var teor = Input.Powders.Sum(powder => powder.w * powder.Q) + Input.wIgniter * Input.Powders.First().Q;

                return (fact / teor - 1) * 100;
            }
        }

        public BackgroundWorker? Worker { get; set; }
        public InletBallisticInput Input { get; set; }
        private readonly double[] _meshXPoints;

        public void Initialize()
        {
            var rho = Input.Powders.Sum(p => p.PowderDensity);

            powdersR = Input.Powders.Sum(p => p.R * p.PowderDensity) / rho;
            powdersAlpha = Input.Powders.Sum(p => p.Alpha * p.PowderDensity) / rho;

            w = Input.Powders.Sum(powder => powder.w) + Input.wIgniter;

            var initial = new double[2 + 2 * Input.Powders.Count];

            for (var i = 0; i < PsiArr.Length; i++)
            {
                PsiArr[i] = 0;
                ZArr[i] = 0;
            }

            W = Input.GetBarrelVolume(Input.CamoraLength);
            Xsn = initial[^2] = Input.CamoraLength;

            double J0Integral = 0.0, J1Integral = 0.0, J2Integral = 0.0, J2SubIntegral = 0.0;

            (J0, J0Integral) = Input.GetJ0(Xsn, elementIntegrationSize, 0, J0Integral);
            (J1, J1Integral) = Input.GetJ1(Xsn, elementIntegrationSize, Xsn, 0, J1Integral);
            (J2, J2Integral, J2SubIntegral) = Input.GetJ2(Xsn, elementIntegrationSize, 0, J2Integral, J2SubIntegral);

            Pressure = GetP(PsiArr, W, Vsn, J0);

            Psn = GetPsn(Pressure, W, Vsn, J0, J1, J2);
            Pkn = GetPkn(Psn, W, Vsn, J1);
            Temperature = GetTemperature(Pressure, PsiArr, W);
            HeatTransfer = GetHeatTransfer(Xsn, Vsn, W, PsiArr);
            Density = Math.Max(Pressure / (powdersAlpha * Pressure + Input.R * Temperature), Environment.Density);

            for (var i = 0; i < PsiArr.Length; i++)
            {
                var powder = Input.Powders[i];
                UkArr[i] = powder.GetBurnSpeed(Input.BoostPressure, Pressure);
                SigmaArr[i] = powder.GetSigma(ZArr[i], PsiArr[i]);
            }

            // psi1, psi2, z1, z2, x, v
            OdeSolver = new BasicLibraryWinForm.ODE.RungeKutta4Solver
            {
                InitialValues = new OdeSolver.OdeSolution(initial),
                EquationsObserverInitialization = () =>
                 {

                 },
                EquationsObserver = (t, x) =>
                {
                    var xsn = x[^2];
                    var vsn = x[^1];

                    W = Input.GetBarrelVolume(xsn);

                    (J0, J0Integral) = Input.GetJ0(xsn, elementIntegrationSize, Xsn, J0Integral);
                    (J1, J1Integral) = Input.GetJ1(xsn, elementIntegrationSize, xsn, Xsn, J1Integral);
                    (J2, J2Integral, J2SubIntegral) = Input.GetJ2(xsn, elementIntegrationSize, Xsn, J2Integral, J2SubIntegral);

                    if (x[^2] >= Input.BarrelLength)
                    {
                        CalculateValuesOnBarrelEnd(Time, Xsn, Vsn, x[^2], x[^1], out _, out xsn, out vsn);
                    }

                    Xsn = xsn;
                    Vsn = vsn;

                    for (var i = 0; i < PsiArr.Length; i++)
                    {
                        PsiArr[i] = x[i];
                        ZArr[i] = x[PsiArr.Length + i];
                    }

                    Pressure = GetP(PsiArr, W, Vsn, J0);
                    Psn = GetPsn(Pressure, W, Vsn, J0, J1, J2);
                    Pkn = GetPkn(Psn, W, Vsn, J1);
                    Temperature = GetTemperature(Pressure, PsiArr, W);
                    Density = Math.Max(Pressure / (powdersAlpha * Pressure + Input.R * Temperature), Environment.Density);
                    HeatTransfer = Math.Max(GetHeatTransfer(Xsn, Vsn, W, PsiArr), Environment.HeatTransfer);

                    for (var i = 0; i < PsiArr.Length; i++)
                    {
                        var powder = Input.Powders[i];
                        UkArr[i] = powder.GetBurnSpeed(Input.BoostPressure, Pressure);
                        SigmaArr[i] = powder.GetSigma(ZArr[i], PsiArr[i]);
                    }
                },
                EquationsSystem = (t, x, dxdt) =>
                {
                    var currentPsi = new double[Input.Powders.Count];
                    var currentZ = new double[Input.Powders.Count];

                    for (var j = 0; j < Input.Powders.Count; j++)
                    {
                        currentPsi[j] = x[j] >= 1 ? 1 : x[j];
                        currentZ[j] = x[Input.Powders.Count + j];
                    }

                    var xsn = x[^2];
                    var vSn = Math.Max(x[^1], 0);
                    Ppr = GetPpr(vSn);

                    W = Input.GetBarrelVolume(xsn);

                    var (j0, _) = Input.GetJ0(xsn, elementIntegrationSize, Xsn, J0Integral);
                    var (j1, _) = Input.GetJ1(xsn, elementIntegrationSize, xsn, Xsn, J1Integral);
                    var (j2, _,_) = Input.GetJ2(xsn, elementIntegrationSize, Xsn, J2Integral, J2SubIntegral);

                    //(J0, J1, J2) = Input.GetJ(xsn, jPointsCount, xsn);

                    var p = GetP(currentPsi, W, vSn, j0);
                    var p_sn = GetPsn(p, W, vSn, j0, j1, j2);

                    var equations = new double[initial.Length];

                    for (var j = 0; j < Input.Powders.Count; j++)
                    {
                        var powder = Input.Powders[j];
                        var uk = powder.GetBurnSpeed(Input.BoostPressure, p);

                        equations[j] = currentPsi[j] < 1
                                                ? powder.PowderArea / powder.PowderVolume * powder.GetSigma(currentZ[j], currentPsi[j]) * uk
                                                : 0;

                        equations[Input.Powders.Count + j] = currentPsi[j] < 1 ? 2 * uk / powder._2e1 : 0;
                    }

                    var mg = Input.Gravity ? Input.q * 9.81 * Math.Sin(Input.ShotAngle) : 0;
                    var S = Input.GetS(xsn);
                    var missileForce = S * (p_sn - Ppr);

                    if (vSn == 0 && p_sn < Input.BoostPressure)
                    {
                        equations[^2] = 0;
                        equations[^1] = 0;
                    }
                    else
                    {
                        equations[^2] = vSn;
                        equations[^1] = Math.Max(0, (1 - Input.FrictionCoefficient) * (missileForce - mg) / Input.q);
                    }

                    return new OdeSolver.OdeSolution(equations);
                },
                CheckStop = (_, x) => x[^2] >= Input.BarrelLength
            };

            OdeSolver.InitializeStepSolver(0);
        }

        public InletBallisticSolver(InletBallisticInput input, Environment environment, double[] meshXPoints)
        {
            Environment = environment;
            Input = input;
            _meshXPoints = meshXPoints;
            AgainstPressure = environment.Pressure;
            AgainstDensity = environment.Density;
            AgainstK = environment.K;

            PsiArr = new double[input.Powders.Count];
            ZArr = new double[input.Powders.Count];
            UkArr = new double[input.Powders.Count];
            SigmaArr = new double[input.Powders.Count];
            // deformations = new double[meshXPoints.Length];

            Pressures = Algebra.Ones(meshXPoints.Length, environment.Pressure);
            Temperatures = Algebra.Ones(meshXPoints.Length, environment.Temperature);
            HeatTransfers = Algebra.Ones(meshXPoints.Length, environment.HeatTransfer);
            GasVelocities = Algebra.Ones(meshXPoints.Length, 0.0);
            Densities = Algebra.Ones(meshXPoints.Length, environment.Density);

            elementIntegrationSize = input.BarrelLength / (meshXPoints.Length - 1);

            WMesh = new double[meshXPoints.Length];
            Smesh = new double[meshXPoints.Length];

            Smesh[0] = Input.GetS(meshXPoints[0]);
            for (var i = 1; i < meshXPoints.Length; i++)
            {
                Smesh[i] = Input.GetS(meshXPoints[i]);

                WMesh[i] = WMesh[i - 1] + Input.GetVolume2(
                    Smesh[i - 1],
                    Smesh[i],
                    _meshXPoints[i] - _meshXPoints[i - 1]);

                if (_meshXPoints[i - 1] <= input.CamoraLength && _meshXPoints[i] > input.CamoraLength)
                    camoraIndex = i - 1;
            }

            //  SSpline = CubicSpline.InterpolatePchip(_meshXPoints, Smesh);
            //  WSpline = CubicSpline.InterpolatePchip(_meshXPoints, WMesh);
        }

        /// <summary>
        ///     Определяет давление в стволе, Па
        /// </summary>
        /// <param name="psi">Доля сгоревшего пороха</param>
        /// <param name="volume">Пройденный объём</param>
        /// <param name="v">Скорость снаряда</param>
        /// <returns>Давление в стволе, Па</returns>
        private double GetP(double[] psi, double volume, double v, double J0)
        {
            var powder0 = Input.Powders.First();

            var numerator = Input.Teta * (Input.wIgniter * powder0.f / powder0.Teta
                                     + Input.Powders.Select((p, i) => p.w * psi[i] * p.f / p.Teta).Sum()
                                     - (1 + J0 / Input.q * (Input.wIgniter + Input.Powders.Sum(p => p.w))) * Input.q * FastMath.Pow2(v) / 2);

            var denominator =
                volume
                - Input.Powders.Select((p, i) => p.w * (1 - psi[i]) / p.PowderDensity).Sum()
                - powder0.Alpha * Input.wIgniter
                - Input.Powders.Select((p, i) => p.w * psi[i] * p.Alpha).Sum();

            return numerator / denominator;
        }

        private double GetPpr(double vsn)
        {
            if (!Input.BackPressure)
                return 0;

            var p = Math.Max(Environment.Pressure, AgainstPressure);
            var rho = Math.Max(Environment.Density, AgainstDensity);

            var k = AgainstK;
            var c = Math.Sqrt(k * p / rho);

            return p * (1 + k * (k + 1) / 4 * FastMath.Pow2(vsn / c) + k * vsn / c * Math.Sqrt(1 + FastMath.Pow2(vsn / c * (k + 1) / 4)));
        }

        /// <summary>
        ///     Определяет давление на снаряд, Па
        /// </summary>
        /// <param name="p">Давление в стволе</param>
        /// <returns></returns>
        private double GetPsn(double p, double volume, double v, double J0, double J1, double J2)
        {
            var w = Input.Powders.Sum(powder => powder.w) + Input.wIgniter;

            return (p + w * FastMath.Pow2(v) / volume * (J0 / 2 + J1 - J2 - 0.5)) /
                   (1 + w / Input.q * (J1 - J2));
        }

        /// <summary>
        ///     Определяет давление на канал ствола, Па
        /// </summary>
        /// <param name="psn">Давление на снаряд</param>
        /// <returns></returns>
        private double GetPkn(double psn, double volume, double v, double J1)
        {
            var w = Input.Powders.Sum(powder => powder.w) + Input.wIgniter;

            return psn * (1 + w / Input.q * J1) + w * FastMath.Pow2(v) / volume * (0.5 - J1);
        }

        private double GetTemperature(double p, double[] psi, double volume)
        {
            var numerator =
                volume
                - Input.Powders.Select((powder, i) => powder.w * (1 - psi[i]) / powder.PowderDensity).Sum()
                - Input.Powders.First().Alpha * Input.wIgniter
                - Input.Powders.Select((powder, i) => powder.w * psi[i] * powder.Alpha).Sum();

            return p * numerator / (Input.R * (Input.wIgniter + Input.Powders.Select((powder, i) => powder.w * psi[i]).Sum()));
        }

        private OdeSolver OdeSolver;

        #region Параметры решения ОЗВБ в текущий момент времени
        public double Time { get; private set; }
        public double[] PsiArr { get; }
        public double[] ZArr { get; }
        public double[] UkArr { get; }
        public double[] SigmaArr { get; }
        public double Xsn { get; private set; }
        public double Vsn { get; private set; }
        public double Pressure { get; private set; }
        public double Psn { get; private set; }
        public double Pkn { get; private set; }
        public double Temperature { get; private set; }
        public double HeatTransfer { get; private set; }
        public double J0 { get; private set; }
        public double J1 { get; private set; }
        public double J2 { get; private set; }
        public double W { get; private set; }
        public double Ppr { get; private set; }
        public double Density { get; private set; }

        #endregion

        /// <summary>
        /// Находится ли снаряд внутри ствола (в процессе выстрела)
        /// </summary>
        public bool MissileInBarrel { get; private set; }

        private double dt;

        #region Рапсределения характеристик по длине ствола
        public double[] Pressures { get; }
        public double[] Temperatures { get; }
        public double[] HeatTransfers { get; }
        public double[] Densities { get; }
        public double[] GasVelocities { get; }
        private readonly double[] WMesh, Smesh;

        #endregion


        /*
                private double[] deformations;
                private CubicSpline WSpline;
                private CubicSpline SSpline;

                public void SetDeformations(double[] rInner, double[] u)
                {
                    jPointsCount = 100;
                    deformations = u;
                    for (var i = 0; i < u.Length; i++)
                    {
                        Smesh[i] = Algebra.GetCircleArea(rInner[i] + u[i]);

                        if (i > 0)
                        {
                            var s1 = Input.GetBarrelS(_meshXPoints[i - 1]);
                            var s2 = Input.GetBarrelS(_meshXPoints[i]);
                            var r1 = Algebra.GetEquivalentCircleR(s1);
                            var r2 = Algebra.GetEquivalentCircleR(s2);

                            WMesh[i] = WMesh[i - 1] + Input.GetVolume(
                                u[i - 1] + r1,
                                u[i] + r2,
                                _meshXPoints[i] - _meshXPoints[i - 1]);
                        }
                    }

                    SSpline = CubicSpline.InterpolatePchip(_meshXPoints, Smesh);
                    WSpline = CubicSpline.InterpolatePchip(_meshXPoints, WMesh);

                    if (missileIndex < 0 || missileIndex >= _meshXPoints.Length)
                        return;

                    var k = Input.Teta + 1;
                    var volume = GetBarrelVolume(Xsn);
                    var S = SSpline.Interpolate(Xsn) - Input.GetBarrelS(_meshXPoints[missileIndex]);

                    var c = Math.Sqrt(2 * k * Pressure * volume * (1 - Math.Pow(Ppr / Pressure, (k - 1) / k)) / (k + 1));
                    var dv = c * S * dt;

                    if (S != 0)
                    {
                        var B = Math.Pow(volume, k) / Math.Pow(volume + dv, k);
                        if (!double.IsFinite(B))
                            return;

                        Pressure = Math.Max(Pressure * B, Environment.Pressure);
                    }
                }
                */

        /// <summary>
        /// Площадь сечения снаряда
        /// </summary>
        private double Ssn => Input.Ssn;

        /// <summary>
        /// Суммарная масса порохов и воспламенителя
        /// </summary>
        private double w;

        private double t_p;

        /// <summary>
        /// Средневзешеная удельная газовая постоянная
        /// </summary>
        private double powdersR;

        /// <summary>
        /// Средневзешенный коволюм
        /// </summary>
        private double powdersAlpha;

        /// <summary>
        /// Средневзешенный коволюм
        /// </summary>
        private readonly int camoraIndex;

        private int missileIndex = -1;

        public void DoShot()
        {
            MissileInBarrel = true;
            missileIndex = -1;
            Initialize();
        }

        private void CalculateDistributions(double dt)
        {
            var isStablePressure = Pkn == Environment.Pressure;
            var isStableGasSpeed = Math.Abs(Vsn) <= StableGasVelocity;

            if (isStablePressure &&
                isStableGasSpeed)
            {
                if (Vsn == 0)
                    return;

                Vsn = 0;

                for (int i = 0; i < GasVelocities.Length; i++)
                {
                    GasVelocities[i] = 0;
                    HeatTransfers[i] = Environment.HeatTransfer;
                }
            }

            var totalVolume = Input.GetBarrelVolume(Input.BarrelLength);

            if (MissileInBarrel)
            {
                for (var i = missileIndex; i < _meshXPoints.Length; i++)
                {
                    if (i >= 0 && _meshXPoints[i] >= Xsn)
                    {
                        missileIndex = i;
                        break;
                    }
                }

                Pressures[0] = Pkn;
                Temperatures[0] = GetTemperature(Pressures[0], PsiArr, W);
                Densities[0] = Density;
                GasVelocities[0] = WMesh[0] / Smesh[0] * Vsn * Ssn / W;
            }
            else
            {
                missileIndex = Pressures.Length - 1;

                if (!isStablePressure)
                {
                    Pressure = Math.Max(Pressure * Math.Exp(-dt / t_p), Environment.Pressure);
                    Density = Math.Max(Density * Math.Exp(-dt / t_p), Environment.Density);
                    Psn = Math.Max(Psn * Math.Exp(-dt / t_p), Environment.Pressure);
                    Pkn = Math.Max(Pkn * Math.Exp(-dt / t_p), Environment.Pressure);
                    Temperature = Math.Max(GetTemperature(Pressure, PsiArr, totalVolume), Environment.Temperature);
                }

                if (!isStableGasSpeed)
                {
                    Vsn = Math.Max(Vsn * Math.Exp(-dt / t_p), 0.0);
                }
            }

            var J1Mesh = new double[_meshXPoints.Length];
            var J1IntegralMesh = new double[_meshXPoints.Length];

            for (var i = 0; i < missileIndex + 1; i++)
            {
                if (!isStablePressure)
                {
                    if (i != 0)
                        (J1Mesh[i], J1IntegralMesh[i]) = Input.GetJ1(_meshXPoints[i], elementIntegrationSize, Xsn, _meshXPoints[i - 1], J1IntegralMesh[i - 1]);

                    Pressures[i] = Math.Max(Pkn
                                       - J1Mesh[i] * (w * Psn / Input.q -
                                                   w * Math.Pow(Vsn, 2) / W)
                                       - w * Math.Pow(Ssn * Vsn * WMesh[i] / Smesh[i], 2) / Math.Pow(W, 3) / 2, Environment.Pressure);

                    Temperatures[i] = Math.Max(GetTemperature(Pressures[i], PsiArr, W), Environment.Temperature);
                    Densities[i] = Density;
                }

                if (!isStableGasSpeed)
                {
                    GasVelocities[i] = Math.Max(WMesh[i] / Smesh[i] * Vsn * Ssn / W, 0);
                }
            }

            for (var i = missileIndex + 1; i < _meshXPoints.Length; i++)
            {
                if (!isStablePressure)
                {
                    Pressures[i] = Math.Max(
                        Pressures[i] * Math.Exp(-dt / t_p),
                        Environment.Pressure);
                    Temperatures[i] = Math.Max(
                        GetTemperature(Pressures[i], PsiArr, totalVolume),
                        Environment.Temperature);
                    Densities[i] = Math.Max(
                        Densities[i] * Math.Exp(-dt / t_p),
                        Environment.Density);
                }

                if (!isStableGasSpeed)
                {
                    GasVelocities[i] = Math.Max(
                        GasVelocities[i] * Math.Exp(-dt / t_p),
                        0.0);
                }
            }

            if (!isStableGasSpeed)
            {
                for (var i = camoraIndex + 1; i <= missileIndex; i++)
                {
                    HeatTransfers[i] = Math.Max(
                        GetHeatTransfer(_meshXPoints[i], GasVelocities[i], W, PsiArr),
                        Environment.HeatTransfer);
                }

                for (var i = missileIndex + 1; i < _meshXPoints.Length; i++)
                {
                    HeatTransfers[i] = Math.Max(
                        GetHeatTransfer(_meshXPoints[i], GasVelocities[i], totalVolume, PsiArr),
                        Environment.HeatTransfer);
                }

                for (var i = 0; i <= camoraIndex; i++)
                {
                    HeatTransfers[i] = HeatTransfers[camoraIndex + 1];
                }
            }

            if (t_p == 0) return;

            AgainstPressure = missileIndex + 1 < Pressures.Length ? Pressures[missileIndex + 1] : Pressures.Average();
            AgainstDensity = missileIndex + 1 < Densities.Length ? Densities[missileIndex + 1] : Densities.Average();
        }

        public void SolveStep(double dt)
        {
            this.dt = dt;

            if (MissileInBarrel)
            {
                MissileInBarrel = !OdeSolver.SolveStep(dt);

                if (!MissileInBarrel)
                {
                    // Определяем параметры для выхода газа
                    var r = Algebra.GetEquivalentCircleR(Ssn); // + deformations.Last();
                    var k = Input.Teta + 1;
                    var B_k = Math.Sqrt(k * Math.Pow(2.0 / (k + 1), (k + 1) / (k - 1)));
                    var totalVolume = Input.GetBarrelVolume(Input.BarrelLength);
                    t_p = totalVolume / (B_k * Math.PI * FastMath.Pow2(r) * Math.Sqrt(powdersR * Temperature));
                    var Err = Error;
                }
            }

            CalculateDistributions(dt);

            Time += dt;
        }

        public void CalculateValuesOnBarrelEnd(
            double timeBefore, double XsnBefore, double VsnBefore,
            double XsnAfter, double VsnAfter,
            out double time, out double Xsn, out double Vsn)
        {
            Xsn = Input.BarrelLength;

            Vsn = Algebra.GetValueAtLine(Input.BarrelLength, XsnBefore, VsnBefore, XsnAfter, VsnAfter);
            time = timeBefore + 2 * (XsnAfter - XsnBefore) / (VsnBefore + VsnAfter);
        }


        public double GetHeatTransfer(double x, double gasSpeed, double volume, double[] psi)
        {
            var d = Input.MissileDiameter;
            var n = Input.GroovesCount;

            var rho = Input.Powders.Sum(p => p.PowderDensity);

            var mu = rho / Input.Powders.Sum(powder => powder.PowderDensity / powder.Viscosity);

            var lambda = 0.5 * Input.Powders.Sum(powder => powder.HeatConductivity * powder.PowderDensity * powder.R) / (rho * Input.R)
                         + 0.5 * rho * Input.R / Input.Powders.Sum(powder => powder.PowderDensity * powder.R / powder.HeatConductivity);
            var cp = Input.Powders.Sum(p => p.Cp * p.PowderDensity) / rho;

            var Pr = cp * mu / lambda;

            var numerator = Input.Powders.Select((powder, i) => powder.w * psi[i]).Sum() + Input.wIgniter;
            var denominator = volume
                              - Input.Powders.Select((powder, i) => powder.w * (1 - psi[i]) / powder.PowderDensity - powder.Alpha * (powder.w * psi[i])).Sum()
                              - Input.Powders.First().Alpha * (Input.wIgniter * psi[0]);

            var rho_gas = numerator / denominator;

            var Re = gasSpeed * d * rho_gas / mu;

            var ht = 0.023 * Math.Pow(Re, 0.8) * Math.Pow(Pr, 0.4) * lambda / d;

            if (Input.GroovesCount == 0 || x <= Input.CamoraLength)
                return ht;

            var sh_opt = 13;
            var sh = Input.GroovesWidth / Input.GroovesDepth;

            var fsh = sh >= sh_opt ? sh_opt / sh : sh / sh_opt;
            var e_roughness = 1.04 * Math.Pow(Pr, 0.04) * Math.Exp(0.85 * fsh);

            return e_roughness * ht;
        }

    }
}