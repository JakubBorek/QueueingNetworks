using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    static class TextWriterExtensions
    {
        public static void WriteDoubleLine(this TextWriter writer, IReadOnlyList<double> values)
        {
            var stringBuilder = new StringBuilder();
            foreach (var v in values)
            {
                stringBuilder.Append(v);
                stringBuilder.Append(' ');
            }
            stringBuilder.TrimEnd();
            writer.WriteLine(stringBuilder);
        }

        public static void WriteIntLine(this TextWriter writer, IReadOnlyList<int> values)
        {
            var stringBuilder = new StringBuilder();
            foreach (var v in values)
            {
                stringBuilder.Append(v);
                stringBuilder.Append(' ');
            }
            stringBuilder.TrimEnd();
            writer.WriteLine(stringBuilder);
        }

        public static void WriteDoubleDictionaryLine(this TextWriter writer, IDictionary<int, double> dictionary, int length)
        {
            var list = new List<double>(length);
            for (int i = 0; i < length; i++)
            {
                list.Add(dictionary.ContainsKey(i) ? dictionary[i] : 0);
            }
            writer.WriteDoubleLine(list);
        }
        public static void WriteNodeTypeList(this TextWriter writer, IReadOnlyList<Node.NodeType> values)
        {
            var stringBuilder = new StringBuilder();
            foreach (var v in values)
            {
                string asString = v == Node.NodeType.Type1 ? "1" : "3";
                stringBuilder.Append(asString);
                stringBuilder.Append(' ');
            }
            stringBuilder.TrimEnd();
            writer.WriteLine(stringBuilder);

        }
        public static void Write(this TextWriter writer, Node.NodeType type)
        {
            string asString = type == Node.NodeType.Type1 ? "1" : "3";
            writer.Write(asString);

        }
    }
}
