using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public static class Generator
    {
        public static Network GenerateAllConnected(int nodesCount, int classCount, int minElementsPerClass, int maxElementsPerClass, int connectionsCount)
        {
            var nodes = generateNodes(nodesCount, classCount, connectionsCount);
            var counts = generateClassCounts(classCount, minElementsPerClass, maxElementsPerClass);
            return new Network(nodes, counts);
        }

        private static List<Node> generateNodes(int nodesCount, int classCount, int connectionsCount)
        {
            var nodes = new List<Node>(nodesCount);
            Random random = new Random();
            for (int i = 0; i < nodesCount; i++)
            {
                var mis = generateMis(classCount, random);
                var connections = generateConnections(nodesCount, classCount, connectionsCount, random);
                nodes.Add(new Node(i, Node.NodeType.Type1, mis, connections));
            }
            return nodes;
        }

        private static List<int> generateClassCounts(int classCount, int minElementsPerClass, int maxElementsPerClass)
        {
            double delta = maxElementsPerClass - minElementsPerClass;
            var random = new Random();
            var counts = new List<int>(classCount);
            for (int i = 0; i < classCount; i++)
            {
                var value = minElementsPerClass + random.NextDouble() * delta;
                counts.Add((int)value);
            }
            return counts;
        }

        private static List<double> generateMis(int classCount, Random random)
        {
            var mis = new List<double>(classCount);
            for (int c = 0; c < classCount; c++)
            {
                mis.Add(random.NextDouble() + 1.0);
            }
            return mis;
        }

        private static List<Connection> generateConnections(int nodeCount, int classCount, int connectionsCount, Random random)
        {
            var connections = new List<Connection>(connectionsCount * classCount);
            double connectionWeight = 1.0 / connectionsCount;

            for (int c = 0; c < classCount; c++)
            {
                var destinations = generateUniqueRandom(nodeCount, connectionsCount, random);
                destinations.ForEach(d =>
                    connections.Add(new Connection(d, c, connectionWeight)));
            }
            return connections;
        }

        private static List<int> generateUniqueRandom(int maxValue, int count, Random random)
        {
            var list = new List<int>(count);
            while (list.Count != count)
            {
                var value = random.Next(0, maxValue);
                if (!list.Contains(value))
                {
                    list.Add(value);
                }
            }
            return list;
        }
    }
}
