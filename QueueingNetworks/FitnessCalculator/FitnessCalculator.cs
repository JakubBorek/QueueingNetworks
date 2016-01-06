using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCalculator
{
    public class FitnessCalculator : IFitnessCalculator
    {
        QueueingNetworks.Network network;
        public FitnessCalculator(QueueingNetworks.Network network)
        {
            this.network = network;
        }
        public double CalculateFitness(int[] solution)
        {
            //TODO: implementacja
            return solution.Sum();
        }
    }
}
