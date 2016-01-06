using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    class Program
    {
        static void Main(string[] args)
        {
            var fitnessCalculator = new FitnessCalculator.FitnessCalculatorStub();
            var progressReporter = new ProgressReporterStub();
            var parameters = new OptimizationParameters
            {
                BestSolutionsCount = 5,
                EliteSolutionsCount = 2,
                ProcessorsMaxCount = 5,
                NodesCount = 5,
                FitnessCalculator = fitnessCalculator,
                ProgressReporter = progressReporter

            };
            Optimization.Run(parameters);
            Console.WriteLine("Started - press any key to stop");
            Console.ReadKey();
            Optimization.Stop();
        }
    }
}
