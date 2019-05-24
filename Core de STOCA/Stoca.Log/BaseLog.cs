using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;
using Stoca.Common;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Stoca.Log
{
    public class BaseLog : ILog
    {
        #region BaseLog fields
        /// <summary>
        /// Nombre por defecto del logger
        /// </summary>
        protected const string LOGGER_NAME = Stoca.Common.CommonFields.LOGGER_NAME;
        /// <summary>
        /// Nombre del Archivo de que contiene las ocnfiguraciones
        /// </summary>
        protected const string CONFIG_FILE = Stoca.Common.CommonFields.CONFIG_FILE;

        /// <summary>
        /// Flag para saber si se loguean errores
        /// </summary>
        private static bool bIsErrorEnable;

        /// <summary>
        /// Instancia de clase que guardar el log  de errores en file
        /// </summary>
        private static SaveLogs m_SaveLogs = new SaveLogs();

        /// <summary>
        /// Instancia de clase que guarda los parametros de configuracion de logger
        /// </summary>
        private static Stoca.Configuration.BaseConfigElement c_BaseConfig = new Stoca.Configuration.BaseConfigElement();
        #endregion BaseLog Fields


        #region Constructores BaseLog
        ///<summary>
        ///Constuctor por defecto
        /// </summary>
        public BaseLog()
        {
            ResolveConfigFile();
        }

        public static SaveLogs DoSaveLogs()
        {
            return m_SaveLogs;
        }
        #endregion

        
        #region Miembros protegidos de BaseLog

        /// <summary>
        /// Obtiene informacion del archivo de configuracion segun el nodo a buscar (ErrorSettings)
        /// </summary>
        protected static void ResolveConfigFile()
        {
            //Obtengo el path del archivo 
            string _Path = Stoca.Common.ToolKit.GetConfigFilePath();
            
            //Verifico si existe el archivo de configuracion 
            if (Stoca.Common.ToolKit.IsExistsFile(_Path, CONFIG_FILE))
            {
                try
                {
                    //Se obtiene la clase deserealizada
                    c_BaseConfig = Stoca.Common.ToolKit.ConvertirXML<Stoca.Configuration.BaseConfigElement>(_Path, "ErrorSettings", CONFIG_FILE);
                    //asigno el parametro como verdadero si se configuro para loguear errores
                    IsErrorEnable(c_BaseConfig.IsLogError);
                }
                catch (Exception ex)
                {
                    Stoca.Common.CommonTexts.MSG_EXCEPTION_SETTING.ToString();
                    throw ex;
                }
                

            }
        }

        #endregion Mienbros protegidos de BaseLog

        #region Miembros ILog 

        /// <summary>
        /// Se determina si se seteo la propiedad de errores para logueo 
        /// </summary>
        /// <param name="enable">True o False</param>
        public static void IsErrorEnable(bool enable)
        {
            bIsErrorEnable = enable;
        }

        /// <summary>
        /// Escribe un mensaje en el log
        /// </summary>
        /// <param name="message"></param>
        public virtual void Error(object message)
        {
            if (bIsErrorEnable)
            {
                DoSaveLogs().SetLogSettings(c_BaseConfig.Pathlog, c_BaseConfig.FileName);
                DoSaveLogs().LogExeption(message.ToString());
            }
        }
        /// <summary>
        /// Escribe un mensaje de error para una excepcion
        /// </summary>
        /// <param name="ex"></param>
        public virtual void Error(object message, Exception ex)
        {
            if (bIsErrorEnable)
            {
                DoSaveLogs().SetLogSettings(c_BaseConfig.Pathlog, c_BaseConfig.FileName);
                DoSaveLogs().LogExeption(message + Stoca.Common.ExceptionManager.GetExceptionFullInfo(ex));
            }
        }

        /// <summary>
        /// Escribe un mensaje de error para una excepcion con objeto command
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="command"></param>
        public virtual void Error(object message, Exception ex, System.Data.IDbCommand command)
        {
            if (bIsErrorEnable)
            {
                DoSaveLogs().SetLogSettings(c_BaseConfig.Pathlog, c_BaseConfig.FileName);
                DoSaveLogs().LogExeption(message + Stoca.Common.ExceptionManager.GetExceptionFullInfo(ex) + Stoca.Common.ExceptionManager.GetCommandInfo(command));
            }
        }

        #endregion Miembros ILog
    }
}
