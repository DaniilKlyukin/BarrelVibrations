using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.OutletBallisticFolder
{
    [Serializable]
    [DataContract(Name = "Метеоданные")]
    public class MeteoData
    {
        /// <summary>
        /// Высота над уровнем моря, м
        /// </summary>
        [DataMember(Name = "Высота над уровнем моря, м")]
        public double Altitude { get; set; }

        /// <summary>
        /// Температура, К
        /// </summary>
        [DataMember(Name = "Температура, К")]
        public double Temperature { get; set; } = 288.2;

        /// <summary>
        /// Давление, Па
        /// </summary>
        [DataMember(Name = "Давление, Па")]
        public double Pressure { get; set; } = 101325;

        /// <summary>
        /// Скорость звука, м/с
        /// </summary>
        [DataMember(Name = "Скорость звука, м/с")]
        public double SoundSpeed { get; set; } = 340.3;

        /// <summary>
        /// Плотность, кг/м³
        /// </summary>
        [DataMember(Name = "Плотность, кг/м³")]
        public double Density { get; set; } = 1.23;

        public MeteoData(
            double altitude,
            double pressure,
            double temperature,
            double density,
            double soundSpeed)
        {
            Altitude = altitude;
            Temperature = temperature;
            Pressure = pressure;
            SoundSpeed = soundSpeed;
            Density = density;
        }
    }
}
