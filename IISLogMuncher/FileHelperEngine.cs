using FileHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static IISLogMuncher.Util;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class FileHelperEngine
    {
        #region class variables
        private const char skipOption = 's';
        private const char topOption = 't';
        private const char emptyLinesOption = 'e';
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private FileHelperEngine<IISLogEntry> engine;
        private CommandLineOptions clo;
        private int topResults = 10;
        #endregion

        #region constructors
        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="clo"></param>
        public FileHelperEngine(CommandLineOptions clo)
        {
            engine = new FileHelperEngine<IISLogEntry>();
            this.clo = clo;
        }
        #endregion

        #region methods
        /// <summary>
        /// Go through each of the files passed on the command line and process them.
        /// </summary>
        public void ProcessFileList()
        {
            SetEngineOptionsBasedOnCommandLine();
            foreach (var file in clo.GetNonOptions())
            {
                ProcessFile(file);
            }
        }

        /// <summary>
        /// Some of the engine options can be changed via the command line. This method
        /// performs that function.
        /// </summary>
        private void SetEngineOptionsBasedOnCommandLine()
        {
            int tmpIntResult;

            if (clo.IsOptionSet(skipOption))
            {
                if (IsValidNumberAndGreaterThanX(clo.GetOption(skipOption), 0, out tmpIntResult))
                {
                    engine.Options.IgnoreFirstLines = tmpIntResult;
                }
                else
                {
                    logger.Error("Error - Unable to convert '" + clo.GetOption(skipOption) +
                        "' into a number for the '" + skipOption + "' option which is greater than 0.");
                }
            }

            if (clo.IsOptionSet('t'))
            {
                if (IsValidNumberAndGreaterThanX(clo.GetOption(topOption), 0, out tmpIntResult))
                {
                    topResults = tmpIntResult;
                }
                else
                {
                    logger.Error("Error - Unable to convert '" + clo.GetOption('t') +
                        "' into a number for the 't' option which is greater than 0.");
                }
            }

            if (clo.IsOptionSet('i'))
            {
                engine.Options.IgnoreEmptyLines = true;
            }
        }

        /// <summary>
        /// Some command line options need integer arguments. This method ensures that they are
        /// valid and greater than a certain number. Usually zero in practice.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="greater"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool IsValidNumberAndGreaterThanX(string value, int greater, out int result)
        {
            bool isValid = false;
            if (Int32.TryParse(value, out result) && (result > greater))
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Process one given file. That is, produce stats on it.
        /// </summary>
        /// <param name="file"></param>
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

        /// <summary>
        /// Generate all the stats from the imported file.
        /// </summary>
        /// <param name="records"></param>
        private void ProvideFileStats(IISLogEntry[] records)
        {
            Dictionary<string, int> ips = new Dictionary<string, int>();
            Dictionary<string, int> twoOctetsOfIP = new Dictionary<string, int>();
            Dictionary<string, int> threeOctetsOfIP = new Dictionary<string, int>();
            Dictionary<string, int> popularStems = new Dictionary<string, int>();
            Dictionary<DateTime, int> hitsPerSecond = new Dictionary<DateTime, int>();
            int val;

            if (clo.IsOptionSet('c'))
            {
                DisplayRecordCount(records.Count());
            }

            foreach (var entry in records)
            {
                AddEntryToDictionary(ips, entry.c_ip);
                AddEntryToDictionary(twoOctetsOfIP, SquashIPAddressIntoOctets(entry.c_ip, 2));
                AddEntryToDictionary(threeOctetsOfIP, SquashIPAddressIntoOctets(entry.c_ip, 3));
                AddEntryToDictionary(popularStems, entry.cs_uri_stem);

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

        private void AddEntryToDictionary(Dictionary<string, int> dict, string value)
        {
            int val;

            if (dict.TryGetValue(value, out val))
                dict[value]++;
            else
                dict.Add(value, 1);
        }

        /// <summary>
        /// Produce a heading. That is a lump of text with enough equals ('=') characters
        /// underneath to give the impression of underlining.
        /// </summary>
        /// <param name="heading"></param>
        private void OutputHeading(string heading)
        {
            Console.WriteLine(heading);
            Console.WriteLine("=".PadLeft(heading.Length, '='));
        }

        /// <summary>
        /// Display the number of records in this imported file.
        /// </summary>
        /// <param name="c"></param>
        private void DisplayRecordCount(int c)
        {
            OutputHeading("Records: " + c);
        }

        /// <summary>
        /// For the purposes of reporting, we want to be able to see IP addresses based on the
        /// first, second or third group of numbers. E.g. 1.2.3.4. This function removes anything 
        /// beyond the octet we are interested in.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="octets"></param>
        /// <returns></returns>
        private string SquashIPAddressIntoOctets(string ip, int octets)
        {
            if (octets < 1 || octets > 3)
            {
                LogAndThrowException(new ArgumentException("Parameter octets must be between 1 and 3, inclusive."));
            }

            int pos = 0;
            for (int i = 0; i < octets; i++)
            {
                pos = ip.IndexOf('.', pos);
                if (pos == -1)
                {
                    logger.Error("This IP should have had a decimal point, but didn't when requesting the " + i + " of " + octets + " octets: " + ip);
                    return string.Empty;
                }
                pos++;
            }
            return ip.Substring(0, pos - 1);
        }
        #endregion 
    }
}
