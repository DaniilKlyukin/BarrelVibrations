using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace InletBallisticLibrary
{
    public static class PowderJsonSerializer
    {
        public static PowderCharge Parse(string jsonStr)
        {
            var powderCharge = new PowderCharge();

            var jObject = JObject.Parse(jsonStr);

            var powders = jObject["Список порохов"].Select<JToken, Powder?>(jx =>
            {
                var type = (PowderType)jx["Тип пороха"].Value<int>();

                switch (type)
                {
                    case PowderType.Tube:
                        return JsonConvert.DeserializeObject<TubePowder>(jx.ToString());
                    case PowderType.Grained:
                        return JsonConvert.DeserializeObject<GrainedPowder>(jx.ToString());
                }
                return null;
            }).ToList();

            if (powders == null || powders.Any(p => p == null))
                throw new InvalidOperationException();

            powderCharge.IgniterMass = GetValue<double>(jObject, "Масса воспламенителя, кг");
            powderCharge.SleeveDensity = GetValue<double>(jObject, "Плотность гильзы, кг/м³");
            powderCharge.SleeveMass = GetValue<double>(jObject, "Масса гильзы, кг");
            powderCharge.Powders = powders;

            return powderCharge;
        }

        private static T? GetValue<T>(JObject? obj, string key)
        {
            if (obj == null)
                return default;

            var jObject = obj;

            if (jObject.ContainsKey(key))
                return jObject[key].Value<T>();

            return default;
        }
    }
}
