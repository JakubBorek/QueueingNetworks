using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    class OpenBcmp
    {
        public uint classNumber { get; private set; }
        public List<Node> nodes { get; private set; }
        public Dictionary<uint, double> entryLambdas { get; private set; }

        public OpenBcmp(uint classNumber, IList<Node> nodes, IDictionary<uint, double> entryLambdas)
        {
            this.classNumber = classNumber;
            this.nodes = new List<Node>(nodes);
            this.entryLambdas = new Dictionary<uint, double>(entryLambdas);
        }

        static public OpenBcmp Parse(TextReader reader)
        {
            uint nodeNumber = reader.ReadUintLine();
            uint classes = reader.ReadUintLine();
            var entryLambdas = reader.ReadDoubleDictionaryLine();
            var connectionDictionary = reader.ReadDoubleMatrixAsDictionaryList(nodeNumber);
            var miMatrix = reader.ReadDoubleMatrix(nodeNumber);
            var nodes = new List<Node>((int)nodeNumber);
            for (int i = 0; i < nodeNumber; i++)
            {
                var n = new Node(miMatrix[i], connectionDictionary[i]);
                nodes.Add(n);
            }
            return new OpenBcmp(classes, nodes, entryLambdas);
        }

        public List<double> calculateLambdas()
        {
            var lambdas = new double[nodes.Count];
            foreach (var p in entryLambdas)
            {
                addLambdasFromPoint(p.Key, p.Value, lambdas);
            }
            return lambdas.ToList();
        }

        public List<List<double>> calculateRhos()
        {
            var rhos = new List<List<double>>();
            var lambdas = calculateLambdas();
            for (uint i = 0; i < nodes.Count; i++)
            {
                rhos.Add(calculateRhosForNode(i, lambdas[(int)i]));
            }
            return rhos;
        }

        private List<double> calculateRhosForNode(uint nodeIdx, double lambda)
        {
            var rhos = new List<double>((int)classNumber);
            nodes[(int)nodeIdx].mi.ForEach(mi => rhos.Add(mi * lambda));
            return rhos;
        }

        private void addLambdasFromPoint(uint entryNode, double entryLambda, double[] lambdas)
        {
            lambdas[entryNode] += entryLambda;
            var node = nodes[(int)entryNode];
            foreach (var c in node.connections)
            {
                var lambda = entryLambda * c.Value;
                addLambdasFromPoint(c.Key, lambda, lambdas);
            }
        }

    }
}
