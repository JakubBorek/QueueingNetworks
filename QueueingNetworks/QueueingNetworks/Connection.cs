using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public class Connection
    {
        public int Destination { get; private set; }
        public int Class { get; private set; }
        public double Weight { get; private set; }

        public Connection (int destination, int classId, double weight)
        {
            Destination = destination;
            Class = classId;
            Weight = weight;
        }
    }
}
