using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.Solutions
{
    /// <summary>
    /// Результаты решения задачи внешней баллистики
    /// </summary>
    [DataContract(Name = "Внешняя баллистика")]
    public class OutletBallistic
    {
        /// <summary>
        /// Моменты времени, сек
        /// </summary>
        [DataMember(Name = "Моменты времени, сек")]
        public IList<double> TimeMoments { get; }

        /// <summary>
        /// Координата X снаряда, м
        /// </summary>
        [DataMember(Name = "Координата X снаряда, м")]
        public IList<double> Xs { get; }

        /// <summary>
        /// Координата Y снаряда, м
        /// </summary>
        [DataMember(Name = "Координата Y снаряда, м")]
        public IList<double> Ys { get; }

        /// <summary>
        /// Координата Z снаряда, м
        /// </summary>
        [DataMember(Name = "Координата Z снаряда, м")]
        public IList<double> Zs { get; }

        /// <summary>
        /// Скорость снаряда, м/с
        /// </summary>
        [DataMember(Name = "Скорость снаряда, м/с")]
        public IList<double> Velocities { get; }

        /// <summary>
        /// Горизонтальный угол наклона траектории снаряда, °
        /// </summary>
        [DataMember(Name = "Горизонтальный угол наклона траектории снаряда, °")]
        public IList<double> HorizontalMovementAngles { get; }

        /// <summary>
        /// Вертикальный угол наклона траектории снаряда, °
        /// </summary>
        [DataMember(Name = "Вертикальный угол наклона траектории снаряда, °")]
        public IList<double> VerticalMovementAngles { get; }

        /// <summary>
        /// Отклонение угла наклона снаряда от траектории по горизонтали, °
        /// </summary>
        [DataMember(Name = "Отклонение угла наклона снаряда от траектории по горизонтали, °")]
        public IList<double> HorizontalDeltaAngles { get; }

        /// <summary>
        /// Отклонение угла наклона снаряда от траектории по вертикали, °
        /// </summary>
        [DataMember(Name = "Отклонение угла наклона снаряда от траектории по вертикали, °")]
        public IList<double> VerticalDeltaAngles { get; }

        /// <summary>
        /// Горизонтальная угловая скорость снаряда, рад/с
        /// </summary>
        [DataMember(Name = "Горизонтальная угловая скорость снаряда, рад/с")]
        public IList<double> HorizontalAngularVelocities { get; }

        /// <summary>
        /// Вертикальная угловая скорость снаряда, рад/с
        /// </summary>
        [DataMember(Name = "Вертикальная угловая скорость снаряда, рад/с")]
        public IList<double> VerticalAngularVelocities { get; }

        /// <summary>
        /// Угловая скорость, рад/с
        /// </summary>
        [DataMember(Name = "Угловая скорость, рад/с")]
        public IList<double> AngularVelocities { get; }

        /// <summary>
        /// Критерий устойчивости
        /// </summary>
        [DataMember(Name = "Критерий устойчивости")]
        public IList<double> StabilityCriterion { get; }

    [JsonConstructor]
        public OutletBallistic(IList<double> timeMoments,
            IList<double> xs,
            IList<double> ys,
            IList<double> zs,
            IList<double> velocities,
            IList<double> horizontalMovementAngles,
            IList<double> verticalMovementAngles,
            IList<double> horizontalDeltaAngles,
            IList<double> verticalDeltaAngles,
            IList<double> horizontalAngularVelocities,
            IList<double> verticalAngularVelocities,
            IList<double> angularVelocities,
            IList<double> stabilityCriterion
            )
        {
            TimeMoments = timeMoments;
            Xs = xs;
            Ys = ys;
            Zs = zs;
            Velocities = velocities;
            HorizontalMovementAngles = horizontalMovementAngles;
            VerticalMovementAngles = verticalMovementAngles;
            HorizontalDeltaAngles = horizontalDeltaAngles;
            VerticalDeltaAngles = verticalDeltaAngles;
            HorizontalAngularVelocities = horizontalAngularVelocities;
            VerticalAngularVelocities = verticalAngularVelocities;
            AngularVelocities = angularVelocities;
            StabilityCriterion = stabilityCriterion;
        }

        public OutletBallistic()
        {
            TimeMoments = new List<double>();
            Xs = new List<double>();
            Ys = new List<double>();
            Zs = new List<double>();
            Velocities = new List<double>();
            HorizontalMovementAngles = new List<double>();
            VerticalMovementAngles = new List<double>();
            AngularVelocities = new List<double>();
            HorizontalDeltaAngles = new List<double>();
            VerticalDeltaAngles = new List<double>();
            HorizontalAngularVelocities = new List<double>();
            VerticalAngularVelocities = new List<double>();
            AngularVelocities = new List<double>();
            StabilityCriterion = new List<double>();
        }
    }
}
