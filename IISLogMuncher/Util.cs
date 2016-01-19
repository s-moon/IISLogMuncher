using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher
{
    public static class Util
    {
        #region class variables
        // NLog
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region methods
        public static void LogAndThrowException(Exception ex)
        {
            logger.Error(ex.Message);
            throw ex;
        }
        #endregion
    }
}
