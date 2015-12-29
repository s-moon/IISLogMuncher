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
            if (options == null)
            {
                throw new ArgumentException("Options must be non-null.");
            }
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
                if (value == null)
                {
                    throw new ArgumentException("Options must be non-null.");
                }
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
                // todo: use constants
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
            var newArgs = reconstructSquashedArgumentsIntoSpacedArguments(args);
            for (int i = 0; i < newArgs.Count; i++)
            {
                if (string.IsNullOrEmpty(newArgs[i]))
                    continue;

                if (newArgs[i].Substring(0, 1) == "-" && newArgs[i].Length > 1)
                {
                    string optionCharacter = newArgs[i].Substring(1, 1);
                    if (isOption(optionCharacter))
                    {
                        if (expectsOptionArgument(optionCharacter))
                        {
                            if (i >= newArgs.Count - 1)
                            {
                                throw new ArgumentException("Missing argument for: " + optionCharacter);
                            }
                            else
                            {
                                clo.SetOption(optionCharacter, newArgs[++i]);
                            }
                        }
                        else
                        {
                            clo.SetOption(optionCharacter, string.Empty);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Unknown option: " + optionCharacter);
                    }
                }
                else
                {
                    clo.AddNonOption(newArgs[i]);
                }
            }
            return clo;
        }

        /// <summary>
        /// Changes arguments of this form: -s3 into: -s 3
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<string> reconstructSquashedArgumentsIntoSpacedArguments(string[] args)
        {
            var modifiedArgs = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                if (!string.IsNullOrEmpty(args[i]))
                {
                    if (args[i][0] == '-')
                    {
                        if (args[i].Length > 2)
                        {
                            modifiedArgs.Add(args[i].Substring(0, 2));
                            modifiedArgs.Add(args[i].Substring(2));
                        }
                        else
                        {
                            modifiedArgs.Add(args[i]);
                        }
                    }
                    else
                    {
                        modifiedArgs.Add(args[i]);
                    }
                }
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
