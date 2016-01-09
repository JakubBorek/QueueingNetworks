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
        public IReadOnlyList<int> ClassMembersCounts { get; private set; }

        public Network(IReadOnlyList<Node> nodes, IReadOnlyList<int> classCount)
        {
            Nodes = new List<Node>(nodes);
            ClassMembersCounts = new List<int>(classCount);
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
            var classMembersCount = reader.ReadIntListLine();
            if(classMembersCount.Count != classCount)
            {
                throw new ArgumentException("");
            }
            var connections = new List<Tuple<int, Connection>>();
            for (int i = 0; i < classCount; i++)
            {
                connections.AddRange(reader.ReadDoubleMatrixAsConnectionList(nodeCount, i));
            }
            var miMatrix = reader.ReadDoubleMatrix(nodeCount);
            var nodes = new List<Node>((int)nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                nodes.Add(new Node(i, miMatrix[i], connections.Where(p => p.Item1 == i).Select(p => p.Item2).ToList()));
            }
            var network = new Network(nodes, classMembersCount);
            return network;
        }

        public void Write(TextWriter writer)
        {
            writer.WriteLine(Nodes.Count);
            writer.WriteLine(ClassCount);
            writer.WriteIntLine(ClassMembersCounts);

            for (int i = 0; i < ClassCount; i++)
            {
                foreach (var n in Nodes)
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
