namespace BarrelVibrations.ViewForms.Common
{
    public class TrackData
    {
        public int TimeIndex { get; }
        public double Time { get; }
        public string MeasureUnit { get; }
        public double[] ValuesDestribution { get; }

        public TrackData(
            int timeIndex,
            double time,
            double[] valuesDestribution,
            string measureUnit)
        {
            TimeIndex = timeIndex;
            Time = time;
            ValuesDestribution = valuesDestribution;
            MeasureUnit = measureUnit;
        }
    }
}
