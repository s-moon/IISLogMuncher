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
            Dictionary<string, int> twoOctetsOfIP = new Dictionary<string, int>();
            Dictionary<string, int> threeOctetsOfIP = new Dictionary<string, int>();
            Dictionary<string, int> popularStems = new Dictionary<string, int>();
            int topResults = 10;
            int val;

            if (clo.IsOptionSet('c'))
            {
                displayRecordCount(records.Count());
                Console.WriteLine();
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

                if (twoOctetsOfIP.TryGetValue(squashIntoOctetsOfIPAddress(entry.c_ip, 2), out val))
                    twoOctetsOfIP[squashIntoOctetsOfIPAddress(entry.c_ip, 2)]++;
                else
                    twoOctetsOfIP.Add(squashIntoOctetsOfIPAddress(entry.c_ip, 2), 1);

                if (threeOctetsOfIP.TryGetValue(squashIntoOctetsOfIPAddress(entry.c_ip, 3), out val))
                    threeOctetsOfIP[squashIntoOctetsOfIPAddress(entry.c_ip, 3)]++;
                else
                    threeOctetsOfIP.Add(squashIntoOctetsOfIPAddress(entry.c_ip, 3), 1);

                if (popularStems.TryGetValue(entry.cs_uri_stem, out val))
                    popularStems[entry.cs_uri_stem]++;
                else
                    popularStems.Add(entry.cs_uri_stem, 1);
            }

            outputHeading("Top " + topResults + " IP requests");
            foreach (var ip in ips.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            outputHeading("Top " + topResults + " 3 octet IP requests");
            foreach (var ip in threeOctetsOfIP.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            outputHeading("Top " + topResults + " 2 octet IP requests");
            foreach (var ip in twoOctetsOfIP.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            outputHeading("Top " + topResults + " popular stems");
            foreach (var popularPage in popularStems.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(popularPage.Value);
                Console.WriteLine(popularPage.Key);
                Console.WriteLine("~-~-");
            }
        }

        private void outputHeading(string heading)
        {
            Console.WriteLine(heading);
            Console.WriteLine("=".PadLeft(heading.Length, '='));
        }

        private void displayRecordCount(int c)
        {
            outputHeading("Records: " + c);
        }

        private string squashIntoOctetsOfIPAddress(string ip, int octets)
        {
            int pos = 0;
            for (int i = 0; i < octets; i++)
            {
                pos = ip.IndexOf('.', pos);
                if (pos == -1)
                {
                    return string.Empty;
                }
                pos++;
            }
            return ip.Substring(0, pos - 1);
        }
    }
}
