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

            var clp = new CommandLineProcessor("achis:t:");
            var clo = clp.ProcessArgs(args);

            if (clo.IsOptionSet('h'))
            {
                ShowHelpText();
            }

            var fhe = new FileHelperEngine(clo);
            fhe.ProcessFileList();

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
            Console.WriteLine("-a".PadLeft(leftPadding, ' ') + " : show all records");
            Console.WriteLine("-c".PadLeft(leftPadding, ' ') + " : count number of records");
            Console.WriteLine("-h".PadLeft(leftPadding, ' ') + " : this help text");
            Console.WriteLine("-i".PadLeft(leftPadding, ' ') + " : ignore empty lines");
            Console.WriteLine("-r".PadLeft(leftPadding, ' ') + " : <report number list>");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 0 = hits per second");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 1 = avg hits per second");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 2 = avg hits per second, per hour");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 3 = ips");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 4 = 3 octet ips");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 5 = 2 octet ips");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 6 = stems");
            Console.WriteLine("  ".PadLeft(leftPadding, ' ') + " : 7 = queries");
            Console.WriteLine("-s <number>".PadLeft(leftPadding, ' ') + " : skip <number> lines from start of input file");
            Console.WriteLine("-t <number>".PadLeft(leftPadding, ' ') + " : top <number> of results to show in summaries");
            Console.WriteLine("See app.config for default options.");
            Console.WriteLine("".PadLeft(linePadding, '-'));
        }
    }
}
