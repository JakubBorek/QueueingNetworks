using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    public class Network
    {
        public IReadOnlyList<Node> Nodes { get; private set; }
        public IReadOnlyCollection<EntryPoint> EntryPoints { get; set; }

        public Network(IReadOnlyList<Node> nodes, IReadOnlyCollection<EntryPoint> entryPoints)
        {
            Nodes = new List<Node>(nodes);
            EntryPoints = new List<EntryPoint>(entryPoints);
        }

        public int ClassCount
        {
            get
            {
                return Nodes.Count > 0 ? Nodes[0].Mi.Count : 0;
            }
        }

        #region fileIO

        public static Network Read(TextReader reader)
        {
            uint nodeCount = reader.ReadSingleUintLine();
            uint classCount = reader.ReadSingleUintLine();
            var entryPoints = new List<EntryPoint>();
            var connections = new List<Tuple<int, Connection>>();
            for (int i = 0; i < classCount; i++)
            {
                entryPoints.AddRange(reader.ReadDoubleDictionaryLine().Select((pair) => new EntryPoint(pair.Key, i, pair.Value)));
                connections.AddRange(reader.ReadDoubleMatrixAsConnectionList(nodeCount, i));
            }
            var miMatrix = reader.ReadDoubleMatrix(nodeCount);
            var nodes = new List<Node>((int)nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                nodes.Add(new Node(i, miMatrix[i], connections.Where(p => p.Item1 == i).Select(p => p.Item2).ToList()));
            }
            var network = new Network(nodes, entryPoints);
            return network;
        }

        public void Write(TextWriter writer)
        {
            writer.WriteLine(Nodes.Count);
            writer.WriteLine(ClassCount);
            for(int i = 0; i < ClassCount; i++)
            {
                writer.WriteDoubleDictionaryLine(EntryPoints.Where(ep => ep.Class == i).ToDictionary(ep => ep.Node, ep => ep.Lambda), Nodes.Count);
                foreach(var n in Nodes)
                {
                    writer.WriteDoubleDictionaryLine(n.Connections.Where(c => c.Class == i).ToDictionary(c => c.Destination, c => c.Weight), Nodes.Count);
                }
            }

            foreach (var n in Nodes)
            {
                writer.WriteDoubleLine(n.Mi);
            }

        }

        #endregion
    }
}
