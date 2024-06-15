namespace BasicLibraryWinForm
{
    public partial class OdeSolver
    {
        public class OdeSolution
        {
            public int Length => Values.Length;
            public double[] Values { get; }

            public OdeSolution(double[] values)
            {
                Values = values;
            }

            public OdeSolution(int length)
            {
                Values = new double[length];
            }

            public double this[int i]
            {
                get => Values[i];
                set => Values[i] = value;
            }

            public static OdeSolution operator +(OdeSolution s1, OdeSolution s2)
            {
                if (s1.Length != s2.Length)
                    throw new ArgumentException();

                var result = new OdeSolution(s1.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = s1[i] + s2[i];
                }

                return result;
            }

            public static OdeSolution operator -(OdeSolution s1, OdeSolution s2)
            {
                if (s1.Length != s2.Length)
                    throw new ArgumentException();

                var result = new OdeSolution(s1.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = s1[i] - s2[i];
                }

                return result;
            }

            public static OdeSolution operator *(OdeSolution s1, double c)
            {
                var result = new OdeSolution(s1.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = c * s1[i];
                }

                return result;
            }

            public static OdeSolution operator /(OdeSolution s1, double c)
            {
                var result = new OdeSolution(s1.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = s1[i] / c;
                }

                return result;
            }

            public static OdeSolution operator *(double c, OdeSolution s1)
            {
                return s1 * c;
            }
        }

    }

}
