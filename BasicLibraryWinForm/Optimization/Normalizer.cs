namespace BasicLibraryWinForm.Optimization
{
    public class Normalizer
    {
        protected double[] mins { get; set; } = new double[0];
        protected double[] maxs { get; set; } = new double[0];

        public Normalizer()
        {

        }


        public Normalizer(double[] mins, double[] maxs)
        {
            this.mins = mins;
            this.maxs = maxs;

            if (mins.Length != maxs.Length)
                throw new ArgumentException();
        }

        public virtual double[] Normalize(double[] x)
        {
            if (x.Length != mins.Length || x.Length != maxs.Length) return x;

            var normalized = new double[x.Length];

            for (int i = 0; i < mins.Length; i++)
            {
                normalized[i] = (x[i] - mins[i]) / (maxs[i] - mins[i]);
            }

            return normalized;
        }

        public virtual double[] DeNormalize(double[] x)
        {
            if (x.Length != mins.Length || x.Length != maxs.Length) return x;

            var deNormalized = new double[x.Length];

            for (int i = 0; i < mins.Length; i++)
            {
                deNormalized[i] = x[i] * (maxs[i] - mins[i]) + mins[i];
            }

            return deNormalized;

        }
    }
}
