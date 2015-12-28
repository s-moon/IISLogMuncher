using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class CommandLineProcessor
    {
        public CommandLineOptions ProcessArgs(string[] args)
        {
            var clo = new CommandLineOptions();
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
                        clo.SetOption("s", newArgs[++i]);
                        break;
                    default:
                        clo.AddNonOption(newArgs[i]);
                        break;
                }
            }
            return clo;
        }

        private List<string> ReconstructArgs(string[] args)
        {
            int i = 0;
            var modifiedArgs = new List<string>();
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
