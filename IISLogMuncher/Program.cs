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

            List<String> modifiedArgs = new List<String>();
            int i = 0;
            while (i < args.Length)
            {
                if (args[i][0] == '-' && args[i].Length != 2)
                {
                    modifiedArgs.Add(args[i].Substring(0, 2).ToLower());
                    modifiedArgs.Add(args[i].Substring(2));
                }
                else
                {
                    modifiedArgs.Add(args[i]);
                }
                i++;
            }

            foreach (var item in modifiedArgs)
                Console.WriteLine("="+item);

            logger.Info("IIS Log Muncher finished.");
        }
    }
}
