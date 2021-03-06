﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using brd;

namespace brdnet
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Console.WriteLine( "==== {0} LIDS ===", BRD.lidList.Length );

            foreach (BRD_Info info in BRD.info)
            {
                Console.WriteLine("NAME: {0}", info.name);
                Console.WriteLine("PID: {0}", info.pid); 

            }

             * */
            System.Collections.Hashtable brdinfo = new System.Collections.Hashtable()
            {

               { "pid" , 2}

            };


            brdinfo["lid"] = 1;

            BRD b = new BRD("tapir[2312412]");

            //BRD b = new BRD( "bambpex(pid=11854,ipaddr=128.0.0.1)/m212x128,devreg" );

            ServiceList serviceList = b.serviceList;

            Console.WriteLine("==== {0} Services ===", serviceList.Length);

            for (int i = 0; i < serviceList.Length; i++)
            {
                Console.WriteLine("NAME: {0}", serviceList[i].name);
            }

            Int32 c = b.rgio.ri(0, 0x100);

            Console.WriteLine("TRD ID: {0}", c);  
        }
    }
}
