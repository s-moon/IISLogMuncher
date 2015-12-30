using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class CommandLineOptions
    {
        private Dictionary<char, string> options = null;
        private List<string> nonOptions = null;

        public CommandLineOptions()
        {
            options = new Dictionary<char, string>();
            nonOptions = new List<string>();
        }

        public string GetOption(char option)
        {
            if (options.ContainsKey(option))
            {
                return options[option];
            }
            else
            {
                return string.Empty;
            }
        }

        public void SetOption(char option, string value)
        {
            options[option] = value;
        }

        public int GetOptionCount()
        {
            return options.Count;
        }

        public List<string> GetNonOptions()
        {
            return nonOptions;
        }

        public void AddNonOption(string param)
        {
            nonOptions.Add(param);
        }
    }
}
