using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    [DelimitedRecord(" ")]
    public class IISLogEntry
    {
        public DateTime date;

        public DateTime time;
    }
}
