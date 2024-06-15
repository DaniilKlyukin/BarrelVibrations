using System;
using BasicLibraryWinForm.PointFolder;
using Modelling.Material;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers.Elements
{

    public class QuadElement : Element
    {
        public QuadElement(Point[] points, MaterialProperties materialProperties, IProblem problem) : base(points, materialProperties, problem)
        {
            U = new double[4];
            V = new double[4];

            ProblemInitializer = problem;

            Update(U, V, 300);
        }

        public override BiPolynomial[] N { get; set; } =
         {
             new(new[,]
             {
                 {0.25,-0.25},
                 {-0.25,0.25}
             }),
             new(new[,]
             {
                 {0.25,-0.25},
                 {0.25,-0.25}
             }),
             new(new[,]
             {
                 {0.25,0.25},
                 {0.25,0.25}
             }),
             new(new[,]
             {
                 {0.25,0.25},
                 {-0.25,-0.25}
             })
         };
        //TODO: локальные координаты
        public override Point[] LocalPoints { get; set; } =
        {
            new(0,0),
            new(1,0),
            new(0,1)
        };

        public override double[] weights { get; protected set; } = { 1.0, 1.0, 1.0, 1.0 };
        public override double[] integration_xi { get; protected set; } = { -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3), -1.0 / Math.Sqrt(3) };
        public override double[] integration_eta { get; protected set; } = { -1.0 / Math.Sqrt(3), -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3) };
    }
}
