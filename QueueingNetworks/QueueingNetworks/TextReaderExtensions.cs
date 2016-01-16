using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    static class TextReaderExtensions
    {
        public static uint ReadSingleUintLine(this TextReader reader)
        {
            string line = reader.ReadLine();
            return uint.Parse(line);
        }
        public static double ReadDoubleLine(this TextReader reader)
        {
            string line = reader.ReadLine();
            return double.Parse(line);
        }

        public static List<double> ReadDoubleListLine(this TextReader reader)
        {
            string line = reader.ReadLine();
            var values = line.Split(' ');
            return values.Select(v => double.Parse(v)).ToList();
        }

        public static List<int> ReadIntListLine(this TextReader reader)
        {
            string line = reader.ReadLine();
            var values = line.Split(' ');
            return values.Select(v => int.Parse(v)).ToList();
        }
        public static Dictionary<int, double> ReadDoubleDictionaryLine(this TextReader reader)
        {
            var list = reader.ReadDoubleListLine();
            var dict = new Dictionary<int, double>();
            int i = 0;
            foreach (var d in list)
            {
                if (d != 0)
                {
                    dict.Add(i, d);
                }
                i++;
            }
            return dict;
        }
        public static List<List<double>> ReadDoubleMatrix(this TextReader reader, uint rows)
        {
            var result = new List<List<double>>();
            for (int i = 0; i < rows; i++)
            {
                result.Add(reader.ReadDoubleListLine());
            }
            return result;
        }
        public static List<Dictionary<int, double>> ReadDoubleMatrixAsDictionaryList(this TextReader reader, uint rows)
        {
            var list = new List<Dictionary<int, double>>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(reader.ReadDoubleDictionaryLine());
            }
            return list;
        }
        public static List<Tuple<int, Connection>> ReadDoubleMatrixAsConnectionList(this TextReader reader, uint rows, int classId)
        {
            var connectionList = new List<Tuple<int, Connection>>();
            var dictionaryList = reader.ReadDoubleMatrixAsDictionaryList(rows);
            for (int i = 0; i < rows; i++)
            {
                connectionList.AddRange(dictionaryList[i].Select(p => Tuple.Create(i, new Connection(p.Key, classId, p.Value))));
            }

            return connectionList;
        }

        public static List<Node.NodeType> ReadNodeTypesLine(this TextReader reader)
        {
            string line = reader.ReadLine();
            var values = line.Split(' ');
            return values.Select(v => v == "1" ? Node.NodeType.Type1 : Node.NodeType.Type3).ToList();
        }
    }
}
