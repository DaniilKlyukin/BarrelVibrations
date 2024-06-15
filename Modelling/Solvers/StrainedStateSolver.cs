using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PointFolder;
using MathNet.Numerics.LinearAlgebra.Double;
using Modelling.Loads;
using Modelling.Material;
using Modelling.MeshFolder;
using Modelling.Solvers.Elements;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers
{
    public abstract class StrainedStateSolver
    {
        protected StrainedStateSolver(
            IProblem problem,
            MaterialProperties materialProperties,
            Dictionary<int, Func<double, double>> constraintsUDictionary,
            Dictionary<int, Func<double, double>> constraintsVDictionary,
            Dictionary<(int, int), LoadFunc> pressureEdgeLoads,
            PressureField pressureField,
            Mesh mesh,
            CoordinatesConverter coordinatesConverter)
        {
            ConstraintsUDictionary = constraintsUDictionary;
            ConstraintsVDictionary = constraintsVDictionary;
            PressureEdgeLoads = pressureEdgeLoads;
            PressureField = pressureField;
            Mesh = mesh;
            Nodes = mesh.Nodes;
            CurrentTimeIndex = 0;
            TimeMoments = new double[1];
            Problem = problem;
            CoordinatesConverter = coordinatesConverter;

            AxisSymmetry = problem switch
            {
                ASProblem => true,
                FlatProblem => false,
                _ => false
            };

            Elements = new List<Element>(mesh.Elements.Length);

            foreach (var element in mesh.Elements)
            {
                switch (element.Length)
                {
                    case 3:
                        Elements.Add(new TriangleElement(element.Select(id => mesh.Nodes[id]).ToArray(), materialProperties, problem));
                        break;
                    case 4:
                        Elements.Add(new QuadElement(element.Select(id => mesh.Nodes[id]).ToArray(), materialProperties, problem));
                        break;
                    case 6:
                        Elements.Add(new Triangle2DegreeElement(element.Select(id => mesh.Nodes[id]).ToArray(), materialProperties, problem));
                        break;
                    case 8:
                        Elements.Add(new Quad2DegreeElement(element.Select(id => mesh.Nodes[id]).ToArray(), materialProperties, problem));
                        break;
                }
            }

            foreach (var element in Elements)
            {
                element.Init();
            }

            InitializeMatricies();
        }
        private void InitializeMatricies()
        {
            var K_dict = CalculateGlobalK();
            var M_dict = CalculateGlobalM();

            K = SparseMatrix.OfIndexed(2 * Nodes.Length, 2 * Nodes.Length, K_dict.Select(kv => Tuple.Create(kv.Key.Item1, kv.Key.Item2, kv.Value)));
            M = SparseMatrix.OfIndexed(2 * Nodes.Length, 2 * Nodes.Length, M_dict.Select(kv => Tuple.Create(kv.Key.Item1, kv.Key.Item2, kv.Value)));
        }

        private SparseMatrix K { get; set; }
        private SparseMatrix M { get; set; }
        public BackgroundWorker Worker { get; set; } = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        public IProblem Problem { get; set; }
        protected Point[] Nodes { get; set; }
        protected Dictionary<(int, int), LoadFunc> PressureEdgeLoads { get; set; }
        public PressureField PressureField { get; }
        public Mesh Mesh { get; }
        private bool AxisSymmetry { get; }
        public Dictionary<int, Func<double, double>> ConstraintsUDictionary { get; }
        public Dictionary<int, Func<double, double>> ConstraintsVDictionary { get; }
        protected SparseVector Loads { get; set; }
        protected List<Element> Elements { get; set; }
        public double[] TimeMoments { get; protected set; }
        public int CurrentTimeIndex { get; protected set; }
        public CoordinatesConverter CoordinatesConverter { get; private set; }


        public abstract void Solve(double tolerance = 0.01, int iterationsLimit = 10);
        public abstract void Solve();
        protected double GetEps(Dictionary<int, double> u0, Dictionary<int, double> v0, Dictionary<int, double> u1, Dictionary<int, double> v1)
        {
            var eps1 = 0.0;
            var eps2 = 0.0;
            var n = u0.Count;

            foreach (var (id, u) in u0)
            {
                if (u1[id] != 0)
                    eps1 += Math.Abs((u1[id] - u) / u1[id]) / n;
            }

            foreach (var (id, v) in v0)
            {
                if (v1[id] != 0)
                    eps1 += Math.Abs((v1[id] - v) / v1[id]) / n;
            }

            return (eps1 + eps2) / 2;
        }


        protected void SolveProblem(SparseMatrix A, SparseVector b, out Dictionary<int, double> u, out Dictionary<int, double> v)
        {
            u = new Dictionary<int, double>();
            v = new Dictionary<int, double>();

            var x = LinearEquationSolver.SolveIterative(A, b);

            for (var i = 0; i < Nodes.Length; i++)
            {
                u.Add(Nodes[i].Id, x[2 * i]);
                v.Add(Nodes[i].Id, x[2 * i + 1]);
            }
        }

        private Dictionary<(int, int), double> CalculateGlobalK()
        {
            var K_dict = new Dictionary<(int, int), double>(2 * Nodes.Length);

            foreach (var element in Elements)
            {
                CalculateK(K_dict, element);
            }

            return K_dict;
        }

        private void CalculateK(Dictionary<(int, int), double> K_dict, Element element)
        {
            for (var i = 0; i < element.Points.Length; i++)
            {
                var index0 = element.Points[i].Id;
                for (var j = 0; j < element.Points.Length; j++)
                {
                    var index1 = element.Points[j].Id;

                    for (var k0 = 0; k0 < 2; k0++)
                    {
                        for (var k1 = 0; k1 < 2; k1++)
                        {
                            var key = (2 * index0 + k0, 2 * index1 + k1);

                            if (K_dict.ContainsKey(key))
                            {
                                K_dict[key] += element.k[2 * i + k0, 2 * j + k1];
                            }
                            else
                            {
                                K_dict.Add(key, element.k[2 * i + k0, 2 * j + k1]);
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<(int, int), double> CalculateGlobalM()
        {
            var M_dict = new Dictionary<(int, int), double>(2 * Nodes.Length);

            foreach (var element in Elements)
            {
                CalculateM(M_dict, element);
            }

            return M_dict;
        }

        private void CalculateM(Dictionary<(int, int), double> M_dict, Element element)
        {
            for (var i = 0; i < element.Points.Length; i++)
            {
                var index0 = element.Points[i].Id;
                for (var j = 0; j < element.Points.Length; j++)
                {
                    var index1 = element.Points[j].Id;

                    for (var k0 = 0; k0 < 2; k0++)
                    {
                        for (var k1 = 0; k1 < 2; k1++)
                        {
                            var key = (2 * index0 + k0, 2 * index1 + k1);

                            if (M_dict.ContainsKey(key))
                            {
                                M_dict[key] += element.M[2 * i + k0, 2 * j + k1];
                            }
                            else
                            {
                                M_dict.Add(key, element.M[2 * i + k0, 2 * j + k1]);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateElements(Dictionary<int, double> u, Dictionary<int, double> v)
        {
            if (!u.Any() || !v.Any())
                return;

            Parallel.ForEach(Elements, element =>
            {
                element.Update(element.Points.Select(p => u[p.Id]).ToArray(),
                    element.Points.Select(p => v[p.Id]).ToArray(), 300);
            });
        }
        protected void SolveIteration(
            double time,
            out Dictionary<int, double> U,
            out Dictionary<int, double> V)
        {
            UpdateLoads(time);
            SetConstraints(K, Loads, time);
            SolveProblem(K, Loads, out U, out V);
        }

        protected void SolveIteration(
            double time,
            double dt,
            Dictionary<int, double> U_1,
            Dictionary<int, double> V_1,
            Dictionary<int, double> U_2,
            Dictionary<int, double> V_2,
            out Dictionary<int, double> U,
            out Dictionary<int, double> V)
        {
            var dt_2 = FastMath.Pow2(dt);

            UpdateLoads(time);

            var d1 = new SparseVector(2 * Nodes.Length);
            var d2 = new SparseVector(2 * Nodes.Length);

            for (var i = 0; i < Nodes.Length; i++)
            {
                d1[2 * i] = U_1[Nodes[i].Id];
                d1[2 * i + 1] = V_1[Nodes[i].Id];

                d2[2 * i] = U_2[Nodes[i].Id];
                d2[2 * i + 1] = V_2[Nodes[i].Id];
            }

            var A = SparseMatrix.OfMatrix(K + M / dt_2);
            var b = Loads + M * (2 * d1 - d2) / dt_2;

            SetConstraints(A, b, time);
            SolveProblem(A, b, out U, out V);
        }


        protected void SolveIteration(
            double time,
            out Dictionary<int, double> U,
            out Dictionary<int, double> V,
            double tolerance,
            int iterationsLimit)
        {
            var iteration = 0;

            var u1 = new Dictionary<int, double>();
            var v1 = new Dictionary<int, double>();

            UpdateLoads(time);

            while (true)
            {
                var u0 = u1.ToDictionary(entry => entry.Key,
                    entry => entry.Value);
                var v0 = v1.ToDictionary(entry => entry.Key,
                    entry => entry.Value);

                UpdateLoads(time, u1, v1, true);
                UpdateElements(u1, v1);
                SetConstraints(K, Loads, time);
                SolveProblem(K, Loads, out u1, out v1);

                var eps = GetEps(u0, v0, u1, v1);

                if (eps > tolerance)
                    Worker.ReportProgress((int)(100 * tolerance / eps), $"t = {time * 1e3:0.000} мс");

                if (u0.Any() && v0.Any() && (eps <= tolerance || iteration >= iterationsLimit || Worker.CancellationPending))
                {
                    U = u1;
                    V = v1;
                    return;
                }

                iteration++;
            }
        }

        protected void SolveIteration(
            double time,
            double dt,
            Dictionary<int, double> U_1,
            Dictionary<int, double> V_1,
            Dictionary<int, double> U_2,
            Dictionary<int, double> V_2,
            out Dictionary<int, double> U,
            out Dictionary<int, double> V,
            double tolerance,
            int iterationsLimit)
        {
            var dt_2 = FastMath.Pow2(dt);
            var iteration = 0;

            var u1 = new Dictionary<int, double>();
            var v1 = new Dictionary<int, double>();

            UpdateLoads(time);

            while (true)
            {
                var u0 = u1.ToDictionary(entry => entry.Key,
                    entry => entry.Value);
                var v0 = v1.ToDictionary(entry => entry.Key,
                    entry => entry.Value);

                UpdateLoads(time, u1, v1, true);
                UpdateElements(u1, v1);

                var d1 = new SparseVector(2 * Nodes.Length);
                var d2 = new SparseVector(2 * Nodes.Length);

                for (var i = 0; i < Nodes.Length; i++)
                {
                    d1[2 * i] = U_1[Nodes[i].Id];
                    d1[2 * i + 1] = V_1[Nodes[i].Id];

                    d2[2 * i] = U_2[Nodes[i].Id];
                    d2[2 * i + 1] = V_2[Nodes[i].Id];
                }

                var A = SparseMatrix.OfMatrix(K + M / dt_2);
                var b = Loads + M * (2 * d1 - d2) / dt_2;

                SetConstraints(A, b, time);
                SolveProblem(A, b, out u1, out v1);

                var eps = GetEps(u0, v0, u1, v1);

                if (eps > tolerance)
                    Worker.ReportProgress((int)(100 * tolerance / eps), $"t = {time * 1e3:0.000} мс");

                if (u0.Any() && v0.Any() && (eps <= tolerance || iteration >= iterationsLimit || Worker.CancellationPending))
                {
                    U = u1;
                    V = v1;
                    return;
                }

                iteration++;
            }
        }

        private double FindPressureInField(Point p, double time)
        {
            var t = 0;

            var w0 = 0.0;
            var w1 = 0.0;

            if (time <= PressureField.PressureLoads[0].TimeMoment)
            {
                w0 = 1;
                w1 = 0;
                t = 0;
            }
            else if (time >= PressureField.PressureLoads.Last().TimeMoment)
            {
                w0 = 0;
                w1 = 1;
                t = PressureField.PressureLoads.Count - 2;
            }
            else
            {
                for (var i = 0; i < PressureField.PressureLoads.Count - 1; i++)
                {
                    var t0 = PressureField.PressureLoads[i].TimeMoment;
                    var t1 = PressureField.PressureLoads[i + 1].TimeMoment;

                    if (time >= t0 && time <= t1)
                    {
                        t = i;
                        w0 = (t1 - time) / (t1 - t0);
                        w1 = (time - t0) / (t1 - t0);
                        break;
                    }
                }
            }

            var p0 = 0.0;
            var minDist0 = double.MaxValue;

            foreach (var pressurePointLoad in PressureField.PressureLoads[t].Loads)
            {
                var d = Point.GetDistance(p, pressurePointLoad.Point);

                if (d < minDist0)
                {
                    minDist0 = d;
                    p0 = pressurePointLoad.Pressure;
                }
            }

            var p1 = 0.0;
            var minDist1 = double.MaxValue;

            foreach (var pressurePointLoad in PressureField.PressureLoads[t + 1].Loads)
            {
                var d = Point.GetDistance(p, pressurePointLoad.Point);

                if (d < minDist1)
                {
                    minDist1 = d;
                    p1 = pressurePointLoad.Pressure;
                }
            }

            return w0 * p0 + w1 * p1;
        }

        protected void UpdateLoads(double time, bool updateNormals = false, bool loadsOnDisplacements = true)
        {
            Loads = new SparseVector(2 * Nodes.Length);

            var sym = AxisSymmetry ? 1 : 0;

            foreach (var ((pId1, pId2), pFunc) in PressureEdgeLoads)
            {
                var u1 = Problem.U.Any() ? Problem.U.Last()[pId1] : 0;
                var v1 = Problem.V.Any() ? Problem.V.Last()[pId1] : 0;
                var u2 = Problem.U.Any() ? Problem.U.Last()[pId2] : 0;
                var v2 = Problem.V.Any() ? Problem.V.Last()[pId2] : 0;

                var dp1 = new Point(u1, v1);
                var dp2 = new Point(u2, v2);

                var p1 = Nodes[pId1];
                var p2 = Nodes[pId2];

                var pc1 = CoordinatesConverter.GetPoint(p1);
                var pc2 = CoordinatesConverter.GetPoint(p2);

                var (nx, ny) = updateNormals
                    ? Algebra.GetClockwisedNormal(pc1 + dp1, pc2 + dp2)
                    : Algebra.GetClockwisedNormal(pc1, pc2);

                var length = Math.Sqrt(FastMath.Pow2(pc1.X - pc2.X) + FastMath.Pow2(pc1.Y - pc2.Y));

                double p;


                if (pFunc.HasValue)
                    p = pFunc.PressureFunc(time);
                else
                {
                    p = loadsOnDisplacements
                        ? FindPressureInField((pc1 + dp1 + pc2 + dp2) / 2, time)
                        : FindPressureInField((pc1 + pc2) / 2, time);
                }

                var fx = nx * p * length;
                var fy = ny * p * length;

                Loads[2 * pId1] += fx * (1 - sym + sym * pc1.Y);
                Loads[2 * pId1 + 1] += fy * (1 - sym + sym * pc1.Y);

                Loads[2 * pId2] += fx * (1 - sym + sym * pc2.Y);
                Loads[2 * pId2 + 1] += fy * (1 - sym + sym * pc2.Y);
            }
        }

        protected void UpdateLoads(double time, Dictionary<int, double> u, Dictionary<int, double> v, bool updateNormals = false, bool loadsOnDisplacements = true)
        {
            Loads = new SparseVector(2 * Nodes.Length);

            var info = new Dictionary<int, (double, double)>();
            foreach (var ((pId1, pId2), pFunc) in PressureEdgeLoads)
            {
                var u1 = 0.0;
                var v1 = 0.0;
                var u2 = 0.0;
                var v2 = 0.0;

                if (u.ContainsKey(pId1))
                    u1 = u[pId1];
                if (v.ContainsKey(pId1))
                    v1 = v[pId1];

                if (u.ContainsKey(pId2))
                    u2 = u[pId2];
                if (v.ContainsKey(pId2))
                    v2 = v[pId2];

                var dp1 = new Point(u1, v1);
                var dp2 = new Point(u2, v2);

                var p1 = Nodes[pId1];
                var p2 = Nodes[pId2];

                var pc1 = CoordinatesConverter.GetPoint(p1);
                var pc2 = CoordinatesConverter.GetPoint(p2);

                var (nx, ny) = updateNormals
                    ? Algebra.GetClockwisedNormal(pc1 + dp1, pc2 + dp2)
                    : Algebra.GetClockwisedNormal(pc1, pc2);

                var length = Math.Sqrt(FastMath.Pow2(pc1.X - pc2.X) + FastMath.Pow2(pc1.Y - pc2.Y));

                double p;

                if (pFunc.HasValue)
                    p = pFunc.PressureFunc(time);
                else
                {
                    p = loadsOnDisplacements
                        ? FindPressureInField((pc1 + dp1 + pc2 + dp2) / 2, time)
                        : FindPressureInField((pc1 + pc2) / 2, time);
                }

                var fx = nx * p * length;
                var fy = ny * p * length;

                if (info.ContainsKey(pId1))
                    info[pId1] = (info[pId1].Item1 + fx, info[pId1].Item2 + fy);
                else
                {
                    info.Add(pId1, (fx, fy));
                }

                if (info.ContainsKey(pId2))
                    info[pId2] = (info[pId2].Item1 + fx, info[pId2].Item2 + fy);
                else
                {
                    info.Add(pId2, (fx, fy));
                }
            }

            var sym = AxisSymmetry ? 1 : 0;

            foreach (var (id, (fx, fy)) in info)
            {
                var r = CoordinatesConverter.GetPoint(Nodes[id]).Y;

                Loads[2 * id] += fx * (1 - sym + sym * r);
                Loads[2 * id + 1] += fy * (1 - sym + sym * r);
            }
        }

        protected void SetConstraints(Dictionary<(int, int), double> A, SparseVector b, double time)
        {
            var keys = A.Keys.ToArray();

            for (var k = 0; k < A.Count; k++)
            {
                var (i, j) = keys[k];

                if (ConstraintsUDictionary.ContainsKey(i / 2) && i % 2 == 0)
                {
                    if (i == j)
                    {
                        A[(i, j)] = 1;
                        b[i] = ConstraintsUDictionary[i / 2](time);
                    }
                    else
                        A[(i, j)] = 0;
                }

                if (ConstraintsVDictionary.ContainsKey(i / 2) && i % 2 == 1)
                {
                    if (i == j)
                    {
                        A[(i, j)] = 1;
                        b[i] = ConstraintsVDictionary[i / 2](time);
                    }
                    else
                        A[(i, j)] = 0;
                }
            }
        }

        protected void SetConstraints(SparseMatrix A, SparseVector b, double time)
        {
            foreach (var (pId, fValue) in ConstraintsUDictionary)
            {
                var i = 2 * pId;
                A.ClearRow(i);
                A[i, i] = 1;
                b[i] = fValue(time);
            }

            foreach (var (pId, fValue) in ConstraintsVDictionary)
            {
                var i = 2 * pId + 1;
                A.ClearRow(i);
                A[i, i] = 1;
                b[i] = fValue(time);
            }
        }
    }
}
