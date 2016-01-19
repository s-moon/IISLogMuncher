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
        #region class variables
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private FileHelperEngine<IISLogEntry> engine;
        private CommandLineOptions clo;
        #endregion

        #region constructors
        public FileHelperEngine(CommandLineOptions clo)
        {
            engine = new FileHelperEngine<IISLogEntry>();
            this.clo = clo;
        }
        #endregion

        #region methods
        public void ProcessFileList()
        {
            SetEngineOptionsBasedOnCommandLine();
            foreach (var file in clo.GetNonOptions())
            {
                ProcessFile(file);
            }
        }

        private void SetEngineOptionsBasedOnCommandLine()
        {
            if (clo.IsOptionSet('s'))
            {
                int result;

                if (Int32.TryParse(clo.GetOption('s'), out result))
                {
                    if (result > 0)
                    {
                        engine.Options.IgnoreFirstLines = result;
                    }
                    else
                    {
                        logger.Error("Error - the 's' option must be supplied with a number greater than zero.");
                    }
                }
                else
                {
                    logger.Error("Error - Unable to convert '" + clo.GetOption('s') + 
                        "' into a number for the 's' option.");
                }
            }

            if (clo.IsOptionSet('i'))
            {
                engine.Options.IgnoreEmptyLines = true;
            }
        }

        private void ProcessFile(string file)
        {
            logger.Info("[" + file + "]");
            try
            {
                //var records = engine.ReadFile(@"E:\Projects\Open Source\IISLogMuncher\" + file);
                var records = engine.ReadFile(@"D:\StephenMoon\GitHub\IISLogMuncher\" + file);
                ProvideFileStats(records);
            }
            catch (DirectoryNotFoundException)
            {
                logger.Error("Error - Unable to reach directory: " + file + "; file will be skipped.");
            }
            catch (FileNotFoundException)
            {
                logger.Error("Error - Unable to open file: " + file + "; file will be skipped.");
            }
            catch (Exception ex)
            {
                logger.Error("Oops. Something catastrophic happened so skipping file: " + file);
                logger.Error("Here's the detailed error message in case you were wondering:");
                logger.Error(ex);
            }
        }

        private void ProvideFileStats(IISLogEntry[] records)
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
                DisplayRecordCount(records.Count());
            }

            if (clo.IsOptionSet('t'))
            {
                int result;

                if (Int32.TryParse(clo.GetOption('t'), out result))
                {
                    if (result > 0)
                    {
                        topResults = result;
                    }
                    else
                    {
                        logger.Error("Error - the 't' option must be supplied with a number greater than zero.");
                    } 
                }
                else
                {
                    logger.Error("Error - Unable to convert '" + clo.GetOption('t') +
                        "' into a number for the 't' option.");
                }
            }

            foreach (var entry in records)
            {
                if (ips.TryGetValue(entry.c_ip, out val))
                    ips[entry.c_ip]++;
                else
                    ips.Add(entry.c_ip, 1);

                if (twoOctetsOfIP.TryGetValue(SquashIPAddressIntoOctets(entry.c_ip, 2), out val))
                    twoOctetsOfIP[SquashIPAddressIntoOctets(entry.c_ip, 2)]++;
                else
                    twoOctetsOfIP.Add(SquashIPAddressIntoOctets(entry.c_ip, 2), 1);

                if (threeOctetsOfIP.TryGetValue(SquashIPAddressIntoOctets(entry.c_ip, 3), out val))
                    threeOctetsOfIP[SquashIPAddressIntoOctets(entry.c_ip, 3)]++;
                else
                    threeOctetsOfIP.Add(SquashIPAddressIntoOctets(entry.c_ip, 3), 1);

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
            OutputHeading("Top " + topResults + " hits per second");
            foreach (var hits in hitsPerSecond.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(hits.Key + " " + hits.Value);
            }

            Console.WriteLine();
            OutputHeading("Average hits per second");
            Console.WriteLine("{0:0}", hitsPerSecond.Values.Average());

            Console.WriteLine();
            OutputHeading("Average hits per second, by hour");
            var groupedHourlyList = hitsPerSecond.GroupBy(u => u.Key.Hour)
                                      .Select(grp => new { GroupID = grp.Key, subList = grp.ToList() })
                                      .ToList();
            foreach (var hourlyHits in groupedHourlyList)
            {
                Console.WriteLine("{0,2:D2}-{1,2:D2} : {2:0}", hourlyHits.GroupID, hourlyHits.GroupID+ 1, hourlyHits.subList.Average(c => c.Value));
            }

            Console.WriteLine();
            OutputHeading("Top " + topResults + " IP requests");
            foreach (var ip in ips.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            OutputHeading("Top " + topResults + " 3 octet IP requests");
            foreach (var ip in threeOctetsOfIP.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            OutputHeading("Top " + topResults + " 2 octet IP requests");
            foreach (var ip in twoOctetsOfIP.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(ip.Key.PadRight(16) + ip.Value);
            }

            Console.WriteLine();
            OutputHeading("Top " + topResults + " popular stems");
            foreach (var popularPage in popularStems.OrderByDescending(v => v.Value).Take(topResults))
            {
                Console.WriteLine(popularPage.Value);
                Console.WriteLine(popularPage.Key);
                Console.WriteLine("~-~-");
            }
        }

        private void OutputHeading(string heading)
        {
            Console.WriteLine(heading);
            Console.WriteLine("=".PadLeft(heading.Length, '='));
        }

        private void DisplayRecordCount(int c)
        {
            OutputHeading("Records: " + c);
        }

        private string SquashIPAddressIntoOctets(string ip, int octets)
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
        #endregion 
    }
}
