using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.Log
{
    public static class LogManager
    {

        /// <summary>
        /// Log Basico
        /// </summary>
        private static BaseLog m_log = new BaseLog();

        public static BaseLog GetLog()
        {
            return m_log;
        }

    }
}
