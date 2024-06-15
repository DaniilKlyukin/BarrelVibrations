using BasicLibraryWinForm.PointFolder;
using Modelling.Material;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers.Elements
{

    public class TriangleElement : Element
    {
        public TriangleElement(Point[] points, MaterialProperties materialProperties, IProblem problem) : base(points, materialProperties, problem)
        {
            U = new double[3];
            V = new double[3];

            ProblemInitializer = problem;

            Update(U, V, 300);
        }

        public override BiPolynomial[] N { get; set; } =
        {
            new(new[,] //1-ξ-η
            {
                {1.0,-1.0},
                {-1.0,0.0}
            }),
            new(new[,] //ξ
            {
                {0.0,0.0},
                {1.0,0.0}
            }),
            new(new[,] //η
            {
                {0.0,1.0},
                {0.0,0.0}
            }),
        };

        public override Point[] LocalPoints { get; set; } =
        {
            new(0,0),
            new(1,0),
            new(0,1)
        };

        public override double[] weights { get; protected set; } = { 0.5 };
        public override double[] integration_xi { get; protected set; } = { 1.0 / 3 };
        public override double[] integration_eta { get; protected set; } = { 1.0 / 3 };
    }
}
