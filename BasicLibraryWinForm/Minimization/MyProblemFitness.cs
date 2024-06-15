using BasicLibraryWinForm;
using GeneticSharp;

namespace BasicLibraryWinForm.Minimization
{
    public class MyProblemFitness : IFitness
    {
        private readonly Func<double[], double> _evalF;

        public MyProblemFitness(Func<double[], double> evalF)
        {
            _evalF = evalF;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();

            var values = genes.Select(g => (double)g.Value).ToArray();

            return _evalF(values);
        }
    }
}
