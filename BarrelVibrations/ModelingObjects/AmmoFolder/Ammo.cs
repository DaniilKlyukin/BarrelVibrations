using BarrelVibrations.ModelingObjects.MissileFolder;
using InletBallisticLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BarrelVibrations.ModelingObjects.AmmoFolder
{
    [Serializable]
    [DataContract(Name = "Боеприпас")]
    public class Ammo
    {
        [DataMember(Name = "Название")]
        public string Name { get; set; } = "Боеприпас";

        [DataMember(Name = "Снаряд")]
        public Missile Missile { get; set; } = new();

        [DataMember(Name = "Пороховой заряд")]
        public PowderCharge PowderCharge { get; set; } = new();

        [IgnoreDataMember]
        public bool Initialized => Missile.Initialized && PowderCharge.Initialized;

        public override string ToString()
        {
            return Name;
        }
    }
}
