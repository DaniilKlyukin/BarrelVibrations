using System;

namespace Modelling.Loads
{
    public class LoadFunc
    {
        public bool HasValue { get; set; }
        public Func<double,double> PressureFunc { get; set; }

        public LoadFunc(bool hasValue, Func<double, double> pressureFunc)
        {
            HasValue = hasValue;
            PressureFunc = pressureFunc;
        }
    }
}
