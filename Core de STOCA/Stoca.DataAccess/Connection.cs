using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Stoca.DataAccess;
using System.Data;
namespace Stoca.DataAccess
{
    public class Connection: IDisposable
    {
        #region Connection fields
        /// <summary>
        /// Nombre por defecto del logger
        /// </summary>
        protected const string LOGGER_NAME = Stoca.Common.CommonFields.LOGGER_NAME;
        /// <summary>
        /// Nombre del Archivo de que contiene las configuraciones
        /// </summary>
        protected const string CONFIG_FILE = Stoca.Common.CommonFields.CONFIG_FILE;
        /// <summary>
        /// Abreviacion para conexion a Microsoft SQL Server
        /// </summary>
        protected const string MSSQL = Stoca.Common.CommonFields.MSSQL;
        /// <summary>
        /// Abreviacion para conexion a Oracle
        /// </summary>
        protected const string ORA = Stoca.Common.CommonFields.ORA;
        /// <summary>
        /// Contiene el ID de la Aplicacion ejecutada
        /// </summary>
        private static string APPID;
        /// <summary>
        /// Ruta del archivo de la aplicacion
        /// </summary>
        private static string sApplicationPath;
        /// <summary>
        /// Confirma parametros de conexion existentes
        /// </summary>
        private static bool bExistConnectionParameters;

        protected IDbConnection m_Connection = null;

        /// <summary>
        /// Campo clase que guarda los parametros de conexion para Microsoft SQL Server
        /// </summary>
        private static Stoca.Configuration.MSSQLConnectionParameters c_MSSQLConnectionParameters = new Stoca.Configuration.MSSQLConnectionParameters();
        
        /// <summary>
        /// Campo clase que guarda los parametros de conexion para Oracle
        /// </summary>
        private static Stoca.Configuration.ORAConnectionParameters c_ORAConnectionParameters = new Stoca.Configuration.ORAConnectionParameters();

        private static Stoca.Configuration.FactorySettings c_FactorySettings = new Stoca.Configuration.FactorySettings();

        #endregion Connection Fields

        public Connection()
        {
            ResolveConfigFile();
        }

        #region Miembros protegidos de Connection

        /// <summary>
        /// Obtiene informacion del archivo de configuracion segun el nodo a buscar (ErrorSettings)
        /// </summary>
        protected static bool ResolveConfigFile()
        {
            bool bValidationOK = false;
            //Obtengo el path del archivo 
            sApplicationPath = Stoca.Common.ToolKit.GetConfigFilePath();

            //Verifico si existe el archivo de configuracion 
            if (!Stoca.Common.ToolKit.IsExistsFile(sApplicationPath, CONFIG_FILE))
            {
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_SETTING.ToString(),
                                                           new FieldAccessException("Error at << Class: Connection | Method: ResolveConfigFile | Error en linea 109 >>"));
                throw new FileNotFoundException();
            }
            else
            {
                bValidationOK = GetFactorySettings();
                bValidationOK = IsValidConnectionString();

            }
            return bValidationOK;
        }
        /// <summary>
        /// Obtiene la configuracion descrita en el archivo CONFIG_FGILE (variable) para luego ser
        /// deserializada obteniendo los datos de configuracion
        /// </summary>
        /// <returns><see cref="Boolean"/></returns>
        protected static bool GetFactorySettings()
        {
            bool bValidationOk = false;
            try
            {
                //Se obtiene la clase deserealizada
                c_FactorySettings = Stoca.Common.ToolKit.ConvertirXML<Stoca.Configuration.FactorySettings>(sApplicationPath, "Factories", CONFIG_FILE);
                //Compruebo si se encontraro datos para el factory
                if (IsExistFactorySettings(c_FactorySettings.ApplicationID, c_FactorySettings.ConnectionType))
                {
                    if (!Stoca.Common.ToolKit.ValidateAppID(c_FactorySettings.ApplicationID))
                    {
                        Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_APPID.ToString(),
                                                            new FieldAccessException("Error at << Class: Connection | Method: ResolveConfigFile | Error en linea 85 >>"));
                        throw new FieldAccessException();
                    }
                    else
                    {
                        bValidationOk = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_APPID.ToString(), ex);
                throw ex;
            }
            return bValidationOk;
        }

        /// <summary>
        /// Comprueba que los valores definidos para FactorySettings no esten en blancos
        /// </summary>
        /// <param name="AppID">Id de la aplicacion</param>
        /// <param name="ConnectionType">Tipo de conexion SQL</param>
        /// <returns><see cref="Boolean"/></returns>
        protected static bool IsExistFactorySettings(string AppID, string ConnectionType)
        {
            bool bIsFactoryDefine = false;
            if (AppID != null && ConnectionType != null)
            {
                bIsFactoryDefine = true;
            }
            return bIsFactoryDefine;
        }
        /// <summary>
        /// Metodo que valida los datos de conexion, tales como Servidor,Base de Datos,
        /// Usuario,Clave,Tipo de seguridad
        /// </summary>
        /// <returns><see cref="Boolean"/></returns>
        protected static bool IsValidConnectionString()
        {
            bool bValidationOK = false;
            //Obtengo los parametros de conexion
            GetConnectionParameters(c_FactorySettings.ApplicationID, c_FactorySettings.ConnectionType);
            if (!IsExistsConnectionParameters(c_MSSQLConnectionParameters, c_ORAConnectionParameters))
            {
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_CONNECTION_TYPE.ToString(),
                                                           new FieldAccessException("Error at << Class: Connection | Method: ResolveConfigFile | Error en linea 94 >>"));
                bValidationOK = false;
                throw new MissingFieldException();
            }
            bValidationOK = true;
            return bValidationOK;
        }

        /// <summary>
        /// Obtiene los Parametros de conexion definidos en el archivo de configuracion
        /// </summary>
        /// <param name="AppID">Id de la aplicacion</param>
        /// <param name="ConnectionType">Tipo de conexion</param>
        /// <returns><see cref="Boolean"/></returns>
        protected static Boolean GetConnectionParameters(string AppID, string ConnectionType)
        {
            bool bIsConnectionParam = false;
            if (Stoca.Common.ToolKit.ValidateAppID(AppID))
            {
                switch (ConnectionType)
                {
                    case MSSQL:
                        try
                        {
                            c_MSSQLConnectionParameters = Stoca.Common.ToolKit.ConvertirXML<Stoca.Configuration.MSSQLConnectionParameters>(sApplicationPath, "MSQLConnectionParameters", CONFIG_FILE);
                            bIsConnectionParam = true;
                        }
                        catch (Exception ex)
                        {
                            Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_MSSQL_PARAMETERS.ToString(), ex);
                            throw ex;
                        }
                        break;
                    case ORA:
                        try
                        {
                            c_ORAConnectionParameters = Stoca.Common.ToolKit.ConvertirXML<Stoca.Configuration.ORAConnectionParameters>(sApplicationPath, "OracleConnectionParameters", CONFIG_FILE);
                            bIsConnectionParam = true;
                        }
                        catch (Exception ex)
                        {
                            Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_ORA_PARAMETERS.ToString(), ex);
                            throw ex;
                        }
                        break;
                    default:
                        break;
                }
            }
            return bIsConnectionParam;
        }
        /// <summary>
        /// Comprueba que los Parametros de conexion fueron definidos
        /// </summary>
        /// <param name="MSSQlParam">Paramtros de MIcrosoft SQL Server</param>
        /// <param name="ORAParam">Parametros de Oracle</param>
        /// <returns><see cref="Boolean"/></returns>
        protected static bool IsExistsConnectionParameters(Configuration.MSSQLConnectionParameters MSSQlParam, Configuration.ORAConnectionParameters ORAParam)
        {
            bool bIsConnectionParam = false;
            if (MSSQlParam != null)
            {
                if (MSSQlParam.MSQLServerName != null)
                {
                    bIsConnectionParam = true;
                }
            }

            if (ORAParam != null)
            {
                if (ORAParam.ORAServerName != null)
                {
                    bIsConnectionParam = true;
                }
            }
            return bIsConnectionParam;
        }

        /// <summary>
        /// Prepara y establece la conexion a la base de datos segun el Stringde Conexion
        /// </summary>
        protected void CreateConnection()
        {
            string sConnectionString = null;
            sConnectionString = GetConnectionString(c_FactorySettings.ConnectionType);
            switch (c_FactorySettings.ConnectionType)
            {
                case Stoca.Common.CommonFields.MSSQL:
                    m_Connection = new System.Data.SqlClient.SqlConnection(sConnectionString);
                    break;
                case Stoca.Common.CommonFields.ORA:
                    //Conexion = new System.Data.OracleClient("");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Retorna el string formado con los parametros de conexion
        /// </summary>
        /// <param name="ConnectionType">Tipo de Conexion</param>
        /// <returns><see cref="System.Data.SqlClient.SqlConnection"/></returns>
        protected static string GetConnectionString(string ConnectionType)
        {
            string sConnectionString = string.Empty;
            switch(ConnectionType)
            {
                case Stoca.Common.CommonFields.MSSQL:
                    sConnectionString = "Data Source = " + c_MSSQLConnectionParameters.MSQLServerName + ";";
                    sConnectionString += "Initial Catalog =" + c_MSSQLConnectionParameters.MSQLDataBase + ";";
                    sConnectionString += "User ID =" + c_MSSQLConnectionParameters.MSQLUser + ";";
                    sConnectionString += "Password =" + c_MSSQLConnectionParameters.MSQLPassword + ";";
                    break;
                case Stoca.Common.CommonFields.ORA:

                    break;
                default:
                    break;
            }
            return sConnectionString;
        }
        #endregion Miembros protegios de Connection

        #region Miembros de IDisposible
        /// <summary>
        /// Retorna true si la conexión se encuentra abierta
        /// </summary>
        /// <returns>Retorna true si la conexión está abierta</returns>
        public virtual bool isOpen()
        {
            if (m_Connection == null) return false;
            return (m_Connection.State == ConnectionState.Open);
        }

        /// <summary>
        /// Cierra la conexión con la base de datos del objeto de tipo conexion ya inicializado en el constructor
        /// </summary>
        public virtual void Close()
        {
            try
            {
                if (m_Connection!= null)
                {
                    if (m_Connection.State == ConnectionState.Open) m_Connection.Close();
                    m_Connection.Dispose();
                    m_Connection = null;
                }
            }
            catch (Exception ex)
            {
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_CLOSE_CONNECTION.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Abre la conexión con la base de datos.
        /// </summary>
        public virtual void Open()
        {
            if (m_Connection == null) CreateConnection();

            try
            {
                m_Connection.Open();
            }
            catch (Exception ex)
            {
                Close();
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_OPEN_CONNECTION.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Libera recursos de la conexión interna.
        /// </summary>
        public virtual void Dispose()
        {
            if (isOpen())
            {
                Close();
            }
            if (m_Connection != null) m_Connection.Dispose();
            m_Connection = null;
        }

        #endregion Miembros de IDBConnection

        public  virtual IDbConnection GetConnection()
        {
            if (m_Connection == null)
            {
                CreateConnection();
                try
                {
                    m_Connection.Open();
                }
                catch (Exception ex)
                {
                    Close();
                    Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_OPEN_CONNECTION.ToString(), ex);
                    throw ex;
                }                   
            }  
            return GetConnection();
        }
        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbCommand"/>
        /// </summary>
        /// <returns>Retorna un objeto <see cref="System.Data.IDbCommand"/></returns>
        public virtual IDbCommand CreateCommand()
        {
            IDbCommand cmd = null;

            if (m_Connection == null) CreateConnection();
            try
            {
                switch(c_FactorySettings.ConnectionType)
                {
                    case MSSQL:
                        cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Connection = m_Connection;
                        cmd.CommandTimeout = c_FactorySettings.ConnetionTimeOut;
                        break;
                    case ORA:
                        break;
                }
            }
            catch (Exception ex)
            {
            Close();
            Stoca.Log.LogManager.GetLog().Error(Stoca.Common.CommonTexts.MSG_EXCEPTION_OPEN_CONNECTION.ToString(), ex);
            throw ex;
            }
            return cmd;
        }
        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbCommand"/>
        /// </summary>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>        
        /// <param name="commandText">Texto del comando</param>        
        /// <returns>Retorna un objeto <see cref="System.Data.IDbCommand"/></returns>
        public virtual IDbCommand CreateCommand(CommandType commandType, string commandText)
        {
            IDbCommand command = CreateCommand();

            command.CommandType = commandType;
            command.CommandText = commandText;
            return command;
        }
        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbDataParameter"/> y lo agrega
        /// a la colección de parámetros del objeto
        /// </summary>
        /// <param name="command">Objeto <see cref="System.Data.IDbCommand"/> al cual se agregará el parámetro</param>        
        /// <param name="paramType">Tipo de dato del parámetro</param>        
        /// <param name="paramDirection">Dirección del parámetro</param>        
        /// <param name="paramName">Nombre del parámetro</param>        
        /// <param name="paramSize">Tamaño del parámetro</param>        
        /// <param name="paramPrecision">Presición parámetro</param>        
        /// <returns>Retorna referencia al <see cref="System.Data.IDbDataParameter"/> que se creó</returns>
        public virtual IDbDataParameter CreateAndAddParameter(IDbCommand command, DbType paramType, ParameterDirection paramDirection, String paramName,int paramSize, byte paramPresicion)
        {
            try
            {
                IDbDataParameter param = command.CreateParameter();
                param.DbType = paramType;
                param.Direction = paramDirection;
                param.ParameterName = paramName;
                param.Size = paramSize;
                param.Precision = paramPresicion;
                command.Parameters.Add(param);
                return param;
            }
            catch (Exception ex)
            {
                Stoca.Log.LogManager.GetLog().Error(Common.CommonTexts.MSG_EXCEPTION_CREATE_AND_ADD_PARAMETER, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbDataParameter"/> y lo agrega
        /// a la colección de parámetros del objeto
        /// </summary>
        /// <param name="command">Objeto <see cref="System.Data.IDbCommand"/> al cual se agregará el parámetro</param>        
        /// <param name="paramType">Tipo de dato del parámetro</param>        
        /// <param name="paramDirection">Dirección del parámetro</param>        
        /// <param name="paramName">Nombre del parámetro</param>        
        /// <param name="paramValue">Valor del parámetro</param>        
        /// <param name="paramSize">Tamaño del parámetro</param>        
        /// <param name="paramPrecision">Presición parámetro</param>        
        /// <returns>Retorna referencia al <see cref="System.Data.IDbDataParameter"/> que se creó</returns>
        public virtual IDbDataParameter CreateAndAddParameter(IDbCommand command, DbType paramType, ParameterDirection paramDirecction, String paramName,Object paramValue, int paramSize, byte paramPresicion)
        {
            IDbDataParameter param = CreateAndAddParameter(command, paramType, paramDirecction, paramName, paramSize, paramPresicion);
            if (paramType == DbType.String)
            {
                param.Value = (paramValue == null ? string.Empty : paramValue);
            }
            else
            {
                param.Value = (paramValue == null ? DBNull.Value : paramValue);
            }
            
            return param;
        }
        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbDataParameter"/> y lo agrega
        /// a la colección de parámetros del objeto
        /// </summary>
        /// <param name="command">Objeto <see cref="System.Data.IDbCommand"/> al cual se agregará el parámetro</param>        
        /// <param name="paramType">Tipo de dato del parámetro</param>        
        /// <param name="paramDirection">Dirección del parámetro</param>        
        /// <param name="paramName">Nombre del parámetro</param>        
        /// <param name="paramValue">Valor del parámetro</param>        
        /// <param name="paramSize">Tamaño del parámetro</param>        
        /// <returns>Retorna referencia al <see cref="System.Data.IDbDataParameter"/> que se creó</returns>
        public virtual  IDbDataParameter CreateAndAddParameter(IDbCommand command, DbType paramType, ParameterDirection paramDirecction, String paramName, Object paramValue, int paramSize)
        {
            return CreateAndAddParameter(command, paramType, paramDirecction, paramName, paramValue, paramSize, 0);
        }
        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbDataParameter"/> y lo agrega
        /// a la colección de parámetros del objeto
        /// </summary>
        /// <param name="command">Objeto <see cref="System.Data.IDbCommand"/> al cual se agregará el parámetro</param>        
        /// <param name="paramType">Tipo de dato del parámetro</param>        
        /// <param name="paramDirection">Dirección del parámetro</param>        
        /// <param name="paramName">Nombre del parámetro</param>        
        /// <param name="paramValue">Valor del parámetro</param>        
        /// <returns>Retorna referencia al <see cref="System.Data.IDbDataParameter"/> que se creó</returns>
        public virtual IDbDataParameter CreateAndAddParameter(IDbCommand command, DbType paramType, ParameterDirection paramDirecction, String paramName, Object paramValue)
        {
            return CreateAndAddParameter(command, paramType, paramDirecction, paramName, paramValue, (paramType == DbType.String ? (paramValue != null ? paramValue.ToString().Length : 10000) : 0), 0);
        }
        /// <summary>
        /// Crea un objeto <see cref="System.Data.IDbDataParameter"/> para manipular
        /// archivos xml y lo agrega a la colección de parámetros del objeto
        /// </summary>
        /// <param name="command">Objeto <see cref="System.Data.IDbCommand"/> al cual se agregará el parámetro</param>        
        /// <param name="paramDirection">Dirección del parámetro</param>        
        /// <param name="paramName">Nombre del parámetro</param>        
        /// <param name="value">Valor del parámetro</param>        
        /// <returns>Retorna referencia al <see cref="System.Data.IDbDataParameter"/> que se creó</returns>
        /// <since>4.0.0.6</since>
        public virtual IDbDataParameter CreateAndAddXmlParameter(IDbCommand command, ParameterDirection paramDirection, String paramName, object value)
        {            
            return CreateAndAddParameter(command, DbType.Xml, paramDirection, paramName, value);
        }

        public virtual DataSet GetDataSet(IDbCommand command)
        {
            DataSet dataSet = null;
            try
            {
                switch (c_FactorySettings.ConnectionType)
                {
                    case MSSQL:
                        System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter();
                        dataAdapter.SelectCommand = (System.Data.SqlClient.SqlCommand)(command);
                        dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        break;
                }
            }
            catch (Exception ex)
            {
                if (dataSet != null)
                {
                    dataSet.Dispose();
                    dataSet = null;
                }
                throw;
            }
            finally
            {
                if (dataSet != null)
                {
                    dataSet.Dispose();
                    command.Connection.Close();
                }
            }
            return dataSet;
        }

        public virtual int ExecuteNoquery(System.Data.IDbCommand command)
        {
            Open();
            int result = command.ExecuteNonQuery();
            Dispose();
            return result;
        }

        public virtual IDataReader ExecuteDataReader (System.Data.IDbCommand command)
        {
            try
            {
                Open();
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.ExceptionManager.EXCEPTION_EXECUTEDATAREADER, ex, command);
                throw ex;
            }
            finally
            {
                Dispose();
            }
            
        }

        public virtual Object ExecuteScalar(System.Data.IDbCommand command)
        {
            try
            {
                Open();
                Close();
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Stoca.Log.LogManager.GetLog().Error(Stoca.Common.ExceptionManager.EXCEPTION_EXECUTESCARLAR , ex, command);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /*public virtual DataSet GetDataSet(string demo, IDbCommand command)
        {
            IConnecion connection = null;
            try
            {
                //connection = GetConnection();
                command.Connection = connection.GetConnection();
                connection.Open();
                return connection.GetDataSet(command);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual DataSet SetDataSet(IDbCommand cmd)
        {
            IDbConnection connection = null;

            connection = GetConnection();
            cmd.Connection = connection;

        }*/

    }
}