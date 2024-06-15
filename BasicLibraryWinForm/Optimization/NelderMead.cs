using System.ComponentModel;
using System.Runtime.Serialization;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates;

namespace BasicLibraryWinForm.Optimization
{
    [Serializable]
    [DataContract(Name = "Оптимизатор Нелдера-Мида")]
    public class NelderMead : Optimizer
    {
        public override OptimizationAlgorithm Algorithm { get; } = OptimizationAlgorithm.NelderMead;

        /// <summary>
        /// Значения функции
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Значения функции")]
        public double[] FN { get; private set; } = new double[0];

        /// <summary>
        ///     Коэффициент отражения
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Нелдера-Мида", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Коэффициент отражения")]
        [DataMember(Name = "Коэффициент отражения")]
        public double ReflectionCoef { get; set; } = 1;

        /// <summary>
        ///     Коэффициент растяжения
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Нелдера-Мида", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Коэффициент растяжения")]
        [DataMember(Name = "Коэффициент растяжения")]
        public double StretchCoef { get; set; } = 2;

        /// <summary>
        ///     Коэффициент сжатия
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Нелдера-Мида", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Коэффициент сжатия")]
        [DataMember(Name = "Коэффициент сжатия")]
        public double CompressionCoef { get; set; } = 0.5;

        /// <summary>
        ///     Итераций до восстановление симплекса
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Нелдера-Мида", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Итераций до восстановление симплекса")]
        [DataMember(Name = "Итераций до восстановление симплекса")]
        public int IterationsToRestore { get; set; } = 50;

        /// <summary>
        ///     Начальная длина стороны симплекса
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Параметры метода Нелдера-Мида", 4, CATEGORIESCOUNT + 1)]
        [Description("")]
        [DisplayName("Начальная длина стороны симплекса")]
        [DataMember(Name = "Начальная длина стороны симплекса")]
        public double InitialSideLength { get; set; } = 0.001;

        /// <summary>
        ///     Параметр длины ребра симплекса, от 0 до 1
        /// </summary>
        [Browsable(true)]
        [IgnoreDataMember]
        [CustomSortedCategory("Параметры метода Нелдера-Мида", 4, CATEGORIESCOUNT + 1)]
        [DisplayName("Длина стороны симплекса")]
        public double SideLength
        {
            get
            {
                var avgLength = 0.0;
                var count = 0;

                for (var i1 = 0; i1 < SimplexXs.Length; i1++)
                    for (var i2 = 0; i2 < SimplexXs.Length; i2++)
                    {
                        if (i1 == i2)
                            continue;

                        count++;

                        var length = 0.0;

                        for (var j = 0; j < SimplexXs[i1].Length; j++)
                            length += FastMath.Pow2(SimplexXs[i1][j] - SimplexXs[i2][j]);

                        avgLength += Math.Sqrt(length);
                    }

                var sideLength = avgLength / count;

                if (double.IsFinite(sideLength))
                    return sideLength;

                return InitialSideLength;
            }
        }

        /// <summary>
        ///     Точки симплекса, N+1 точка с N координатами
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Точки симплекса")]
        public double[][] SimplexXs { get; private set; } = new[] { new double[1] };

        /// <summary>
        ///     Пошрешность расчета
        /// </summary>
        public override double GetEps()
        {
            var averageF = FN.Average();

            return Math.Sqrt(FN.Sum(f => FastMath.Pow2(f - averageF)) / FN.Length);
        }

        protected override void GetCurrentBest(out double[] x, out double f)
        {
            var fmin = double.MaxValue;
            var imin = 0;

            for (var i = 0; i < FN.Length; i++)
                if (FN[i] < fmin)
                {
                    fmin = FN[i];
                    imin = i;
                }

            x = ConvertBack(SimplexXs[imin]);
            f = fmin;
        }

        /// <summary>
        ///     Создает из точки X регулярный симплекс
        /// </summary>
        /// <param name="X"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        private double[][] GenerateRegularSimplexPoints(double[] X, double L)
        {
            var simplex = new double[N + 1][];

            for (var i = 0; i < simplex.Length; i++)
                simplex[i] = new double[N];

            var n = simplex.Length - 1;
            var d1 = L * (Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2));
            var d2 = L * (Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2));

            for (var i = 0; i < simplex[0].Length; i++) simplex[0][i] = X[i];

            for (var i = 1; i < simplex.Length; i++)
                for (var j = 0; j < simplex[i].Length; j++)
                    if (i != j)
                        simplex[i][j] = simplex[0][j] + d1;
                    else
                        simplex[i][j] = simplex[0][j] + d2;

            return simplex;
        }

        private double[] Reflection(double[] xc, int ih)
        {
            var xr = new double[xc.Length];
            for (var i = 0; i < SimplexXs[ih].Length; i++)
                xr[i] = (1.0 + ReflectionCoef) * xc[i] - SimplexXs[ih][i];

            return xr;
        }

        private double[] Stretch(double[] xc, double[] xr)
        {
            var xe = new double[xc.Length];
            for (var i = 0; i < xc.Length; i++)
                xe[i] = (1 - StretchCoef) * xc[i] + StretchCoef * xr[i];
            return xe;
        }

        private double[] Compression(double[] xc, double[] xh)
        {
            var xs = new double[xc.Length];
            for (var i = 0; i < xc.Length; i++)
                xs[i] = (1 - CompressionCoef) * xc[i] + CompressionCoef * xh[i];
            return xs;
        }


        /// <summary>
        /// </summary>
        /// <param name="ih">индекс точки с набольшим значением f</param>
        /// <param name="fh">набольшее значением f</param>
        /// <param name="ig">индекс точки с 2 по величине значением f</param>
        /// <param name="fg">2 по величине f</param>
        /// <param name="il">индекс точки с наименьшим значением f</param>
        /// <param name="fl">наименьшее значение f</param>
        private void GetSorded(
            out int ih, out double fh,
            out int ig, out double fg,
            out int il, out double fl) // Минимальный элемент массива и его индекс
        {
            ih = 0;
            ig = 0;
            il = 0;
            fh = double.MinValue;
            fg = double.MinValue;
            fl = double.MaxValue;

            for (var i = 0; i < SimplexXs.Length; i++)
            {
                if (FN[i] > fh)
                {
                    fh = FN[i];
                    ih = i;
                }

                if (FN[i] > fg && FN[i] < fh)
                {
                    fg = FN[i];
                    ig = i;
                }

                if (FN[i] < fl)
                {
                    fl = FN[i];
                    il = i;
                }
            }
        }

        private double[] GetGravityCenter(int ih) // Центр тяжести симплекса
        {
            var xc = new double[N];

            for (var i = 0; i < SimplexXs.Length; i++)
            {
                if (i == ih)
                    continue;

                for (var j = 0; j < SimplexXs[i].Length; j++) xc[j] += SimplexXs[i][j] / (SimplexXs.Length - 1);
            }

            return xc;
        }

        private void RestoreSimplex(double[] normalizedVect, double sideLength)
        {
            SimplexXs = GenerateRegularSimplexPoints(normalizedVect, sideLength);
            FN = Enumerable.Repeat(double.MaxValue, SimplexXs.Length).ToArray();

            for (var i = 0; i < SimplexXs.Length; i++)
            {
                FN[i] = GetF(SimplexXs[i]);
            }
        }

        private void Reduction(int il)
        {
            for (var i = 0; i < SimplexXs.Length; i++)
            {
                if (i == il)
                    continue;

                for (var j = 0; j < SimplexXs[i].Length; j++)
                    SimplexXs[i][j] = SimplexXs[il][j] + (SimplexXs[i][j] - SimplexXs[il][j]) / 2;

                FN[i] = GetF(SimplexXs[i]);
            }
        }

        // Выполняет поиск экстремума (минимума) функции F
        public override void Optimize(bool resumeCalculation = false)
        {
            var lastBestSolution = BestSolution.Item2;

            if (!resumeCalculation)
            {
                BestSolution = (new double[0], double.MaxValue);
                History.Clear();
                FCalculations = 0;
                StableIterations = 0;
                CurrentIteration = 0;
                RestoreSimplex(ConvertForward(x0), InitialSideLength);
            }

            while (CurrentIteration < MaxIterations)
            {
                CurrentIteration++;
                UpdateBestSolution();

                if (Math.Abs(lastBestSolution - BestSolution.Item2) <= Tolerance)
                    StableIterations++;
                else
                    StableIterations = 0;

                lastBestSolution = BestSolution.Item2;

                if (SideLength <= Tolerance || StableIterations >= StableIterationsToStop || GetEps() <= Tolerance || Worker != null && Worker.CancellationPending)
                {
                    WriteIterationResult(CurrentIteration);
                    Observer?.Invoke(BestSolution.Item1, BestSolution.Item2);
                    break;
                }

                if (IterationsToRestore > 0 && CurrentIteration % IterationsToRestore == 0)
                    RestoreSimplex(ConvertForward(BestSolution.Item1), SideLength);

                if (SaveResultsInFile && CurrentIteration % IterationsToWrite == 0)
                {
                    WriteIterationResult(CurrentIteration);
                    Observer?.Invoke(BestSolution.Item1, BestSolution.Item2);
                }

                GetSorded(
                    out var ih, out var fh,
                    out var ig, out var fg,
                    out var il, out var fl);

                var xc = GetGravityCenter(ih);

                var xr = Reflection(xc, ih);
                var fr = GetF(xr);

                if (fr < fl)
                {
                    var xe = Stretch(xc, xr);
                    var fe = GetF(xe);

                    if (fe < fr)
                    {
                        SimplexXs[ih] = xe.Copy();
                        FN[ih] = fe;
                    }
                    else
                    {
                        SimplexXs[ih] = xr.Copy();
                        FN[ih] = fr;
                    }

                    continue;
                }

                if (fr < fg)
                {
                    SimplexXs[ih] = xr.Copy();
                    FN[ih] = fr;
                    continue;
                }

                if (fh > fr)
                {
                    SimplexXs[ih] = xr.Copy();
                    FN[ih] = fr;
                }

                var xs = Compression(xc, SimplexXs[ih]);
                var fs = GetF(xs);

                if (fs < fh)
                {
                    SimplexXs[ih] = xs.Copy();
                    FN[ih] = fs;
                }
                else
                    Reduction(il);
            }
        }
    }
}