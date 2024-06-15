using System;
using System.Linq;
using System.Runtime.Serialization;
using MathNet.Numerics.Interpolation;

namespace Modelling.Material
{
    public class MaterialProperty
    {
        [DataMember] public string PropertyName { get; }
        [DataMember] public double[] Keys { get; private set; }
        [DataMember] public double[] Values { get; private set; }
        [IgnoreDataMember] public Func<double, double> Function { get; private set; }


        public MaterialProperty(string propertyName, double[] keys, double[] values)
        {
            PropertyName = propertyName;
            Keys = keys;
            Values = values;

            CreateFunc(MaterialPropertyFunction.Spline);
        }

        public void CreateFunc(MaterialPropertyFunction function)
        {
            (Keys, Values) = CorrectLength(Keys, Values);

            var value = Values.First();

            if (Values.Length == 1 || function == MaterialPropertyFunction.Constant)
            {
                Function = _ => value;
                return;
            }

            if (Values.Length == 2 || function == MaterialPropertyFunction.Line)
            {
                var spline = LinearSpline.Interpolate(Keys, Values);
                Function = T => spline.Interpolate(T);
                return;
            }

            var spline3 = CubicSpline.InterpolatePchip(Keys, Values);
            Function = T => spline3.Interpolate(T);
        }

        private (double[], double[]) CorrectLength(double[] temperatures, double[] values)
        {
            var length = Math.Min(temperatures.Length, values.Length);

            var temperaturesSame = new double[length];
            var valuesSame = new double[length];

            for (var i = 0; i < length; i++)
            {
                temperaturesSame[i] = temperatures[i];
                valuesSame[i] = values[i];
            }

            return (temperaturesSame, valuesSame);
        }

        public override string ToString()
        {
            return PropertyName;
        }
    }
}
