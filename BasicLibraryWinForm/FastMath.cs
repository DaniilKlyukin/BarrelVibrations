namespace BasicLibraryWinForm
{
    public static class FastMath
    {
        public static double Pow2(double value)
        {
            return value * value;
        }

        public static double Pow3(double value)
        {
            return value * value * value;
        }

        public static double Pow4(double value)
        {
            return value * value * value * value;
        }

        public static double MinMax(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
