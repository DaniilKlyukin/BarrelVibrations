using BasicLibraryWinForm.PropertiesTemplates;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BasicLibraryWinForm.Optimization
{
    [Serializable]
    [DataContract(Name = "Случайный поиск")]
    public class RandomDescend : Optimizer
    {
        private const int GENERATIONS = 2;
        private Random random = new Random();
        private double fPrevious = double.MaxValue;

        /// <summary>
        /// Значения X
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Значения X")]
        private double[] xCurrent { get; set; } = new double[0];

        /// <summary>
        /// Значение функции
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Значение функции")]
        private double fCurrent = double.MaxValue;

        /// <summary>
        /// Радиус поиска
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры случайного поиска", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Радиус поиска")]
        [DataMember(Name = "Радиус поиска")]
        public double Radius { get; set; } = 0.001;

        /// <summary>
        /// Коэффициент уменьшения радиуса
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры случайного поиска", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Коэффициент уменьшения радиуса")]
        [DataMember(Name = "Коэффициент уменьшения радиуса")]
        public double ReductionCoefficient { get; set; } = 2;

        /// <summary>
        /// Итераций после установления для уменьшения радиуса
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры случайного поиска", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Итераций после установления для уменьшения радиуса")]
        [DataMember(Name = "Итераций после установления для уменьшения радиуса")]
        public double StableIterationsToReduction { get; set; } = 5000;

        public override OptimizationAlgorithm Algorithm => OptimizationAlgorithm.RandomDescend;

        public override double GetEps()
        {
            return Math.Abs(fCurrent - fPrevious);
        }

        protected override void GetCurrentBest(out double[] x, out double f)
        {
            x = ConvertBack(xCurrent);
            f = fCurrent;
        }

        public override void Optimize(bool resumeCalculation = false)
        {
            if (!resumeCalculation)
            {
                BestSolution = (new double[0], double.MaxValue);
                History.Clear();
                FCalculations = 0;
                StableIterations = 0;
                CurrentIteration = 0;
                xCurrent = x0.Copy();
            }

            while (CurrentIteration < MaxIterations)
            {
                var xNew = RandomStep(out var fNew);

                if (fNew < fCurrent)
                {
                    for (int i = 0; i < xNew.Length; i++)
                    {
                        xCurrent[i] = xCurrent[i] + 2 * (xNew[i] - xCurrent[i]);
                    }

                    fCurrent = GetF(xCurrent);
                }

                UpdateBestSolution();

                if (Math.Abs(fCurrent - BestSolution.Item2) <= Tolerance)
                    StableIterations++;
                else
                    StableIterations = 0;

                if (StableIterations % StableIterationsToReduction == 0)
                    Radius /= ReductionCoefficient;

                if (StableIterations >= StableIterationsToStop || Worker != null && Worker.CancellationPending)
                {
                    WriteIterationResult(CurrentIteration);
                    Observer?.Invoke(BestSolution.Item1, BestSolution.Item2);
                    break;
                }

                CurrentIteration++;

                if (SaveResultsInFile && CurrentIteration % IterationsToWrite == 0)
                {
                    WriteIterationResult(CurrentIteration);
                    Observer?.Invoke(BestSolution.Item1, BestSolution.Item2);
                }
            }
        }

        private double[] RandomStep(out double fNew)
        {
            var xNew = xCurrent.Copy();
            fNew = double.MaxValue;

            for (int i = 0; i < xCurrent.Length; i++)
            {
                for (int j = 0; j < GENERATIONS; j++)
                {
                    var xProba = xNew.Copy();

                    xProba[i] +=
                        j % 2 == 0
                        ? Radius * random.NextDouble()
                        : -Radius * random.NextDouble();

                    var f = GetF(xProba);

                    if (f < fNew)
                    {
                        xNew = xProba;
                        fNew = f;
                    }
                }
            }

            return xNew;
        }

    }
}
