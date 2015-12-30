using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public class EntryPoint
    {
        public int Node { get; private set; }
        public int Class { get; private set; }
        public double Lambda { get; private set; }

        public EntryPoint(int node, int classId, double lambda)
        {
            Node = node;
            Class = classId;
            Lambda = lambda;
        }
    }
}
