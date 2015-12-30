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
            foreach(var v in values)
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
            for(int i = 0; i < length; i++)
            {
                list.Add(dictionary.ContainsKey(i) ? dictionary[i] : 0);
            }
            writer.WriteDoubleLine(list);
        }
    }
}
