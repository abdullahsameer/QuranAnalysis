

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuranLibrary;
using System.Diagnostics;
using BinaryMiracleAnalysis;

namespace MakePageMp3s
{
    class Program
    {
        static void Main(string[] args)
        {

            Analyze a = new Analyze();
            do
            {
                a.Go();
                Console.ReadKey();
            } while (true);

            //for (var i = 0; i < 1000000; i++)
            //{
            //    a.Go();
            //    //Console.ReadKey();
            //}
        }
        
    }
}
