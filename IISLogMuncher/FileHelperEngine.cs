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
                var records = engine.ReadFile(@"E:\Projects\Open Source\IISLogMuncher\" + file);

                if (clo.IsOptionSet('c'))
                {
                    Console.WriteLine("Records: {0}", records.Count());
                }

                foreach (var record in records)
                {
                    Console.WriteLine(record.date);
                }
            }
        }
    }
}
