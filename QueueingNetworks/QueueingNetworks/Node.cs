using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public class Node
    {
        public int Id { get; private set; }
        public IReadOnlyList<double> Mi { get; private set; }
        public IReadOnlyList<Connection> Connections { get; private set; }
        public Node(int id, IReadOnlyList<double> mi, IReadOnlyList<Connection> connections)
        {
            Id = id;
            Mi = new List<double>(mi);
            Connections = new List<Connection>(connections);
        }  

        public double GetTransitionProbability(int destination, int classId)
        {
            var foundConnections = Connections.Where(c => c.Class == classId && c.Destination == destination);
            if(foundConnections.Any())
            {
                return foundConnections.First().Weight;
            }
            else
            {
                return 0;
            }
        }
    }
}
