using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BasicLibraryWinForm;
using System.Reflection;
using Visualization.OpenGL;
using static ScottPlot.Plottable.PopulationPlot;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations
{
    public static class VisualizationConverterTool
    {
        public static IEnumerable<GLSurface> GetBarrelSurfaces(
            Barrel barrel, Color color, int angleSegments = 32, double minAngle = 0, double maxAngle = 6.28)
        {
            return GetBarrelSurfaces(
                barrel,
                angleSegments: angleSegments,
                minAngle: minAngle,
                maxAngle: maxAngle,
                basicColor: color);
        }

        public static IEnumerable<GLSurface> GetBarrelSurfaces(
            Barrel barrel,
            double[]? values = null,
            double[]? deltaX = null,
            double[]? deltaY = null,
            double[]? deltaZ = null,
            double[]? deltaR = null,
            int angleSegments = 32, double minAngle = 0, double maxAngle = 6.28, Color? basicColor = null)
        {
            if (!barrel.Initialized)
                yield break;

            if (values == null || values.Length != barrel.X.Length)
                values = new double[barrel.X.Length];

            if (deltaX == null || deltaX.Length != barrel.X.Length)
                deltaX = new double[barrel.X.Length];

            if (deltaY == null || deltaY.Length != barrel.X.Length)
                deltaY = new double[barrel.X.Length];

            if (deltaZ == null || deltaZ.Length != barrel.X.Length)
                deltaZ = new double[barrel.X.Length];

            if (deltaR == null || deltaR.Length != barrel.X.Length)
                deltaR = new double[barrel.X.Length];

            var dxFunc = Algebra.GetFunc(barrel.X, deltaX);
            var dyFunc = Algebra.GetFunc(barrel.X, deltaY);
            var dzFunc = Algebra.GetFunc(barrel.X, deltaZ);
            var drFunc = Algebra.GetFunc(barrel.X, deltaR);

            var min = values.Min();
            var max = values.Max();

            var da = (maxAngle - minAngle) / angleSegments;

            for (var i = 0; i < angleSegments; i++)
            {
                var a0 = minAngle + i * da;
                var a1 = a0 + da;
                var r0 = barrel.InnerD.First() / 2;
                var r1 = barrel.InnerD.Last() / 2;

                var colorLast = basicColor ?? Algebra.GetHeatColor(values[^1], min, max);
                var colorFirst = basicColor ?? Algebra.GetHeatColor(values[0], min, max);

                //дульный срез
                yield return new GLSurface(
                    colorLast,
                    GetVisualizationXYZ(barrel.X.Last(), r1, a0, dxFunc, dyFunc, dzFunc, drFunc),
                    GetVisualizationXYZ(barrel.X.Last(), r1, a1, dxFunc, dyFunc, dzFunc, drFunc),
                    barrel.GetBoardCoordinate(barrel.X.Length - 1, a1) + GetVisualizationDXYZ(barrel.X.Last(), a1, dxFunc, dyFunc, dzFunc, drFunc),
                    barrel.GetBoardCoordinate(barrel.X.Length - 1, a0) + GetVisualizationDXYZ(barrel.X.Last(), a0, dxFunc, dyFunc, dzFunc, drFunc));

                //казеный срез
                yield return new GLSurface(
                    colorFirst,
                    GetVisualizationXYZ(barrel.X.First(), r0, a0, dxFunc, dyFunc, dzFunc, drFunc),
                    GetVisualizationXYZ(barrel.X.First(), r0, a1, dxFunc, dyFunc, dzFunc, drFunc),
                    barrel.GetBoardCoordinate(0, a1) + GetVisualizationDXYZ(barrel.X.First(), a1, dxFunc, dyFunc, dzFunc, drFunc),
                    barrel.GetBoardCoordinate(0, a0) + GetVisualizationDXYZ(barrel.X.First(), a0, dxFunc, dyFunc, dzFunc, drFunc));
            }

            //центральная часть
            for (int i = 0; i < barrel.X.Length - 1; i++)
            {
                var x0 = barrel.X[i];
                var x1 = barrel.X[i + 1];
                var r0 = barrel.InnerD[i] / 2;
                var r1 = barrel.InnerD[i + 1] / 2;

                var colorCurrent = basicColor ?? Algebra.GetHeatColor(values[i], min, max);
                var colorNext = basicColor ?? Algebra.GetHeatColor(values[i], min, max);

                for (var j = 0; j < angleSegments; j++)
                {
                    var a0 = minAngle + j * da;
                    var a1 = a0 + da;

                    //внутренняя часть
                    yield return new GLSurface(
                        new ColoredPoint(GetVisualizationXYZ(x0, r0, a0, dxFunc, dyFunc, dzFunc, drFunc), colorCurrent),
                        new ColoredPoint(GetVisualizationXYZ(x1, r1, a0, dxFunc, dyFunc, dzFunc, drFunc), colorNext),
                        new ColoredPoint(GetVisualizationXYZ(x1, r1, a1, dxFunc, dyFunc, dzFunc, drFunc), colorNext),
                        new ColoredPoint(GetVisualizationXYZ(x0, r0, a1, dxFunc, dyFunc, dzFunc, drFunc), colorCurrent));

                    //внешняя часть
                    yield return new GLSurface(
                        new ColoredPoint(barrel.GetBoardCoordinate(i, a0), colorCurrent) + GetVisualizationDXYZ(x0, a0, dxFunc, dyFunc, dzFunc, drFunc),
                        new ColoredPoint(barrel.GetBoardCoordinate(i + 1, a0), colorNext) + GetVisualizationDXYZ(x1, a0, dxFunc, dyFunc, dzFunc, drFunc),
                        new ColoredPoint(barrel.GetBoardCoordinate(i + 1, a1), colorNext) + GetVisualizationDXYZ(x1, a1, dxFunc, dyFunc, dzFunc, drFunc),
                        new ColoredPoint(barrel.GetBoardCoordinate(i, a1), colorCurrent) + GetVisualizationDXYZ(x0, a1, dxFunc, dyFunc, dzFunc, drFunc));
                }

                if (Math.Abs(maxAngle - minAngle) >= 2 * Math.PI * 0.98)
                    continue;

                yield return new GLSurface(
                    new ColoredPoint(GetVisualizationXYZ(x0, r0, minAngle, dxFunc, dyFunc, dzFunc, drFunc), colorCurrent),
                    new ColoredPoint(GetVisualizationXYZ(x1, r1, minAngle, dxFunc, dyFunc, dzFunc, drFunc), colorNext),
                    new ColoredPoint(barrel.GetBoardCoordinate(i + 1, minAngle), colorNext) + GetVisualizationDXYZ(x1, minAngle, dxFunc, dyFunc, dzFunc, drFunc),
                    new ColoredPoint(barrel.GetBoardCoordinate(i, minAngle), colorCurrent) + GetVisualizationDXYZ(x0, minAngle, dxFunc, dyFunc, dzFunc, drFunc));

                yield return new GLSurface(
                    new ColoredPoint(GetVisualizationXYZ(x0, r0, maxAngle, dxFunc, dyFunc, dzFunc, drFunc), colorCurrent),
                    new ColoredPoint(GetVisualizationXYZ(x1, r1, maxAngle, dxFunc, dyFunc, dzFunc, drFunc), colorNext),
                    new ColoredPoint(barrel.GetBoardCoordinate(i + 1, maxAngle), colorNext) + GetVisualizationDXYZ(x1, maxAngle, dxFunc, dyFunc, dzFunc, drFunc),
                    new ColoredPoint(barrel.GetBoardCoordinate(i, maxAngle), colorCurrent) + GetVisualizationDXYZ(x0, maxAngle, dxFunc, dyFunc, dzFunc, drFunc));
            }
        }

        public static IEnumerable<GLSurface> GetBarrelSectionSurfaces(Barrel barrel, int sectionIndex, Color basicColor)
        {
            foreach (var element in barrel.Meshes[sectionIndex].Elements)
            {
                var points = element.Select(e => barrel.Meshes[sectionIndex].Nodes[e])
                    .Select(p => new ColoredPoint(p, basicColor)).ToArray();

                yield return new GLSurface(points, new Point(1, 0, 0));
            }
        }

        public static IEnumerable<List<GLSurface>> GetBarrelSectionsSurfaces(Barrel barrel, Color basicColor)
        {
            foreach (var mesh in barrel.Meshes)
            {
                var sectionSurfaces = new List<GLSurface>();

                foreach (var element in mesh.Elements)
                {
                    var points = element.Select(e => mesh.Nodes[e])
                        .Select(p => new ColoredPoint(p, basicColor)).ToArray();

                    sectionSurfaces.Add(new GLSurface(points, new Point(1, 0, 0)));
                }

                yield return sectionSurfaces;
            }
        }

        public static IEnumerable<GLSurface> GetMissileSurfaces(
            Barrel barrel,
            Missile missile,
            double missileX,
            double[]? deltaX = null,
            double[]? deltaY = null,
            double[]? deltaZ = null,
            Color? color = null,
            int angleSegments = 32)
        {
            if (color?.A == 0)
                yield break;

            if (color == null)
                color = Color.Aquamarine;

            var da = 2 * Math.PI / angleSegments;
            var r = missile.Diameter / 2;

            if (deltaX == null || deltaX.Length != barrel.X.Length)
                deltaX = new double[barrel.X.Length];

            if (deltaY == null || deltaY.Length != barrel.X.Length)
                deltaY = new double[barrel.X.Length];

            if (deltaZ == null || deltaZ.Length != barrel.X.Length)
                deltaZ = new double[barrel.X.Length];

            var dxFunc = Algebra.GetFunc(barrel.X, deltaX);
            var dyFunc = Algebra.GetFunc(barrel.X, deltaY);
            var dzFunc = Algebra.GetFunc(barrel.X, deltaZ);

            var backPoints = new List<ColoredPoint>();

            for (var i = 0; i < angleSegments; i++)
            {
                var a = i * da;
                var p = new ColoredPoint(GetVisualizationXYZ(missileX, r, a, dxFunc, dyFunc, dzFunc, null), color.Value);
                backPoints.Add(p);
            }

            yield return new GLSurface(backPoints);

            for (var i = 0; i < angleSegments; i++)
            {
                var a0 = i * da;
                var a1 = (i + 1) * da;

                var x0 = missileX;
                var x1 = x0 + missile.BodyLength;
                var x2 = x1 + missile.HeadLength;

                var p0 = new ColoredPoint(GetVisualizationXYZ(x0, r, a0, dxFunc, dyFunc, dzFunc, null), color.Value);
                var p1 = new ColoredPoint(GetVisualizationXYZ(x1, r, a0, dxFunc, dyFunc, dzFunc, null), color.Value);
                var p2 = new ColoredPoint(GetVisualizationXYZ(x1, r, a1, dxFunc, dyFunc, dzFunc, null), color.Value);
                var p3 = new ColoredPoint(GetVisualizationXYZ(x0, r, a1, dxFunc, dyFunc, dzFunc, null), color.Value);

                var p4 = new ColoredPoint(GetVisualizationXYZ(x2, 0, a1, dxFunc, dyFunc, dzFunc, null), color.Value);

                yield return new GLSurface(new List<ColoredPoint> { p0, p1, p2, p3 });

                yield return new GLSurface(new List<ColoredPoint> { p1, p2, p4 });
            }
        }

        public static IEnumerable<GLSurface> GetPowderSurfaces(
            double missileX,
            Barrel barrel,
            double burntPowder,
            double sleeveThickness = 0,
            double[]? deltaX = null,
            double[]? deltaY = null,
            double[]? deltaZ = null,
            double[]? deltaR = null,
            Color? color = null,
            int angleSegments = 32)
        {
            if (burntPowder >= 1 || color?.A == 0)
                yield break;

            if (color == null)
                color = Color.SaddleBrown;

            if (deltaX == null || deltaX.Length != barrel.X.Length)
                deltaX = new double[barrel.X.Length];

            if (deltaY == null || deltaY.Length != barrel.X.Length)
                deltaY = new double[barrel.X.Length];

            if (deltaZ == null || deltaZ.Length != barrel.X.Length)
                deltaZ = new double[barrel.X.Length];

            if (deltaR == null || deltaR.Length != barrel.X.Length)
                deltaR = new double[barrel.X.Length];

            var dxFunc = Algebra.GetFunc(barrel.X, deltaX);
            var dyFunc = Algebra.GetFunc(barrel.X, deltaY);
            var dzFunc = Algebra.GetFunc(barrel.X, deltaZ);
            var drFunc = Algebra.GetFunc(barrel.X, deltaR);

            var da = 2 * Math.PI / angleSegments;

            var backPoints = new List<ColoredPoint>();

            color = Color.FromArgb((int)(color.Value.A * (1 - burntPowder)), color.Value.R, color.Value.G, color.Value.B);

            for (var i = 0; i < angleSegments; i++)
            {
                var a = i * da;
                var p = new ColoredPoint(GetVisualizationXYZ(barrel.X[0] + sleeveThickness, 0.5 * barrel.InnerD[0] - sleeveThickness, a, dxFunc, dyFunc, dzFunc, drFunc), color.Value);

                backPoints.Add(p);
            }

            yield return new GLSurface(backPoints) { WithEdges = false };

            var xLast = 0.0;
            var iLast = 0;


            for (var i = 0; i < barrel.X.Length - 1; i++)
            {
                if (barrel.X[i] > missileX)
                    break;

                if (barrel.X[i] <= sleeveThickness && barrel.X[i + 1] <= sleeveThickness)
                    continue;

                var x0 = barrel.X[i] <= sleeveThickness ? sleeveThickness : barrel.X[i];
                var x1 = barrel.X[i + 1] > barrel.CamoraLength ? barrel.CamoraLength : barrel.X[i + 1];

                for (var j = 0; j < angleSegments; j++)
                {
                    var a0 = j * da;
                    var a1 = (j + 1) * da;
                    var r0 = x0 <= barrel.CamoraLength ? 0.5 * barrel.InnerD[i] - sleeveThickness : 0.5 * barrel.InnerD[i];
                    var r1 = x1 <= barrel.CamoraLength ? 0.5 * barrel.InnerD[i + 1] - sleeveThickness : 0.5 * barrel.InnerD[i + 1];

                    var p0 = new ColoredPoint(
                        GetVisualizationXYZ(x0, r0, a0, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p1 = new ColoredPoint(
                        GetVisualizationXYZ(x1, r1, a0, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p2 = new ColoredPoint(
                        GetVisualizationXYZ(x1, r1, a1, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p3 = new ColoredPoint(
                        GetVisualizationXYZ(x0, r0, a1, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    yield return new GLSurface(new List<ColoredPoint> { p0, p1, p2, p3 }) { WithEdges = false };
                }

                xLast = x1;
                iLast = i + 1;
            }

            var frontPoints = new List<ColoredPoint>();

            color = Color.FromArgb((int)(255 * (1 - (missileX - barrel.CamoraLength) / barrel.X.Last())), color.Value.R, color.Value.G, color.Value.B);

            var rLast = xLast <= barrel.CamoraLength ? 0.5 * barrel.InnerD[iLast] - sleeveThickness : 0.5 * barrel.InnerD[iLast];

            for (var i = 0; i < angleSegments; i++)
            {
                var a = i * da;
                var p = new ColoredPoint(
                    GetVisualizationXYZ(xLast, rLast, a, dxFunc, dyFunc, dzFunc, drFunc),
                    color.Value);

                frontPoints.Add(p);
            }

            yield return new GLSurface(frontPoints) { WithEdges = false };
        }

        public static IEnumerable<GLSurface> GetSleeveSurfaces(
           Barrel barrel,
           double sleeveThickness = 0,
           double[]? deltaX = null,
           double[]? deltaY = null,
           double[]? deltaZ = null,
           double[]? deltaR = null,
           Color? color = null,
           int angleSegments = 32)
        {
            if (sleeveThickness <= 0 || color?.A == 0)
                yield break;

            if (color == null)
                color = Color.SandyBrown;

            if (deltaX == null || deltaX.Length != barrel.X.Length)
                deltaX = new double[barrel.X.Length];

            if (deltaY == null || deltaY.Length != barrel.X.Length)
                deltaY = new double[barrel.X.Length];

            if (deltaZ == null || deltaZ.Length != barrel.X.Length)
                deltaZ = new double[barrel.X.Length];

            if (deltaR == null || deltaR.Length != barrel.X.Length)
                deltaR = new double[barrel.X.Length];

            var dxFunc = Algebra.GetFunc(barrel.X, deltaX);
            var dyFunc = Algebra.GetFunc(barrel.X, deltaY);
            var dzFunc = Algebra.GetFunc(barrel.X, deltaZ);
            var drFunc = Algebra.GetFunc(barrel.X, deltaR);

            var da = 2 * Math.PI / angleSegments;

            var backPoints = new List<ColoredPoint>();

            for (var i = 0; i < angleSegments; i++)
            {
                var a = i * da;
                var p = new ColoredPoint(GetVisualizationXYZ(barrel.X[0], 0.5 * barrel.InnerD[0], a, dxFunc, dyFunc, dzFunc, drFunc), color.Value);

                backPoints.Add(p);
            }

            yield return new GLSurface(backPoints);

            var backPoints2 = new List<ColoredPoint>();

            for (var i = 0; i < angleSegments; i++)
            {
                var a = i * da;
                var p = new ColoredPoint(GetVisualizationXYZ(barrel.X[0] + sleeveThickness, 0.5 * barrel.InnerD[0] - sleeveThickness, a, dxFunc, dyFunc, dzFunc, drFunc), color.Value);

                backPoints2.Add(p);
            }

            yield return new GLSurface(backPoints2);

            for (var i = 0; i < barrel.X.Length - 1; i++)
            {
                if (barrel.X[i] > barrel.CamoraLength)
                    break;

                var x1 = barrel.X[i + 1] > barrel.CamoraLength ? barrel.CamoraLength : barrel.X[i + 1];

                for (var j = 0; j < angleSegments; j++)
                {
                    var a0 = j * da;
                    var a1 = (j + 1) * da;

                    var p0 = new ColoredPoint(
                        GetVisualizationXYZ(barrel.X[i], 0.5 * barrel.InnerD[i], a0, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p1 = new ColoredPoint(
                        GetVisualizationXYZ(x1, 0.5 * barrel.InnerD[i + 1], a0, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p2 = new ColoredPoint(
                        GetVisualizationXYZ(x1, 0.5 * barrel.InnerD[i + 1], a1, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p3 = new ColoredPoint(
                        GetVisualizationXYZ(barrel.X[i], 0.5 * barrel.InnerD[i], a1, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    yield return new GLSurface(new List<ColoredPoint> { p0, p1, p2, p3 });
                }
            }

            var xLast = 0.0;
            var iLast = 0;

            for (var i = 0; i < barrel.X.Length - 1; i++)
            {
                if (barrel.X[i] > barrel.CamoraLength)
                    break;

                if (barrel.X[i + 1] < sleeveThickness)
                    continue;

                var x0 = barrel.X[i] < sleeveThickness ? sleeveThickness : barrel.X[i];
                var x1 = barrel.X[i + 1] > barrel.CamoraLength ? barrel.CamoraLength : barrel.X[i + 1];

                for (var j = 0; j < angleSegments; j++)
                {
                    var a0 = j * da;
                    var a1 = (j + 1) * da;

                    var p0 = new ColoredPoint(
                        GetVisualizationXYZ(x0, 0.5 * barrel.InnerD[i] - sleeveThickness, a0, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p1 = new ColoredPoint(
                        GetVisualizationXYZ(x1, 0.5 * barrel.InnerD[i + 1] - sleeveThickness, a0, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p2 = new ColoredPoint(
                        GetVisualizationXYZ(x1, 0.5 * barrel.InnerD[i + 1] - sleeveThickness, a1, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    var p3 = new ColoredPoint(
                        GetVisualizationXYZ(x0, 0.5 * barrel.InnerD[i] - sleeveThickness, a1, dxFunc, dyFunc, dzFunc, drFunc),
                        color.Value);

                    yield return new GLSurface(new List<ColoredPoint> { p0, p1, p2, p3 });
                }

                xLast = x1;
                iLast = i + 1;
            }

            for (var j = 0; j < angleSegments; j++)
            {
                var a0 = j * da;
                var a1 = (j + 1) * da;

                var p0 = new ColoredPoint(
                    GetVisualizationXYZ(xLast, 0.5 * barrel.InnerD[iLast], a0, dxFunc, dyFunc, dzFunc, drFunc),
                    color.Value);

                var p1 = new ColoredPoint(
                    GetVisualizationXYZ(xLast, 0.5 * barrel.InnerD[iLast] - sleeveThickness, a0, dxFunc, dyFunc, dzFunc, drFunc),
                    color.Value);

                var p2 = new ColoredPoint(
                    GetVisualizationXYZ(xLast, 0.5 * barrel.InnerD[iLast] - sleeveThickness, a1, dxFunc, dyFunc, dzFunc, drFunc),
                    color.Value);

                var p3 = new ColoredPoint(
                    GetVisualizationXYZ(xLast, 0.5 * barrel.InnerD[iLast], a1, dxFunc, dyFunc, dzFunc, drFunc),
                    color.Value);

                yield return new GLSurface(new List<ColoredPoint> { p0, p1, p2, p3 });
            }
        }

        private static Point GetVisualizationXYZ(
            double x, double r, double a, Func<double, double>? dxFunc, Func<double, double>? dyFunc, Func<double, double>? dzFunc, Func<double, double>? drFunc)
        {
            var dx = dxFunc?.Invoke(x) ?? 0;
            var dy = dyFunc?.Invoke(x) ?? 0;
            var dz = dzFunc?.Invoke(x) ?? 0;
            var dr = drFunc?.Invoke(x) ?? 0;

            return new(x + dx, (r + dr) * Math.Sin(a) + dy, (r + dr) * Math.Cos(a) + dz);
        }

        private static Point GetVisualizationDXYZ(
    double x, double a, Func<double, double>? dxFunc, Func<double, double>? dyFunc, Func<double, double>? dzFunc, Func<double, double>? drFunc)
        {
            var dx = dxFunc?.Invoke(x) ?? 0;
            var dy = dyFunc?.Invoke(x) ?? 0;
            var dz = dzFunc?.Invoke(x) ?? 0;
            var dr = drFunc?.Invoke(x) ?? 0;

            return new(dx, dr * Math.Sin(a) + dy, dr * Math.Cos(a) + dz);
        }
    }
}
