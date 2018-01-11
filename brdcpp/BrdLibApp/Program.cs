using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using brd;

namespace BrdLibApp
{
    class Program
    {
        static void Main(string[] args)
        {

            BRD b = BRD.open();

            ServiceList ss = b.serviceList;

            foreach (var s in ss.toArray() )
                Console.WriteLine( "{0} - {1}", s.name, s.attr );

        }
    }
}
