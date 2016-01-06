using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Optimization
{
    public static class Optimization
    {
        public static void Run(OptimizationParameters parameters)
        {
            if(BeesAlgorithm.Running)
            {
                BeesAlgorithm.Cancel();     
                while(BeesAlgorithm.Running)
                {
                    Thread.Sleep(5);
                }          
            }
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                BeesAlgorithm.ScoutsAmount = parameters.ScoutCount;
                BeesAlgorithm.setProcessorsMaxAmount(parameters.ProcessorsMaxCount);
                BeesAlgorithm.setNodesAmount(parameters.NodesCount);
                BeesAlgorithm.setElisteSolutionsAmount(parameters.EliteSolutionsCount);
                BeesAlgorithm.setBestSolutionsAmount(parameters.BestSolutionsCount);
                BeesAlgorithm.FitnessCalculator = parameters.FitnessCalculator;
                new BeesAlgorithm();
            }).Start();

            new Thread(() =>
            {
                while (!BeesAlgorithm.Running || BeesAlgorithm.BestSolution == null)
                {
                    Thread.Sleep(100);
                }
                while(BeesAlgorithm.Running)
                {
                    var solution = BeesAlgorithm.BestSolution;
                    parameters.ProgressReporter.ReportProgress(solution.Item2, solution.Item1);
                    Thread.Sleep(500);
                }
            }).Start();
        }

        public static void Stop()
        {
            BeesAlgorithm.Cancel();
        }
    }
}
