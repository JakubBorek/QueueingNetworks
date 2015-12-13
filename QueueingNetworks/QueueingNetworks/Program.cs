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
            var network = OpenBcmp.Parse(reader);
            var rhos = network.calculateRhos();
            foreach (var r in rhos)
            {
                PrintList(r);
            }
            Console.ReadKey();
        }

        private static void PrintList(IList<double> list)
        {
            foreach (var l in list)
            {
                Console.Write(l + " ");
                Console.WriteLine("");
            }
        }
    }
}
