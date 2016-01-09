using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public class FileHelperEngine
    {
        public FileHelperEngine(CommandLineOptions clo)
        {
            var engine = new FileHelperEngine<IISLogEntry>();

            if (clo.IsOptionSet('s'))
            {
                engine.Options.IgnoreFirstLines = Int32.Parse(clo.GetOption('s'));
            }

            engine.Options.IgnoreEmptyLines = true;
            var records = engine.ReadFile(@"E:\Projects\Open Source\IISLogMuncher\input.txt");

            foreach (var record in records)
            {
                Console.WriteLine(record.date);
            }
        }
    }
}
