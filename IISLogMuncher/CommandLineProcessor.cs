using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class CommandLineProcessor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, string> optionDictionary = null;
        private const char OPTION_INDICATOR = '-';
        private const char OPTION_ARGUMENT = ':';
        private const char OPTION_NO_ARGUMENT = 'X';
        private string _options;

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
                    string message = "Options must be non-null.";
                    logger.Error(message);
                    throw new ArgumentException(message);
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

        public CommandLineProcessor()
        {
            Options = string.Empty;
        }

        public CommandLineProcessor(string options)
        {
            if (options == null)
            {
                string message = "Options must be non-null.";
                logger.Error(message);
                throw new ArgumentException(message);
            }
            Options = options;
        }

        public CommandLineOptions ProcessArgs(string[] args)
        {
            var clo = new CommandLineOptions();
            var newArgs = reconstructSquashedArgumentsIntoSpacedArguments(args);
            for (int i = 0; i < newArgs.Count; i++)
            {
                if (string.IsNullOrEmpty(newArgs[i]))
                    continue;

                if (newArgs[i].Substring(0, 1) == OPTION_INDICATOR.ToString() && newArgs[i].Length > 1)
                {
                    string optionCharacter = newArgs[i].Substring(1, 1);
                    if (isOption(optionCharacter))
                    {
                        if (expectsOptionArgument(optionCharacter))
                        {
                            if (i >= newArgs.Count - 1)
                            {
                                string message = "Missing argument for: " + optionCharacter;
                                logger.Error(message);
                                throw new ArgumentException(message);
                            }
                            else
                            {
                                clo.SetOption(optionCharacter, newArgs[++i]);
                            }
                        }
                        else
                        {
                            clo.SetOption(optionCharacter, OPTION_NO_ARGUMENT.ToString());
                        }
                    }
                    else
                    {
                        string message = "Unknown option: " + optionCharacter;
                        logger.Error(message);
                        throw new ArgumentException(message);
                    }
                }
                else
                {
                    clo.AddNonOption(newArgs[i]);
                }
            }
            return clo;
        }

        private Dictionary<string, string> buildOptionDictionary(string value)
        {
            var od = new Dictionary<string, string>();
            for (int i = 0; i < Options.Length; i++)
            {
                if (i < Options.Length - 1 && Options[i + 1] == OPTION_ARGUMENT)
                {
                    od.Add(Options[i++].ToString(), OPTION_ARGUMENT.ToString());
                }
                else
                {
                    od.Add(Options[i].ToString(), OPTION_ARGUMENT.ToString());
                }
            }
            return od;
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
                    if (args[i][0] == OPTION_INDICATOR)
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
            if (optionDictionary == null)
            {
                string message = "No options defined.";
                logger.Error(message);
                throw new ArgumentException(message);
            }
            else
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
            return (value == OPTION_ARGUMENT.ToString());
        }
    }
}
