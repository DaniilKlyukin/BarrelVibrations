using System.ComponentModel;
using BasicLibraryWinForm.Minimization;

namespace BarrelVibrations.Solvers
{

    public partial class SolverIdentifier
    {

        private readonly BackgroundWorker worker;
        private readonly MainSolver mainSolver;
        private readonly TargetCalculator targetCalculator;
        private readonly ParametricSolverCloner identifierCloner;

        public SolverIdentifier(
            MainSolver mainSolver,
            TargetCalculator targetCalculator,
            ParametricSolverCloner identifierCloner,
            BackgroundWorker worker)
        {
            this.worker = worker;
            this.mainSolver = mainSolver;
            this.targetCalculator = targetCalculator;
            this.identifierCloner = identifierCloner;
        }

        public double Identify(
            out MainSolver resultSolver,
            double expected,
            double min,
            double max,
            double tolerance = 1e-2)
        {
            var parameter = MinimumFinder.GetRootNsection(p =>
            {
                var solver = identifierCloner.GеtClone(mainSolver, p);
                solver.Initialize();
                solver.Solve();

                return targetCalculator.GеtTargerValue(solver) - expected;
            },
            min, max, tolerance, 1e-16,
            observer: (i, arg, target) =>
            {
                worker.ReportProgress(0, $"Идентификация, итерация: {i}, отклонение: {target:0.00}");

                return worker.CancellationPending;
            });

            resultSolver = identifierCloner.GеtClone(mainSolver, parameter);
            resultSolver.Initialize();
            resultSolver.Solve();

            return parameter;
        }
    }
}
