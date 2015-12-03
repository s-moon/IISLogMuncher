using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace IISLogMuncher
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFiles { get; set; }

        [Option('s', DefaultValue = 0, HelpText = "Number of lines to skip at the start of the file.")]
        public int SkipLines { get; set; }
    }
}
