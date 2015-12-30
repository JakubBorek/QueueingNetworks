using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueingNetworks
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new System.IO.StreamReader("one_class_open_bcmp.txt");
            var network = Network.Read(reader);
            var writer = new System.IO.StreamWriter("copy.txt");
            network.Write(writer);
            writer.Flush();
            Console.ReadKey();
        }
    }
}
