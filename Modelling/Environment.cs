using System;

namespace Modelling
{
    [Serializable]
    public class Environment
    {
        public double Temperature { get; }
        public double HeatTransfer { get; }
        public double Pressure { get; }
        public double K { get; }
        public double Density { get; }

        public Environment(double temperature, double heatTransfer, double pressure, double k, double density)
        {
            Temperature = temperature;
            HeatTransfer = heatTransfer;
            Pressure = pressure;
            K = k;
            Density = density;
        }
    }
}
