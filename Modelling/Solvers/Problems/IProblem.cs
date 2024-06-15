using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Modelling.Material;
using Modelling.Solvers.Elements;

namespace Modelling.Solvers.Problems
{
    public interface IProblem
    {
        public List<Dictionary<int, double>> U { get; set; }
        public List<Dictionary<int, double>> V { get; set; }
        public DenseMatrix GetD(double temperature, MaterialProperties materialProperties);
        public void InitializeElement(Element element, out Matrix<double> B, out Matrix<double> k, out Matrix<double> M);
        public Vector<double> CalculateLoad(Element element, double pressure, (int, int) edge);
        public void CalculateParameters<TElement>(List<TElement> elements, Dictionary<int, double> u, Dictionary<int, double> v) where TElement : Element;
        public string[] MeasureUnits { get; }
        public string[] VariableNames { get; }
        public double[] ValueMultipliers { get; }
        public List<Dictionary<int, double>>[] Solutions { get; }
        public void InitializeSolutions();
    }
}
