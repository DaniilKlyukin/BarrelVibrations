using BarrelVibrations.ModelingObjects.AmmoFolder;
using InletBallisticLibrary;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarrelVibrations.ModelingObjects.MissileFolder;

namespace BarrelVibrations
{
    public static class AmmoJsonSerializer
    {
        public static List<Ammo> Parse(string json)
        {
            var ammo = new List<Ammo>();

            if (string.IsNullOrEmpty(json))
                return ammo;

            var jArray = JArray.Parse(json);

            foreach (var token in jArray)
            {
                var powderChargeJson = token["Пороховой заряд"];
                var missileJson = token["Снаряд"];

                var a = new Ammo();

                a.Name = token["Название"].ToString();
                a.PowderCharge = PowderJsonSerializer.Parse(powderChargeJson.ToString());
                a.Missile = JsonConvert.DeserializeObject<Missile>(missileJson.ToString());

                ammo.Add(a);
            }

            return ammo;
        }
    }
}
