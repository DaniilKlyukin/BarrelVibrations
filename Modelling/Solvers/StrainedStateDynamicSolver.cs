using System;
using System.Collections.Generic;
using System.Linq;
using Modelling.Loads;
using Modelling.Material;
using Modelling.MeshFolder;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers
{
    public class StrainedStateDynamicSolver : StrainedStateSolver
    {
        private readonly Dictionary<int, double> _u0;
        private readonly Dictionary<int, double> _v0;

        public StrainedStateDynamicSolver(
            IProblem problem,
            MaterialProperties materialProperties,
            List<double> timeMoments,
            Dictionary<int, double> U0,
            Dictionary<int, double> V0,
            Dictionary<int, Func<double, double>> constraintsUDictionary,
            Dictionary<int, Func<double, double>> constraintsVDictionary,
            Dictionary<(int, int), LoadFunc> pressureEdgeLoads,
            PressureField pressureField,
            Mesh mesh,
            CoordinatesConverter coordinatesConverter) 
            : base(problem, materialProperties, constraintsUDictionary, constraintsVDictionary, pressureEdgeLoads, pressureField, mesh, coordinatesConverter)
        {
            TimeMoments = timeMoments.ToArray();

            _u0 = U0;
            _v0 = V0;

            foreach (var element in Elements)
            {
                element.Update(element.Points.Select(p => U0[p.Id]).ToArray(), element.Points.Select(p => V0[p.Id]).ToArray(), 300);
            }
            Problem.InitializeSolutions();
        }

        public override void Solve(double tolerance = 0.01, int iterationsLimit = 10)
        {
            //SolveIteration(TimeMoments[0], out var _u0, out var _v0, tolerance, iterationsLimit);
            Problem.CalculateParameters(Elements, _u0, _v0);

            //SolveIteration(TimeMoments[1], out var u0, out var v0, tolerance, iterationsLimit);
            Problem.CalculateParameters(Elements, _u0, _v0);

            for (var i = 2; i < TimeMoments.Length; i++)
            {
                if (Worker.CancellationPending)
                    return;

                SolveIteration(TimeMoments[i], TimeMoments[i] - TimeMoments[i - 1], Problem.U[i - 1], Problem.V[i - 1], Problem.U[i - 2], Problem.V[i - 2], out var u, out var v, tolerance, iterationsLimit);
                Problem.CalculateParameters(Elements, u, v);

                Worker.ReportProgress(100 * i / (TimeMoments.Length - 1), this);
                CurrentTimeIndex = i;
            }

            Worker.ReportProgress(100, this);
        }

        public override void Solve()
        {
            //SolveIteration(TimeMoments[0], out var _u0, out var _v0);
            Problem.CalculateParameters(Elements, _u0, _v0);

            // SolveIteration(TimeMoments[1], out var u0, out var v0);
            Problem.CalculateParameters(Elements, _u0, _v0);

            for (var i = 2; i < TimeMoments.Length; i++)
            {
                if (Worker.CancellationPending)
                    return;

                SolveIteration(TimeMoments[i], TimeMoments[i] - TimeMoments[i - 1], Problem.U[i - 1], Problem.V[i - 1], Problem.U[i - 2], Problem.V[i - 2], out var u, out var v);
                Problem.CalculateParameters(Elements, u, v);

                Worker.ReportProgress(100 * i / (TimeMoments.Length - 1), this);
                CurrentTimeIndex = i;
            }

            Worker.ReportProgress(100, this);
        }
    }
}