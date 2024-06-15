using System;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PointFolder;

namespace Modelling.Geometry
{
    [Serializable]
    public class PolygonObject : IGeometryObject
    {
        [DataMember(Name = "Узлы")]
        public Point[] Points { get; }

        public PolygonObject(Point[] points)
        {
            Points = points;
        }

        [DataMember(Name = "Операция")]
        public Operation Operation { get; }

        [DataMember(Name = "Тип")] public GeometryType Type { get; } = GeometryType.Polygon;
    }
}
