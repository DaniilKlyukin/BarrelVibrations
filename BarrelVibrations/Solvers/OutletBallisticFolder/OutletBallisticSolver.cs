using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BarrelVibrations.Solvers.Solutions;
using BasicLibraryWinForm;
using BasicLibraryWinForm.Minimization;
using MathNet.Numerics.Interpolation;
using Environment = BarrelVibrations.ModelingObjects.EnvironmentFolder.Environment;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations.Solvers.OutletBallisticFolder;

public class OutletBallisticSolver
{
    private readonly Wind wind;
    private Missile missile;
    private readonly CubicSpline derivationResistantSpline;

    public OutletBallisticSolver(
        Terrain terrain,
        Wind wind)
    {
        Terrain = terrain;
        this.wind = wind;
        derivationResistantSpline = CubicSpline.InterpolatePchip(
            new double[] { 0, 0.6, 0.8, 1.1, 1.5, 2.9, 4.9, 100 },
            new double[] { 0.206, 0.207, 0.204, 0.181, 0.212, 0.321, 0.3695, 0.370 });
    }

    public Terrain Terrain { get; }

    /// <summary>
    ///     Находится ли снаряд над поверхностью
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private bool IsMissileAboveTerrain(double x, double y, double z)
    {
        return y > Terrain.GetAltitude(x, z);
    }

    /// <summary>
    /// Решает задачу внешней баллистики
    /// </summary>
    /// <param name="x0">Начальная координата X дна снаряда, м</param>
    /// <param name="y0">Начальная координата Y дна снаряда, м</param>
    /// <param name="z0">Начальная координата Z дна снаряда, м</param>
    /// <param name="vx0">Дополнительная внешняя скорость по Ox, м/с</param>
    /// <param name="vy0">Дополнительная внешняя скорость по Oy, м/с</param>
    /// <param name="vz0">Дополнительная внешняя скорость по Oz, м/с</param>
    /// <param name="v">Скорость снаряда, м/с</param>
    /// <param name="verticalMovementAngle">Вертикальный угол движения снаряда, рад</param>
    /// <param name="horizontalMovementAngle">Горизонтальный угол движения снаряда, рад</param>
    /// <param name="verticalDeltaAngle">Вертикальный угол наклона снаряда относительно направления движения, рад</param>
    /// <param name="horizontalDeltaAngle">Горизонтальный угол наклона снаряда относительно направления движения, рад</param>
    /// <param name="angularRotationVelocity">Угловая скорость вращения снаряда, рад/с</param>
    /// <param name="tolerance">Требуемая точность</param>
    /// <returns></returns>
    public OutletBallistic Solve(
        Missile missile,
        Environment environment,
        double x0,
        double y0,
        double z0,
        double vx0,
        double vy0,
        double vz0,
        double v,
        double verticalMovementAngle,
        double horizontalMovementAngle,
        double verticalDeltaAngle,
        double horizontalDeltaAngle,
        double angularVerticalVelocity,
        double angularHorizontalVelocity,
        double angularRotationVelocity,
        double timeStep = 1e-4,
        bool missileVibrations = true)
    {
        this.missile = missile;

        var resultArraysSize = 5000;

        var times = new List<double>();
        var X = new List<double>();
        var Y = new List<double>();
        var Z = new List<double>();
        var V = new List<double>();

        var verticalMovementAngles = new List<double>();
        var horizontalMovementAngles = new List<double>();
        var verticalDeltaAngles = new List<double>();
        var horizontalDeltaAngles = new List<double>();

        var verticalAngularVelocities = new List<double>();
        var horizontalAngularVelocities = new List<double>();
        var angularRotationVelocities = new List<double>();

        var R = 287.0;

        var epsList = new List<double>();
        var tauList = new List<double>();

        var solver = new BasicLibraryWinForm.ODE.RungeKutta4Solver
        {
            InitialValues = new OdeSolver.OdeSolution(new[]
        {
                x0,
                y0,
                z0,
                Math.Sqrt(FastMath.Pow2(v + vx0) + FastMath.Pow2(vy0) + FastMath.Pow2(vz0)),
                verticalMovementAngle,
                horizontalMovementAngle,
                verticalDeltaAngle,
                horizontalDeltaAngle,
                angularVerticalVelocity,
                angularHorizontalVelocity,
                angularRotationVelocity,
            }),
            EquationsObserverInitialization = () =>
            {
                times = new List<double>();
                X = new List<double>();
                Y = new List<double>();
                Z = new List<double>();
                V = new List<double>();

                verticalMovementAngles = new List<double>();
                horizontalMovementAngles = new List<double>();
                verticalDeltaAngles = new List<double>();
                horizontalDeltaAngles = new List<double>();

                verticalAngularVelocities = new List<double>();
                horizontalAngularVelocities = new List<double>();
                angularRotationVelocities = new List<double>();
            },
            EquationsObserver = (t, x) =>
            {
                times.Add(t);

                X.Add(x[0]);
                Y.Add(x[1]);
                Z.Add(x[2]);
                V.Add(x[3]);

                verticalMovementAngles.Add(x[4]);
                horizontalMovementAngles.Add(x[5]);
                verticalDeltaAngles.Add(x[6]);
                horizontalDeltaAngles.Add(x[7]);

                verticalAngularVelocities.Add(x[8]);
                horizontalAngularVelocities.Add(x[9]);
                angularRotationVelocities.Add(x[10]);
            },
            EquationsSystem = (t, vec, dxdt) =>
            {
                var x = vec[0];
                var y = vec[1];
                var z = vec[2];
                var vk = vec[3];
                var verticalMovementAngle = vec[4];
                var horizontalMovementAngle = vec[5];

                var d2 = missileVibrations ? vec[6] : 0;
                var d1 = missileVibrations ? vec[7] : 0;

                var w2 = missileVibrations ? vec[8] : 0;
                var w1 = missileVibrations ? vec[9] : 0;
                var wx = vec[10];

                var (e1, e2) = GetWindAngles(x, y, z, vk, verticalMovementAngle, horizontalMovementAngle);

                var a1 = d1 - e1;
                var a2 = d2 - e2;

                var angle = Math.Sqrt(FastMath.Pow2(a1) + FastMath.Pow2(a2));
                var a = environment.MeteoTable.SoundSpeed(y);
                var M = vk / a;
                var p = environment.MeteoTable.Pressure(y);
                var T = environment.MeteoTable.Temperature(y);
                var g = Physics.GetGravity(y);
                var knm = derivationResistantSpline.Interpolate(M);

                var beta = FastMath.Pow2(vk) * p / (2 * R * T);

                var wx_ = wx * missile.Length / vk;

                double cx, cy, cz, mx = 0, m1 = 0, m2 = 0;

                if (missileVibrations && angularRotationVelocity != 0)
                {
                    var cRow = GetRegressionCRow(M);
                    cx = missile.ix * GetCxk(cRow, M, angle);
                    cy = GetCyk(cRow, M, wx_, a1, a2);
                    cz = GetCzk(cRow, M, wx_, a1, a2);

                    mx = GetMx(M, wx_);
                    m1 = GetMHorizontal(M, wx_, a1, a2);
                    m2 = GetMVertical(M, wx_, a1, a2);
                }
                else
                {
                    if (missile.IsSubcaliber)
                        (cx, cy, cz) = FormResistanceCoefficientFinder1958.GetResistanceCoefficients(M);
                    else
                        (cx, cy, cz) = FormResistanceCoefficientFinder1943.GetResistanceCoefficients(M);

                    w1 = 0;
                    w2 = 0;
                    cx *= missile.ix;
                }

                var derivation = missile.iz * GetDerivationFunction(M) * missile.Ix * wx * dxdt[4] / missile.Length;

                var equations = new OdeSolver.OdeSolution(vec.Length)
                {
                    [0] = vk * Math.Cos(verticalMovementAngle) * Math.Cos(horizontalMovementAngle),  //x
                    [1] = vk * Math.Sin(verticalMovementAngle),                                      //y
                    [2] = -vk * Math.Cos(verticalMovementAngle) * Math.Sin(horizontalMovementAngle), //z
                    [3] = -g * Math.Sin(verticalMovementAngle) - cx * beta * missile.SectionArea / missile.MassAfterShot, //vk

                    [4] = -g * Math.Cos(verticalMovementAngle) / vk - cy * beta * missile.SectionArea / (missile.MassAfterShot * vk),
                    [5] = (derivation - cz * beta * missile.SectionArea) / (missile.MassAfterShot * vk * Math.Cos(verticalMovementAngle)),

                    [6] = w2 - dxdt[5] * a1 * Math.Sin(verticalMovementAngle) - dxdt[4] * Math.Cos(a1),
                    [7] = (w1 - dxdt[5] * Math.Cos(verticalMovementAngle + a2) - dxdt[4] * a1 * a2) / Math.Cos(a2),

                    [8] = (m2 * beta * missile.SectionArea * missile.Length + missile.Ix * wx * w1) / missile.Iy,
                    [9] = (m1 * beta * missile.SectionArea * missile.Length - missile.Ix * wx * w2) / missile.Iz,

                    [10] = -mx * beta * missile.SectionArea * missile.Length / missile.Ix
                };

                return equations;
            },
            ErrorFunc = (sol0, sol1) =>
            {
                if (sol0.Count <= 1 || sol1.Count <= 1)
                    return 0;

                var x0_old = sol0[^2][0];
                var x1_old = sol0.Last()[0];
                var y0_old = sol0[^2][1];
                var y1_old = sol0.Last()[1];
                var z0_old = sol0[^2][2];
                var z1_old = sol0.Last()[2];

                var x_old = Algebra.GetValueAtLine(0, y0_old, x0_old, y1_old, x1_old);
                var z_old = Algebra.GetValueAtLine(0, y0_old, z0_old, y1_old, z1_old);

                var x0_new = sol1[^2][0];
                var x1_new = sol1.Last()[0];
                var y0_new = sol1[^2][1];
                var y1_new = sol1.Last()[1];
                var z0_new = sol1[^2][2];
                var z1_new = sol1.Last()[2];

                var x_new = Algebra.GetValueAtLine(0, y0_new, x0_new, y1_new, x1_new);
                var z_new = Algebra.GetValueAtLine(0, y0_new, z0_new, y1_new, z1_new);

                var d_old = Algebra.GetDistance(0, 0, x_old, z_old);
                var d_new = Algebra.GetDistance(0, 0, x_new, z_new);

                var eps = Math.Abs(d_old / d_new - 1);

                epsList.Add(eps);
                tauList.Add(times.Last() / times.Count);

                return eps;
            },
            CheckStop = (t, x) => !IsMissileAboveTerrain(x[0], x[1], x[2])
        };

        solver.InitializeStepSolver(0);

        while (true)
        {
            if (solver.SolveStep(timeStep))
                break;
        }

        if (times.Count < 2)
        {
            return new OutletBallistic();
        }

        var y_2 = Y[^2];
        var y_1 = Y.Last();

        var t0 = times[^2];
        var t1 = times.Last();

        var endTime = Algebra.GetValueAtLine(Terrain.GetAltitude(X.Last(), Z.Last()), y_2, t0, y_1, t1);

        var dt = endTime / (resultArraysSize - 1);

        var calculationResults = new List<double>[]
        {
            X, Y, Z, V,
            horizontalMovementAngles, verticalMovementAngles,
            horizontalDeltaAngles, verticalDeltaAngles,
            horizontalAngularVelocities, verticalAngularVelocities,
            angularRotationVelocities
        };

        var calculationSplines = calculationResults.Select(d => LinearSpline.Interpolate(times, d));

        var timeMoments = Algebra.Linspace(0, endTime, resultArraysSize);

        var interpolatedArrays = calculationSplines.Select(s => timeMoments.Select(t => s.Interpolate(t)).ToArray()).ToArray();

        var stabilityCriterion = new List<double>();

        for (var i = 0; i < timeMoments.Length; i++)
        {
            var x = interpolatedArrays[0][i];
            var y = interpolatedArrays[1][i];
            var z = interpolatedArrays[2][i];
            var vk = interpolatedArrays[3][i];

            var ha = interpolatedArrays[4][i];
            var va = interpolatedArrays[5][i];

            var dz = interpolatedArrays[6][i];
            var dy = interpolatedArrays[7][i];

            var wx = interpolatedArrays[10][i];

            var (ez, ey) = GetWindAngles(x, y, z, vk, va, ha);

            var az = dz - ez;
            var ay = dy - ey;

            var angle = Math.Sqrt(FastMath.Pow2(az) + FastMath.Pow2(ay));

            var a = environment.MeteoTable.SoundSpeed(y);
            var M = vk / a;

            var alpha1 = missile.Ix * wx / (2 * missile.Iy);

            var p = environment.MeteoTable.Pressure(y);
            var T = environment.MeteoTable.Temperature(y);

            var beta = FastMath.Pow2(vk) * p / (2 * R * T);

            var beta1 = GetMz(M, angle) * beta * missile.SectionArea * missile.Length / missile.Iy;

            stabilityCriterion.Add(Math.Sqrt(1 - beta1 / FastMath.Pow2(alpha1)));
        }

        return new OutletBallistic(
            timeMoments,
            interpolatedArrays[0],
            interpolatedArrays[1],
            interpolatedArrays[2],
            interpolatedArrays[3],
            interpolatedArrays[4].Select(Algebra.ConvertRadToGrad).ToArray(),
            interpolatedArrays[5].Select(Algebra.ConvertRadToGrad).ToArray(),
            interpolatedArrays[6].Select(Algebra.ConvertRadToGrad).ToArray(),
            interpolatedArrays[7].Select(Algebra.ConvertRadToGrad).ToArray(),
            interpolatedArrays[8],
            interpolatedArrays[9],
            interpolatedArrays[10],
            stabilityCriterion
        );
    }

    private (double ez, double ey) GetWindAngles(
        double x, double y, double z,
        double vk, double verticalMovementAngle, double horizontalMovementAngle)
    {
        var windVelocity = wind.GetWindVelocity(x, y, z);
        var windAngle = wind.GetWindAngle(x, y, z);
        var risingWind = wind.GetRisingWindVelocity(x, y, z);

        var wx = -windVelocity * Math.Cos(windAngle + horizontalMovementAngle) * Math.Cos(verticalMovementAngle) + risingWind * Math.Sin(verticalMovementAngle);
        var wy = windVelocity * Math.Cos(windAngle + horizontalMovementAngle) * Math.Sin(verticalMovementAngle) + risingWind * Math.Cos(verticalMovementAngle);
        var wz = -windVelocity * Math.Sin(windAngle + horizontalMovementAngle);

        var v = Math.Sqrt(vk * vk - 2 * wx * vk + windVelocity * windVelocity + risingWind * risingWind);

        var e2 = Math.Asin(-wy / v);

        return (Math.Asin(wz / (v * Math.Cos(e2))), e2);
    }

    public static IEnumerable<Point> CalculateLineFireHitpoints(
        List<OutletBallistic> ballisticResults,
        double distance,
        double fireAngle,
        double x0 = 0)
    {
        var planeX = Math.Cos(fireAngle) * distance;

        var Ax = planeX - x0;

        foreach (var ballisticShot in ballisticResults)
        {
            var endTime = ballisticShot.TimeMoments.Last();

            var xt = Algebra.GetFunc(
                ballisticShot.TimeMoments.ToArray(),
                ballisticShot.Xs.ToArray(), false);

            var yt = Algebra.GetFunc(
                ballisticShot.TimeMoments.ToArray(),
                ballisticShot.Ys.ToArray(), false);

            var zt = Algebra.GetFunc(
                ballisticShot.TimeMoments.ToArray(),
                ballisticShot.Zs.ToArray(), false);

            var t = MinimumFinder.GetRootNsection(t =>
            {
                return Algebra.GetDistanceToSurface(
                    Ax,
                    0,
                    0,
                    -Ax * planeX,
                    new Point(xt(t), 0, zt(t)), true); // плоскость цели, перпендикулярная земле и орудию
            }, 0, endTime);

            yield return new Point(xt(t), yt(t), zt(t));
        }
    }

    public static double CalculatePlaneSpread(double[] zs, double[] ys, out double centerZ, out double centerY)
    {
        centerZ = 0;
        centerY = 0;

        if (zs.Length != ys.Length || zs.Length < 2)
            return 0;

        centerZ = zs.Average();
        centerY = ys.Average();

        if (zs.Length == 2)
        {
            return Algebra.GetDistance(zs[0], ys[0], zs[1], ys[1]);
        }

        var cz = zs.Average();
        var cy = ys.Average();
        centerZ = cz;
        centerY = cy;

        var circle = SmallestEnclosingCircle.MakeCircle(zs.Select((z, i) => new Point(z, ys[i])).ToList());

        centerZ = circle.Center.X;
        centerY = circle.Center.Y;
        return 2 * circle.Radius;
    }

    private double GetMx(double M, double wx_)
    {
        return (missile.Regressions.MxRegression[0] + missile.Regressions.MxRegression[1] * M + missile.Regressions.MxRegression[2] * FastMath.Pow2(M)) * wx_
            + missile.Regressions.MxRegression[3] * M;
    }

    private double GetMy(double M, double wx_, double angle)
    {
        return (missile.Regressions.MyRegression[0] + missile.Regressions.MyRegression[1] * M + missile.Regressions.MyRegression[2] * FastMath.Pow2(M))
            * (angle + missile.Regressions.MyRegression[3] * FastMath.Pow2(angle)) * wx_;
    }

    private double GetMz(double M, double angle)
    {
        return (missile.Regressions.MzRegression[0] + missile.Regressions.MzRegression[1] * M + missile.Regressions.MzRegression[2] * FastMath.Pow2(M)) * angle;
    }

    private double GetMVertical(double M, double wx_, double horizontalAngle, double verticalAngle)
    {
        return GetMz(M, verticalAngle) - GetMy(M, wx_, horizontalAngle);
    }

    private double GetMHorizontal(double M, double wx_, double horizontalAngle, double verticalAngle)
    {
        return GetMz(M, horizontalAngle) + GetMy(M, wx_, verticalAngle);
    }

    /// <summary>
    /// Функция деривации fz(M)
    /// </summary>
    /// <param name="M">Число Маха</param>
    /// <returns></returns>
    private double GetDerivationFunction(double M)
    {
        var lcm = Math.Abs(missile.MassCenter.X - missile.BodyLength);

        var h = lcm + 0.57 * missile.HeadLength - 0.16 * missile.Diameter;

        return FastMath.Pow3(missile.Length) * derivationResistantSpline.Interpolate(M) / (missile.Diameter * h);
    }

    private double GetCxk(int regressionCRow, double M, double angle)
    {
        return
            missile.Regressions.CxRegression[regressionCRow, 0] +
            missile.Regressions.CxRegression[regressionCRow, 1] * M +
            missile.Regressions.CxRegression[regressionCRow, 2] * FastMath.Pow2(M) +
            missile.Regressions.CxRegression[regressionCRow, 3] * FastMath.Pow3(M) +
            missile.Regressions.CxRegression[regressionCRow, 4] * FastMath.Pow2(angle);
    }

    private double GetCy(int regressionCRow, double M, double angle)
    {
        return
            missile.Regressions.CyRegression[regressionCRow, 1] * angle +
            missile.Regressions.CyRegression[regressionCRow, 2] * FastMath.Pow3(angle) +
            missile.Regressions.CyRegression[regressionCRow, 3] * M * angle +
            missile.Regressions.CyRegression[regressionCRow, 4] * M * FastMath.Pow3(angle);
    }

    private double GetCyk(int regressionCRow, double M, double wx_, double horizontalAngle, double verticalAngle)
    {
        return
            -GetCy(regressionCRow, M, verticalAngle)
            + GetCz(wx_, horizontalAngle);
    }

    private double GetCz(double wx_, double angle)
    {
        return
            (missile.Regressions.CzRegression[1] * angle + missile.Regressions.CzRegression[2] * FastMath.Pow3(angle)) * wx_;
    }

    private double GetCzk(int regressionCRow, double M, double wx_, double horizontalAngle, double verticalAngle)
    {
        return
            -GetCy(regressionCRow, M, horizontalAngle)
            - GetCz(wx_, verticalAngle);
    }

    private int GetRegressionCRow(double M)
    {
        if (M <= MissileRegressions.CMachs.First())
            return 0;

        if (M >= MissileRegressions.CMachs.Last())
            return missile.Regressions.CxRegression.GetLength(0) - 1;

        for (int i = 0; i < MissileRegressions.CMachs.Length - 1; i++)
        {
            if (M >= MissileRegressions.CMachs[i] && M <= MissileRegressions.CMachs[i + 1])
                return i;
        }

        return 0;
    }
}