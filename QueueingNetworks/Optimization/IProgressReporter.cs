using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueingNetworks;

namespace Optimization
{
    interface IProgressReporter
    {
        void ReportProgress(double fitness, QueueingNetworks.Network bestFit);
    }

    class ProgressReporterStub : IProgressReporter
    {
        public void ReportProgress(double fitness, QueueingNetworks.Network bestFit)
        {
            //do nothing
        }
    }
}
