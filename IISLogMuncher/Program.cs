using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace IISLogMuncher
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("IIS Log Muncher starting.");

            var clp = new CommandLineProcessor("hs:");
            var clo = clp.ProcessArgs(args);

            if (clo.IsOptionSet('h'))
            {
                ShowHelpText();
            }

            logger.Info("IIS Log Muncher finished.");
        }

        private static void ShowHelpText()
        {
            Console.WriteLine("".PadLeft(40, '-'));
            Console.WriteLine("IIS Log Muncher v");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("-h".PadLeft(10, ' ') + " : help");
            Console.WriteLine("-s <arg>".PadLeft(10, ' ') + " : skip lines from input file");
            Console.WriteLine("".PadLeft(40, '-'));
        }
    }
}
