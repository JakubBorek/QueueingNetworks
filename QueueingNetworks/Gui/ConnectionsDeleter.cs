using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    static class ConnectionsDeleter
    {

        public static void DeleteForDestination(List<Node> nodes, int destinationToDelete)
        {
            var nodeToDelete = nodes[destinationToDelete];
            foreach (var n in nodes)
            {
                var connectionsToDelete = new List<Connection>();
                foreach (var c in n.Connections)
                {
                    if(c.To == nodeToDelete)
                    {
                        connectionsToDelete.Add(c);
                    }
                }
                foreach(var dc in connectionsToDelete)
                {
                    n.Connections.Remove(dc);
                }
            }
        }
    }
}
