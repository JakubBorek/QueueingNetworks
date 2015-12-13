using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public class Node
    {
        public List<double> mi { get; private set; }
        public Dictionary<uint, double> connections { get; private set; }

        public Node(IList<double> mi, Dictionary<uint, double> connections)
        {
            this.mi = new List<double>(mi);
            this.connections = new Dictionary<uint, double>(connections);
        }  
    }
}
