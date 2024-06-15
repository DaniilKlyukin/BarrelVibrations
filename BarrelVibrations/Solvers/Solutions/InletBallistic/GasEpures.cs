using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.Solutions.InletBallistic
{
    [Serializable]
    [DataContract(Name = "Данные эпюр распределения параметров газа при выстреле")]
    public class GasEpures
    {
        public void Clear(int arrayLength)
        {
            Pressures = new double[arrayLength];
            Temperatures = new double[arrayLength];
            HeatTransfers = new double[arrayLength];
            Densities = new double[arrayLength];
            GasVelocities = new double[arrayLength];
        }

        /// <summary>
        /// Давление, Па
        /// </summary>
        [DataMember(Name = "Давление, Па")]
        public double[] Pressures { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Температура, К
        /// </summary>
        [DataMember(Name = "Температура, К")]
        public double[] Temperatures { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Коэффициент теплопередачи, Вт/(м²K)
        /// </summary>
        [DataMember(Name = "Коэффициент теплопередачи, Вт/(м²K)")]
        public double[] HeatTransfers { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Плотность, кг/м³
        /// </summary>
        [DataMember(Name = "Плотность, кг/м³")]
        public double[] Densities { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Скорость газа, м/с
        /// </summary>
        [DataMember(Name = "Скорость газа, м/с")]
        public double[] GasVelocities { get; set; } = Array.Empty<double>();
    }
}
