using NLog;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using static IISLogMuncher.Util;

namespace IISLogMuncher
{
    public class CommandLineProcessor
    {
        // NLog
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Dictionary of known options
        private Dictionary<char, string> optionDictionary = null;

        // Constants used for options
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
                if (value != null && AreOnlyValidCharactersUsedForOptions(value))
                {
                    options = value;
                    optionDictionary = BuildOptionDictionary(options);
                }
                else
                {
                    LogAndThrowException(new ArgumentException("Options must be non-null and contain characters from [a-zA-Z0-9:]."));
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
                LogAndThrowException(new ArgumentException("Options must be non-null."));
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
            var newArgs = ReconstructSquashedArgumentsIntoSpacedArguments(args);

            var clo = new CommandLineOptions();

            SetDefaultArgumentsIfAny(clo);

            for (int i = 0; i < newArgs.Count; i++)
            {
                if (CouldBeAnOption(newArgs[i]) && newArgs[i].Length > 1)
                {
                    i = ProcessPossibleOption(newArgs, clo, i);
                }
                else
                {
                    ProcessNonOption(newArgs, clo, i);
                }
            }
            return clo;
        }

        private void SetDefaultArgumentsIfAny(CommandLineOptions clo)
        {
            string setting;
            foreach (char o in this.Options)
            {
                if (char.IsLetter(o) && (setting = ReadSetting(o + "OptionDefault")) != "Not Found")
                {
                    clo.SetOption(o, setting);
                }
            }
        }

        private string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                LogAndThrowException(new InvalidOperationException("Error reading app setting: " + key));
                return null;
            }
        }


        /// <summary>
        /// Handle cases where this clearly isn't an option of the form '-x'.
        /// </summary>
        /// <param name="newArgs"></param>
        /// <param name="clo"></param>
        /// <param name="i"></param>
        private void ProcessNonOption(List<string> newArgs, CommandLineOptions clo, int i)
        {
            clo.AddNonOption(newArgs[i]);
        }

        /// <summary>
        /// Handle cases where this *could be* an option like '-x'. Where it is a possible option is
        /// in the fact that someone might use '-x' but 'x' is not a declared option in the option list.
        /// </summary>
        /// <param name="newArgs"></param>
        /// <param name="clo"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private int ProcessPossibleOption(List<string> newArgs, CommandLineOptions clo, int i)
        {
            char optionCharacter = newArgs[i].ElementAt(1);
            if (IsKnownOption(optionCharacter))
            {
                if (ExpectsOptionArgument(optionCharacter))
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
                LogAndThrowException(new ArgumentException("Unknown option: " + optionCharacter));
            }

            return i;
        }

        /// <summary>
        /// Does this argument begin with the option character (-)?
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool CouldBeAnOption(string v)
        {
            if (!string.IsNullOrEmpty(v) && v.ElementAt(0) == OptionIndicator)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creates a dictionary made up with the list of possible options and whether they take arguments
        /// </summary>
        /// <param name="listOfOptions"></param>
        /// <returns></returns>
        private Dictionary<char, string> BuildOptionDictionary(string listOfOptions)
        {
            var od = new Dictionary<char, string>();
            for (int i = 0; i < listOfOptions.Length; i++)
            {
                if (i < listOfOptions.Length - 1 && Options[i + 1] == OptionArgument)
                {
                    od.Add(listOfOptions.ElementAt(i++), OptionArgument.ToString()); // skip colon
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
        private List<string> ReconstructSquashedArgumentsIntoSpacedArguments(string[] args)
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
        private bool IsKnownOption(char option)
        {
            if (optionDictionary == null)
            {
                LogAndThrowException(new ArgumentException("No options defined."));
            }
            return true;
        }

        /// <summary>
        /// Does this option need an argument to go with it?
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private bool ExpectsOptionArgument(char option)
        {
            if (IsKnownOption(option))
            {
                string value = string.Empty;
                optionDictionary.TryGetValue(option, out value);
                return (value == OptionArgument.ToString());
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Are these options valid? Must be alphanumeric or colon
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool AreOnlyValidCharactersUsedForOptions(string value)
        {
            return Regex.Matches(value, @"^[a-zA-Z0-9:]*$").Count != 0;
        }
        #endregion
    }
}
