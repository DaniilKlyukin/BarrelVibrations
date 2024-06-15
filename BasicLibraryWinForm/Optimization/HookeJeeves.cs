using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BasicLibraryWinForm.Optimization
{
    [Serializable]
    [DataContract(Name = "Оптимизатор Хука-Дживса")]
    public class HookeJeeves : Optimizer
    {
        /// <summary>
        /// Значения X
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Значения X")]
        private double[] xCurrent { get; set; } = new double[0];

        /// <summary>
        /// Значения функции
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Значения функции")]
        private double fCurrent { get; set; } = double.MaxValue;

        /// <summary>
        /// Шаг
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Хука-Дживса", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Шаг")]
        [DataMember(Name = "Шаг")]
        [TypeConverter(typeof(CountArrayConverter))]
        public double[] Step { get; set; } = new[] { 0.001 };

        /// <summary>
        /// Коэффициент уменьшения шага
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Хука-Дживса", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Коэффициент уменьшения шага")]
        [DataMember(Name = "Коэффициент уменьшения шага")]
        public double ReductionCoefficient { get; set; } = 2;

        public override OptimizationAlgorithm Algorithm => OptimizationAlgorithm.HookeJeeves;


        public override double GetEps()
        {
            return Step.Average();
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
                CurrentIteration++;
                if (SaveResultsInFile && CurrentIteration % IterationsToWrite == 0)
                {
                    WriteIterationResult(CurrentIteration);
                    Observer?.Invoke(BestSolution.Item1, BestSolution.Item2);
                }
                UpdateBestSolution();

                var xSearch = SearchStep(out var fSearch);

                if (fSearch < fCurrent)
                {
                    var xNew = new double[xSearch.Length];

                    for (int i = 0; i < xSearch.Length; i++)
                    {
                        xNew[i] = xCurrent[i] + 2 * (xSearch[i] - xCurrent[i]);
                    }

                    var fNew = GetF(xNew);

                    if (fNew < fSearch)
                    {
                        xCurrent = xNew;
                        fCurrent = fNew;
                    }
                    else
                    {
                        xCurrent = xSearch;
                        fCurrent = fSearch;
                    }
                }
                else
                {
                    for (var i = 0; i < Step.Length; i++)
                        Step[i] = Math.Max(Step[i] / ReductionCoefficient, 1e-15);


                    if (Step.Max() <= Tolerance)
                        return;

                    continue;
                }

                if (Math.Abs(fCurrent - BestSolution.Item2) <= Tolerance)
                    StableIterations++;
                else
                    StableIterations = 0;

                var xDist = Math.Sqrt(xCurrent.Select((v, i) => FastMath.Pow2(v - xSearch[i])).Sum());

                if (StableIterations >= StableIterationsToStop || GetEps() <= Tolerance || Worker != null && Worker.CancellationPending)
                {
                    WriteIterationResult(CurrentIteration);
                    Observer?.Invoke(BestSolution.Item1, BestSolution.Item2);
                    break;
                }
            }
        }

        private double[] SearchStep(out double fNew)
        {
            var xNew = xCurrent.Copy();
            fNew = double.MaxValue;

            for (int i = 0; i < xCurrent.Length; i++)
            {
                var xProbaR = xNew.Copy();
                xProbaR[i] += Step[i];
                var xProbaL = xNew.Copy();
                xProbaL[i] -= Step[i];

                var fR = GetF(xProbaR);
                var fL = GetF(xProbaL);

                if (fR < fNew)
                {
                    xNew = xProbaR;
                    fNew = fR;
                }

                if (fL < fNew)
                {
                    xNew = xProbaL;
                    fNew = fL;
                }
            }

            return xNew;
        }
    }
}
