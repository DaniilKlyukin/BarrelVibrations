namespace BarrelVibrations.Solvers
{
    public class SolverStatus
    {
        public double Percentage { get; set; }
        public double Time { get; set; }

        public SolverStatus(double percentage, double time)
        {
            Percentage = percentage;
            Time = time;
        }
    }
}
