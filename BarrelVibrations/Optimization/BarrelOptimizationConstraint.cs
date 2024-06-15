using BasicLibraryWinForm;
using BasicLibraryWinForm.Optimization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BarrelVibrations.Optimization
{
    [Serializable]
    [DataContract(Name = "Ограничения оптимизации формы ствола")]
    public class BarrelOptimizationConstraint : OptimizationConstraint
    {
        [DataMember(Name = "Координаты X ствола, м")]
        public double[] BarrelX { get; } = Array.Empty<double>();

        [DataMember(Name = "Минимальная толщина ствола, м")]
        public double[] ThicknessMins { get; } = Array.Empty<double>();

        [DataMember(Name = "Максимальная масса ствола, кг")]
        public double MaxMass { get; }

        [DataMember(Name = "Внешний диаметр должен убывать")]
        public bool IsOuterDiameterNeedDecrease { get; } = true;

        [JsonConstructor]
        public BarrelOptimizationConstraint(
            double[] barrelX,
            double[] thicknessMins,
            double maxMass,
            bool isOuterDiameterNeedDecrease)
        {
            BarrelX = barrelX;
            ThicknessMins = thicknessMins;
            MaxMass = maxMass;
            IsOuterDiameterNeedDecrease = isOuterDiameterNeedDecrease;
        }

        public bool IsValid(
            double[] barrelX,
            double[]? thickness,
            double[]? outerD,
            double mass)
        {
            if (mass > MaxMass)
                return false;

            if (outerD != null)
            {
                for (var i = 0; i < outerD.Length - 1; i++)
                {
                    if (outerD[i + 1] > outerD[i])
                        return false;
                }
            }

            return IsValidParameter(barrelX, thickness, ThicknessMins);
        }

        private bool IsValidParameter(double[] barrelX, double[]? parameter, double[]? constraintMin = null, double[]? constraintMax = null)
        {
            if (parameter == null)
                return true;

            var parameterFunc = Algebra.GetFunc(barrelX, parameter);

            for (int i = 0; i < BarrelX.Length; i++)
            {
                var value = parameterFunc(BarrelX[i]);

                if (value < (constraintMin?[i] ?? double.MinValue) || value > (constraintMax?[i] ?? double.MaxValue))
                    return false;
            }

            return true;
        }

        public BarrelOptimizationConstraint()
        {

        }

        public override string ToString()
        {
            return $"{(ThicknessMins?.Length ?? 0) + 1} Ограничений";
        }
    }
}
