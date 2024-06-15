namespace Modelling.Loads
{
    public class PressureTimeLoad
    {
        public double TimeMoment { get; set; }
        public double Pressure { get; set; }

        public PressureTimeLoad(double timeMoment, double pressure)
        {
            TimeMoment = timeMoment;
            Pressure = pressure;
        }

        public override string ToString()
        {
            return $"{TimeMoment*1e3:0.00} мс, {Pressure/1e6:0.0} МПа";
        }
    }
}
