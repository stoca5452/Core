using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.Log
{
   public class SaveLogs
    {
        #region SaveLogs Fields
        /// <summary>
        /// Ruta donde se creara el archivo para el log
        /// </summary>
        private static string sPathLog;

        /// <summary>
        /// Nombre del archivo log
        /// </summary>
        private static string sLogFileName;

        /// <summary>
        /// Establece los parametros de configuracion para el logger de errores
        /// </summary>
        /// <param name="LogPath">Ruta donde se guardar el arhivo de LOGS</param>
        /// <param name="LogName">Nombre del Archivo LOG</param>
        public virtual void SetLogSettings(string LogPath,string LogName)
        {
            sPathLog = LogPath;
            sLogFileName = LogName;

        }
        #endregion SaveLogs Fields

        /// <summary>
        /// Retorna el Path del Archivo Log
        /// </summary>
        /// <returns></returns>
        protected static string GetLogPath()
        {
            return sPathLog ;
        }
        /// <summary>
        /// Guarda el mesanje de error suministrado
        /// </summary>
        /// <param name="Message">Mensaje</param>
        protected virtual void CreateLog(string Message)
        {
            // Ruta + nombre del archivo log a crear
            string sLogFileName = GetLogFileName();
            string sFullPath = sPathLog + "\\" + sLogFileName;
            if (sPathLog != null)
            {
                if (sLogFileName != null)
                {
                    if (Stoca.Common.ToolKit.IsExistsFile(sPathLog, sLogFileName))
                    {
                        AddLogMessage(sFullPath, Message);
                    }
                    else
                    {
                        CreateAndAddLogMessage(sFullPath, Message);
                    }
                }        
            }
            
        }

        /// <summary>
        /// Graba la excepcion
        /// </summary>
        /// <param name="message">Mensaje a grabar</param>
        public virtual void LogExeption(string message)
        {
            CreateLog(message);
        }

        /// <summary>
        /// Crea el archivo LOG y añade el mensaje de error
        /// </summary>
        /// <param name="FullPath">Path del archivo log completo</param>
        /// <param name="Message">Mensaje a grabar</param>
        protected virtual void CreateAndAddLogMessage(string FullPath,string Message)
        {
            System.IO.StreamWriter MyLog =  new System.IO.StreamWriter(FullPath,false);
            MyLog.WriteLine(Message);
            MyLog.Close();
        }

        /// <summary>
        /// Añade al archivo existente el mensaje de error 
        /// </summary>
        /// <param name="FullPath">Path del archivo log completo</param>
        /// <param name="Message">Mensaje a grabar</param>
        protected virtual void AddLogMessage(string FullPath, string Message)
        {
            System.IO.StreamWriter MyLogs = new System.IO.StreamWriter(FullPath, true);
            MyLogs.WriteLine(Message);
            MyLogs.Close();
        }

        /// <summary>
        /// Retorna el Nombre final del archivo log con la fecha actual
        /// </summary>
        /// <returns></returns>
        protected virtual string GetLogFileName()
        {
            return sLogFileName + "_" + DateTime.Today.ToString("ddMMyyyy") + ".log";

        }

    }
}
