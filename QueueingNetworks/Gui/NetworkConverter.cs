using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    static class NetworkConverter
    {
        public static List<Node> NetworkToNodes(QueueingNetworks.Network network)
        {
            var nodeList = new List<Node>(network.Nodes.Count);
            foreach (var node in network.Nodes)
            {
                var graphNode = new Node();
                graphNode.Name = (node.Id + 1).ToString();
                graphNode.Mi = new List<double>(node.Mi);
                nodeList.Add(graphNode);
            }

            for (int i = 0; i < network.Nodes.Count; i++)
            {
                nodeList[i].Connections = network.Nodes[i].Connections.Select(c => new Connection()
                {
                    Class = c.Class + 1,
                    To = nodeList[c.Destination],
                    Weight = c.Weight,
                }).ToList();
            }
            return nodeList;
        }

        public static QueueingNetworks.Network NodesToNetwork(List<Node> graphNodes)
        {
            var queueNodes = new List<QueueingNetworks.Node>(graphNodes.Count);
            int id = 0;
            foreach (var graphNode in graphNodes)
            {
                queueNodes.Add(new QueueingNetworks.Node(id, graphNode.Mi, (graphNode.Connections.Select(
                    gc => new QueueingNetworks.Connection(findIndex(gc.To, graphNodes), gc.Class - 1, gc.Weight))).ToList()));
                id++;
            }
            var entryPoints = new List<QueueingNetworks.EntryPoint>();
            var network = new QueueingNetworks.Network(queueNodes, entryPoints);
            return network;
        }

        private static int findIndex(Node node, List<Node> list)
        {
            return list.FindIndex(n => n == node);
        }
    }
}
