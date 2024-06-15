using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.Solutions
{
    [Serializable]
    [DataContract(Name = "Дульные параметры")]
    public class ShotParameters
    {
        /// <summary>
        /// Время выстрела, сек
        /// </summary>
        [DataMember(Name = "Время выстрела, сек")]
        public double Time { get; }

        /// <summary>
        /// Максимальное давление на снаряд, Па
        /// </summary>
        [DataMember(Name = "Максимальное давление на снаряд, Па")]
        public double ProjetileMaxPressure { get; }

        /// <summary>
        /// Максимальное давление, Па
        /// </summary>
        [DataMember(Name = "Максимальное давление, Па")]
        public double MaxPressure { get; }

        /// <summary>
        /// Скорость снаряда, м/с
        /// </summary>
        [DataMember(Name = "Скорость снаряда, м/с")]
        public double ProjectileSpeed { get; }

        /// <summary>
        /// Смещение дульного среза по Oy, м
        /// </summary>
        [DataMember(Name = "Смещение дульного среза по Oy, м")]
        public double BarrelEndY { get; }

        /// <summary>
        /// Смещение дульного среза по Oz, м
        /// </summary>
        [DataMember(Name = "Смещение дульного среза по Oz, м")]
        public double BarrelEndZ { get; }

        /// <summary>
        /// Горизонтальный угол вылета, град
        /// </summary>
        [DataMember(Name = "Горизонтальный угол вылета, град")]
        public double HorizontalAngle { get; }

        /// <summary>
        /// Вертикальный угол вылета, град
        /// </summary>
        [DataMember(Name = "Вертикальный угол вылета, град")]
        public double VerticalAngle { get; }

        /// <summary>
        /// Горизонтальный угол нутации, рад
        /// </summary>
        [DataMember(Name = "Горизонтальный угол нутации, рад")]
        public double HorizontalRotationAngle { get; }

        /// <summary>
        /// Вертикальный угол нутации, рад
        /// </summary>
        [DataMember(Name = "Вертикальный угол нутации, рад")]
        public double VerticalRotationAngle { get; }

        /// <summary>
        /// Скорость дульного среза, м/с
        /// </summary>
        [DataMember(Name = "Скорость дульного среза, м/с")]
        public double BarrelEndVelocity { get; }

        [JsonConstructor]
        public ShotParameters(
            double time,
            double projetileMaxPressure,
                              double maxPressure,
                              double projectileSpeed,
                              double barrelEndY,
                              double barrelEndZ,
                              double horizontalAngle,
                              double verticalAngle,
                              double horizontalRotationAngle,
                              double verticalRotationAngle,
                              double barrelEndSpeed)
        {
            Time = time;
            ProjetileMaxPressure = projetileMaxPressure;
            MaxPressure = maxPressure;

            ProjectileSpeed = projectileSpeed;
            BarrelEndY = barrelEndY;
            BarrelEndZ = barrelEndZ;
            HorizontalAngle = horizontalAngle;
            VerticalAngle = verticalAngle;
            HorizontalRotationAngle = horizontalRotationAngle;
            VerticalRotationAngle = verticalRotationAngle;
            BarrelEndVelocity = barrelEndSpeed;
        }

        public ShotParameters() { }
    }
}
