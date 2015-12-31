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
        private const char OptionIndicator = '-';
        private const char OptionArgument = ':';
        private const string OptionNoArgument = "";
        private string options;

        #region properties
        /// <summary>
        /// Getter and Setter for the options which are of the form: "s:hv"
        /// </summary>
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
        #endregion

        #region constructors
        /// <summary>
        /// A standard object will have no options.
        /// </summary>
        public CommandLineProcessor()
        {
            Options = string.Empty;
        }

        /// <summary>
        /// Supply a list of options to initialise with. E.g. "s:hv"
        /// </summary>
        /// <param name="options"></param>
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
        #endregion

        #region methods
        /// <summary>
        /// Given an array of arguments, process them by building a dictionary of valid options and their argument values.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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
                            clo.SetOption(optionCharacter, OptionNoArgument);
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

        /// <summary>
        /// Does this argument begin with the option character (-)?
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool couldBeAnOption(string v)
        {
            if (!string.IsNullOrEmpty(v) && 
                    v.ElementAt(0) == OptionIndicator)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creates a dictionary made up with the list of possible options and whether they take arguments
        /// </summary>
        /// <param name="listOfOptions"></param>
        /// <returns></returns>
        private Dictionary<char, string> buildOptionDictionary(string listOfOptions)
        {
            var od = new Dictionary<char, string>();
            for (int i = 0; i < listOfOptions.Length; i++)
            {
                if (i < listOfOptions.Length - 1 && Options[i + 1] == OptionArgument)
                {
                    od.Add(listOfOptions.ElementAt(i++), OptionArgument.ToString());
                }
                else
                {
                    od.Add(listOfOptions.ElementAt(i), OptionNoArgument);
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
                    if (args[i][0] == OptionIndicator)
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

        /// <summary>
        /// Does this option exist in our dictionary of valid options?
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Does this option need an argument to go with it?
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private bool expectsOptionArgument(char option)
        {
            if (isKnownOption(option))
            {
                string value = string.Empty;
                optionDictionary.TryGetValue(option, out value);
                return (value == OptionArgument.ToString());
            }

            return false;
        }
        #endregion
    }
}
