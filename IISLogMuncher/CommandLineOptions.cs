using NLog;
using System;
using System.Collections.Generic;
using static IISLogMuncher.Util;

namespace IISLogMuncher
{
    public class CommandLineOptions
    {
        #region class variables
        // NLog
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<char, string> options = null;
        private List<string> nonOptions = null;
        #endregion

        #region constructors
        /// <summary>
        /// Create a standard object
        /// </summary>
        public CommandLineOptions()
        {
            options = new Dictionary<char, string>();
            nonOptions = new List<string>();
        }
        #endregion

        #region methods
        /// <summary>
        /// Has the requested option been set by the user?
        /// </summary>
        /// <param name="option">Character representing the option to search for. E.g. 'h'.</param>
        /// <returns></returns>
        public bool IsOptionSet(char option)
        {
            return options.ContainsKey(option);
        }

        /// <summary>
        /// Return the value supplied with an option. String.Empty if none supplied. Exception if it doesn't exist.
        /// </summary>
        /// <param name="option">Character representing the option to retrieve. E.g. 'h'.</param>
        /// <returns></returns>
        public string GetOption(char option)
        {
            if (!IsOptionSet(option))
            {
                LogAndThrowException(new InvalidOperationException("Option: " + option + " does not exist."));
            }

            return options[option];
        }

        /// <summary>
        /// Set the value of an option to value.
        /// </summary>
        /// <param name="option">Character representing the option to assign. E.g. 'h'.</param>
        /// <param name="value">Value of the option, should it have one. Empty string otherwise.</param>
        public void SetOption(char option, string value)
        {
            options[option] = value;
        }

        /// <summary>
        /// Return the number of options that are known.
        /// </summary>
        /// <returns></returns>
        public int GetOptionCount()
        {
            return options.Count;
        }

        /// <summary>
        /// Return a list of options which aren't of this form: -s
        /// </summary>
        /// <returns></returns>
        public List<string> GetNonOptions()
        {
            return nonOptions;
        }

        /// <summary>
        /// Add an option which isn't of the form: -s
        /// </summary>
        /// <param name="param">A non-option such as 'filename.txt'.</param>
        public void AddNonOption(string param)
        {
            nonOptions.Add(param);
        }
        #endregion
    }
}
