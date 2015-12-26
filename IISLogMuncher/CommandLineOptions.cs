﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class CommandLineOptions
    {
        private Dictionary<string, string> options = null;
        private List<string> nonOptions = null;

        public CommandLineOptions()
        {
            options = new Dictionary<string, string>();
            nonOptions = new List<string>();
        }

        public string GetOption(string name)
        {
            if (options.ContainsKey(name))
            {
                return options[name];
            }
            else
            {
                return string.Empty;
            }
        }

        public void SetOption(string name, string value)
        {
            options[name] = value;
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
