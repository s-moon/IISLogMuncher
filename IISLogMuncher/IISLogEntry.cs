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
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime date;

        public string time;

        public string cs_method;

        public string cs_uri_stem;

        public string cs_uri_query;

        public string s_port;

        public string cs_username;

        public string c_ip;

        public string cs;

        public string sc_status;

        public string sc_substatus;

        public string sc_win32_status;

        public string sc_bytes;

        public string time_taken;

    }
}
//ate time cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) sc-status sc-substatus sc-win32-status sc-bytes time-taken

