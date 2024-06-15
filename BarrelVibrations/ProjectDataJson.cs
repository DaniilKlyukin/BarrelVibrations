using System.Runtime.Serialization;
using BarrelVibrations.ModelingObjects;
using BarrelVibrations.ModelingObjects.AmmoFolder;
using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ModelingObjects.FiringSystemFolder;
using BarrelVibrations.ModelingObjects.MaterialFolder;
using BarrelVibrations.ModelingObjects.MeshFolder;
using BarrelVibrations.ModelingObjects.MissileFolder;
using BarrelVibrations.Optimization;
using BarrelVibrations.Solvers;
using BarrelVibrations.Solvers.Solutions;
using BarrelVibrations.Solvers.Solutions.InletBallistic;
using BasicLibraryWinForm;
using BasicLibraryWinForm.Optimization;
using InletBallisticLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Environment = BarrelVibrations.ModelingObjects.EnvironmentFolder.Environment;


namespace BarrelVibrations
{
    [Serializable]
    [DataContract(Name = "Данные проекта")]
    public class ProjectDataJson
    {
        [DataMember(Name = "Основной решатель")] public MainSolver MainSolver { get; set; }
        [DataMember(Name = "Алгоритм оптимизации")] public Optimizer Optimizer { get; set; }

        [JsonConstructor]
        public ProjectDataJson(
            MainSolver mainSolver,
            Optimizer optimizer)
        {
            MainSolver = mainSolver;
            Optimizer = optimizer;
        }

        private static T ReadJson<T>(JToken? token, string propertryName) where T : new()
        {
            if (token == null)
                return new T();

            var str = token[propertryName]?.ToString();

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(str) ?? new T();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    return new T();
                }
            }
            return new T();
        }

        private static PowderCharge ParsePowderCharge(JToken? token)
        {
            if (token == null)
                return new PowderCharge();

            var str = token["Пороховой заряд"]?.ToString();

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    return PowderJsonSerializer.Parse(str) ?? new PowderCharge();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    return new PowderCharge();
                }
            }

            return new PowderCharge();
        }

        private static Optimizer ParseOptimizer(JToken? token)
        {
            if (token == null)
                return new BarrelNelderMead();

            var str = token.ToString();

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    return OptimizerJSONConverter.ParseJson(str) ?? new BarrelNelderMead();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    return new BarrelNelderMead();
                }
            }

            return new BarrelNelderMead();
        }

        public static ProjectDataJson LoadJson(string path)
        {
            var jsonStr = File.ReadAllText(path);

            if (jsonStr == null)
                throw new Exception($"Не выходит прочитать файл {path}");

            var rootJObject = JObject.Parse(jsonStr);

            if (!rootJObject.HasValues)
                throw new Exception($"Не выходит прочитать как json {jsonStr}");

            var mainSolverToken = rootJObject["Основной решатель"];

            var mainSolver = DeserializeSolver(mainSolverToken.ToString());

            var optToken = rootJObject["Алгоритм оптимизации"];

            return new ProjectDataJson(
                mainSolver,
                ParseOptimizer(optToken));
        }

        public static MainSolver DeserializeSolver(string str)
        {
            var mainSolverToken = JObject.Parse(str);

            var mainSolver = new MainSolver
            {
                Material = ReadJson<Material>(mainSolverToken, "Материал"),
                Environment = ReadJson<Environment>(mainSolverToken, "Окружающая среда"),
                MuzzleBreak = ReadJson<MuzzleBreak>(mainSolverToken, "Дульный тормоз"),
                Barrel = ReadJson<Barrel>(mainSolverToken, "Ствол"),
                FiringSystem = ReadJson<FiringSystem>(mainSolverToken, "Боевая система"),
                MeshProperties = ReadJson<MeshProperties>(mainSolverToken, "Параметры сетки"),
                ModelProperties = ReadJson<ModelProperties>(mainSolverToken, "Параметры модели"),
                Ammo = AmmoJsonSerializer.Parse(mainSolverToken["Боеприпасы"]?.ToString() ?? ""),

                TimeMoments = ReadJson<List<double>>(mainSolverToken, "Моменты времени, сек"),
                InletBallistic = ReadJson<InletBallistic>(mainSolverToken, "Данные решения задачи внутренней баллистики"),
                GasEpures = ReadJson<GasEpures>(mainSolverToken, "Данные эпюр распределения параметров газа при выстреле"),
                TemperatureField = ReadJson<TemperatureField>(mainSolverToken, "Результаты решения задачи теплопроводности"),
                Deflection = ReadJson<Deflection>(mainSolverToken, "Данные решения задачи начального прогиба"),
                Vibrations = ReadJson<Vibrations>(mainSolverToken, "Данные решения задачи колебаний"),
                OutletBallistic = ReadJson<List<OutletBallistic>>(mainSolverToken, "Результаты решения задачи внешней баллистики"),
                ShotsParameters = ReadJson<List<ShotParameters>>(mainSolverToken, "Дульные параметры стрельбы")
            };

            return mainSolver;
        }
    }
}
