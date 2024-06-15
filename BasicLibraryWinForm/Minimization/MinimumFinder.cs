using GeneticSharp;

namespace BasicLibraryWinForm.Minimization
{
    public static class MinimumFinder
    {
        public static double[] GetMinimumGenetic(IFitness fitness, ChromosomeBase chromosome, out double bestFitness, int minSize = 100, int maxSize = 200, int generations = 10000)
        {
            var cores = Environment.ProcessorCount;
            var tasks = new Task[cores];

            var genetics = new GeneticAlgorithm[cores];

            for (var i = 0; i < tasks.Length; i++)
            {
                genetics[i] = createAlgorithm(fitness, chromosome, minSize, maxSize, generations);
            }

            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(genetics[i].Start);
            }

            Task.WaitAll(tasks);

            bestFitness = genetics.Select(g => g.BestChromosome.Fitness ?? double.MinValue).Max();

            var chromosomes = genetics.Select(g => g.BestChromosome).ToArray();
            return chromosomes.OrderByDescending(ch => ch.Fitness).ToArray().First().GetGenes().Select(g => (double)g.Value).ToArray();
        }

        private static GeneticAlgorithm createAlgorithm(IFitness fitness, ChromosomeBase chromosome, int minSize = 100, int maxSize = 200, int generations = 10000)
        {
            var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new UniformMutation();

            var population = new Population(minSize, maxSize, chromosome);

            return new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new GenerationNumberTermination(generations)
            };
        }

        public static double GetRootNsection(
            Func<double, double> function,
            double min,
            double max,
            double funcTolerance = 1e-10,
            double paramTolerance = 1e-10,
            int coresCount = 0,
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            Func<int, double, double, bool> observer = null)
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        {
            var cores = coresCount < 1 ? Environment.ProcessorCount : coresCount;

            Task<double>[] taskArray ={
                Task<double>.Factory.StartNew(() => function(min)),
                Task<double>.Factory.StartNew(() => function(max))};

            var fmin = taskArray[0].Result;
            var fmax = taskArray[1].Result;

            if (Math.Abs(max - min) < paramTolerance || Math.Abs(fmin) < funcTolerance || Math.Abs(fmax) < funcTolerance)
                return (max + min) / 2;

            if (Math.Sign(fmin) == Math.Sign(fmax))
            {
                if (Math.Abs(fmin) < Math.Abs(fmax))
                    return min;
                else if (Math.Abs(fmin) > Math.Abs(fmax))
                    return max;
                else
                    return (max + min) / 2;
            }

            var n = cores + 1;
            var t_array = new double[n + 1];
            t_array[0] = min;
            t_array[n] = max;

            var f_array = new double[n + 1];
            f_array[0] = fmin;
            f_array[n] = fmax;

            var iteration = 0;

            while (true)
            {
                iteration++;
                var step = (t_array[n] - t_array[0]) / n;

                var isStop = observer?.Invoke(
                    iteration,
                    (t_array[n] + t_array[0]) / 2,
                    (f_array[n] + f_array[0]) / 2);

                var tasks = new Task<double>[cores];

                for (var i = 1; i < n; i++)
                {
                    var t = t_array[0] + i * step;
                    t_array[i] = t;
                    tasks[i - 1] = Task<double>.Factory.StartNew(() => function(t));
                }

                if (Math.Abs(t_array[n] - t_array[0]) < paramTolerance || isStop.HasValue && isStop.Value)
                {
                    var results = new Dictionary<double, double>();

                    for (var i = 1; i < n; i++)
                    {
                        results.Add(t_array[i], tasks[i - 1].Result);
                    }

                    return results.OrderBy(pair => pair.Value).First().Key;
                }

                for (var i = 1; i < n; i++)
                {
                    f_array[i] = tasks[i - 1].Result;

                    if (Math.Abs(f_array[i]) < funcTolerance)
                        return t_array[i];
                }

                for (var i = 0; i < n; i++)
                {
                    if (Math.Sign(f_array[i]) != Math.Sign(f_array[i + 1]))
                    {
                        t_array[0] = t_array[i];
                        t_array[n] = t_array[i + 1];
                        f_array[0] = f_array[i];
                        f_array[n] = f_array[i + 1];
                        break;
                    }
                }
            }
        }

        public static (double, double) GetMinimumFullSearch(Func<double, double> function, double left, double right, double tolerance = 1e-8, int maxIterations = 1000)
        {
            var n = Environment.ProcessorCount;

            var dx = (right - left) / n;

            var tasks = new Task<(double, double)>[n];
            var iteration = 0;

            var yMinOld = double.MaxValue;

            while (true)
            {
                for (var i = 0; i < n; i++)
                {
                    var x0 = left + i * dx;
                    var x1 = (i + 1) * dx;
                    tasks[i] = Task.Run(() => getMinimumFullSearch(function, x0, x1));
                }

                Task.WaitAll(tasks);

                var (xMin, yMin) = tasks.First().Result;
                var t = 0;

                for (var i = 1; i < tasks.Length; i++)
                {
                    var (x, y) = tasks[i].Result;

                    if (y < yMin)
                    {
                        yMin = y;
                        xMin = x;
                        t = i;
                    }
                }

                left = Math.Max(xMin - dx, left);
                right = Math.Min(xMin + dx, right);

                dx = (right - left) / n;

                if (iteration >= maxIterations || Math.Abs(yMin - yMinOld) <= tolerance)
                {
                    return (xMin, yMin);
                }

                yMinOld = yMin;

                iteration++;
            }
        }

        private static (double, double) getMinimumFullSearch(Func<double, double> function, double left, double right)
        {
            var n = 20;

            var dx = (right - left) / n;
            var x = left;
            var y = function(x);

            for (var i = 0; i <= n; i++)
            {
                var x1 = left + i * dx;
                var y1 = function(x1);

                if (y1 < y)
                {
                    x = x1;
                    y = y1;
                }
            }

            return (x, y);
        }
    }
}
