using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Reflection;

namespace IISLogMuncher
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("IIS Log Muncher (" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")" + " starting.");

            var clp = new CommandLineProcessor("his:");
            var clo = clp.ProcessArgs(args);

            if (clo.IsOptionSet('h'))
            {
                ShowHelpText();
            }

            var fhe = new FileHelperEngine(clo);

            logger.Info("IIS Log Muncher finished.");
        }

        private static void ShowHelpText()
        {
            int linePadding = 65;
            int leftPadding = 15;
            Console.WriteLine("".PadLeft(linePadding, '-'));
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("IIS Log Muncher v" + version.Major + "." + version.Minor);
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("-h".PadLeft(leftPadding, ' ') + " : this help text");
            Console.WriteLine("-i".PadLeft(leftPadding, ' ') + " : ignore empty lines");
            Console.WriteLine("-s <number>".PadLeft(leftPadding, ' ') + " : skip <number> lines from start of input file");
            Console.WriteLine("".PadLeft(linePadding, '-'));
        }
    }
}
