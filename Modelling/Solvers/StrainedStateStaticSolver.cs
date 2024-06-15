using System;
using System.Collections.Generic;
using Modelling.Loads;
using Modelling.Material;
using Modelling.MeshFolder;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers
{
    public class StrainedStateStaticSolver : StrainedStateSolver
    {
        public StrainedStateStaticSolver(
            IProblem problem,
            MaterialProperties materialProperties,
            Dictionary<int, Func<double, double>> constraintsUDictionary,
            Dictionary<int, Func<double, double>> constraintsVDictionary,
            Dictionary<(int, int), LoadFunc> pressureEdgeLoads,
            PressureField pressureField,
            Mesh mesh,
            CoordinatesConverter coordinatesConverter)
            : base(problem, materialProperties, constraintsUDictionary, constraintsVDictionary, pressureEdgeLoads, pressureField, mesh, coordinatesConverter)
        {
            foreach (var element in Elements)
            {
                element.Update(new double[element.Points.Length], new double[element.Points.Length], 300);
            }
            Problem.InitializeSolutions();
        }

        public override void Solve(double tolerance = 0.01, int iterationsLimit = 10)
        {
            SolveIteration(0, out var U, out var V, tolerance, iterationsLimit);

            Problem.CalculateParameters(Elements, U, V);

            Worker.ReportProgress(100, this);
        }

        public override void Solve()
        {
            SolveIteration(0, out var U, out var V);

            Problem.CalculateParameters(Elements, U, V);

            Worker.ReportProgress(100, this);
        }
    }
}