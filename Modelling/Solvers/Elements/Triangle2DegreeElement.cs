using BasicLibraryWinForm.PointFolder;
using Modelling.Material;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers.Elements
{

    public class Triangle2DegreeElement : Element
    {
        public Triangle2DegreeElement(Point[] points, MaterialProperties materialProperties, IProblem problem) : base(points, materialProperties, problem)
        {
            U = new double[6];
            V = new double[6];

            ProblemInitializer = problem;

            Update(U, V, 300);
        }

        public override BiPolynomial[] N { get; set; } =
        {
            new(new[,]
            {
                {1, -3, 2},
                {-3, 4, 0},
                {2, 0, 0.0}
            }),
            new(new[,]
            {
                {0, 0, 0},
                {0, 4.0, 0},
                {0, 0, 0}
            }),
            new(new[,]
            {
                {0.0, -1, 2},
                {0, 0, 0},
                {0, 0, 0}
            }),
            new(new[,]
            {
                {0.0, 0, 0},
                {4, -4, 0},
                {-4, 0, 0}
            }),
            new(new[,]
            {
                {0.0,  0, 0},
                {-1, 0, 0},
                {2,  0, 0}
            }),
            new(new[,]
            {
                {0.0, 4, -4},
                {0, -4, 0},
                {0, 0, 0}
            }),
        };
        //TODO: локальные координаты
        public override Point[] LocalPoints { get; set; } =
        {
            new(0,0),
            new(1,0),
            new(0,1)
        };
        public override double[] weights { get; protected set; } = { 1.0 / 6, 1.0 / 6, 1.0 / 6 };
        public override double[] integration_xi { get; protected set; } = { 0.5, 0.5, 0 };
        public override double[] integration_eta { get; protected set; } = { 0.5, 0, 0.5 };
    }
}
