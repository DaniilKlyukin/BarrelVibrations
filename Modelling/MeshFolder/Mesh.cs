using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BasicLibraryWinForm;
using MathNet.Numerics.LinearAlgebra.Double;
using Modelling.Geometry;
using GmshNet;
using BasicLibraryWinForm.PointFolder;

namespace Modelling.MeshFolder
{
    using occ = Gmsh.Model.Occ;
    [Serializable]
    [DataContract(Name = "Расчетная сетка")]
    public class Mesh
    {
        const int DIM0 = 0, DIM1 = 1, DIM2 = 2;

        [DataMember(Name = "Индексы ребер на границе")]
        public List<Tuple<int, int>> BorderEdges { get; private set; } = new();


        [DataMember(Name = "Сгруппированные индексы ребер на границе")]
        public List<List<Tuple<int, int>>> BorderEdgesGroups { get; private set; } = new();


        [DataMember(Name = "Координаты узлов сетки, м")]
        public Point[] Nodes { get; private set; } = Array.Empty<Point>();


        [DataMember(Name = "Индексы узлов элементов")]
        public int[][] Elements { get; private set; } = Array.Empty<int[]>();

        public static Mesh GenerateGMSH(
            double x,
            List<IGeometryObject> geometries,
            double innerDiameter,
            double step,
            int groovesCount,
            double groovesInitialAngle,
            double groovesDepth,
            double groovesWidth)
        {
            Gmsh.Initialize();
            Gmsh.Option.SetNumber("General.Terminal", 1);
            Gmsh.Model.Add("t1");

            var toAdd = new List<(int, int)>();
            var toSubstract = new List<(int, int)>();
            var angularWidth = 2 * Math.Asin(groovesWidth / innerDiameter);

            var exceptSizeTags = new List<(int, int)>();

            if (groovesCount > 0)
            {
                var da = 2 * Math.PI / groovesCount;

                for (int i = 0; i < groovesCount; i++)
                {
                    toSubstract.Add((DIM2, AddGroove(innerDiameter / 2, angularWidth, groovesDepth, groovesInitialAngle + i * da, out var tags)));

                    exceptSizeTags.AddRange(tags.Where(t => t.Item1 == DIM0).ToArray());
                }
            }

            foreach (var geom in geometries)
            {
                if (geom is CircleObject co)
                {
                    var id = occ.AddDisk(co.Center.X, co.Center.Y, 0, co.Radius, co.Radius);

                    if (co.Operation == Operation.Add)
                        toAdd.Add((DIM2, id));
                    if (co.Operation == Operation.Subtract)
                        toSubstract.Add((DIM2, id));
                }
            }

            var fusedTags = new[] { toAdd.First() };

            if (toAdd.Count > 1)
            {
                occ.Fuse(fusedTags, toAdd.Skip(1).ToArray(), out fusedTags, out _);
            }
            if (toSubstract.Count > 0)
            {
                occ.Cut(fusedTags, toSubstract.ToArray(), out var substractedTags, out _);
            }

            /*
            geo.AddPoint(0, 0, 0, lc, 1);
            geo.AddPoint(0.1, 0, 0, lc, 2);
            geo.AddPoint(0.1, 0.3, 0, lc, 3);
            geo.AddPoint(0, 0.3, 0, lc, 4);

            geo.AddLine(1, 2, 1);
            geo.AddLine(3, 2, 2);
            geo.AddLine(3, 4, 3);
            geo.AddLine(4, 1, 4);

            geo.AddCurveLoop(new int[] { 4, 1, -2, 3 }, 1);
            geo.AddPlaneSurface(new int[] { 1 }, 1);
            Gmsh.Model.AddPhysicalGroup(1, new int[] { 1, 2, 4 }, 5);*/

            //var ps = Gmsh.Model.AddPhysicalGroup(2, new int[] { 1 });

            //Gmsh.Model.SetPhysicalName(2, ps, "My surface");

            //geo.Remove(new[] { (0, 1), (0, 2) });
            occ.Synchronize();

            var entities = Gmsh.Model.GetEntities(0);
            var setSizeTags = entities.Except(exceptSizeTags).ToArray();
            Gmsh.Model.Mesh.SetSize(setSizeTags, step);

            Gmsh.Model.Mesh.Generate(2);

            Gmsh.Model.Mesh.GetNodes(out var nodesTags, out var coordsCoord, out var parametricCoords);
            Gmsh.Model.Mesh.GetElements(out var types, out var elementsTags, out var elementNodesTags);

            Gmsh.Finalize();

            var points = new List<Point>();

            var pointsCount = coordsCoord.Length / 3;

            for (int i = 0; i < pointsCount; i++)
            {
                var p = new Point(x, coordsCoord[3 * i + 1], coordsCoord[3 * i]);
                p.SetId((int)nodesTags[i] - 1);
                points.Add(p);
            }

            var elementsCount = elementNodesTags[1].Length / 3;
            var elems = new int[elementsCount][];

            for (int i = 0; i < elementsCount; i++)
            {
                elems[i] = new[]
                {
                    (int)elementNodesTags[1][3 * i]-1,
                    (int)elementNodesTags[1][3 * i + 1]-1,
                    (int)elementNodesTags[1][3 * i + 2] -1};
            }

            var mesh = new Mesh()
            {
                Nodes = points.ToArray(),
                Elements = elems
            };

            PostProcess(mesh);

            return mesh;
        }

        private static int AddGroove(double innerR, double angularWidth, double depth, double rotation, out (int, int)[] tags)
        {
            var meshSize = depth;

            const int angularSegmentsCount = 8;
            var groovePoints = new List<int>();
            var angleFrom = rotation - angularWidth / 2;
            var angleTo = rotation + angularWidth / 2;
            var da = (angleTo - angleFrom) / angularSegmentsCount;

            for (int i = 0; i <= angularSegmentsCount; i++) // Внутренние точки нареза
            {
                var angle = angleFrom + da * i;

                groovePoints.Add(
                    occ.AddPoint(
                        innerR * Math.Cos(angle),
                        innerR * Math.Sin(angle),
                        0, meshSize));
            }

            for (int i = angularSegmentsCount; i >= 0; i--) // Внешние точки нареза
            {
                var angle = angleFrom + da * i;

                groovePoints.Add(
                    occ.AddPoint(
                        (innerR + depth) * Math.Cos(angle),
                        (innerR + depth) * Math.Sin(angle),
                        0, meshSize));
            }

            //groovePoints[0] = occ.AddPoint(innerR * Math.Cos(rotation - angularWidth / 2), innerR * Math.Sin(rotation - angularWidth / 2), 0, meshSize);
            //groovePoints[1] = occ.AddPoint(innerR * Math.Cos(rotation + angularWidth / 2), innerR * Math.Sin(rotation + angularWidth / 2), 0, meshSize);
            //groovePoints[2] = occ.AddPoint((innerR + depth) * Math.Cos(rotation + angularWidth / 2), (innerR + depth) * Math.Sin(rotation + angularWidth / 2), 0, meshSize);
            //groovePoints[3] = occ.AddPoint((innerR + depth) * Math.Cos(rotation - angularWidth / 2), (innerR + depth) * Math.Sin(rotation - angularWidth / 2), 0, meshSize);

            var grooveLines = new List<int>();

            for (int i = 0; i < groovePoints.Count - 1; i++)
            {
                grooveLines.Add(occ.AddLine(groovePoints[i], groovePoints[i + 1]));
            }
            grooveLines.Add(occ.AddLine(groovePoints[groovePoints.Count - 1], groovePoints[0]));

            //grooveLines[0] = occ.AddLine(groovePoints[0], groovePoints[1]);
            //grooveLines[1] = occ.AddLine(groovePoints[1], groovePoints[2]);
            //grooveLines[2] = occ.AddLine(groovePoints[2], groovePoints[3]);
            //grooveLines[3] = occ.AddLine(groovePoints[3], groovePoints[0]);

            var loop = occ.AddCurveLoop(grooveLines.ToArray());

            var surface = occ.AddPlaneSurface(new[] { loop });

            tags = new (int, int)[groovePoints.Count + grooveLines.Count + 2];

            for (int i = 0; i < groovePoints.Count; i++)
                tags[i] = (DIM0, groovePoints[i]);

            for (int i = 0; i < grooveLines.Count; i++)
                tags[groovePoints.Count + i] = (DIM1, grooveLines[i]);

            tags[groovePoints.Count + grooveLines.Count] = (DIM1, loop);
            tags[groovePoints.Count + grooveLines.Count + 1] = (DIM2, surface);

            return surface;
        }

        private static void PostProcess(Mesh mesh)
        {
            var edgesM = new SparseMatrix(mesh.Nodes.Length, mesh.Nodes.Length);

            foreach (var element in mesh.Elements)
            {
                for (var i0 = 0; i0 < element.Length; i0++)
                {
                    var i1 = i0 == element.Length - 1 ? 0 : i0 + 1;

                    edgesM[element[i0], element[i1]] += 1;
                    edgesM[element[i1], element[i0]] -= 1;
                }
            }

            var BorderEdgesHashSet = new HashSet<Tuple<int, int>>();

            foreach (var (i, j, v) in edgesM.Storage.EnumerateNonZeroIndexed())
            {
                var edge1 = Tuple.Create(i, j);
                var edge2 = Tuple.Create(j, i);

                if (!BorderEdgesHashSet.Contains(edge1) && !BorderEdgesHashSet.Contains(edge2))
                {
                    if (v > 0)
                        BorderEdgesHashSet.Add(edge1);
                }
            }

            mesh.BorderEdges = new List<Tuple<int, int>>(BorderEdgesHashSet);
            mesh.BorderEdgesGroups = new List<List<Tuple<int, int>>>();
            var freeEdges = new HashSet<Tuple<int, int>>(mesh.BorderEdges);

            while (freeEdges.Any())
            {
                var edge = freeEdges.First();

                var group = new List<Tuple<int, int>> { edge };

                freeEdges.Remove(edge);

                while (true)
                {
                    var gNode = group.Last();

                    var nextEdges = freeEdges.Where(e => e.Item1 == gNode.Item2 && e.Item2 != gNode.Item1
                                                         || e.Item2 == gNode.Item1 && e.Item1 != gNode.Item2
                                                         || e.Item1 == gNode.Item1 && e.Item2 != gNode.Item2
                                                         || e.Item2 == gNode.Item2 && e.Item1 != gNode.Item1).ToArray();

                    if (!nextEdges.Any())
                        break;

                    var nextEdge = nextEdges.First();

                    if (nextEdge.Item1 == gNode.Item1 || nextEdge.Item2 == gNode.Item2)
                    {
                        group.Add(Tuple.Create(nextEdge.Item2, nextEdge.Item1));
                    }
                    else
                    {
                        group.Add(nextEdge);
                    }

                    freeEdges.Remove(nextEdge);
                }

                mesh.BorderEdgesGroups.Add(group);
            }
        }

        public List<(int, double)> GetWeightedNearPoint(double x, double y)
        {
            for (var i = 0; i < Elements.Length; i++)
            {
                var points = Elements[i].Select(e => Nodes[e]).ToList();
                var xs = points.Select(p => p.X).ToList();
                var ys = points.Select(p => p.Y).ToList();

                if (Algebra.InPolygon(xs, ys, x, y))
                {
                    var result = new List<(int, double)>();
                    var distancies = points.Select(p => Point.GetDistance(x, y, p.X, p.Y)).ToArray();
                    var sumDistance = distancies.Sum();
                    var weights = distancies.Select(d => sumDistance / d).ToArray();
                    var sumWeight = weights.Sum();

                    for (var j = 0; j < points.Count; j++)
                    {
                        result.Add((points[j].Id, weights[j] / sumWeight));
                    }

                    return result;
                }
            }

            var near = GetNearestPoint(x, y, out _);

            return new List<(int, double)> { (near.Id, 1) };
        }

        public Point GetNearestPoint(double x, double y, out double distance)
        {
            var minDist = double.MaxValue;
            var minId = -1;

            foreach (var p in Nodes)
            {
                var dist = Point.GetDistance(x, y, p.X, p.Y);

                if (dist < minDist)
                {
                    minDist = dist;
                    minId = p.Id;
                }
            }

            distance = minDist;

            return Nodes[minId];
        }
    }
}
