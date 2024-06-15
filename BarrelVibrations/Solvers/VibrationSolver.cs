using BarrelVibrations.ModelingObjects;
using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.FiringSystemFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BasicLibraryWinForm;
using MathNet.Numerics.Interpolation;

namespace BarrelVibrations.Solvers;

public class VibrationSolver
{
    public Missile Missile { get; set; }

    private readonly bool calculateMissileGravity;
    private readonly Barrel barrel;
    private readonly MuzzleBreak muzzleBreak;
    private readonly double[] deflectionX;
    private readonly double[] deflectionY;
    private readonly double[] deflectionZ;
    private readonly double E;
    private readonly double rho;
    private readonly double[] F;

    private readonly double g;
    private readonly int I;
    private readonly double[] Jy;
    private readonly double[] Jz;
    private readonly double smoothFrictionCoefficient;
    private readonly double[] S;
    private readonly double shotAngle;
    private readonly ModelingObjects.EnvironmentFolder.Environment environment;
    private readonly FiringSystem firingSystem;
    private readonly double[] Wy;
    private readonly double[] Wz;
    private readonly double[] xs;
    private double time;
    private double dt, dtPrevious, dt_2;

    public bool Calculate { get; }
    public int Iteration { get; private set; }

    /// <summary>
    ///     Текущие локальные перемещения ствола по X
    /// </summary>
    public double[] VibrationXCurrent { get; private set; }

    /// <summary>
    ///     Текущие локальные перемещения ствола по Y
    /// </summary>
    public double[] VibrationYCurrent { get; private set; }

    /// <summary>
    ///     Текущие локальные перемещения ствола по Z
    /// </summary>
    public double[] VibrationZCurrent { get; private set; }

    /// <summary>
    ///     Предыдущие локальные перемещения ствола по X
    /// </summary>
    private double[] VibrationXPrevious;

    /// <summary>
    ///     Предыдущие локальные перемещения ствола по Y
    /// </summary>
    private double[] VibrationYPrevious;

    /// <summary>
    ///     Предыдущие локальные перемещения ствола по Z
    /// </summary>
    private double[] VibrationZPrevious;

    /// <summary>
    ///     Предпредыдущие локальные перемещения ствола по X
    /// </summary>
    private double[] VibrationXPrevious2;

    /// <summary>
    ///     Предпредыдущие локальные перемещения ствола по Y
    /// </summary>
    private double[] VibrationYPrevious2;

    /// <summary>
    ///     Предпредыдущие локальные перемещения ствола по Z
    /// </summary>
    private double[] VibrationZPrevious2;

    /// <summary>
    /// Скорость по Ox
    /// </summary>
    public IEnumerable<double> VelocityX => dt == 0 ? Enumerable.Repeat(0.0, VibrationXCurrent.Length) : VibrationXCurrent.Select((v, i) => (v - VibrationXPrevious[i]) / dt);

    /// <summary>
    /// Скорость по Oy
    /// </summary>
    public IEnumerable<double> VelocityY => dt == 0 ? Enumerable.Repeat(0.0, VibrationYCurrent.Length) : VibrationYCurrent.Select((v, i) => (v - VibrationYPrevious[i]) / dt);

    /// <summary>
    /// Скорость по Oz
    /// </summary>
    public IEnumerable<double> VelocityZ => dt == 0 ? Enumerable.Repeat(0.0, VibrationZCurrent.Length) : VibrationZCurrent.Select((v, i) => (v - VibrationZPrevious[i]) / dt);

    /// <summary>
    /// Ускорение по Ox
    /// </summary>
    public IEnumerable<double> AccelerationX => dt == 0 || dtPrevious == 0
        ? Enumerable.Repeat(0.0, VibrationXCurrent.Length)
        : VibrationXCurrent.Select((v, i) => ((v - VibrationXPrevious[i]) / dt - (VibrationXPrevious[i] - VibrationXPrevious2[i]) / dtPrevious) / dt_2);

    /// <summary>
    /// Ускорение по Oy
    /// </summary>
    public IEnumerable<double> AccelerationY => dt == 0 || dtPrevious == 0
        ? Enumerable.Repeat(0.0, VibrationYCurrent.Length)
        : VibrationYCurrent.Select((v, i) => ((v - VibrationYPrevious[i]) / dt - (VibrationYPrevious[i] - VibrationYPrevious2[i]) / dtPrevious) / dt_2);

    /// <summary>
    /// Ускорение по Oz
    /// </summary>
    public IEnumerable<double> AccelerationZ => dt == 0 || dtPrevious == 0
        ? Enumerable.Repeat(0.0, VibrationZCurrent.Length)
        : VibrationZCurrent.Select((v, i) => ((v - VibrationZPrevious[i]) / dt - (VibrationZPrevious[i] - VibrationZPrevious2[i]) / dtPrevious) / dt_2);

    /// <summary>
    /// Положение всей установки (БМП, танка, гаубицы и т.п.) в текущий момент времени
    /// </summary>
    public double SystemXCurrent { get; private set; }

    /// <summary>
    /// Положение всей установки (БМП, танка, гаубицы и т.п.) в предыдущий момент времени
    /// </summary>
    private double SystemXPrevious;

    /// <summary>
    /// Положение всей установки (БМП, танка, гаубицы и т.п.) в предпредыдущий момент времени
    /// </summary>
    private double SystemXPrevious2;

    /// <summary>
    /// Горизонтальный угол наклона дульного среза, рад
    /// </summary>
    public double BarrelHorizontalAngle
    {
        get
        {
            var dz = VibrationZCurrent[I] - VibrationZCurrent[I - 1] + Wz[I] - Wz[I - 1];

            return Math.Atan(dz / (xs[I] - xs[I - 1]));
        }
    }

    /// <summary>
    /// Вертикальный угол наклона дульного среза, рад
    /// </summary>
    public double BarrelVerticalAngle
    {
        get
        {
            var dy = VibrationYCurrent[I] - VibrationYCurrent[I - 1] + Wy[I] - Wy[I - 1];

            return shotAngle + Math.Atan(dy / (xs[I] - xs[I - 1]));
        }
    }

    /// <summary>
    /// Горизонтальная угловая скорость дульного среза, рад/с
    /// </summary>
    public double BarrelEndHorizontalAngularVelocity
    {
        get
        {
            if (dt == 0)
                return 0;

            var dz = VibrationZPrevious[I] - VibrationZPrevious[I - 1] + Wz[I] - Wz[I - 1];
            var previousAngle = Math.Atan(dz / (xs[I] - xs[I - 1]));

            return (BarrelHorizontalAngle - previousAngle) / dt;
        }
    }

    /// <summary>
    /// Вертикальная угловая скорость дульного среза, рад/с
    /// </summary>
    public double BarrelEndVerticalAngularVelocity
    {
        get
        {
            if (dt == 0)
                return 0;

            var dy = VibrationYPrevious[I] - VibrationYPrevious[I - 1] + Wy[I] - Wy[I - 1];
            var previousAngle = shotAngle + Math.Atan(dy / (xs[I] - xs[I - 1]));

            return (BarrelVerticalAngle - previousAngle) / dt;
        }
    }

    /// <summary>
    /// Координата x дульного среза в глобальной системе координат
    /// </summary>
    public double BarrelEndX => SystemXCurrent + (xs.Last() + VibrationXCurrent.Last()) * Math.Cos(shotAngle) - (VibrationYCurrent.Last() + Wy.Last()) * Math.Sin(shotAngle) + SystemXCurrent;

    /// <summary>
    /// Координата y дульного среза в глобальной системе координат
    /// </summary>
    public double BarrelEndY => (xs.Last() + VibrationXCurrent.Last()) * Math.Sin(shotAngle) + (VibrationYCurrent.Last() + Wy.Last()) * Math.Cos(shotAngle);

    /// <summary>
    /// Координата z дульного среза в глобальной системе координат
    /// </summary>
    public double BarrelEndZ => VibrationZCurrent.Last() + Wz.Last();

    private double MissileInteractionCoefficient => Physics.GetInteractionCoefficient(barrel.GroovesCount, smoothFrictionCoefficient, barrel.GroovesSlope);

    private readonly double[] d2Wy;
    private readonly double[] d2Wz;

    private bool missileInBarrel;
    private double missileCoordinate;
    private int missileBackI;
    private int missileFrontI;
    private double Psn;
    private double[] p1;

    private CubicSpline? barrelMovementsSpline;

    public VibrationSolver(
        Material material,
        Barrel barrelGeometry,
        MuzzleBreak muzzleBreak,
        Missile missile,
        ModelingObjects.EnvironmentFolder.Environment environment,
        FiringSystem firingSystem,
        double missileFrictionCoefficient,
        double shotAngle,
        double[] xDeflection,
        double[] yDeflection,
        double[] zDeflection,
        bool calculate,
        bool calculateMissileGravity,
        double g = 9.81)
    {
        this.barrel = barrelGeometry;
        this.muzzleBreak = muzzleBreak;
        this.Missile = missile;
        this.smoothFrictionCoefficient = missileFrictionCoefficient;
        this.shotAngle = shotAngle;
        this.environment = environment;
        this.firingSystem = firingSystem;
        F = barrelGeometry.F;
        S = barrelGeometry.S;
        Jy = barrelGeometry.Jy;
        Jz = barrelGeometry.Jz;
        Wy = barrelGeometry.Wy;
        Wz = barrelGeometry.Wz;
        I = barrelGeometry.X.Length - 1;
        xs = barrelGeometry.X;

        if (firingSystem.BarrelMovements.Any())
            barrelMovementsSpline = CubicSpline.InterpolateAkima(
                firingSystem.BarrelMovements.Select(data => data.Time),
                firingSystem.BarrelMovements.Select(data => data.Displacement));

        d2Wy = new double[xs.Length];
        d2Wz = new double[xs.Length];
        p1 = new double[xs.Length];

        for (var i = 0; i < d2Wy.Length; i++)
        {
            d2Wy[i] = Algebra.GetDerivative2(xs, Wy, i);
            d2Wz[i] = Algebra.GetDerivative2(xs, Wz, i);
        }

        this.calculateMissileGravity = calculateMissileGravity;

        E = material.YoungModulus;
        rho = material.Density;

        this.g = g;
        Calculate = calculate;
        VibrationXCurrent = xDeflection.Copy();
        VibrationYCurrent = yDeflection.Copy();
        VibrationZCurrent = zDeflection.Copy();

        VibrationXPrevious = xDeflection.Copy();
        VibrationYPrevious = yDeflection.Copy();
        VibrationZPrevious = zDeflection.Copy();

        VibrationXPrevious2 = xDeflection.Copy();
        VibrationYPrevious2 = yDeflection.Copy();
        VibrationZPrevious2 = zDeflection.Copy();

        deflectionX = xDeflection.Copy();
        deflectionY = yDeflection.Copy();
        deflectionZ = zDeflection.Copy();
    }


    public void SolveIteration(
        double time,
        double dt,
        double[] p1,
        double psn,
        double missileCoordinate,
        double[] forceX,
        double[] torqueY,
        double[] torqueZ,
        bool missileInBarrel)
    {
        if (!Calculate)
            return;

        this.time = time;
        dtPrevious = this.dt;
        this.dt = dt;

        if (dtPrevious == 0)
            dtPrevious = dt;

        dt_2 = dt * dtPrevious;

        if (Iteration > 1)
        {
            Task[] copyTasks =
            {
                Task.Run(() => VibrationXPrevious2 = VibrationXPrevious.Copy()),
                Task.Run(() => VibrationYPrevious2 = VibrationYPrevious.Copy()),
                Task.Run(() => VibrationZPrevious2 = VibrationZPrevious.Copy())
            };

            SystemXPrevious2 = SystemXPrevious;

            Task.WaitAll(copyTasks);
        }

        if (Iteration > 0)
        {
            Task[] copyTasks =
            {
                Task.Run(() => VibrationXPrevious = VibrationXCurrent.Copy()),
                Task.Run(() => VibrationYPrevious = VibrationYCurrent.Copy()),
                Task.Run(() => VibrationZPrevious = VibrationZCurrent.Copy())
            };

            SystemXPrevious = SystemXCurrent;

            Task.WaitAll(copyTasks);
        }

        Psn = psn;
        this.missileCoordinate = missileCoordinate;
        this.missileInBarrel = missileInBarrel;

        (missileBackI, missileFrontI) = GetMissileIndecies();

        CalculateIteration(p1,
            forceX,
            torqueY,
            torqueZ);

        Iteration++;
    }

    private void CalculateIteration(
        double[] p1,
        double[] forceX,
        double[] torqueY,
        double[] torqueZ)
    {
        this.p1 = p1;

        if (firingSystem.IsFullSystemMoving)
            CalculateRecoil(p1[0]);

        CalculateF1(p1, forceX, out var f1);

        VibrationXCurrent = CalculateX(f1);

        CalculateF23(p1, VibrationXCurrent, forceX, torqueY, torqueZ, out var fx, out var f2, out var f3);

        Task[] calcTasks =
        {
            Task.Run(() =>
                VibrationYCurrent = CalculateY(fx, f2)),
            Task.Run(() =>
                VibrationZCurrent = CalculateZ(fx, f3))
        };

        Task.WaitAll(calcTasks);
    }

    private (int, int) GetMissileIndecies()
    {
        if (!missileInBarrel)
            return (-1, -1);

        var (i0, i1) = Algebra.BinarySearch(xs, missileCoordinate);
        var (j0, j1) = Algebra.BinarySearch(xs, Math.Min(missileCoordinate + Missile.BodyLength, xs.Last()));

        int iBack;
        if (Math.Abs(missileCoordinate - xs[i0]) < Math.Abs(missileCoordinate - xs[i1]))
            iBack = i0;
        else
            iBack = i1;

        int iFront;
        if (Math.Abs(missileCoordinate + Missile.BodyLength - xs[j0]) < Math.Abs(missileCoordinate + Missile.BodyLength - xs[j1]))
            iFront = j0;
        else
            iFront = j1;

        return (iBack, iFront);
    }

    private double GetBackForce(double pressure) => -S[0] * pressure;
    private double GetForwardForce(double pressure) => muzzleBreak.Efficiency * pressure * S[^1];

    private void CalculateRecoil(double pressure)
    {
        var backForce = GetBackForce(pressure);
        var forwardForce = GetForwardForce(pressure);

        var force = backForce + forwardForce;
        var systemFrictionForce = (-Math.Sign(SystemXPrevious - SystemXPrevious2) * firingSystem.FullSystemMass * g
            - force * Math.Sin(shotAngle)) * firingSystem.FullSystemFrictionCoefficient;

        SystemXCurrent = 2 * SystemXPrevious - SystemXPrevious2
                                + (force * Math.Cos(shotAngle) + systemFrictionForce) * dt_2 / firingSystem.FullSystemMass;
    }

    private double[] CalculateX(double[] f1)
    {
        var A = new double[I + 1, I + 1];
        var b = new double[I + 1];

        for (var i = 0; i <= I; i++)
        {
            if (i == 0 || barrel.FixationsAreas.Any(area => xs[i] >= area.From && xs[i] <= area.To))
            {
                if (firingSystem.IsMovingBarrel)
                {
                    if (barrelMovementsSpline != null && time <= firingSystem.BarrelMovements.Last().Time)
                    {
                        A[i, i] = 1;

                        b[i] = deflectionX[i] + barrelMovementsSpline.Interpolate(time);
                    }
                    else
                    {
                        A[i, i] = rho * F[i] / dt_2
                            + E * (F[i] + F[i + 1]) / FastMath.Pow2(xs[i + 1] - xs[i])
                            + 2 * firingSystem.MovingPartsStiffness / (xs[i + 1] - xs[i])
                            + 2 * firingSystem.MovingPartsDamping / (dt * (xs[i + 1] - xs[i]));

                        A[i, i + 1] = -E * (F[i] + F[i + 1]) / FastMath.Pow2(xs[i + 1] - xs[i]);

                        b[i] = rho * F[i] * (2 * VibrationXPrevious[i] - VibrationXPrevious2[i]) / dt_2
                            - 2 * p1[i] * S[i] / (xs[i + 1] - xs[i])
                            + 2 * firingSystem.MovingPartsDamping * VibrationXPrevious[i] / (dt * (xs[i + 1] - xs[i]))
                            - (3 * f1[i] + f1[i + 1]) / 4;
                    }
                }
                else
                {
                    A[i, i] = 1;

                    b[i] = deflectionX[i];
                }
            }
            else if (i == I)
            {
                A[i, i - 1] = -F[i] * E / (xs[i] - xs[i - 1]);
                A[i, i] = F[i] * E / (xs[i] - xs[i - 1]);

                b[i] = f1[i];
            }
            else
            {
                var du = (2 * VibrationXPrevious[i] - VibrationXPrevious2[i]) / dt_2;

                var hR1 = xs[i + 1] - xs[i];
                var hR1_2 = (xs[i + 1] - xs[i - 1]) / 2;
                var h = xs[i] - xs[i - 1];
                var FR1_2 = (F[i + 1] + F[i]) / 2;
                var FL1_2 = (F[i - 1] + F[i]) / 2;

                A[i, i - 1] = -E * FL1_2 / h;

                A[i, i] = E * (FR1_2 / hR1 + FL1_2 / h)
                          + hR1_2 * rho * F[i] / dt_2;

                A[i, i + 1] = -E * FR1_2 / hR1;

                b[i] = hR1_2 * rho * F[i] * du - hR1_2 * f1[i];
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
            var dv = (2 * VibrationYPrevious[i] - VibrationYPrevious2[i]) / dt_2;

            if (i == 0 || barrel.FixationsAreas.Any(area => xs[i] >= area.From && xs[i] <= area.To))
            {
                A[i, i] = 1;

                b[i] = deflectionY[i];
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
                          fx[i] * (1.0 / hR1 + 1.0 / h) +
                          hR1_2 * rho * F[i] / dt_2);

                A[i, i + 1] = E * Jz[i] / (hR1 * hR1 * hR1_2) +
                                E * Jz[i] / (h * hR1 * hR1_2) -
                                fx[i] / hR1;

                b[i] = hR1_2 * (f2[i] - rho * F[i] * dv);
            }
            else if (i == I)
            {
                var h = xs[i] - xs[i - 1];
                var hL1 = xs[i - 1] - xs[i - 2];
                var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                A[i, i - 2] = -E * Jz[i - 1] / (h * hL1 * hL1_2);

                A[i, i - 1] = E * Jz[i - 1] / (h * hL1 * hL1_2) +
                              E * Jz[i - 1] / (h * h * hL1_2);

                A[i, i] = -(E * Jz[i - 1] / (h * h * hL1_2) +
                            0.5 * h * rho * F[i] / dt_2);

                b[i] = h * 0.5 * (f2[i] - rho * F[i] * dv);
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
                          fx[i] * (1.0 / hR1 + 1.0 / h) +
                          hR1_2 * rho * F[i] / dt_2);

                A[i, i + 1] = E * Jz[i + 1] / (hR1 * hR2 * hR3_2) +
                                E * Jz[i + 1] / (hR1 * hR1 * hR3_2) +
                                E * Jz[i] / (hR1 * hR1 * hR1_2) +
                                E * Jz[i] / (h * hR1 * hR1_2) -
                                fx[i] / hR1;

                A[i, i + 2] = -E * Jz[i + 1] / (hR1 * hR2 * hR3_2);

                b[i] = hR1_2 * (f2[i] - rho * F[i] * dv);
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
            var dw = (2 * VibrationZPrevious[i] - VibrationZPrevious2[i]) / dt_2;

            if (i == 0 || barrel.FixationsAreas.Any(area => xs[i] >= area.From && xs[i] <= area.To))
            {
                A[i, i] = 1;

                b[i] = deflectionZ[i];
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
                          fx[i] * (1.0 / hR1 + 1.0 / h) +
                          hR1_2 * rho * F[i] / dt_2);

                A[i, i + 1] = E * Jy[i] / (hR1 * hR1 * hR1_2) +
                                E * Jy[i] / (h * hR1 * hR1_2) -
                                fx[i] / hR1;

                b[i] = hR1_2 * (f3[i] - rho * F[i] * dw);
            }
            else if (i == I)
            {
                var h = xs[i] - xs[i - 1];
                var hL1 = xs[i - 1] - xs[i - 2];
                var hL1_2 = (xs[i] - xs[i - 2]) / 2;

                A[i, i - 2] = -E * Jy[i - 1] / (h * hL1 * hL1_2);

                A[i, i - 1] = E * Jy[i - 1] / (h * hL1 * hL1_2) +
                                E * Jy[i - 1] / (h * h * hL1_2);

                A[i, i] = -(E * Jy[i - 1] / (h * h * hL1_2) +
                            0.5 * h * rho * F[i] / dt_2);

                b[i] = h * 0.5 * (f3[i] - rho * F[i] * dw);
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
                          fx[i] * (1.0 / hR1 + 1.0 / h) +
                          hR1_2 * rho * F[i] / dt_2);

                A[i, i + 1] = E * Jy[i + 1] / (hR1 * hR2 * hR3_2) +
                                E * Jy[i + 1] / (hR1 * hR1 * hR3_2) +
                                E * Jy[i] / (hR1 * hR1 * hR1_2) +
                                E * Jy[i] / (h * hR1 * hR1_2) -
                                fx[i] / hR1;

                A[i, i + 2] = -E * Jy[i + 1] / (hR1 * hR2 * hR3_2);

                b[i] = hR1_2 * (f3[i] - rho * F[i] * dw);
            }
        }

        return LinearEquationSolver.SolveDiagonalSystem(A, b, 5);
    }

    private void CalculateF1(double[] p1, double[] Fx, out double[] f1)
    {
        f1 = new double[xs.Length];

        for (int i = 1; i < xs.Length - 3; i++)
        {
            f1[i] =
                rho * F[i] * g * Math.Sin(shotAngle)
                - (Fx[i + 1] - Fx[i - 1]) / (xs[i + 1] - xs[i - 1])
                + p1[i] * (S[i + 1] - S[i - 1]) / (xs[i + 1] - xs[i - 1]);
        }

        for (int i = xs.Length - 3; i < xs.Length - 1; i++)
        {
            f1[i] =
                rho * F[i] * g * Math.Sin(shotAngle)
                + Fx[i - 1] / (xs[i + 1] - xs[i - 1])
                + p1[i] * (S[i + 1] - S[i - 1]) / (xs[i + 1] - xs[i - 1]);
        }

        f1[0] = rho * F[0] * g * Math.Sin(shotAngle)
                - (Fx[1] - Fx[0]) / (xs[1] - xs[0])
                + p1[0] * (S[1] - S[0]) / (xs[1] - xs[0]);

        f1[^1] = -muzzleBreak.Mass * g * Math.Sin(shotAngle)
            + muzzleBreak.Efficiency * p1[^1] * S[^1];

        if (missileInBarrel && calculateMissileGravity && missileBackI != missileFrontI)
        {
            var maxI = Math.Min(xs.Length - 2, missileFrontI);
            for (int i = missileBackI; i <= maxI; i++)
            {
                f1[i] += (Math.Sin(shotAngle) * g * Missile.Mass - S[i] * Psn * MissileInteractionCoefficient) / Missile.BodyLength;
            }
        }
    }

    private void CalculateF23(double[] p1, double[] u, double[] forceX, double[] torqueY, double[] torqueZ, out double[] fx, out double[] f2, out double[] f3)
    {
        f2 = new double[xs.Length];
        f3 = new double[xs.Length];
        fx = new double[xs.Length];

        for (int i = 0; i < xs.Length; i++)
        {
            var iR = i == xs.Length - 1 ? i : i + 1;
            var iL = i == 0 ? i : i - 1;

            fx[i] = F[i] * E * (u[iR] - u[iL]) / (xs[iR] - xs[iL])
                + forceX[i] + p1[i] * S[i];

            var fy = 0.0;
            var fz = 0.0;

            if (i < xs.Length - 2)
            {
                fy = Algebra.GetDerivative2(xs, torqueY, i);
                fz = Algebra.GetDerivative2(xs, torqueZ, i);
            }
            else if (i == xs.Length - 2)
            {
                // момент силы на дульном срезе = 0
                fy = ((-torqueY[i]) / (xs[i + 1] - xs[i]) - (torqueY[i] - torqueY[i - 1]) / (xs[i] - xs[i - 1])) / (0.5 * (xs[i + 1] - xs[i - 1]));
                fz = ((-torqueZ[i]) / (xs[i + 1] - xs[i]) - (torqueZ[i] - torqueZ[i - 1]) / (xs[i] - xs[i - 1])) / (0.5 * (xs[i + 1] - xs[i - 1]));
            }
            else if (i == xs.Length - 1)
            {
                // момент силы = 0 и перерезывающая сила = 0, умножили на 2 т.к. 0,5f
                fy = 2 * torqueY[i - 1] / FastMath.Pow2(xs[i] - xs[i - 1]);
                fz = 2 * torqueZ[i - 1] / FastMath.Pow2(xs[i] - xs[i - 1]);
            }

            f2[i] =
                rho * F[i] * g * Math.Cos(shotAngle)
                - fy
                - fx[i] * d2Wy[i];

            f3[i] = -fz
                - fx[i] * d2Wz[i];
        }

        if (missileInBarrel && calculateMissileGravity && missileBackI != missileFrontI)
        {
            var maxI = Math.Min(xs.Length - 2, missileFrontI);
            for (int i = missileBackI; i <= maxI; i++)
            {
                f2[i] += Math.Cos(shotAngle) * g * Missile.Mass / Missile.BodyLength;
            }
        }

        f2[^1] += 2 * muzzleBreak.Mass * g * Math.Cos(shotAngle) / (xs[^1] - xs[^2]); // умножаем на 2 т.к. 0,5f
    }
}