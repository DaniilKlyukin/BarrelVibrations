using GeneticSharp;

namespace BasicLibraryWinForm.Minimization
{
    public class SimpleChromosome : ChromosomeBase
    {
        private int length;
        private BasicRandomization random = new BasicRandomization();
        public SimpleChromosome(int length) : base(length)
        {
            this.length = length;
            CreateGenes();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(random.GetDouble());
        }

        public override IChromosome CreateNew()
        {
            return new SimpleChromosome(length);
        }
    }
}
