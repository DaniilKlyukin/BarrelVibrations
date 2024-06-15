using BasicLibraryWinForm;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder
{
    [DataContract(Name = "Карта ветра")]
    [Serializable]
    public class Wind
    {
        [JsonConstructor]
        public Wind(
            double[] xs,
            double[] ys,
            double[] zs,
            double[,,] windVelocity,
            double[,,] windAngle,
            double[,,] risingWindVelocity)
        {
            Xs = xs;
            Ys = ys;
            Zs = zs;
            WindVelocityMap = windVelocity;
            WindAngleMap = windAngle;
            RisingWindVelocityMap = risingWindVelocity;

            WindVelocitySpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                WindVelocityMap);

            WindAngleSpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                WindAngleMap);

            RisingWindVelocitySpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                RisingWindVelocityMap);
        }

        public Wind()
        {
            WindVelocitySpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                WindVelocityMap);

            WindAngleSpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                WindAngleMap);

            RisingWindVelocitySpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                RisingWindVelocityMap);
        }

        public void Update()
        {
            WindVelocitySpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                WindVelocityMap);

            WindAngleSpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                WindAngleMap);

            RisingWindVelocitySpline = new TriLinearSpline(
                Xs,
                Ys,
                Zs,
                RisingWindVelocityMap);
        }

        [DataMember(Name = "X, м")]
        public double[] Xs { get; set; } = new double[] { -1e5, 1e5 };

        [DataMember(Name = "Y, м")]
        public double[] Ys { get; set; } = new double[] { 0, 2e4 };

        [DataMember(Name = "Z, м")]
        public double[] Zs { get; set; } = new double[] { -1e5, 1e5 };


        /// <summary>
        /// Карта скорости ветра, м/с
        /// </summary>
        [DataMember(Name = "Карта скорости ветра, м/с")]
        public double[,,] WindVelocityMap { get; set; } = new double[,,]
        {
            {
                {0, 0 },
                {0, 0 }
            },
            {
                {0, 0 },
                {0, 0 }
            },
        };

        /// <summary>
        /// Карта угла ветра, рад
        /// </summary>
        [DataMember(Name = "Карта угла ветра, рад")]
        public double[,,] WindAngleMap { get; set; } = new double[,,]
{
            {
                {0, 0 },
                {0, 0 }
            },
            {
                {0, 0 },
                {0, 0 }
            },
};

        /// <summary>
        /// Карта скорости восходящего ветра, м/с
        /// </summary>
        [DataMember(Name = "Карта скорости восходящего ветра, м/с")]
        public double[,,] RisingWindVelocityMap { get; set; } = new double[,,]
{
            {
                {0, 0 },
                {0, 0 }
            },
            {
                {0, 0 },
                {0, 0 }
            },
};

        [IgnoreDataMember]
        public TriLinearSpline WindVelocitySpline { get; set; }
        [IgnoreDataMember]
        public TriLinearSpline WindAngleSpline { get; set; }
        [IgnoreDataMember]
        public TriLinearSpline RisingWindVelocitySpline { get; set; }


        public double GetWindVelocity(double x0, double y0, double z0)
        {
            return WindVelocitySpline.Interpolate(x0, y0, z0);
        }

        public double GetWindAngle(double x0, double y0, double z0)
        {
            return WindAngleSpline.Interpolate(x0, y0, z0);
        }

        public double GetRisingWindVelocity(double x0, double y0, double z0)
        {
            return RisingWindVelocitySpline.Interpolate(x0, y0, z0);
        }

        public override string ToString()
        {
            return $"Карта ветра [{WindVelocityMap.GetLength(0)} x {WindVelocityMap.GetLength(1)} x {WindVelocityMap.GetLength(2)}]";
        }
    }
}
