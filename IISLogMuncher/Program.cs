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

            int skipLines = 0;
            int i = 0;
            while (i < args.Length)
            {
                if (args[i][0] == '-')
                {
                    switch (args[i].Substring(0,2))
                    {
                        case "-s":
                            if (i == args.Length || Int32.TryParse(args[++i], out skipLines) == false)
                            {
                                throw new ArgumentException("Missing or invalid split lines number.");
                            }
                            Console.WriteLine("-s option : " + skipLines);
                            break;
                        default:
                            Console.WriteLine("Unknown option: " + args[i]);
                            break;
                    }
                }
                else
                {
                    // not an option with a hyphen
                    Console.WriteLine("Filename: " + args[i]);
                }
                i++;
            }

            logger.Info("IIS Log Muncher finished.");
        }
    }
}
