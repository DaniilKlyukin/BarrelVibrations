using BasicLibraryWinForm.PropertiesTemplates;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace BasicLibraryWinForm.Optimization
{
    [Serializable]
    [DataContract(Name = "Оптимизатор")]
    public abstract class Optimizer
    {
        [Browsable(false)]
        public delegate (bool, double) Optimization(double[] x);

        protected const ushort CATEGORIESCOUNT = 4;

        protected readonly string fileName = "optimization.txt";

        /// <summary>
        ///     Требуемая точность
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Расчет", 1, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Требуемая точность")]
        [DataMember(Name = "Требуемая точность")]
        public double Tolerance { get; set; } = 1e-8;

        /// <summary>
        ///     Штрафное значение
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Расчет", 1, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Штрафное значение")]
        [DataMember(Name = "Штрафное значение")]
        public double Penalty { get; set; } = 1000;

        /// <summary>
        ///     Ограничения
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Ограничения", 2, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Ограничения")]
        [DataMember(Name = "Ограничения")]
        public virtual OptimizationConstraint Constraint { get; set; } = new();

        /// <summary>
        ///     Итераций для записи результата
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Сохранение", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Итераций для записи результата")]
        [DataMember(Name = "Итераций для записи результата")]
        public int IterationsToWrite { get; set; } = 1;

        /// <summary>
        ///     Сохранять результаты в файл
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Сохранение", 3, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Сохранять результаты в файл")]
        [DataMember(Name = "Сохранять результаты в файл")]
        public bool SaveResultsInFile { get; set; }

        /// <summary>
        ///     Лимит итераций
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Дополнительно", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Лимит итераций")]
        [DataMember(Name = "Лимит итераций")]
        public int MaxIterations { get; set; } = 1000;

        /// <summary>
        ///     Итераций до остановки если значение функции не меняется
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Дополнительно", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Итераций до остановки после установления")]
        [DataMember(Name = "Итераций до остановки после установления")]
        public int StableIterationsToStop { get; set; } = 50;

        /// <summary>
        ///     Вычислений функции
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [CustomSortedCategory("Дополнительно", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Вычислений функции")]
        [DataMember(Name = "Вычислений функции")]
        public int FCalculations { get; protected set; }

        /// <summary>
        ///     История оптимизации
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [CustomSortedCategory("Дополнительно", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("История оптимизации")]
        [DataMember(Name = "История оптимизации")]
        public List<(int, double)> History { get; private set; } = new List<(int, double)>();

        [Browsable(true)]
        [ReadOnly(true)]
        [CustomSortedCategory("Дополнительно", 4, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Алгоритм оптимизации")]
        [DataMember(Name = "Алгоритм оптимизации")]
        [TypeConverter(typeof(MyEnumConverter))]
        public abstract OptimizationAlgorithm Algorithm { get; }

        /// <summary>
        ///     Текущая итерация
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Текущая итерация")]
        public int CurrentIteration { get; protected set; }

        /// <summary>
        ///     Итераций без изменения значения функции
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Итераций без изменения значения функции")]
        public int StableIterations { get; protected set; }

        /// <summary>
        ///     Начальная точка
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Начальная точка")]
        public double[] x0 { get; set; } = new double[0];

        /// <summary>
        ///     Лучшее решение за время оптимизации
        /// </summary>
        [Browsable(false)]
        [DataMember(Name = "Лучшее решение за время оптимизации")]
        public (double[], double) BestSolution { get; protected set; } = (new double[0], double.MaxValue);

        /// <summary>
        ///     N - число аргументов функции (размерность пространства)
        /// </summary>
        [Browsable(false)]
        [IgnoreDataMember]
        public int N => x0.Length;

        [Browsable(false)]
        [IgnoreDataMember]
        public Action<double[], double> Observer { get; set; } = (_, _) => { };

        [Browsable(false)]
        [IgnoreDataMember]
        public BackgroundWorker Worker { get; set; } = new BackgroundWorker();

        [Browsable(false)]
        [IgnoreDataMember]
        public bool Initialized => OptimizationFunction != null && x0.Any();

        [IgnoreDataMember]
        [Browsable(false)]
        public Optimization? OptimizationFunction { get; set; }

        [IgnoreDataMember]
        [Browsable(false)]
        public Normalizer? Normalizer { get; set; }

        /// <summary>
        ///     Пошрешность расчета
        /// </summary>
        public abstract double GetEps();

        /// <summary>
        ///     Перевести произвольный вектор x к интервалу от 0 до 1
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected double[] ConvertForward(double[] x)
        {
            return Normalizer?.Normalize(x) ?? x;
        }

        /// <summary>
        ///     Перевести вектор x из интервала от 0 до 1 к исходному
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected double[] ConvertBack(double[] x)
        {
            return Normalizer?.DeNormalize(x) ?? x;
        }

        protected virtual void WriteIterationResult(int iteration)
        {
            var sb = new StringBuilder();

            var (x, f) = BestSolution;

            sb.Append($"\nИтерация:\t{iteration}");
            sb.Append($"\tВычислений F:\t{FCalculations}");
            sb.Append($"\tX:\t{string.Join("\t", x)}");
            sb.Append($"\tF(X):\t{f}");
            sb.Append($"\te:\t{GetEps()}");

            File.AppendAllText(fileName, sb.ToString());
        }

        protected abstract void GetCurrentBest(out double[] x, out double f);

        protected void UpdateBestSolution()
        {
            GetCurrentBest(out var x, out var f);

            var (_, fb) = BestSolution;

            if (f < fb)
            {
                BestSolution = (x, f);

                History.Add((CurrentIteration, f));
            }
        }

        public abstract void Optimize(bool resumeCalculation = false);

        protected double GetF(double[] localX)
        {
            if (OptimizationFunction == null)
                return 0;

            var (IsValid, f) = OptimizationFunction(ConvertBack(localX));
            FCalculations++;
            return IsValid ? f : f + Penalty;
        }
    }
}
