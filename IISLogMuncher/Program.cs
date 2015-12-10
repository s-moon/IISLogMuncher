using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using NLog;

namespace IISLogMuncher
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("IIS Log Muncher starting.");

            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            if (result.Errors.Count() == 0)
            {
                Console.WriteLine("Filenames: " + result.Value.InputFiles);
                Console.WriteLine("Skip Lines: " + result.Value.SkipLines);
            }

            logger.Info("IIS Log Muncher finished.");
        }
    }
}
