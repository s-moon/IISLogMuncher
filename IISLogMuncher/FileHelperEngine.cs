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
        public FileHelperEngine()
        {
            var engine = new FileHelperEngine<IISLogEntry>();
            engine.Options.IgnoreFirstLines = 4;
            engine.Options.IgnoreEmptyLines = true;
            var records = engine.ReadFile(@"E:\Projects\Open Source\IISLogMuncher\input.txt");

            foreach (var record in records)
            {
                Console.WriteLine(record.date);
            }
        }
    }
}
