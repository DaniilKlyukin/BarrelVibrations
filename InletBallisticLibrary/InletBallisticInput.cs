using BasicLibraryWinForm;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;

namespace InletBallisticLibrary
{
    public class InletBallisticInput
    {
        private static GaussLegendreIntegral glIntegral = new GaussLegendreIntegral(5);

        public int GroovesCount { get; set; }
        public double GroovesSlope { get; set; }
        public double GroovesDepth { get; }
        public double GroovesWidth { get; }

        public InletBallisticInput(
            PowderCharge charge,
            double boostPressure,
            double q,
            double missileDiameter,
            double camoraLength,
            double addCamoraVolume,
            double[] barrelX,
            double[] barrelDiameters,
            double[] barrelS,
            int groovesCount,
            double groovesSlope,
            double groovesDepth,
            double groovesWidth,
            bool backPressure,
            double frictionCoefficient,
            bool gravity,
            double shotAngle)
        {
            GroovesCount = groovesCount;
            GroovesSlope = groovesSlope;
            GroovesDepth = groovesDepth;
            GroovesWidth = groovesWidth;
            Charge = charge;
            BoostPressure = boostPressure;
            this.q = q;
            MissileDiameter = missileDiameter;
            CamoraLength = camoraLength;
            AddCamoraVolume = addCamoraVolume;
            this.barrelX = barrelX;
            this.barrelDiameters = barrelDiameters;
            BackPressure = backPressure;
            smoothFrictionCoefficient = frictionCoefficient;
            Gravity = gravity;
            ShotAngle = shotAngle;

            var rho = Powders.Sum(p => p.PowderDensity);

            var cp = Powders.Sum(p => p.Cp * p.PowderDensity) / rho;
            var cv = Powders.Sum(p => p.Cv * p.PowderDensity) / rho;

            Teta = cp / cv - 1;
            R = Powders.Sum(p => p.R * p.PowderDensity) / rho;

            FrictionCoefficient = Physics.GetInteractionCoefficient(groovesCount, smoothFrictionCoefficient, groovesSlope);
        }

        public double GetBarrelVolume(double x)
        {
            var volume = 0.0;

            for (int i = 0; i < barrelX.Length - 1; i++)
            {
                if (x >= barrelX[i] && x <= barrelX[i + 1])
                {
                    var d = Algebra.GetValueAtLine(x, barrelX[i], barrelDiameters[i], barrelX[i + 1], barrelDiameters[i + 1]);
                    volume += GetVolume(barrelDiameters[i] / 2, d / 2, x - barrelX[i]);
                    break;
                }
                volume += GetVolume(barrelDiameters[i] / 2, barrelDiameters[i + 1] / 2, barrelX[i + 1] - barrelX[i]);
            }

            var sleeveCoef = x >= CamoraLength ? 1 : x / CamoraLength;

            return volume + AddCamoraVolume - Charge.SleeveVolume * sleeveCoef;
        }

        public double GetCamoraVolume()
        {
            return GetBarrelVolume(CamoraLength) + AddCamoraVolume - Charge.SleeveVolume;
        }

        public double GetS(double x)
        {
            if (x >= barrelX.Last())
                return Algebra.GetCircleArea(barrelDiameters.Last() / 2);
            if (x <= barrelX.First())
                return Algebra.GetCircleArea(barrelDiameters.First() / 2);

            for (int i = 0; i < barrelX.Length - 1; i++)
            {
                if (x >= barrelX[i] && x <= barrelX[i + 1])
                {
                    var d = Algebra.GetValueAtLine(x, barrelX[i], barrelDiameters[i], barrelX[i + 1], barrelDiameters[i + 1]);
                    return Algebra.GetCircleArea(d / 2);
                }
            }

            throw new ArgumentException();
        }

        public double GetVolume(double r1, double r2, double h)
        {
            return Math.PI * h * (r1 * r1 + r1 * r2 + r2 * r2) / 3.0;
        }

        public double GetVolume2(double s1, double s2, double h)
        {
            return h * (s1 + Math.Sqrt(s1 * s2) + s2) / 3.0;
        }

        public double FrictionCoefficient { get; }
        public double R { get; set; }

        public double Teta { get; set; }

        public double[] barrelX { get; }
        public double[] barrelDiameters { get; }
        public bool BackPressure { get; }
        private double smoothFrictionCoefficient { get; }
        public bool Gravity { get; }
        public double ShotAngle { get; }

        public double BarrelLength => barrelX.Last();

        public IList<Powder> Powders => Charge.Powders;

        public double wIgniter => Charge.IgniterMass;
        public PowderCharge Charge { get; }
        public double BoostPressure { get; }
        /// <summary>
        ///     Масса снаряда до вылета из ствола, кг
        /// </summary>
        public double q { get; }

        /// <summary>
        ///     Диаметр снаряда, м
        /// </summary>
        public double MissileDiameter { get; }

        public double Ssn => GetS(barrelX.Last());

        /// <summary>
        ///     Длина каморы, м
        /// </summary>
        public double CamoraLength { get; }

        /// <summary>
        ///     Дополнительный объём каморы, м³
        /// </summary>
        public double AddCamoraVolume { get; }
        
        /*
        public (double, double, double) GetJ(double x, int integrationPointsCount, double xSn)
        {
            var xs = new double[integrationPointsCount + 1];
            var integral0 = new double[integrationPointsCount + 1];
            var integral1 = new double[integrationPointsCount + 1];
            var integral2 = new double[integrationPointsCount + 1];

            var wSn = GetBarrelVolume(xSn);
            var sSn = GetS(xSn);

            var dx = x / integrationPointsCount;

            for (var i = 0; i <= integrationPointsCount; i++)
            {
                xs[i] = i * dx;

                var w = GetBarrelVolume(xs[i]);
                var s = GetS(xs[i]);

                integral0[i] = FastMath.Pow2(w) / s;
                integral1[i] = w / s;
            }

            var int0Spline = CubicSpline.InterpolatePchip(xs, integral0);
            var int1Spline = CubicSpline.InterpolatePchip(xs, integral1);

            for (var i = 1; i <= integrationPointsCount; i++)
            {
                integral2[i] = integral2[i - 1] + int1Spline.Integrate(xs[i - 1], xs[i]) * GetS(xs[i]);
            }

            var int2Spline = CubicSpline.InterpolatePchip(xs, integral2);

            return (FastMath.Pow2(sSn) / FastMath.Pow3(wSn) * int0Spline.Integrate(0, x),
                    FastMath.Pow2(sSn) / FastMath.Pow2(wSn) * int1Spline.Integrate(0, x),
                    FastMath.Pow2(sSn) / FastMath.Pow3(wSn) * int2Spline.Integrate(0, x));
        }*/

        private double IntegrateByGeometry(double a, double b, double elementSize, Func<double, double> f)
        {
            var corners = new List<double>
            {
                a
            };

            for (var i = 0; i < barrelX.Length; i++)
            {
                if (barrelX[i] > a && barrelX[i] < b)
                    corners.Add(barrelX[i]);
            }

            if (b < barrelX.Last())
                corners.Add(b);

            var integral = 0.0;

            for (int i = 0; i < corners.Count - 1; i++)
            {
                var x0 = corners[i];
                var x1 = corners[i + 1];

                if (Math.Abs(x1 - x0) < 1e-15)
                    continue;

                var subSegmentsCount = (int)Math.Round((x1 - x0) / elementSize);
                subSegmentsCount = subSegmentsCount <= 0 ? 1 : subSegmentsCount;
                var dx = (x1 - x0) / subSegmentsCount;

                for (int j = 0; j < subSegmentsCount; j++)
                {
                    var x00 = x0 + j * dx;
                    var x11 = x00 + dx;

                    integral += glIntegral.Integrate(x00, x11, f);
                }
            }

            return integral;
        }

        public (double, double) GetJ0(double xSn, double elementSize, double xSnLast = 0, double integralLast = 0)
        {
            var wSn = GetBarrelVolume(xSn);
            var sSn = GetS(xSn);

            var integral = integralLast + IntegrateByGeometry(
                xSnLast,
                xSn,
                elementSize,
                x => FastMath.Pow2(GetBarrelVolume(x)) / GetS(x));

            return (FastMath.Pow2(sSn) / FastMath.Pow3(wSn) * integral, integral);
        }

        public (double, double) GetJ1(double x, double elementSize, double xSn, double xLast = 0, double integralLast = 0)
        {
            var wSn = GetBarrelVolume(xSn);
            var sSn = GetS(xSn);

            var integral = integralLast + IntegrateByGeometry(
                xLast,
                x,
                elementSize,
                xi => GetBarrelVolume(xi) / GetS(xi));

            return (FastMath.Pow2(sSn) / FastMath.Pow2(wSn) * integral, integral);
        }

        public (double, double, double) GetJ2(double xSn, double elementSize, double xSnLast = 0, double integralLast = 0, double subIntegralLast = 0)
        {
            var wSn = GetBarrelVolume(xSn);
            var sSn = GetS(xSn);

            var subXLast = xSnLast;

            var integral = integralLast + IntegrateByGeometry(
                xSnLast,
                xSn,
                elementSize,
                x =>
                {
                    var subIntegral = subIntegralLast + IntegrateByGeometry(
                        subXLast,
                        x,
                        elementSize,
                        xi => GetBarrelVolume(xi) / GetS(xi));

                    return subIntegral * GetS(x);
                });

            var subIntegral = subIntegralLast + IntegrateByGeometry(
                xSnLast,
                xSn,
                elementSize,
                xi => GetBarrelVolume(xi) / GetS(xi));

            return (FastMath.Pow2(sSn) / FastMath.Pow3(wSn) * integral, integral, subIntegral);
        }
    }
}