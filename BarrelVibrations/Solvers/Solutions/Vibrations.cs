using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.Solutions
{
    [Serializable]
    [DataContract(Name = "Данные решения задачи колебаний")]
    public class Vibrations
    {
        public void Add(
            double barrelEndX, double barrelEndY, double barrelEndZ,
            double barrelEndVelocityX, double barrelEndVelocityY, double barrelEndVelocityZ,
            double barrelHorizontalAngle, double barrelVerticalAngle,
            double barrelXLocation,
            double systemXLocation,
            double pointRVibration)
        {
            BarrelEndXs.Add(barrelEndX);
            BarrelEndYs.Add(barrelEndY);
            BarrelEndZs.Add(barrelEndZ);

            BarrelEndVelocityXs.Add(barrelEndVelocityX);
            BarrelEndVelocityYs.Add(barrelEndVelocityY);
            BarrelEndVelocityZs.Add(barrelEndVelocityZ);

            BarrelHorizontalAngles.Add(barrelHorizontalAngle);
            BarrelVerticalAngles.Add(barrelVerticalAngle);

            BarrelXLocations.Add(barrelXLocation);
            SystemXLocations.Add(systemXLocation);

            PointRVibrations.Add(pointRVibration);
        }

        public void Clear(int xLength)
        {
            BarrelEndXs.Clear();
            BarrelEndYs.Clear();
            BarrelEndZs.Clear();

            BarrelEndVelocityXs.Clear();
            BarrelEndVelocityYs.Clear();
            BarrelEndVelocityZs.Clear();

            BarrelHorizontalAngles.Clear();
            BarrelVerticalAngles.Clear();

            BarrelXLocations.Clear();
            SystemXLocations.Clear();

            PointRVibrations.Clear();
            EpureInnerRVibrations = new double[xLength];
        }

        /// <summary>
        /// Перемещения дульного среза по Ox в ЛСК, м
        /// </summary>
        [DataMember(Name = "Перемещения дульного среза по Ox в ЛСК, м")]
        public List<double> BarrelEndXs { get; set; } = new();

        /// <summary>
        /// Перемещения дульного среза по Oy в ЛСК, м
        /// </summary>
        [DataMember(Name = "Перемещения дульного среза по Oy в ЛСК, м")]
        public List<double> BarrelEndYs { get; set; } = new();

        /// <summary>
        /// Перемещения дульного среза по Oz в ЛСК, м
        /// </summary>
        [DataMember(Name = "Перемещения дульного среза по Oz в ЛСК, м")]
        public List<double> BarrelEndZs { get; set; } = new();

        /// <summary>
        /// Скорость дульного среза по Ox, м/с
        /// </summary>
        [DataMember(Name = "Скорость дульного среза по Ox, м/с")]
        public List<double> BarrelEndVelocityXs { get; set; } = new();

        /// <summary>
        /// Скорость дульного среза по Oy, м/с
        /// </summary>
        [DataMember(Name = "Скорость дульного среза по Oy, м/с")]
        public List<double> BarrelEndVelocityYs { get; set; } = new();

        /// <summary>
        /// Скорость дульного среза по Oz, м/с
        /// </summary>
        [DataMember(Name = "Скорость дульного среза по Oz, м/с")]
        public List<double> BarrelEndVelocityZs { get; set; } = new();

        /// <summary>
        /// Углы наклона дульного среза по горизонтали, град
        /// </summary>
        [DataMember(Name = "Углы наклона дульного среза по горизонтали")]
        public List<double> BarrelHorizontalAngles { get; set; } = new();

        /// <summary>
        /// Углы наклона дульного среза по вертикали, град
        /// </summary>
        [DataMember(Name = "Углы наклона дульного среза по вертикали")]
        public List<double> BarrelVerticalAngles { get; set; } = new();

        /// <summary>
        /// Положение ствола по X, м
        /// </summary>
        [DataMember(Name = "Положение ствола по X, м")]
        public List<double> BarrelXLocations { get; set; } = new();

        /// <summary>
        /// Положение установки по X, м
        /// </summary>
        [DataMember(Name = "Положение установки по X, м")]
        public List<double> SystemXLocations { get; set; } = new();

        /// <summary>
        /// Радиальные колебания в точке, м
        /// </summary>
        [DataMember(Name = "Радиальные колебания в точке, м")]
        public List<double> PointRVibrations { get; set; } = new();

        /// <summary>
        /// Эпюра радиальных колебаний, м
        /// </summary>
        [DataMember(Name = "Эпюра радиальных колебаний, м")]
        public double[] EpureInnerRVibrations { get; set; } = new double[0];
    }
}
