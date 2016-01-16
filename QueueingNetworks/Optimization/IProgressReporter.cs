using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueingNetworks;

namespace Optimization
{
    public interface IProgressReporter
    {
        void ReportProgress(double fitness, int[] solution, double[][] kir);
    }
}
