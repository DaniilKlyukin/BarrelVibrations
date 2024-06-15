using BarrelVibrations.ModelingObjects.BarrelFolder;
using BasicLibraryWinForm.Optimization;

namespace BarrelVibrations.Optimization
{
    public class BarrelNormalizer : Normalizer
    {
        private const double DiameterMaxCoefficient = 4;

        public BarrelNormalizer(double[] dInner, StiffenersType stiffenersType)
        {
            var mins = new List<double>();
            var maxs = new List<double>();

            mins.AddRange(dInner.Select(d => d * 1e-3).ToArray());
            maxs.AddRange(dInner.Select(d => DiameterMaxCoefficient * d * 1e-3).ToArray());

            if (stiffenersType == StiffenersType.None)
            {
                this.mins = mins.ToArray();
                this.maxs = maxs.ToArray();
                return;
            }

            //диаметр ребер жесткости
            mins.AddRange(Enumerable.Repeat(0.0, dInner.Length).ToArray());
            maxs.AddRange(dInner.Select(d => DiameterMaxCoefficient * d * 1e-3).ToArray());

            //расстояние до ребер жесткости
            mins.AddRange(dInner.Select(d => 0.5 * d * 1e-3).ToArray());
            maxs.AddRange(dInner.Select(d => 0.5 * DiameterMaxCoefficient * d * 1e-3).ToArray());

            this.mins = mins.ToArray();
            this.maxs = maxs.ToArray();
        }

        public BarrelNormalizer(double[] dInner, StiffenersType stiffenersType, double[] minThickness)
        {
            var mins = new List<double>();
            var maxs = new List<double>();

            mins.AddRange(dInner.Select((d, i) => d * 1e-3 + 2 * minThickness[i]).ToArray());
            maxs.AddRange(dInner.Select((d, i) => DiameterMaxCoefficient * d * 1e-3 + 2 * minThickness[i]).ToArray());

            if (stiffenersType == StiffenersType.None)
            {
                this.mins = mins.ToArray();
                this.maxs = maxs.ToArray();
                return;
            }

            //диаметр ребер жесткости
            mins.AddRange(Enumerable.Repeat(0.0, dInner.Length).ToArray());
            maxs.AddRange(dInner.Select((d, i) => DiameterMaxCoefficient * d * 1e-3 + 2 * minThickness[i]).ToArray());

            //расстояние до ребер жесткости
            mins.AddRange(dInner.Select((d, i) => 0.5 * (d * 1e-3 + 2 * minThickness[i])).ToArray());
            maxs.AddRange(dInner.Select((d, i) => 0.5 * (DiameterMaxCoefficient * d * 1e-3 + 2 * minThickness[i])).ToArray());

            this.mins = mins.ToArray();
            this.maxs = maxs.ToArray();
        }
    }
}
