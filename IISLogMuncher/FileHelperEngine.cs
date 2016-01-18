using FileHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class FileHelperEngine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private FileHelperEngine<IISLogEntry> engine;
        private CommandLineOptions clo;

        public FileHelperEngine(CommandLineOptions clo)
        {
            engine = new FileHelperEngine<IISLogEntry>();
            this.clo = clo;
        }

        private void SetEngineOptionsBasedOnCommandLine()
        {
            if (clo.IsOptionSet('s'))
            {
                engine.Options.IgnoreFirstLines = Int32.Parse(clo.GetOption('s'));
            }

            if (clo.IsOptionSet('i'))
            {
                engine.Options.IgnoreEmptyLines = true;
            }
        }

        public void ProcessFileList()
        {
            SetEngineOptionsBasedOnCommandLine();
            foreach (var file in clo.GetNonOptions())
            {
                ProcessFile(file);
            }
        }

        private void ProcessFile(string file)
        {
            logger.Info("[" + file + "]");
            try
            {
                //var records = engine.ReadFile(@"E:\Projects\Open Source\IISLogMuncher\" + file);
                var records = engine.ReadFile(@"D:\StephenMoon\GitHub\IISLogMuncher\" + file);
                ProvideFileStats(clo, records);
            }
            catch (DirectoryNotFoundException ex)
            {
                logger.Error("Error - Unable to reach directory: " + file + "; file will be skipped.");
            }
            catch (FileNotFoundException ex)
            {
                logger.Error("Error - Unable to open file: " + file + "; file will be skipped.");
            }
            catch (Exception ex)
            {
                logger.Error("Oops. Something catastrophic happened so skipping file: " + file);
                logger.Error(ex);
            }
        }

        private void ProvideFileStats(CommandLineOptions clo, IISLogEntry[] records)
        {
            Dictionary<string, int> ips = new Dictionary<string, int>();
            Dictionary<string, int> twoOctetsOfIP = new Dictionary<string, int>();
            Dictionary<string, int> threeOctetsOfIP = new Dictionary<string, int>();
            Dictionary<string, int> popularStems = new Dictionary<string, int>();
            Dictionary<DateTime, int> hitsPerSecond = new Dictionary<DateTime, int>();
            int topResults = 10;
            int val;

            if (clo.IsOptionSet('c'))
            {
                displayRecordCount(records.Count());
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

                if (hitsPerSecond.TryGetValue(entry.time, out val))
                    hitsPerSecond[entry.time]++;
                else
                    hitsPerSecond.Add(entry.time, 1);
            }

            Console.WriteLine();
            outputHeading("Top " + topResults + " hits per second");
            foreach (var hits in hitsPerSecond.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(hits.Key + " " + hits.Value);
            }

            Console.WriteLine();
            outputHeading("Average hits per second");
            Console.WriteLine("{0:0}", hitsPerSecond.Values.Average());

            Console.WriteLine();
            outputHeading("Average hits per second, by hour");
            var groupedHourlyList = hitsPerSecond.GroupBy(u => u.Key.Hour)
                                      .Select(grp => new { GroupID = grp.Key, subList = grp.ToList() })
                                      .ToList();
            foreach (var hourlyHits in groupedHourlyList)
            {
                Console.WriteLine("{0,2:D2}-{1,2:D2} : {2:0}", hourlyHits.GroupID, hourlyHits.GroupID+ 1, hourlyHits.subList.Average(c => c.Value));
            }

            Console.WriteLine();
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
