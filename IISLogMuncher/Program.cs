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
            int linePadding = 65;
            int leftPadding = 15;
            Console.WriteLine("".PadLeft(linePadding, '-'));
            Console.WriteLine("IIS Log Muncher v");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("-h".PadLeft(leftPadding, ' ') + " : this help text");
            Console.WriteLine("-s <number>".PadLeft(leftPadding, ' ') + " : skip <number> lines from start of input file");
            Console.WriteLine("".PadLeft(linePadding, '-'));
        }
    }
}
