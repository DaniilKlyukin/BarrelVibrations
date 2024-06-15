using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.Solutions
{
    [Serializable]
    [DataContract(Name = "Результаты решения задачи теплопроводности")]
    public class TemperatureField
    {
        public void Clear()
        {
            LongitudinalEpureXs = Array.Empty<double>();
            RadialEpureRadiuses = Array.Empty<double>();

            LongitudinalEpureInnerSurface = Array.Empty<double>();
            LongitudinalEpureOuterSurface = Array.Empty<double>();
            RadialEpure = Array.Empty<double>();

            MaxTemperate.Clear();
            PointTemperate.Clear();

            AverageTemperatureInnerSurface.Clear();
            AverageTemperatureOuterSurface.Clear();
        }

        public void Add(
            double maxTemperature,
            double pointTemperature,
            double averageTemperatureInnerSurface,
            double averageTemperatureOuterSurface)
        {
            MaxTemperate.Add(maxTemperature);
            PointTemperate.Add(pointTemperature);

            AverageTemperatureInnerSurface.Add(averageTemperatureInnerSurface);
            AverageTemperatureOuterSurface.Add(averageTemperatureOuterSurface);
        }


        /// <summary>
        /// X продольной эпюры, м
        /// </summary>
        [DataMember(Name = "X продольной эпюры, м")]
        public double[] LongitudinalEpureXs { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Радиусы радиальной эпюры, м
        /// </summary>
        [DataMember(Name = "Радиусы радиальной эпюры, м")]
        public double[] RadialEpureRadiuses { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Продольная эпюра температуры внутренней поверхности ствола, К
        /// </summary>
        [DataMember(Name = "Продольная эпюра температуры внутренней поверхности ствола, К")]
        public double[] LongitudinalEpureInnerSurface { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Продольная эпюра температуры внешней поверхности ствола, К
        /// </summary>
        [DataMember(Name = "Продольная эпюра температуры внешней поверхности ствола, К")]
        public double[] LongitudinalEpureOuterSurface { get; set; } = Array.Empty<double>();
        /// <summary>
        /// Радиальная эпюра температуры, К
        /// </summary>
        [DataMember(Name = "Радиальная эпюра температуры, К")]
        public double[] RadialEpure { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Динамика максимальной температуры, К
        /// </summary>
        [DataMember(Name = "Динамика максимальной температуры, К")]
        public List<double> MaxTemperate { get; set; } = new();

        /// <summary>
        /// Динамика температуры в точке, К
        /// </summary>
        [DataMember(Name = "Динамика температуры в точке, К")]
        public List<double> PointTemperate { get; set; } = new();

        /// <summary>
        /// Средняя температура внутренней поверхности, К
        /// </summary>
        [DataMember(Name = "Средняя температура внутренней поверхности, К")]
        public List<double> AverageTemperatureInnerSurface { get; set; } = new();

        /// <summary>
        /// Средняя температура внешней поверхности, К
        /// </summary>
        [DataMember(Name = "Средняя температура внешней поверхности, К")]
        public List<double>  AverageTemperatureOuterSurface { get; set; } = new();
    }
}
