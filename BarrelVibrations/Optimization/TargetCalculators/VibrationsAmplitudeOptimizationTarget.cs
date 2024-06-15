using BarrelVibrations.Solvers;
using BasicLibraryWinForm;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public class VibrationsAmplitudeOptimizationTarget : OptimizationTargetCalculator
    {
        public override OptimizationTarget OptimizationTarget => OptimizationTarget.VibrationsAmplitude;

        public override double Calculate(MainSolver mainSolver)
        {
            return mainSolver.GetVibrationsMainResults().VibrationsAmplitude;
        }

        public override string GetTargetText()
        {
            return "Амплитуда колебаний, мкм";
        }

        public override string ToString()
        {
            return "Амплитуда колебаний";
        }
    }
}
