using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace IISLogMuncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            if (result.Errors.Count() == 0)
            {
                Console.WriteLine("Filenames: " + result.Value.InputFiles);
                Console.WriteLine("Skip Lines: " + result.Value.SkipLines);
            }
        }
    }
}
