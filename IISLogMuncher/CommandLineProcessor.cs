using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public static class CommandLineProcessor
    {
        private static List<string> modifiedArgs = new List<string>();

        public static void ProcessArgs(string[] args)
        {
            var newArgs = ReconstructArgs(args);
            for (int i = 0; i < newArgs.Count; i++)
            {
                switch(newArgs[i])
                {
                    case "-s":
                        if (i >= newArgs.Count)
                        {
                            throw new ArgumentException("Missing option argument for 's'");
                        }
                        CommandLineOptions.SetOption("s", newArgs[++i]);
                        break;
                    default:
                        CommandLineOptions.AddParameter(newArgs[i]);
                        break;
                }
            }
        }

        private static List<string> ReconstructArgs(string[] args)
        {
            int i = 0;
            while (i < args.Length)
            {
                if (args[i][0] == '-')
                {
                    if (args[i].Length != 2)
                    {
                        modifiedArgs.Add(args[i].Substring(0, 2).ToLower());
                        modifiedArgs.Add(args[i].Substring(2));
                    }
                    else
                    {
                        modifiedArgs.Add(args[i].ToLower());
                    }
                }
                else
                {
                    modifiedArgs.Add(args[i]);
                }
                i++;
            }

            return modifiedArgs;
        }
    }
}
