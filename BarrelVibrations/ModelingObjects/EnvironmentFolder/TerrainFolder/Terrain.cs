using BasicLibraryWinForm;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder
{
    [DataContract(Name = "Карта высот")]
    [Serializable]
    public class Terrain
    {
        [JsonConstructor]
        public Terrain(
            double[] xs,
            double[] zs,
            double[,] altitudeMap)
        {
            Xs = xs;
            Zs = zs;
            AltitudeMap = altitudeMap;

            AltitudeSpline = new BiLinearSpline(
                Xs,
                Zs,
                AltitudeMap);
        }

        public Terrain()
        {
            AltitudeSpline = new BiLinearSpline(
                Xs,
                Zs,
                AltitudeMap);
        }

        [DataMember(Name = "X, м")]
        public double[] Xs { get; set; } = new double[] { -1e5, 1e5 };

        [DataMember(Name = "Z, м")]
        public double[] Zs { get; set; } = new double[] { -1e5, 1e5 };


        [DataMember(Name = "Карта высот, м")]
        public double[,] AltitudeMap { get; set; } = new double[,]
        {
            { 0, 0 },
            { 0, 0 }
        };

        [IgnoreDataMember]
        public BiLinearSpline AltitudeSpline { get; set; }


        public double GetAltitude(double x0, double z0)
        {
            return AltitudeSpline.Interpolate(x0, z0);
        }

        public override string ToString()
        {
            return $"Карта высот [{AltitudeMap.GetLength(0)} x {AltitudeMap.GetLength(1)}]";
        }
    }
}
