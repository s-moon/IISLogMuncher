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
            Console.WriteLine("".PadLeft(55, '-'));
            Console.WriteLine("IIS Log Muncher v");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("-h".PadLeft(15, ' ') + " : this help text");
            Console.WriteLine("-s <number>".PadLeft(15, ' ') + " : skip <number> lines from input file");
            Console.WriteLine("".PadLeft(55, '-'));
        }
    }
}
