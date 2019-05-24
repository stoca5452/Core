using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.Common
{
    /// <summary>
    /// Contiene definiciones de variables de errores para usarse en el LOG 
    /// </summary>
    public static class CommonTexts
    {
        #region Stoca Exception Texts
        /// <summary>
        /// Retorna mensaje de error cuando hay error en el Archivo AppSettings (seccion ErrorSetting)
        /// </summary>
        public static string MSG_EXCEPTION_SETTING { get { return ("Configuracion de Errores no presente o no valida"); } }
        /// <summary>
        /// Retorna mesaje de error general al no encontrar el servidor de base de datos
        /// </summary>
        public static string MSG_EXCEPTION_NETWORK { get { return ("Error de Red | Error al conectarse al servidor"); } }
        /// <summary>
        /// Retorna mensaje de error general al no encontrar informacion personalizada de base de datos
        /// </summary>
        public static string MSG_EXCEPTION_GENERAL_ERROR { get { return ("Error en base de datos : Error general : No se detecto un error personalizado en base de datos"); } }
        /// <summary>
        /// Retorna mensaje de error general al no encontrar la definicion de tipo de conexion al SGBD
        /// </summary>
        public static string MSG_EXCEPTION_CONNECTION_TYPE { get { return ("Error en acceder al archivo de configuracion .xml, no se encuentra definido el " +
                                                                            "( TIPO DE CONEXION ) de conexion a usar"); } }
        /// <summary>
        /// Retorna mensaje de error general al no encontra la definicion de los Paramtros para MSSQL
        /// </summary>
        public static string MSG_EXCEPTION_MSSQL_PARAMETERS { get { return ("Error en acceder al archivo de configuracion .xml, no se encuentra definda la seccion" +
                                                                            " MSQLConnectionParameters (Conexion a Microsoft SQL Server"); } }
        /// <summary>
        /// Retorna mensaje de error general al no encontra la definicion de los Paramtros para Oracle
        /// </summary>
        public static string MSG_EXCEPTION_ORA_PARAMETERS {get { return ("Error en acceder al archivo de configuracion .xml, no se encuentra definda la seccion" +
                        " OracleConnectionParameters (Conexion a Oracle");}}
        /// <summary>
        /// Retorna mensaje de error general al no encontrar o coincidir el ID de la aplicacion
        /// </summary>
        public static string MSG_EXCEPTION_APPID { get { return ("El ID de la aplicacion no esta definido o no es valido para la ejecucion de esta aplicacion"); } }
        
        /// <summary>
        /// Retorna mensaje de error general al no poder cerrar la conexion al servidor
        /// </summary>
        public static string MSG_EXCEPTION_CLOSE_CONNECTION { get { return ("Error al cerrar la conexion de base de datos"); } }
        /// <summary>
        /// Retorna mensaje de error general al no poder abrir la conexion al servidor
        /// </summary>
        public static string MSG_EXCEPTION_OPEN_CONNECTION { get { return ("Error al abrir la cadena de conexion de base de datos"); } }
        /// <summary>
        /// Retorna mensaje de error general al no poder crear y agregar un parametro a un objeto <see cref="System.Data.IDbCommand"/>
        /// </summary>
        public static string MSG_EXCEPTION_CREATE_AND_ADD_PARAMETER { get { return ("Error al añadir parametro a la coleccion de parametros Metodo CreateAndAddParameter"); } }
        #endregion Stoca Exception Texts
    }
}
