using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCalculator
{
    public interface IFitnessCalculator
    {
        Tuple<double, double[]> CalculateFitness(int[] solution);
    }
}
