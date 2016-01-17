using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public static class Generator
    {/*
        public static Network GenerateAllConnected(int nodesCount, int classCount, int minElementsPerClass, int maxElementsPerClass, int connectionsCount)
        {
            var nodes = generateNodes(connections, nodesCount, classCount);
            var counts = generateClassCounts(classCount, minElementsPerClass, maxElementsPerClass);
            return new Network(nodes, counts);
        }

        private static List<Node> generateNodes(int nodesCount, int classCount)
        {
            var nodes = new List<Node>(nodesCount);
            Random random = new Random();
            for (int i = 0; i < nodesCount; i++)
            {
                var mis = generateMis(classCount, random);
                var connections = generateConnections(nodesCount, classCount);
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

        private static List<Connection> generateConnections(int nodeCount, int classCount, int connectionsCount)
        {
            var connections = new List<Connection>(nodeCount * classCount);
            double connectionWeight = 1.0 / nodeCount;
            for (int n = 0; n < nodeCount; n++)
            {
                for (int c = 0; c < classCount; c++)
                {
                    connections.Add(new Connection(n, c, connectionWeight));
                }
            }
            return connections;
        }*/
    }
}
