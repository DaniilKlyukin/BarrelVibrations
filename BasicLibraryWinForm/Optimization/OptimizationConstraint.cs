using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BasicLibraryWinForm.Optimization
{
    [Serializable]
    [DataContract(Name = "Ограничения оптимизации")]
    public class OptimizationConstraint
    {
        /// <summary>
        ///     Нижние границы
        /// </summary>
        [Browsable(true)]
        [DisplayName("Нижние границы")]
        [DataMember(Name = "Нижние границы")]
        public double[]? xMins { get; protected set; }

        /// <summary>
        ///     Верхние границы
        /// </summary>
        [Browsable(true)]
        [DisplayName("Верхние границы")]
        [DataMember(Name = "Верхние границы")]
        public double[]? xMaxs { get; protected set; }

        [JsonConstructor]
        public OptimizationConstraint(double[]? xMins, double[]? xMaxs)
        {
            if (xMins != null && xMaxs != null && xMins.Length != xMaxs.Length)
                throw new ArgumentException();

            this.xMins = xMins;
            this.xMaxs = xMaxs;
        }

        public virtual bool IsValid(double[] x)
        {
            if (xMins == null || xMaxs == null || x.Length != xMins.Length)
                return true;

            for (int i = 0; i < xMins.Length; i++)
            {
                if (x[i] < xMins[i] || x[i] > xMaxs[i])
                    return false;
            }

            return true;
        }

        public OptimizationConstraint()
        {

        }
    }
}
