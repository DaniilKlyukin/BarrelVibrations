using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.ShotFolder
{
    [Serializable]
    [DataContract(Name = "Выстрел")]
    public class Shot
    {
        [DataMember(Name = "Момент времени начала выстрела, сек")]
        public double ShotTimeMoment { get; set; }

        [DataMember(Name = "Индекс боеприпаса")]
        public int AmmoIndex { get; set; }
    }
}
