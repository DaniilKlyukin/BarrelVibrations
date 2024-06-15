using System.ComponentModel;
using BarrelVibrations.ModelingObjects;
using BarrelVibrations.Solvers.Solutions;
using BarrelVibrations.Solvers.Solutions.InletBallistic;
using InletBallisticLibrary;
using BasicLibraryWinForm;
using Point = BasicLibraryWinForm.PointFolder.Point;
using BarrelVibrations.Solvers.RadialDeformationsSolvers;
using BarrelVibrations.Solvers.OutletBallisticFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BarrelVibrations.ModelingObjects.BarrelFolder;
using System.Runtime.Serialization;
using Environment = BarrelVibrations.ModelingObjects.EnvironmentFolder.Environment;
using BarrelVibrations.ModelingObjects.MeshFolder;
using BarrelVibrations.ModelingObjects.AmmoFolder;
using BarrelVibrations.ModelingObjects.ShotFolder;
using BarrelVibrations.ModelingObjects.FiringSystemFolder;

namespace BarrelVibrations.Solvers
{
    public delegate bool IsStopDelegate(double time, bool missileInBarrel);

    [Serializable]
    [DataContract(Name = "Основной решатель")]
    public class MainSolver
    {
        public IsStopDelegate IsStop { get; set; } = (time, missileInBarrel) => true;

        [IgnoreDataMember]
        public BackgroundWorker Worker { get; set; } = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

        [IgnoreDataMember]
        public bool MissileInBarrel => InletBallisticSolver?.MissileInBarrel ?? false;

        #region Сохранение
        [DataMember(Name = "Моменты времени, сек")]
        public List<double> TimeMoments { get; set; } = new();

        [DataMember(Name = "Данные решения задачи внутренней баллистики")]
        public InletBallistic InletBallistic { get; set; } = new();

        [DataMember(Name = "Данные эпюр распределения параметров газа при выстреле")]
        public GasEpures GasEpures { get; set; } = new();

        [DataMember(Name = "Результаты решения задачи теплопроводности")]
        public TemperatureField TemperatureField { get; set; } = new();

        [DataMember(Name = "Данные решения задачи начального прогиба")]
        public Deflection Deflection { get; set; } = new();

        [DataMember(Name = "Данные решения задачи колебаний")]
        public Vibrations Vibrations { get; set; } = new();

        [DataMember(Name = "Результаты решения задачи внешней баллистики")]
        public List<OutletBallistic> OutletBallistic { get; set; } = new();

        [DataMember(Name = "Дульные параметры стрельбы")]
        public List<ShotParameters> ShotsParameters { get; set; } = new();
        #endregion



        #region Входные данные
        [DataMember(Name = "Материал")]
        public Material Material { get; set; } = new();

        [DataMember(Name = "Боеприпасы")]
        public List<Ammo> Ammo { get; set; } = new();

        [DataMember(Name = "Окружающая среда")]
        public Environment Environment { get; set; } = new();

        [DataMember(Name = "Дульный тормоз")]
        public MuzzleBreak MuzzleBreak { get; set; } = new();

        [DataMember(Name = "Ствол")]
        public Barrel Barrel { get; set; } = new();

        [DataMember(Name = "Боевая система")]
        public FiringSystem FiringSystem { get; set; } = new();

        [DataMember(Name = "Параметры сетки")]
        public MeshProperties MeshProperties { get; set; } = new();

        [DataMember(Name = "Параметры модели")]
        public ModelProperties ModelProperties { get; set; } = new();
        #endregion


        private readonly List<ResultWriter> resultWriters = new();


        [IgnoreDataMember]
        public InletBallisticSolver? InletBallisticSolver { get; private set; }

        [IgnoreDataMember]
        public TemperatureSolver? TemperatureSolver { get; private set; }


        [IgnoreDataMember]
        public RadialVibrationsSolver? RadialVibrationsSolver { get; private set; }

        [IgnoreDataMember]
        public DeflectionSolver? DeflectionSolver { get; private set; }

        [IgnoreDataMember]
        public VibrationSolver? VibrationSolver { get; private set; }

        [IgnoreDataMember]
        public OutletBallisticSolver? OutletBallisticSolver { get; private set; }

        [IgnoreDataMember]
        public bool Initialized =>
            InletBallisticSolver != null &&
            TemperatureSolver != null &&
            RadialVibrationsSolver != null &&
            RadialVibrationsSolver != null &&
            DeflectionSolver != null &&
            VibrationSolver != null &&
            OutletBallisticSolver != null;

        [IgnoreDataMember]
        public int CurrentShotIndex { get; set; }

        [IgnoreDataMember]
        public Ammo CurrentAmmo
        {
            get
            {
                if (!ModelProperties.Shots.Any() || CurrentShotIndex < 0 || CurrentShotIndex >= Ammo.Count)
                    return Ammo.FirstOrDefault() ?? new Ammo();

                return Ammo[ModelProperties.Shots[CurrentShotIndex].AmmoIndex];
            }
        }

        public MainSolver()
        {
            IsStop = (time, missileInBarrel) => time >= ModelProperties.EndTime;
        }

        public void Initialize()
        {
            ModelProperties.MainTimeStep = 1.0 / (1 << 17);
            ModelProperties.StepsToSaveResults = 4;

            var avgInnerR = new double[Barrel.X.Length];
            var avgOuterR = new double[Barrel.X.Length];

            for (int i = 0; i < Barrel.X.Length; i++)
            {
                var S = Barrel.X[i] <= Barrel.CamoraLength
                    ? Math.PI * FastMath.Pow2(Barrel.InnerD[i] / 2)
                    : Math.PI * FastMath.Pow2(Barrel.InnerD[i] / 2) + Barrel.GroovesWidth * 1e-3 * Barrel.GroovesDepth * 1e-3 * Barrel.GroovesCount;

                avgInnerR[i] = Math.Sqrt(S / Math.PI);
                avgOuterR[i] = Math.Sqrt((Barrel.F[i] + S) / Math.PI);
            }

            DeflectionSolver = new DeflectionSolver(
                    Material,
                    Barrel,
                    MuzzleBreak,
                    CurrentAmmo.Missile,
                    Algebra.ConvertGradToRad(ModelProperties.FiringAngle),
                    Environment,
                    ModelProperties.CalculateDeformations,
                    ModelProperties.CalculateMissileGravity);

            DeflectionSolver.Solve();
            Deflection.WriteResults(
                DeflectionSolver.XDeflection,
                DeflectionSolver.YDeflection,
                DeflectionSolver.ZDeflection);

            var barrelSFunc = Algebra.GetFunc(Barrel.X, Barrel.S);

            var barrelS = Barrel.BarrelSections.Select(v => barrelSFunc(v.X * 1e-3)).ToArray();

            var input = new InletBallisticInput(
                CurrentAmmo.PowderCharge,
                ModelProperties.ForcePressure * 1e6,
                CurrentAmmo.Missile.Mass,
                CurrentAmmo.Missile.Diameter,
                Barrel.CamoraLength,
                Barrel.AddCamoraVolume,
                Barrel.BarrelSections.Select(v => v.X * 1e-3).ToArray(),
                Barrel.BarrelSections.Select(v => v.dInner * 1e-3).ToArray(),
                barrelS,
                Barrel.GroovesCount,
                Barrel.GroovesSlope,
                Barrel.GroovesDepth * 1e-3,
                Barrel.GroovesWidth * 1e-3,
                ModelProperties.IsBackPressure,
                ModelProperties.MissileFrictionCoefficient,
                ModelProperties.CalculateMissileGravity,
                Algebra.ConvertGradToRad(ModelProperties.FiringAngle));

            var inletBallisticEnvironment = new InletBallisticLibrary.Environment(
                Environment.Temperature,
                Environment.HeatTransfer,
                Environment.Pressure,
                Environment.k,
                Environment.Density);

            InletBallisticSolver = new InletBallisticSolver(input, inletBallisticEnvironment, Barrel.X);

            InletBallisticSolver.Initialize();

            TemperatureSolver = new TemperatureSolver(
                Barrel,
                Material,
                avgInnerR,
                avgOuterR,
                MeshProperties.RGrowSpeed,
                MeshProperties.MaxPointsRCount,
                MeshProperties.InitialRAreaSize * 1e-3,
                Environment.Temperature,
                ModelProperties.CalculateTemperatures
            );

            RadialVibrationsSolver = new RadialVibrationsSolver(
                Barrel,
                Material,
                Environment,
                avgInnerR,
                avgOuterR,
                MeshProperties.RGrowSpeed,
                MeshProperties.MaxPointsRCount,
                RadialVibrationsSolverType.AxisymmetricFreeX,
                true);

            VibrationSolver = new VibrationSolver(
                Material,
                Barrel,
                MuzzleBreak,
                CurrentAmmo.Missile,
                Environment,
                FiringSystem,
                ModelProperties.MissileFrictionCoefficient,
                Algebra.ConvertGradToRad(ModelProperties.FiringAngle),
                DeflectionSolver.XDeflection,
                DeflectionSolver.YDeflection,
                DeflectionSolver.ZDeflection,
                ModelProperties.CalculateVibrations,
                ModelProperties.CalculateMissileGravity);

            OutletBallisticSolver = new OutletBallisticSolver(
                Environment.Terrain,
                Environment.Wind);
        }

        private void initSolver()
        {
            if (!Initialized) return;

            resultWriters.Clear();

            if (ModelProperties.SaveDistributionsInFiles)
            {
                resultWriters.Add(new ResultWriter(Resource.GasPressuresFile,
                    () => InletBallisticSolver.Pressures.GetString()));

                resultWriters.Add(new ResultWriter(Resource.GasTemperaturesFile,
                    () => InletBallisticSolver.Temperatures.GetString()));

                resultWriters.Add(new ResultWriter(Resource.GasHeatTransfersFile,
                    () => InletBallisticSolver.HeatTransfers.GetString()));

                resultWriters.Add(new ResultWriter(Resource.GasDensitiesFile,
                    () => InletBallisticSolver.Densities.GetString()));

                resultWriters.Add(new ResultWriter(Resource.GasVelocitiesFile,
                    () => InletBallisticSolver.GasVelocities.GetString()));

                resultWriters.Add(new ResultWriter(Resource.VibrationsXFile,
                    () => VibrationSolver.VibrationXCurrent.GetString()));

                resultWriters.Add(new ResultWriter(Resource.VibrationsYFile,
                    () => VibrationSolver.VibrationYCurrent.GetString()));

                resultWriters.Add(new ResultWriter(Resource.VibrationsZFile,
                    () => VibrationSolver.VibrationZCurrent.GetString()));

                resultWriters.Add(new ResultWriter(Resource.VibrationsRInner,
                    () => RadialVibrationsSolver.InnerU.GetString()));

                resultWriters.Add(new ResultWriter(Resource.TemperatureBarrelInner,
                    () => TemperatureSolver.TemperaturesCurrent.GetColumnSlice(0).ToArray().GetString()));
            }

            TimeMoments.Clear();

            InletBallistic.Clear();
            GasEpures.Clear(Barrel.X.Length);
            TemperatureField.Clear();
            Vibrations.Clear(Barrel.X.Length);
            OutletBallistic.Clear();
            ShotsParameters.Clear();

            save(0);
        }

        private void UpdateShotParameters()
        {
            var barrelSFunc = Algebra.GetFunc(Barrel.X, Barrel.S);

            var barrelS = Barrel.BarrelSections.Select(v => barrelSFunc(v.X * 1e-3)).ToArray();

            var input = new InletBallisticInput(
                CurrentAmmo.PowderCharge,
                ModelProperties.ForcePressure * 1e6,
                CurrentAmmo.Missile.Mass,
                CurrentAmmo.Missile.Diameter,
                Barrel.CamoraLength,
                 Barrel.AddCamoraVolume,
                Barrel.BarrelSections.Select(v => v.X * 1e-3).ToArray(),
                Barrel.BarrelSections.Select(v => v.dInner * 1e-3).ToArray(),
                barrelS,
                Barrel.GroovesCount,
                Barrel.GroovesSlope,
                Barrel.GroovesDepth * 1e-3,
                Barrel.GroovesWidth * 1e-3,
                ModelProperties.IsBackPressure,
                ModelProperties.MissileFrictionCoefficient,
                ModelProperties.CalculateMissileGravity,
                Algebra.ConvertGradToRad(ModelProperties.FiringAngle));

            InletBallisticSolver.Input = input;
            InletBallisticSolver.Initialize();

            VibrationSolver.Missile = CurrentAmmo.Missile;
        }

        public void Solve()
        {
            if (!Initialized) return;

            initSolver();

            var time = ModelProperties.MainTimeStep;

            var shotsQueue = new Queue<Shot>(ModelProperties.Shots);

            var missileWasInBarrel = false;

            CurrentShotIndex = 0;
            var currentStepsToSave = 1;
            var currentStepsToShow = 1;
            var currentStepsToVibrationsCalculation = 1;
            var lastAddCalcTime = -ModelProperties.MainTimeStep;
            var lastWorkerPcnt = 0.0;

            var innerRs = Barrel.InnerD.Select(d => d / 2).ToArray();

            while (true)
            {
                var shot = shotsQueue.Any() ? shotsQueue.Peek() : null;

                var shotTime = shot?.ShotTimeMoment ?? 0;

                var timeToShot = shotTime - time;

                if (shotsQueue.Any() && InletBallisticSolver.MissileInBarrel == false && timeToShot <= 0)
                {
                    UpdateShotParameters();

                    InletBallisticSolver.DoShot();
                    shotsQueue.Dequeue();
                    missileWasInBarrel = true;
                    CurrentShotIndex++;
                }

                if (CurrentShotIndex > 0)
                    InletBallisticSolver.SolveStep(ModelProperties.MainTimeStep);

                var isShot = missileWasInBarrel && !InletBallisticSolver.MissileInBarrel;
                var isSave = currentStepsToSave == ModelProperties.StepsToSaveResults || time + ModelProperties.MainTimeStep >= ModelProperties.EndTime;
                var isShow = currentStepsToShow == ModelProperties.StepsToShowProgress || time + ModelProperties.MainTimeStep >= ModelProperties.EndTime;

                if (currentStepsToVibrationsCalculation == ModelProperties.VibrationsStepMultiplier)
                {
                    var dt = time - lastAddCalcTime;

                    TemperatureSolver.SolveIteration(
                        dt,
                        InletBallisticSolver.Temperatures,
                        InletBallisticSolver.HeatTransfers,
                        Environment.Temperature,
                        Environment.HeatTransfer);

                    var (p1, fx, ty, tz) = PreProcessVibrations();

                    if (ModelProperties.CalculateRadialVibrations)
                    {
                        RadialVibrationsSolver.SolveIteration(
                            dt,
                            p1,
                            TemperatureSolver.TemperaturesCurrent);

                        //InletBallisticSolver.SetDeformations(innerRs, RadialVibrationsSolver.InnerU);
                    }

                    VibrationSolver.SolveIteration(
                        time,
                        dt,
                        p1,
                        InletBallisticSolver.Psn - Environment.Pressure,
                        InletBallisticSolver.Xsn,
                        fx,
                        ty,
                        tz,
                        InletBallisticSolver.MissileInBarrel);

                    lastAddCalcTime = time;
                    currentStepsToVibrationsCalculation = 0;
                }

                if (isShot)
                {
                    var fireAngle = Algebra.ConvertGradToRad(ModelProperties.FiringAngle);

                    var phi = Math.Atan(VibrationSolver.VelocityY.LastOrDefault() / (InletBallisticSolver.Vsn + VibrationSolver.VelocityX.LastOrDefault()));
                    var psi = Math.Atan(VibrationSolver.VelocityZ.LastOrDefault() / (InletBallisticSolver.Vsn + VibrationSolver.VelocityX.LastOrDefault()));

                    var angularVelocity = getMissileInitialAngularVelocity(InletBallisticSolver.Vsn);

                    var vibrationShotResult = OutletBallisticSolver.Solve(
                        CurrentAmmo.Missile,
                        Environment,
                        VibrationSolver.BarrelEndX,
                        VibrationSolver.BarrelEndY +
                            Environment.Terrain.GetAltitude(VibrationSolver.BarrelEndX, VibrationSolver.BarrelEndZ) +
                            FiringSystem.GroundDistance,
                        VibrationSolver.BarrelEndZ,
                        VibrationSolver.VelocityX.LastOrDefault(),
                        VibrationSolver.VelocityY.LastOrDefault(),
                        VibrationSolver.VelocityZ.LastOrDefault(),
                        InletBallisticSolver.Vsn,
                        VibrationSolver.BarrelVerticalAngle,
                        VibrationSolver.BarrelHorizontalAngle,
                        phi,
                        psi,
                        VibrationSolver.BarrelEndVerticalAngularVelocity,
                        VibrationSolver.BarrelEndHorizontalAngularVelocity,
                        angularVelocity,
                        ModelProperties.OutletBallisticTimeStep,
                        ModelProperties.MissileVibrations);

                    OutletBallistic.Add(vibrationShotResult);

                    missileWasInBarrel = false;
                }

                if (isShot)
                    saveShotParameters(time);

                if (isSave)
                {
                    /* var header = "x, м\tp, Па\tT, K\ta, Вт/(м*К)\n";
                     var rows = InletBallisticSolver.Pressures.Select((p, i) => $"{Barrel.X[i]}\t{p}\t{InletBallisticSolver.Temperatures[i]}\t{InletBallisticSolver.HeatTransfers[i]}").ToArray();
                     File.WriteAllText($"Results\\inlet_{time:0.0000}.txt", header + string.Join("\n", rows));*/
                    save(time);
                    currentStepsToSave = 0;
                }

                if (isShow)
                {
                    currentStepsToShow = 0;
                    var pcnt = 100 * time / ModelProperties.EndTime;

                    if (pcnt - lastWorkerPcnt >= 0.5)
                    {
                        Worker?.ReportProgress((int)Math.Round(pcnt), new SolverStatus(pcnt, time));
                        lastWorkerPcnt = pcnt;
                    }

                    if (Worker is { CancellationPending: true })
                        break;
                }

                if (IsStop.Invoke(time, InletBallisticSolver?.MissileInBarrel ?? false))
                    break;

                time += ModelProperties.MainTimeStep;
                currentStepsToVibrationsCalculation++;
                currentStepsToSave++;
                currentStepsToShow++;
            }

            foreach (var rw in resultWriters)
                rw.Close();
        }

        private (double[], double[], double[], double[]) PreProcessVibrations()
        {
            var p = InletBallisticSolver.Pressures;

            var nu = Material.PoissonRatio;
            var a = Material.LinearThermalExpansion;
            var E = Material.YoungModulus;
            var T0 = Environment.Temperature;
            var p0 = Environment.Pressure;

            var p1 = new double[p.Length];
            var forceX = new double[p1.Length];
            var torqueY = new double[p1.Length];
            var torqueZ = new double[p1.Length];

            for (int i = 0; i < p1.Length; i++)
            {
                p1[i] = p[i] - p0;

                forceX[i] = nu * Barrel.LongitudinalUnitForce[i] * p1[i];
                torqueY[i] = nu * Barrel.UnitTorqueY[i] * p1[i];
                torqueZ[i] = nu * Barrel.UnitTorqueZ[i] * p1[i];
            }

            if (ModelProperties.CalculateTemperatures)
            {
                Parallel.For(0, p1.Length, i =>
                {
                    for (int j = 0; j < TemperatureSolver.RadiusesTotal.GetLength(1) - 1; j++)
                    {
                        var r = TemperatureSolver.RadiusesTotal[i, j];
                        var rRight = TemperatureSolver.RadiusesTotal[i, j + 1];

                        var area = Math.PI * (FastMath.Pow2(rRight) - FastMath.Pow2(r));
                        var T = TemperatureSolver.TemperaturesCurrent[i, j] - T0;

                        forceX[i] -= a * E * T * area;
                        torqueY[i] -= a * E * T * area * Barrel.Wy[i];
                        torqueZ[i] -= a * E * T * area * Barrel.Wz[i];
                    }
                });
            }

            return (p1, forceX, torqueY, torqueZ);
        }

        private double getMissileInitialAngularVelocity(double vsn)
        {
            var angularVelocity = 2 * Math.PI / (CurrentAmmo.Missile.Diameter * Barrel.GroovesSlope / vsn);

            if (double.IsFinite(angularVelocity))
                return angularVelocity;

            return 0;
        }

        private void save(double time)
        {
            TimeMoments.Add(time);
            saveInletBallistic();
            saveInletBallisticEpures();

            if (TemperatureSolver.Calculate)
                saveTemperatures();

            if (VibrationSolver.Calculate)
                saveVibrations();

            foreach (var rw in resultWriters)
                rw.Write();
        }

        private void saveInletBallistic()
        {
            InletBallistic.TimeMoments.Add(InletBallisticSolver.Time);
            InletBallistic.Xsn.Add(InletBallisticSolver.Xsn);
            InletBallistic.Vsn.Add(InletBallisticSolver.Vsn);
            InletBallistic.P.Add(InletBallisticSolver.Pressure);
            InletBallistic.Psn.Add(InletBallisticSolver.Psn);
            InletBallistic.Pkn.Add(InletBallisticSolver.Pkn);
            InletBallistic.T.Add(InletBallisticSolver.Temperature);
            InletBallistic.W.Add(InletBallisticSolver.W);
            InletBallistic.J0.Add(InletBallisticSolver.J0);
            InletBallistic.J1.Add(InletBallisticSolver.J1);
            InletBallistic.J2.Add(InletBallisticSolver.J2);
            InletBallistic.Ppr.Add(InletBallisticSolver.Ppr);
            InletBallistic.Density.Add(InletBallisticSolver.Density);
            InletBallistic.Psi.Add(InletBallisticSolver.PsiArr.Copy());
            InletBallistic.Z.Add(InletBallisticSolver.ZArr.Copy());
            InletBallistic.U.Add(InletBallisticSolver.UkArr.Copy());
            InletBallistic.Sigma.Add(InletBallisticSolver.SigmaArr.Copy());
        }

        private void saveInletBallisticEpures()
        {
            for (var i = 0; i < Barrel.X.Length; i++)
            {
                GasEpures.Pressures[i] = Math.Max(GasEpures.Pressures[i], InletBallisticSolver.Pressures[i]);
                GasEpures.Temperatures[i] = Math.Max(GasEpures.Temperatures[i], InletBallisticSolver.Temperatures[i]);
                GasEpures.HeatTransfers[i] = Math.Max(GasEpures.HeatTransfers[i], InletBallisticSolver.HeatTransfers[i]);
                GasEpures.Densities[i] = Math.Max(GasEpures.Densities[i], InletBallisticSolver.Densities[i]);
                GasEpures.GasVelocities[i] = Math.Max(GasEpures.GasVelocities[i], InletBallisticSolver.GasVelocities[i]);
            }
        }

        private void saveTemperatures()
        {
            TemperatureField.LongitudinalEpureXs = Barrel.X;
            TemperatureField.LongitudinalEpureInnerSurface = TemperatureSolver?.MaxTemperatures?.GetColumnSlice(0)?.ToArray() ?? Array.Empty<double>();
            TemperatureField.LongitudinalEpureOuterSurface = TemperatureSolver?.MaxTemperatures?.GetColumnSlice(TemperatureSolver.RadiusesTotal.GetLength(1) - 1)?.ToArray() ?? Array.Empty<double>();

            TemperatureField.RadialEpureRadiuses = TemperatureSolver?.RadiusesTotal?.GetRowSlice(TemperatureSolver.PointMaxTemperatureI)?.ToArray() ?? Array.Empty<double>();
            TemperatureField.RadialEpure = TemperatureSolver?.MaxTemperatures?.GetRowSlice(TemperatureSolver.PointMaxTemperatureI)?.ToArray() ?? Array.Empty<double>();

            TemperatureField.Add(
                TemperatureSolver?.PointMaxTemperature ?? 0,
                TemperatureSolver?.PointTemperature ?? 0,
                TemperatureSolver?.AverageTemperatureInnerSurface ?? 0,
                TemperatureSolver?.AverageTemperatureOuterSurface ?? 0);
        }

        private void saveVibrations()
        {
            Vibrations.EpureInnerRVibrations
                = RadialVibrationsSolver.InnerU.Select((u, i) => Math.Max(u, Vibrations.EpureInnerRVibrations[i])).ToArray();

            Vibrations.Add(VibrationSolver.VibrationXCurrent.LastOrDefault(), VibrationSolver.VibrationYCurrent.LastOrDefault(), VibrationSolver.VibrationZCurrent.LastOrDefault(),
                VibrationSolver.VelocityX.LastOrDefault(), VibrationSolver.VelocityY.LastOrDefault(), VibrationSolver.VelocityZ.LastOrDefault(),
                Algebra.ConvertRadToGrad(VibrationSolver.BarrelHorizontalAngle), Algebra.ConvertRadToGrad(VibrationSolver.BarrelVerticalAngle),
                VibrationSolver.VibrationXCurrent[0],
                VibrationSolver.SystemXCurrent,
                RadialVibrationsSolver.PointDeformation);
        }

        private void saveShotParameters(double time)
        {
            getMissileRotationAngles(out var horizontal, out var vertical);

            var timeIndex = InletBallistic.TimeMoments.BinarySearch(time);

            ShotsParameters.Add(new ShotParameters(
                time,
                InletBallistic.Psn.Skip(timeIndex).Max(),
                InletBallistic.Pkn.Skip(timeIndex).Max(),
                InletBallisticSolver.Vsn,
                VibrationSolver.BarrelEndY,
                VibrationSolver.BarrelEndZ,
                VibrationSolver.BarrelHorizontalAngle,
                VibrationSolver.BarrelVerticalAngle,
                horizontal,
                vertical,
                Math.Sqrt(FastMath.Pow2(VibrationSolver.VelocityX.LastOrDefault()) + FastMath.Pow2(VibrationSolver.VelocityY.LastOrDefault()) + FastMath.Pow2(VibrationSolver.VelocityZ.LastOrDefault()))));
        }

        private void getMissileRotationAngles(out double horizontal, out double vertical)
        {
            vertical = Math.Atan(VibrationSolver.VelocityY.LastOrDefault() / (InletBallisticSolver.Vsn + VibrationSolver.VelocityX.LastOrDefault()));
            horizontal = Math.Atan(VibrationSolver.VelocityZ.LastOrDefault() / (InletBallisticSolver.Vsn + VibrationSolver.VelocityX.LastOrDefault()));
        }

        public double GetSleeveThickness()
        {
            var dSum = 0.0;
            var length = 0.0;

            for (var i = 0; i < Barrel.InnerD.Length - 1; i++)
            {
                if (Barrel.X[i + 1] > Barrel.CamoraLength)
                    break;

                dSum += (Barrel.X[i + 1] - Barrel.X[i]) * (Barrel.InnerD[i + 1] + Barrel.InnerD[i]) / 2;
                length = Barrel.X[i + 1];
            }

            var avgD = dSum / length;

            var a = -Barrel.CamoraLength;
            var b = FastMath.Pow2(avgD / 2) + avgD * Barrel.CamoraLength;
            var c = -CurrentAmmo.PowderCharge.SleeveVolume / Math.PI;

            return (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        }

        public InletBallisticMainResults GetInletBallisticMainResults()
        {
            if (!InletBallistic.TimeMoments.Any())
                return new InletBallisticMainResults();

            return
                 new InletBallisticMainResults
                 {
                     PknMax = InletBallistic.Pkn.Max() * 1e-6,
                     PsnMax = InletBallistic.Psn.Max() * 1e-6,
                     Vmax = ShotsParameters.Max(v => v.ProjectileSpeed)
                 };
        }

        public VibrationsMainResults GetVibrationsMainResults()
        {
            if (!Vibrations.BarrelEndYs.Any())
                return new VibrationsMainResults();

            var y0 = Vibrations.BarrelEndYs.FirstOrDefault();
            var z0 = Vibrations.BarrelEndZs.FirstOrDefault();

            var ya0 = Vibrations.BarrelVerticalAngles.FirstOrDefault();
            var za0 = Vibrations.BarrelHorizontalAngles.FirstOrDefault();

            var vibrationAmplitude = 0.0;
            var angleAmplitude = 0.0;

            for (var i = 0; i < Vibrations.BarrelEndYs.Count; i++)
            {
                var vibrationA = Math.Sqrt(FastMath.Pow2(Vibrations.BarrelEndYs[i] - y0) + FastMath.Pow2(Vibrations.BarrelEndZs[i] - z0));
                var angleA = Math.Sqrt(FastMath.Pow2(Vibrations.BarrelVerticalAngles[i] - ya0) + FastMath.Pow2(Vibrations.BarrelHorizontalAngles[i] - za0));

                if (vibrationA > vibrationAmplitude)
                    vibrationAmplitude = vibrationA;

                if (angleA > angleAmplitude)
                    angleAmplitude = angleA;
            }

            return
                 new VibrationsMainResults
                 {
                     VibrationsYAmplitude = 0.5 * (Vibrations.BarrelEndYs.Max() - Vibrations.BarrelEndYs.Min()) * 1e6,
                     VibrationsZAmplitude = 0.5 * (Vibrations.BarrelEndZs.Max() - Vibrations.BarrelEndZs.Min()) * 1e6,
                     VibrationsAmplitude = vibrationAmplitude * 1e6,
                     AngleAmplitude = angleAmplitude,
                 };
        }
    }
}
