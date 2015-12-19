using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public static class CommandLineOptions
    {
        private static Dictionary<string, string> options = new Dictionary<string, string>();
        private static List<string> loose = new List<string>();

        public static string GetOption(string name)
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

        public static void SetOption(string name, string value)
        {
            options[name] = value;
        }

        public static List<string> GetParameters()
        {
            return loose;
        }

        public static void AddParameter(string param)
        {
            loose.Add(param);
        }
    }
}
