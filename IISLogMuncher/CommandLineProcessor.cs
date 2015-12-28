using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class CommandLineProcessor
    {
        private Dictionary<string, string> optionDictionary = null;
        private string _options;

        public CommandLineProcessor()
        {
            Options = string.Empty;
        }

        public CommandLineProcessor(string options)
        {
            Options = options;
        }

        public string Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
                if (optionDictionary != null)
                {
                    optionDictionary.Clear();
                    optionDictionary = null;
                }
                optionDictionary = buildOptionDictionary(Options);
            }
        }

        private Dictionary<string, string> buildOptionDictionary(string value)
        {
            var od = new Dictionary<string, string>();
            for (int i = 0; i < Options.Length; i++)
            {
                if (i < Options.Length - 1 && Options[i + 1] == ':')
                {
                    od.Add(Options[i++].ToString(), "Y");
                }
                else
                {
                    od.Add(Options[i].ToString(), "N");
                }
            }
            return od;
        }

        public CommandLineOptions ProcessArgs(string[] args)
        {
            var clo = new CommandLineOptions();
            var newArgs = ReconstructArgs(args);
            for (int i = 0; i < newArgs.Count; i++)
            {
                switch (newArgs[i].Substring(0, 1))
                {
                    case "-":
                        if (isOption(newArgs[i].Substring(1, 1)))
                        {
                            if (expectsOptionArgument(newArgs[i].Substring(1, 1)))
                            {
                                if (i >= newArgs.Count - 1)
                                {
                                    throw new ArgumentException("Missing argument for: " + newArgs[i].Substring(1, 1));
                                }
                                clo.SetOption(newArgs[i].Substring(1, 1), newArgs[++i]);
                            }
                            else
                            {
                                clo.SetOption(newArgs[i].Substring(1, 1), "");
                            }
                        }
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

        private bool isOption(string option)
        {
            return optionDictionary.ContainsKey(option);
        }

        private bool expectsOptionArgument(string option)
        {
            if (!isOption(option))
            {
                return false;
            }
            string value = string.Empty;
            optionDictionary.TryGetValue(option, out value);
            return (value == "Y");
        }
    }
}
