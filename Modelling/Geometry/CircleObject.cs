using System;
using System.Runtime.Serialization;
using BasicLibraryWinForm.PointFolder;

namespace Modelling.Geometry
{
    [Serializable]
    [DataContract(Name = "Окружность")]
    public class CircleObject : IGeometryObject
    {
        [DataMember(Name = "Центр")]
        public Point Center { get; }
        [DataMember(Name = "Радиус")]
        public double Radius { get; }
        [DataMember(Name = "Операция")]
        public Operation Operation { get; }
        [DataMember(Name = "Тип")] public GeometryType Type { get; } = GeometryType.Circle;

        public CircleObject(Point center, double radius, Operation operation)
        {
            Center = center;
            Radius = radius;
            Operation = operation;
        }

        public override string ToString()
        {
            return $"Окружность X:{Center.X:0.000} Y:{Center.Y:0.000} R:{Radius:0.000} {Operation}";
        }
    }
}
