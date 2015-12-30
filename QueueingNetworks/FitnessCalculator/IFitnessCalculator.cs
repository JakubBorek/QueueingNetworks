using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCalculator
{
    interface IFitnessCalculator
    {
        double CalculateFitness(QueueingNetworks.Network network);
    }
}
