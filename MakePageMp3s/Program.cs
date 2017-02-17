

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuranLibrary;
using System.Diagnostics;

namespace MakePageMp3s
{
    class Program
    {

        
        static void Main(string[] args)
        {
            //string defaultPath;
            //if (args == null || args.Length ==0 || string.IsNullOrEmpty(args[0]))
            //    defaultPath = "";
            //else
            //    defaultPath = args[0];

            if (args.Length == 0)
            {
                Console.WriteLine("MakePageMp3s");
                Console.WriteLine("Parameters: list of directories to process");
                Console.WriteLine("for example MakePageMp3s.exe Sudais_128kbps Husary_46kbps");
                Console.WriteLine("for use parameter all for all in recitationsInXml.php");

            }
            var program = new MakePageMp3s();
            if (args.Length == 1 && args[0] == "all")
            {
                List<Recitation> recitations = MakePageMp3s.GetRecitations();
                foreach (Recitation recitation in recitations)
                {
                    program.Execute(recitation);
                }        
                return;
            }

            //do a specific one
            
            foreach (string recitation in args)
            {
                program.Execute(new Recitation { subfolder = recitation, name = recitation });
            }        
            
        }
        
    }
}
