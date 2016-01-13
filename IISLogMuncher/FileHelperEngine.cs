using FileHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class FileHelperEngine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public FileHelperEngine(CommandLineOptions clo)
        {
            var engine = new FileHelperEngine<IISLogEntry>();

            if (clo.IsOptionSet('s'))
            {
                engine.Options.IgnoreFirstLines = Int32.Parse(clo.GetOption('s'));
            }

            if (clo.IsOptionSet('i'))
            {
                engine.Options.IgnoreEmptyLines = true;
            }

            foreach (var file in clo.GetNonOptions())
            {
                logger.Info("[" + file + "]");
                //var records = engine.ReadFile(@"E:\Projects\Open Source\IISLogMuncher\" + file);
                var records = engine.ReadFile(@"D:\StephenMoon\GitHub\IISLogMuncher\" + file);
                ProcessFile(clo, records);
            }
        }

        private void ProcessFile(CommandLineOptions clo, IISLogEntry[] records)
        {
            Dictionary<string, int> ips = new Dictionary<string, int>();
            Dictionary<string, int> mutedIps = new Dictionary<string, int>();
            Dictionary<string, int> popularStems = new Dictionary<string, int>();
            int topResults = 10;
            int val;

            if (clo.IsOptionSet('c'))
            {
                Console.WriteLine("Records: {0}", records.Count());
            }

            if (clo.IsOptionSet('t'))
            {
                topResults = Int32.Parse(clo.GetOption('t'));
            }

            foreach (var entry in records)
            {
                if (ips.TryGetValue(entry.c_ip, out val))
                    ips[entry.c_ip]++;
                else
                    ips.Add(entry.c_ip, 1);

                if (mutedIps.TryGetValue(mutedIP(entry.c_ip), out val))
                    mutedIps[mutedIP(entry.c_ip)]++;
                else
                    mutedIps.Add(mutedIP(entry.c_ip), 1);

                if (popularStems.TryGetValue(entry.cs_uri_stem, out val))
                    popularStems[entry.cs_uri_stem]++;
                else
                    popularStems.Add(entry.cs_uri_stem, 1);
            }

            // sort list
            Console.WriteLine("Top " + topResults + " IP hits");
            foreach (var ip in ips.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Top " + topResults + " IP groups");
            foreach (var mutedIp in mutedIps.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(mutedIp.Key.PadRight(16) + mutedIp.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Top " + topResults + " Popular stems");
            foreach (var popularPage in popularStems.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(popularPage.Value);
                Console.WriteLine(popularPage.Key);
                Console.WriteLine("~-~-");
            }
        }

        private string mutedIP(string ip)
        {
            return ip.Substring(0, ip.LastIndexOf("."));
        }
    }
}
