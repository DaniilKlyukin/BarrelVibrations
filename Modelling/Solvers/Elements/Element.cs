using System;
using BasicLibraryWinForm.PointFolder;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Modelling.Material;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers.Elements
{
    public abstract class Element
    {
        protected Element(Point[] points, MaterialProperties materialProperties, IProblem problemInitializer)
        {
            Points = points;
            MaterialProperties = materialProperties;
            ProblemInitializer = problemInitializer;
        }

        public Point[] Points { get; set; }
        public abstract Point[] LocalPoints { get; set; }
        public MaterialProperties MaterialProperties { get; set; }
        public IProblem ProblemInitializer { get; set; }
        public DenseMatrix D { get; private set; }
        public double[] U { get; protected set; }
        public double[] V { get; protected set; }
        public Matrix<double> k { get; private set; }
        public Matrix<double> B { get; private set; }
        public Matrix<double> M { get; private set; }

        public void Init()
        {
            ProblemInitializer.InitializeElement(this, out var BMatrix, out var kMatrix, out var Mmatrix);

            B = BMatrix;
            k = kMatrix;
            M = Mmatrix;
        }

        public Vector<double> GetLoad(double p, (int, int) edge)
        {
            return ProblemInitializer.CalculateLoad(this, p, edge);
        }

        public void Update(double[] u, double[] v, double temperature)
        {
            if (u.Length != Points.Length || v.Length != Points.Length)
                throw new ArgumentException();

            U = u;
            V = v;

            D = ProblemInitializer.GetD(temperature, MaterialProperties);
        }

        public void GetStrainStress(double[] u, double[] v, out Vector<double> strain, out Vector<double> stress)
        {
            var delta = new DenseVector(2 * Points.Length);

            for (var j = 0; j < Points.Length; j++)
            {
                delta[2 * j] = u[j];
                delta[2 * j + 1] = v[j];
            }

            strain = B * delta;
            stress = D * strain;
        }

        public abstract BiPolynomial[] N { get; set; }
        public abstract double[] weights { get; protected set; }
        public abstract double[] integration_xi { get; protected set; }
        public abstract double[] integration_eta { get; protected set; }
    }
}
