using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.Common
{
    /// <summary>
    /// Manejador de Excepciones
    /// <Revision>1.0.0.0</Revision>
    /// </summary>
    public class ExceptionManager
    {
        #region ExceptionManager Fields
        /// <summary>
        /// Clave: mensaje original
        /// </summary>
        public const string EXCEPTION_ORIGINAL_MESSAGE_KEY = "OriginalMessage";

        /// <summary>
        /// Clave: información de comando
        /// </summary>
        public const string EXCEPTION_COMMAND_INFO_KEY = "CommandInfo";

        /// <summary>
        /// Clave: Error de Objeto ExecuteScarlar
        /// </summary>
        public const string EXCEPTION_EXECUTESCARLAR = "Error en ejecucion de objeto ExcecuteScalar ";

        /// <summary>
        /// Clave: Error de Objeto ExecuteDataReader
        /// </summary>
        public const string EXCEPTION_EXECUTEDATAREADER = "Error en ejecucion de objeto ExecuteDataReader";

        /// <summary>
        /// Tracking id para excepciones
        /// </summary>
        protected const string EXCEPTION_TRACKING_ID_KEY = "TrackingId";

        /// <summary>
        /// Clave para Nombre del servidor
        /// </summary>
        protected const string EXCEPTION_SERVER_NAME_KEY = "ServerName";
        /// <summary>
        /// Clave para información extendida
        /// </summary>
        protected const string EXCEPTION_EXTENDED_INFORMATION_KEY = "ExtendendInformation";

        

        #endregion ExceptionManager Fields

        /// <summary>
        /// Obtiene información de la Escepción.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetExceptionInfo(Exception exception)
        {
            if (exception != null)
            {
                StringBuilder buffer = new StringBuilder();

                buffer.AppendFormat("\t\tTipo: {0}\r\n", exception.GetType().ToString());
                //buffer.AppendFormat("\t\tMensaje: {0}\r\n", GetFormattedMessage(exception));
                buffer.AppendFormat("\t\tFuente: {0}\r\n", exception.Source);

                return buffer.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Obtiene información de le Escepción Interna.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetInnerExceptionsInfo(Exception exception)
        {
            if (exception == null)
            {
                return "";
            }

            StringBuilder str = new StringBuilder();
            Exception ex = exception.InnerException;

            while (ex != null)
            {
                str.AppendFormat("{0}", GetExceptionInfo(ex));
                ex = ex.InnerException;

                if (ex != null)
                {
                    str.Append("\t\t----------------------------------------------------------------------------------------------------------------------------------------------------------\r\n");
                }
            }
            return str.ToString();
        }   

        public static string GetExceptionFullInfo(Exception ex)
        {
            StringBuilder buffer = new StringBuilder();

            char[] cCharsToTrim = new char[2] { '\r', '\n' };
            buffer.AppendFormat("\r\n\tType:{0}\r\n", ex.GetType().ToString());
            buffer.AppendFormat("\tMessage:{0}\r\n", GetFormattedMessage(ex)).ToString();
            buffer.AppendFormat("\tSource:{0}\r\n", ex.Source).ToString();
            buffer.AppendFormat("\tTraking id:{0}\r\n", GetTrakingId(ex));
            buffer.AppendFormat("\tServer:{0}\r\n", GetServerName(ex));
            string sUserMessage = GetFriendlyMessage(ex);
            buffer.AppendFormat("\tUserMessage: {0}\r\n", sUserMessage);
            string sExtendedMessage = GetFriendlyExtendedMessage(ex);
            buffer.AppendFormat("\tUserMessageExtended: {0}\r\n", (sUserMessage != sExtendedMessage ? sExtendedMessage : null));

            string sText;
            // agrega mensaje original
            sText = GetOriginalMessage(ex);
            if (sText.Length > 0)
                buffer.AppendFormat("\tOriginal Message: {0}\r\n", sText);

            // agrega información extendida
            sText = GetExtendedInfo(ex);
            if (sText.Length > 0)
                buffer.AppendFormat("\tExtended Information: {0}\r\n", sText);

            // agrega información de las excepciones anidadas
            sText = GetInnerExceptionsInfo(ex);
            if (sText.Length > 0)
                buffer.AppendFormat("\tInner Exception: \r\n{0}", sText);

            // agrega información de los datos de la excepción
            sText = GetExceptionDataFull(ex, true);
            if (sText.Length > 0)
                buffer.AppendFormat("\tException Data: \r\n{0}\r\n", sText.TrimEnd(cCharsToTrim));

            // agrega información del comando
            sText = GetCommandInfo(ex);
            if (sText.Length > 0)
                buffer.AppendFormat("\tCommand Information:  \r\n{0}\r\n", sText.TrimEnd(cCharsToTrim));
            // agrega trace del stack
            buffer.AppendFormat("\tStack track: \r\n{0}\r\n\r\n", GetStackTrace(ex).TrimEnd(cCharsToTrim));
            return buffer.ToString();
        }


        public static string GetFormattedMessage(Exception ex)
        {
            string sText;
            if (ex == null || ex.Message == null)
            {
                sText = "";
            }
            else
            {
                sText = ex.Message;
                sText.Trim();
                sText = sText.Replace("\n", "\r\n\t\t");
            }
            return sText;
        }

        /// <summary>
        /// Retorna el id de tracking de una excepción o crea un nuevo id.
        /// </summary>
        /// <param name="ex">Excepción</param>
        /// <returns>string</returns>
        public static string GetTrakingId(Exception ex)
        {
            if (ex.Data.Contains(EXCEPTION_TRACKING_ID_KEY))
            {
                return (string)ex.Data[EXCEPTION_TRACKING_ID_KEY];
            }

            string sId = GenerateTrakingId(ex);
            //AddExceptionTrakingId(ex, sId);

            return sId;
        }

        /// <summary>
        /// Genera el traking id 
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>Traking id Generado</returns>
        public static string GenerateTrakingId(Exception ex)
        {
            return Stoca.Common.ToolKit.GenerateHashKey(ex.GetHashCode().ToString());
        }

        /// <summary>
        /// Agrega el id de traking
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <param name="Id">Id de Traking</param>
        public static void AddExceptionTrakingId(Exception ex, string Id)
        {
            //AddExceptionData(ex, EXCEPTION_TRACKING_ID_KEY, Id);
        }

        /// <summary>
        /// Retorna el nombre del servidor
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>Nombre del Servidor</returns>
        public static string GetServerName(Exception ex)
        {
            if (ex.Data.Contains(EXCEPTION_SERVER_NAME_KEY))
            {
                return (string)ex.Data[EXCEPTION_SERVER_NAME_KEY];
            }

            string sServer = Stoca.Common.ToolKit.GetMachineName();
            //AddExceptionData(ex, EXCEPTION_SERVER_NAME_KEY, sServer);
            return sServer;
        }

        /// <summary>
        /// Retorna el mensaje para mostrar al usuario
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>Mensaje al usuario</returns>
        public static string GetFriendlyMessage(Exception ex)
        {
            System.Net.Sockets.SocketException SocketException = ex as System.Net.Sockets.SocketException;
            if (SocketException != null)
            {
                return Stoca.Common.CommonTexts.MSG_EXCEPTION_NETWORK;
            }

            //recupero texto extendido de la excepcion
            string sText = GetExtendedInfo(ex);
            if (sText != null && sText.Length > 0)
                return sText;

            //extrae el mensaje de la base de datos
            return GetCustomDatabaseMessage(ex);
        }

        /// <summary>
        /// Retorna mensaje extendido para el usuarioa mostrar
        /// </summary>
        /// <param name="ex">Excpcion</param>
        /// <returns>Mensaje extendido</returns>
        public static string GetFriendlyExtendedMessage(Exception ex)
        {
            string sSource = ex.GetType().ToString();

            //determia el tipo de excepcion a sobreescrbir
            if (sSource == "System.Net.Sockets.SocketException")
            {
                return Stoca.Common.CommonTexts.MSG_EXCEPTION_NETWORK;
            }

            //recupero texto extendido de la excepcion
            string sText = GetExtendedInfo(ex);
            if (sText != null && sText.Length > 0)
                return sText;

            //extrae el mensaje de la base de datos
            return GetCustomDatabaseMessage(ex);
        }

        /// <summary>
        /// Retorna informacion extendida de la expcecion
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>string</returns>
        public static string GetExtendedInfo(Exception ex)
        {
            if (ex.Data.Contains(EXCEPTION_EXTENDED_INFORMATION_KEY))
            {
                return (string)ex.Data[EXCEPTION_EXTENDED_INFORMATION_KEY];
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Retorna el mensaje de error de la base de datos personalizado
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>string</returns>
        public static string GetCustomDatabaseMessage(Exception ex)
        {
            Exception innerEx = ex;
            string sMessage;
            
            while (innerEx != null)
            {
                //recupero el mensaje de la excepcion
                sMessage = innerEx.Message;

                //determino si esta presente el error en la base de datos
                if (IsCustomErrorFromDatabase(sMessage))
                    return ExtractDatabaseErrorMessage(sMessage);

                /* POR HACER Y COMPROBAR
                //determina si se trata de un error personalizado
                if (IsOraclaMessage(sMessage))
                {

                }

                if (IsMSQLMessage(sMessage))
                {

                }
                */

                innerEx = innerEx.InnerException;

                //determina si no hay mas excepciones y no se puede serguir recupernado mas excepciones inerternas
                if ((innerEx == null) && sMessage != null && sMessage.Length > 0)
                    return sMessage;               
            }

            return Stoca.Common.CommonTexts.MSG_EXCEPTION_GENERAL_ERROR;

        }

        /// <summary>
        /// Determina si el texto posee error personalizado
        /// </summary>
        /// <param name="ErrorInfo">Mensaje de error a analizar</param>
        /// <returns>boolean</returns>
        public static bool IsCustomErrorFromDatabase(string ErrorInfo)
        {
            return (ErrorInfo.IndexOf("#") >= 0);
        }

        /// <summary>
        /// Extrae información del error (o de alguna excepción) generado en la base de datos.
        /// El error debe tener el formato #texto#.
        /// </summary>
        /// <param name="ErrorInfo">Texto a buscar</param>
        /// <returns>string</returns>
        public static string ExtractDatabaseErrorMessage(string ErrorInfo)
        {
            int nStartOfMessage, nEndOfMessage, nPos, nEnd;
            int nParamIxd = 1;
            string sResult = ErrorInfo;
            string sParamIxd, sValue;
            bool bContinue = true;

            //recupero el primer caracter
            nStartOfMessage = sResult.IndexOf("#");
            if (nStartOfMessage >=0)
            {
                //recupero posicion del segundo caracter delimitador
                nEndOfMessage = sResult.IndexOf("#", nStartOfMessage + 1);
                if (nEndOfMessage >0)
                {
                    //recupero el cuerpo del mensaje
                    sResult = sResult.Substring(nStartOfMessage, nEndOfMessage - nStartOfMessage - 1);
                    
                    //busco parametros para eliminar
                    while (bContinue)
                    {
                        sValue = null;
                        sParamIxd = "%" + nParamIxd.ToString() + "%";
                        nPos = ErrorInfo.IndexOf(sParamIxd, nEndOfMessage);
                        if (nPos > 0)
                        {
                            // busco inicio del valor del parametro
                            nPos = nPos + sParamIxd.Length;
                            nEnd = ErrorInfo.IndexOf("%", nPos);
                            if (nEnd == -1)
                            {
                                //determina fin del valor
                                nEnd = ErrorInfo.IndexOf("#,", nPos);
                                if (nEnd == -1) nEnd = ErrorInfo.IndexOf("\n", nPos);
                                if (nEnd == -1)
                                    sValue = ErrorInfo.Substring(nPos);
                                else
                                    sValue = ErrorInfo.Substring(nPos, nEnd - nPos);
                                bContinue = false;
                            }
                            else
                            {
                                sValue = ErrorInfo.Substring(nPos, nEnd - nPos);
                                //reemplazo el valor
                                sResult = sResult.Replace(sParamIxd, sValue);
                                //proximo parametro
                                nParamIxd++;
                            }
                        }
                        else
                        {
                            bContinue = false;
                        }
                    }
                }
            }
            return sResult;
        }

        /// <summary>
        /// Retorna el mensaje original de la excepcion
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>string</returns>
        public static string GetOriginalMessage(Exception ex)
        {
            if (ex.Data.Contains(EXCEPTION_ORIGINAL_MESSAGE_KEY))
            {
                return (string)ex.Data[EXCEPTION_ORIGINAL_MESSAGE_KEY];
            }

            return "";
        }

        /// <summary>
        /// Recupera informacion de la coleecion de datos de la excepcion, incluyendo las excepciones
        /// interna
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <param name="">Flag para recuperar solo datos personalizados</param>
        /// <returns>string</returns>
        public static string GetExceptionDataFull(Exception excepcion, bool OnlyCustomData)
        {
            Exception ex;
            StringBuilder buffer = new StringBuilder();
            ex = excepcion;
            while (ex != null)
            {
                buffer.Append(GetExceptionData(ex, OnlyCustomData));
                ex = ex.InnerException;
            }
            return buffer.ToString();
        }

        public static string GetExceptionData(Exception ex, bool OnlyCustomData)
        {
            bool bAdd;
            if (ex != null && ex.Data != null && ex.Data.Count > 0)
            {
                StringBuilder str = new StringBuilder();

                foreach (string key in ex.Data.Keys)
                {
                    //filtra los datos personalizados
                    if (OnlyCustomData)
                    {

                        switch (key)
                        {
                            case EXCEPTION_ORIGINAL_MESSAGE_KEY:
                                bAdd = false;
                                break;
                            case EXCEPTION_COMMAND_INFO_KEY:
                                bAdd = false;
                                break;
                            case EXCEPTION_TRACKING_ID_KEY:
                                bAdd = false;
                                break;
                            case EXCEPTION_SERVER_NAME_KEY:
                                bAdd = false;
                                break;
                            default:
                                bAdd = true;
                                break;
                        }
                    }
                    else
                    {
                        bAdd = true;
                    }

                    if (bAdd)
                    {
                        object obj = ex.Data[key];
                        str.AppendFormat("\t\t{0} = {1}\r\n", key, (obj == null ? "null" : obj.ToString()));
                    }
                }
                return str.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Información del Comando que generó la Escepción.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetCommandInfo(Exception ex)
        {
            if (ex.Data.Contains(EXCEPTION_COMMAND_INFO_KEY))
                return (string)ex.Data[EXCEPTION_COMMAND_INFO_KEY];
            return "";
        }

        /// <summary>
        /// Trace del Stack.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetStackTrace(Exception ex)
        {
            StringBuilder buffer = new StringBuilder();
            Exception innEx = ex.InnerException;
            while (innEx != null)
            {
                buffer.Append(innEx.StackTrace);
                innEx = innEx.InnerException;
            }
            buffer.Append(ex.StackTrace);
            return buffer.ToString();
        }

        public static string GetCommandInfo(IDbCommand command)
        {
            if (command != null)
            {
                string sParametro = string.Empty;
                string sProcedure = string.Empty;
                StringBuilder str = new StringBuilder();

                str.AppendFormat("IDbCommand Info {0}; - [{1}] Parameters", command.CommandText, command.Parameters.Count);

                foreach (IDataParameter par in command.Parameters)
                {
                    sProcedure = command.CommandText.ToUpper().ToString();
                    sParametro = par.ParameterName.ToUpper().ToString();

                    //verificar si se coloca nombre de proedimientos y parametro de clave para q no los tome en cuenta (inicio de sesion)

                    str.AppendFormat("\r\n\t\t{0} = {1}", par.ParameterName, (par.Value == null ? "null" : par.Value.ToString()));
                }

                return str.ToString();
            }
            else
            {
                return "";
            }
        }


    }
}
