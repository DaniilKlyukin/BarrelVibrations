using System;
using BasicLibraryWinForm.PointFolder;
using Modelling.Material;
using Modelling.Solvers.Problems;

namespace Modelling.Solvers.Elements
{

    public class Quad2DegreeElement : Element
    {
        public Quad2DegreeElement(Point[] points, MaterialProperties materialProperties, IProblem problem) : base(points, materialProperties, problem)
        {
            U = new double[8];
            V = new double[8];

            ProblemInitializer = problem;

            Update(U, V, 300);
        }

        public override BiPolynomial[] N { get; set; } =
         {
             new(new[,]
             {
                 {-0.25,  0,     0.25},
                 { 0,     0.25, -0.25},
                 { 0.25, -0.25,  0}
             }),
             new(new[,]
             {
                 { 0.5, -0.5, 0},
                 { 0,    0,   0},
                 {-0.5,  0.5, 0}
             }),
             new(new[,]
             {
                 {-0.25,  0,    0.25},
                 { 0,    -0.25, 0.25},
                 { 0.25, -0.25, 0},
             }),
             new(new[,]
             {
                 {0.5, 0, -0.5},
                 {0.5, 0, -0.5},
                 {0,   0,  0}
             }),
             new(new[,]
             {
                 {-0.25, 0,    0.25},
                 { 0,    0.25, 0.25},
                 { 0.25, 0.25, 0}
             }),
             new(new[,]
             {
                 { 0.5,   0.5, 0},
                 { 0,     0,   0},
                 {-0.5, -0.5, 0}
             }),
             new(new[,]
             {
                 {-0.25, 0,     0.25},
                 { 0,    -0.25, -0.25},
                 { 0.25,  0.25,  0}
             }),
             new(new[,]
             {
                 { 0.5, 0, -0.5},
                 {-0.5, 0,  0.5},
                 { 0,   0,  0}
             })
         };
        //TODO: локальные координаты
        public override Point[] LocalPoints { get; set; } =
        {
            new(0,0),
            new(1,0),
            new(0,1)
        };


        public override double[] weights { get; protected set; } =
        {
            25 / 81.0,
            40 / 81.0,
            25 / 81.0,
            40 / 81.0,
            64 / 81.0,
            40 / 81.0,
            25 / 81.0,
            40 / 81.0,
            25 / 81.0
        };
        public override double[] integration_xi { get; protected set; } =
        {
            -Math.Sqrt(0.6),
            0,
            Math.Sqrt(0.6),

            -Math.Sqrt(0.6),
            0,
            Math.Sqrt(0.6),

            -Math.Sqrt(0.6),
            0,
            Math.Sqrt(0.6)
        };

        public override double[] integration_eta { get; protected set; } =
        {
            -Math.Sqrt(0.6),
            -Math.Sqrt(0.6),
            -Math.Sqrt(0.6),

            0,
            0,
            0,

            Math.Sqrt(0.6),
            Math.Sqrt(0.6),
            Math.Sqrt(0.6)
        };
    }
}
