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
        private Dictionary<char, string> optionDictionary = null;
        private const char OPTION_INDICATOR = '-';
        private const char OPTION_ARGUMENT = ':';
        private const char OPTION_NO_ARGUMENT = 'X';
        private string options;

        public string Options
        {
            get
            {
                return options;
            }
            set
            {
                if (value != null)
                {
                    options = value;
                    optionDictionary = buildOptionDictionary(options);
                }
                else
                {
                    string message = "Options must be non-null.";
                    logger.Error(message);
                    throw new ArgumentException(message);
                }
            }
        }

        public CommandLineProcessor()
        {
            Options = string.Empty;
        }

        public CommandLineProcessor(string options)
        {
            if (options != null)
            {
                Options = options;
            }
            else
            {
                string message = "Options must be non-null.";
                logger.Error(message);
                throw new ArgumentException(message);
            }
            
        }

        public CommandLineOptions ProcessArgs(string[] args)
        {
            var newArgs = reconstructSquashedArgumentsIntoSpacedArguments(args);

            var clo = new CommandLineOptions();
            for (int i = 0; i < newArgs.Count; i++)
            {
                if (couldBeAnOption(newArgs[i]) && newArgs[i].Length > 1)
                {
                    char optionCharacter = newArgs[i].ElementAt(1);
                    if (isKnownOption(optionCharacter))
                    {
                        if (expectsOptionArgument(optionCharacter))
                        {
                            if (i < newArgs.Count - 1)
                            {
                                clo.SetOption(optionCharacter, newArgs[++i]);
                            }
                            else
                            {
                                string message = "Missing argument for: " + optionCharacter;
                                logger.Error(message);
                                throw new ArgumentException(message);
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

        private bool couldBeAnOption(string v)
        {
            if (!string.IsNullOrEmpty(v) && 
                    v.ElementAt(0) == OPTION_INDICATOR)
                return true;
            else
                return false;
        }

        private Dictionary<char, string> buildOptionDictionary(string listOfOptions)
        {
            var od = new Dictionary<char, string>();
            for (int i = 0; i < listOfOptions.Length; i++)
            {
                if (i < listOfOptions.Length - 1 && Options[i + 1] == OPTION_ARGUMENT)
                {
                    od.Add(listOfOptions.ElementAt(i++), OPTION_ARGUMENT.ToString());
                }
                else
                {
                    od.Add(listOfOptions.ElementAt(i), OPTION_NO_ARGUMENT.ToString());
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

        private bool isKnownOption(char option)
        {
            if (optionDictionary != null)
            {
                return optionDictionary.ContainsKey(option);
            }
            else
            {
                string message = "No options defined.";
                logger.Error(message);
                throw new ArgumentException(message);
            }
        }

        private bool expectsOptionArgument(char option)
        {
            if (isKnownOption(option))
            {
                string value = string.Empty;
                optionDictionary.TryGetValue(option, out value);
                return (value == OPTION_ARGUMENT.ToString());
            }

            return false;
        }
    }
}
