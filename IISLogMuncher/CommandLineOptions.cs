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

        public static string Option(string name)
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
    }
}
