namespace Modelling.Loads
{
    public class PressureLoadsInputFile
    {
        public double[] X { get; set; }
        public double[] Y { get; set; }
        public double[] TimeMoments { get; set; }
        public double[,] Pressures { get; set; }

        public PressureLoadsInputFile(double[] x, double[] y, double[] timeMoments, double[,] pressures)
        {
            X = x;
            Y = y;
            TimeMoments = timeMoments;
            Pressures = pressures;
        }
    }
}
