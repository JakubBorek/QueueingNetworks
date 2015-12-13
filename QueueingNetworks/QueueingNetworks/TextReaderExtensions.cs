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
        public static uint ReadUintLine(this TextReader reader)
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
        public static Dictionary<uint, double> ReadDoubleDictionaryLine(this TextReader reader)
        {
            var list = reader.ReadDoubleListLine();
            var dict = new Dictionary<uint, double>();
            uint i = 0;
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
        public static List<Dictionary<uint, double>> ReadDoubleMatrixAsDictionaryList(this TextReader reader, uint rows)
        {
            var list = new List<Dictionary<uint, double>>();
            for(int i = 0; i < rows; i++)
            {
                list.Add(reader.ReadDoubleDictionaryLine());
            }
            return list;
        }
    }
}
