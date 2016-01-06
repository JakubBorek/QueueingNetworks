using FitnessCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public struct OptimizationParameters
    {
        public int ScoutCount;
        public int ProcessorsMaxCount;
        public int EliteSolutionsCount;
        public int BestSolutionsCount;
        public int NodesCount;
        public IFitnessCalculator FitnessCalculator;
        public IProgressReporter ProgressReporter;
    }
}
