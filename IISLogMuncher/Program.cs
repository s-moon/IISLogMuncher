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

            var modifiedArgs = CommandLineProcessor.ProcessArgs(args);

            foreach (var item in modifiedArgs)
                Console.WriteLine("="+item);

            logger.Info("IIS Log Muncher finished.");
        }
    }
}
