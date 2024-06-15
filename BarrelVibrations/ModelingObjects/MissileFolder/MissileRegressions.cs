using BasicLibraryWinForm;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.MissileFolder
{
    [Serializable]
    [DataContract(Name = "Коэффициенты регрессии для снаряда")]
    public class MissileRegressions : ICloneable
    {
        public MissileRegressions() { }

        [JsonConstructor]
        public MissileRegressions(
            double[,] cxRegression,
            double[,] cyRegression,
            double[] czRegression,
            double[] mxRegression,
            double[] myRegression,
            double[] mzRegression)
        {
            for (int i = 0; i < CxRegression.GetLength(0); i++)
            {
                for (int j = 0; j < CxRegression.GetLength(1); j++)
                {
                    CxRegression[i, j] = cxRegression[i, j];
                    CyRegression[i, j] = cyRegression[i, j];
                }
            }

            for (int i = 0; i < CzRegression.Length; i++)
            {
                CzRegression[i] = czRegression[i];
            }

            for (int i = 0; i < MxRegression.Length; i++)
            {
                MxRegression[i] = mxRegression[i];
                MyRegression[i] = myRegression[i];
                MzRegression[i] = mzRegression[i];
            }
        }

        [IgnoreDataMember]
        public static double[] CMachs => new double[] { 0, 0.8, 1.0, 1.2, 5.0 };

        /// <summary>
        /// Коэффициенты регрессии b момента mx
        /// </summary>
        [DataMember(Name = "Коэффициенты регрессии b момента mx")]
        public double[] MxRegression { get; } = new[] { 81e-5, -33e-5, 5e-5, 0 };

        /// <summary>
        /// Коэффициенты регрессии b момента my
        /// </summary>
        [DataMember(Name = "Коэффициенты регрессии b момента my")]
        public double[] MyRegression { get; } = new[] { 90e-4, -32e-4, 7e-4, 6.0042 };

        /// <summary>
        /// Коэффициенты регрессии b момента mz
        /// </summary>
        [DataMember(Name = "Коэффициенты регрессии b момента mz")]
        public double[] MzRegression { get; } = new[] { 0.6964, 0.1414, -0.0532, 0 };

        /// <summary>
        /// Коэффициенты регрессии a сопротивления cx
        /// </summary>
        [DataMember(Name = "Коэффициенты регрессии a момента cx")]
        public double[,] CxRegression { get; } = new[,]
        {
            { 0.203, 0, 0, 0, 2.870 },
            { -0.731, 4.467, -7.014, 3.618, 3.423 },
            { -22.57, 54.91, -43.33, 11.34, 4.365 },
            { 0.625, -0.031, -0.061, 0.012, 4.999 },
        };

        /// <summary>
        /// Коэффициенты регрессии a сопротивления cy
        /// </summary>
        [DataMember(Name = "Коэффициенты регрессии a момента cy")]
        public double[,] CyRegression { get; } = new[,]
        {
            { 0, 1.536, 1.836, 0, 0.304 },
            { 0, -2.565, 4.853, 7.368, -2.891 },
            { 0, -5.232, 6.372, 12.217, -5.201 },
            { 0, -0.408, 8.519, 2.379, -0.430 },
        };

        /// <summary>
        /// Коэффициенты регрессии a сопротивления cz
        /// </summary>
        [DataMember(Name = "Коэффициенты регрессии a момента cz")]
        public double[] CzRegression { get; } = new[] { 0, 0.117, 0.311, 0, 0 };

        public object Clone()
        {
            return new MissileRegressions(
                CxRegression.Copy(),
                CyRegression.Copy(),
                CzRegression.Copy(),
                MxRegression.Copy(),
                MyRegression.Copy(),
                MzRegression.Copy());
        }

        public override string ToString()
        {
            return $"Регрессионные зависимости";
        }
    }
}
