using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    static class GraphBuilder
    {
        static public Graph FromNodes(IEnumerable<Node> nodes)
        {
            var graph = new Graph();
            graph.AddVertexRange(nodes);
            foreach (var n in nodes)
            {
                foreach (var c in n.Connections)
                {
                    graph.AddEdge(new Edge(n, c.To));
                }
            }

            return graph;
        }

        private static List<Node> sampleNodes;

        static public Graph GetSample()
        {
            if (sampleNodes == null)
            {
                var node1 = new Node() { Name = "1" };
                var node2 = new Node() { Name = "2" };
                sampleNodes = new List<Node> { node1, node2 };
                var conn = new Connection { Class = 0, To = node2, Weight = 1 };
                node1.Connections.Add(conn);
            }
            return FromNodes(sampleNodes);
        }
    }
}
