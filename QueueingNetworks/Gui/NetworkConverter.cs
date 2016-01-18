using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    static class NetworkConverter
    {
        public static Tuple<List<Node>, List<int>> NetworkToNodes(QueueingNetworks.Network network)
        {
            var nodeList = new List<Node>(network.Nodes.Count);
            foreach (var node in network.Nodes)
            {
                var graphNode = new Node();
                graphNode.Type = node.Type;
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
            var countList = new List<int>(network.ClassMembersCounts);
            return Tuple.Create(nodeList, countList);
        }

        public static QueueingNetworks.Network NodesToNetwork(List<Node> graphNodes, List<int> counts)
        {
            var queueNodes = new List<QueueingNetworks.Node>(graphNodes.Count);
            int id = 0;
            foreach (var graphNode in graphNodes)
            {
                queueNodes.Add(new QueueingNetworks.Node(id, graphNode.Type, getMis(graphNode, counts.Count), getConnections(graphNode, counts.Count, graphNodes)));
                id++;
            }
            var network = new QueueingNetworks.Network(queueNodes, counts);
            return network;
        }

        private static List<QueueingNetworks.Connection> getConnections(Node node, int classCount, List<Node> allNodes)
        {
            var list = new List<QueueingNetworks.Connection>(node.Connections.Count);
            for (int c = 0; c < classCount; c++)
            {
                var distinctClassConnections = removeDuplicates(node.Connections.Where(conn => conn.Class - 1 == c).Where(conn => conn.Weight != 0));
                var sum = distinctClassConnections.Sum(conn => conn.Weight);
                if (sum != 0)
                {
                    double scale = 1 / sum;
                    list.AddRange(distinctClassConnections.Select(conn => new QueueingNetworks.Connection(findIndex(conn.To, allNodes), conn.Class - 1, conn.Weight * scale)));
                }
            }
            return list;
        }

        private static List<Connection> removeDuplicates(IEnumerable<Connection> input)
        {
            var distinctItems = input.GroupBy(x => x.To).Select(y => y.First());
            return distinctItems.ToList();
        }

        private static List<double> getMis(Node node, int count)
        {
            node.UpdateMiLength(count);
            return node.Mi;

        }

        private static int findIndex(Node node, List<Node> list)
        {
            return list.FindIndex(n => n == node);
        }
    }
}
