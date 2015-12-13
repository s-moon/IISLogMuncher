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
                switch(args[i])
                {
                    case "-s":
                        if (i == args.Length || Int32.TryParse(args[++i], out skipLines) == false)
                        {
                            throw new ArgumentException("Missing or invalid split lines number.");
                        }
                        Console.WriteLine("-s option : " + skipLines);
                        break;
                    default:
                        Console.WriteLine("Filename: " + args[i]);
                        break;
                }
                i++;
            }

            logger.Info("IIS Log Muncher finished.");
        }
    }
}
