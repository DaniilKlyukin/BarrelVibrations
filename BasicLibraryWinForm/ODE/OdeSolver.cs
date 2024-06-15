namespace BasicLibraryWinForm
{

    public abstract partial class OdeSolver
    {
        public Func<double, OdeSolution, bool> CheckStop { get; set; } = (_, _) => true;
        public Func<List<OdeSolution>, List<OdeSolution>, double> ErrorFunc { get; set; } = GetError;
        public Func<double, OdeSolution, OdeSolution, OdeSolution> EquationsSystem { get; set; } = (_, _, _) => new(0);
        public Action<double, OdeSolution> EquationsObserver { get; set; } = (_, _) => { };
        public Action EquationsObserverInitialization { get; set; } = () => { };

        public OdeSolution InitialValues { get; set; } = new(0);
        public OdeSolution CurrentSolution { get; protected set; } = new(0);
        protected OdeSolution LastSolution { get; set; } = new(0);
        protected OdeSolution GetDerivative(double dt) => (CurrentSolution - LastSolution) / dt;
        public double CurrentTime { get; protected set; }
        public double Dt { get; private set; }
        public int Iteration { get; protected set; }

        private List<OdeSolution> solutionListPrevious = new();
        private List<OdeSolution> solutionListCurrent = new();

        public void Solve(double t0, double initialDt, double tolerance = 1e-8)
        {
            Dt = initialDt;
            solutionListPrevious = SolveApproximate(t0, Dt);

            while (true)
            {
                Dt /= 2;
                solutionListCurrent = SolveApproximate(t0, Dt);

                if (ErrorFunc(solutionListPrevious, solutionListCurrent) <= tolerance)
                {
                    return;
                }

                solutionListPrevious = new List<OdeSolution>(solutionListCurrent);
            }
        }

        public void Solve(double t0, double dt)
        {
            CurrentSolution = new OdeSolution(InitialValues.Values.Copy());
            LastSolution = new OdeSolution(InitialValues.Values.Copy());
            CurrentTime = t0;
            EquationsObserverInitialization();
            Iteration = 0;

            while (true)
            {
                if (SolveStep(dt))
                    return;
            }
        }

        public void InitializeStepSolver(double t0)
        {
            Iteration = 0;
            CurrentSolution = new OdeSolution(InitialValues.Values.Copy());
            LastSolution = new OdeSolution(InitialValues.Values.Copy());
            CurrentTime = t0;
            EquationsObserverInitialization();
        }

        public bool SolveStep(double dt)
        {
            EquationsObserver(CurrentTime, CurrentSolution);

            if (CheckStop(CurrentTime, CurrentSolution))
                return true;

            var isStop = SolveInnerStep(dt);

            if (Iteration > 0)
                LastSolution = new OdeSolution(CurrentSolution.Values.Copy());

            Iteration++;
            CurrentTime += dt;

            return isStop;
        }

        protected abstract bool SolveInnerStep(double dt);

        private List<OdeSolution> SolveApproximate(double t0, double dt)
        {
            var solution = new List<OdeSolution>();
            Iteration = 0;
            CurrentSolution = new OdeSolution(InitialValues.Values.Copy());
            LastSolution = new OdeSolution(InitialValues.Values.Copy());
            CurrentTime = t0;
            EquationsObserverInitialization();

            while (true)
            {
                var copy = new OdeSolution(CurrentSolution.Values.Copy());

                solution.Add(copy);

                if (SolveStep(dt))
                    break;
            }

            return solution;
        }

        private static double GetError(List<OdeSolution> solution0, List<OdeSolution> solution1)
        {
            var error = 0.0;

            var last0 = solution0.Last();
            var last1 = solution1.Last();

            for (var i = 0; i < last0.Length; i++)
            {
                if (Math.Abs(last1[i]) > 1e-15 && Math.Abs(last0[i]) > 1e-15)
                    error = Math.Max(Math.Abs(last0[i] - last1[i]) / Math.Abs(last1[i]), error);
            }

            return error;
        }
    }

}
