using BarrelVibrations.Solvers;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public abstract class OptimizationTargetCalculator
    {
        [Browsable(false)]
        [DataMember(Name = "Целевая функция")]
        public abstract OptimizationTarget OptimizationTarget { get; }

        public abstract string GetTargetText();

        public abstract double Calculate(MainSolver mainSolver);
    }
}
