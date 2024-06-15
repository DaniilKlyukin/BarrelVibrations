using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BasicLibraryWinForm.Optimization;
using BasicLibraryWinForm;
using BarrelVibrations.Optimization.TargetCalculators;

namespace BarrelVibrations.Optimization
{
    public static class OptimizerJSONConverter
    {
        private static OptimizationTargetCalculator ReadJsonTarget(JObject jObject)
        {
            var name = "Метод расчета целевой функции";

            if (!jObject.ContainsKey(name))
                return new VibrationsAmplitudeOptimizationTarget();

            var jInner = ((JObject)jObject[name]);
            var nameInner = "Целевая функция";

            if (!jInner.ContainsKey(nameInner))
                return new VibrationsAmplitudeOptimizationTarget();

            var target = (OptimizationTarget)int.Parse(jInner[nameInner].ToString());
            var optStr = ((JObject)jObject[name]).ToString();

            switch (target)
            {
                case OptimizationTarget.VibrationsAmplitude:
                    return JsonConvert.DeserializeObject<VibrationsAmplitudeOptimizationTarget>(optStr);
                case OptimizationTarget.WeightedCriterion:
                    return JsonConvert.DeserializeObject<WeightedOptimizationTarget>(optStr);
                case OptimizationTarget.ShotsVibrationsSpread:
                    return JsonConvert.DeserializeObject<ShotsVibrationsSpreadOptimizationTarget>(optStr);
                case OptimizationTarget.ShotsWeightedCriterion:
                    return JsonConvert.DeserializeObject<ShotsWeightedOptimizationTarget>(optStr);
                case OptimizationTarget.ProjectilesSpread:
                    return JsonConvert.DeserializeObject<SpreadOptimizationTarget>(optStr);
                case OptimizationTarget.VibrationsIntegral:
                    return JsonConvert.DeserializeObject<VibrationsIntegralOptimizationTarget>(optStr);
            }

            return new VibrationsAmplitudeOptimizationTarget();
        }

        private static OptimizationAlgorithm ReadJsonAlgorithm(JObject jObject)
        {
            var name = "Алгоритм оптимизации";

            if (jObject.ContainsKey(name))
            {
                var str = jObject[name]?.ToString();

                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<OptimizationAlgorithm>(str);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        return OptimizationAlgorithm.NelderMead;
                    }
                }
            }

            return OptimizationAlgorithm.NelderMead;
        }

        private static BarrelOptimizationConstraint ReadJsonBarrelConstraint(JObject jObject)
        {
            var name = "Ограничения";

            if (jObject.ContainsKey(name))
            {
                var str = jObject[name]?.ToString();

                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<BarrelOptimizationConstraint>(str) ?? new();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        return new BarrelOptimizationConstraint();
                    }
                }
            }

            return new BarrelOptimizationConstraint();
        }

        public static Optimizer ParseJson(string jsonStr)
        {
            var jObject = JObject.Parse(jsonStr);

            var optAlgorithm = ReadJsonAlgorithm(jObject);
            var constraint = ReadJsonBarrelConstraint(jObject);
            var target = ReadJsonTarget(jObject);

            Optimizer optimizer = new BarrelNelderMead();

            switch (optAlgorithm)
            {
                case OptimizationAlgorithm.NelderMead:
                    {
                        optimizer = JsonConvert.DeserializeObject<BarrelNelderMead>(jsonStr) ?? new();
                        ((BarrelNelderMead)optimizer).OptimizationTargetCalculator = target;
                    }
                    break;
                case OptimizationAlgorithm.HookeJeeves:
                    {
                        optimizer = JsonConvert.DeserializeObject<BarrelHookeJeeves>(jsonStr) ?? new();
                        ((BarrelHookeJeeves)optimizer).OptimizationTargetCalculator = target;
                    }
                    break;
                case OptimizationAlgorithm.RandomDescend:
                    {
                        optimizer = JsonConvert.DeserializeObject<BarrelRandomDescend>(jsonStr) ?? new();
                        ((BarrelRandomDescend)optimizer).OptimizationTargetCalculator = target;
                    }
                    break;
            }

            optimizer.Constraint = constraint;

            return optimizer;
        }
    }
}
