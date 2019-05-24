using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Stoca.Common
{
    /// <summary>
    /// Utilidades Genericas
    /// </summary>
    public static class ToolKit 
    {
        /// <summary>
        /// Determina si existe el archivo de log
        /// </summary>
        /// <param name="FilePath"> Ruta del archivo</param>
        /// <param name="FileName"> Nombre del archivo</param>
        /// <returns></returns>
        public static bool IsExistsFile(string FilePath, string FileName)
        {
            return System.IO.File.Exists(FilePath + "\\" + FileName);
        }

        /// <summary>
        /// Retorna el mensaje del error formateado.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>string</returns>
        public static string GetFormattedMessage(Exception ex)
        {
            string sText;
            if (ex == null || ex.Message == null)
                return "";
            sText = ex.Message;
            sText.Trim();
            sText = sText.Replace("\n", "\r\n\t\t");
            return sText;
        }

        /// <summary>
        /// Gnera un codigo hash unico
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Hash key generado</returns>

        public static string GenerateHashKey(string key)
        {
            string sHash = new Random().GetHashCode().ToString();
            sHash += DateTime.Now.ToString("yyyymmddhhmmssff");
            sHash += new Random().GetHashCode().ToString();
            return sHash;
        }

        /// <summary>
        /// Retorna el nombre de la PC
        /// </summary>
        /// <returns>Nomre de la PC</returns>
        public static string GetMachineName()
        {
            return System.Environment.MachineName;    
        }

        /// <summary>
        /// Obtiene el path del archivo de configuracion
        /// </summary>
        /// <returns></returns>
        public static string GetConfigFilePath()
        {
            string _sPath = System.AppDomain.CurrentDomain.RelativeSearchPath;
            if (_sPath == null || _sPath.Length == 0)
            {
                _sPath = System.AppDomain.CurrentDomain.BaseDirectory;
            }
            return _sPath;
        }
        /// <summary>
        /// Retorna una clase deserializada 
        /// </summary>
        /// <typeparam name="T">Clase a desserializar</typeparam>
        /// <param name="Path">Ruta del archivo de configuracion</param>
        /// <param name="nodeDescrip">Nodo xml a buscar en el archivo de configracion</param>
        /// <param name="ConfigFile">Nombre del archivo de configuracion de la Aplicacion</param>
        /// <returns></returns>
        public static T ConvertirXML<T>(string Path, string nodeDescrip, string ConfigFile) where T : class
        {
            string XMLString = File.ReadAllText(Path + "\\" + ConfigFile);
            XmlDocument xmlData = new XmlDocument();
            xmlData.LoadXml(XMLString);
            XmlNode node = xmlData.GetElementsByTagName(nodeDescrip)[0];
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.OuterXml);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T));
            T result = (ser.Deserialize(stm) as T);
            return result;
        }

        public static bool ValidateAppID (string AppID)
        {
            bool bIsValidAppID = false;
            switch (AppID)
            {
                case Stoca.Common.CommonFields.AppID_RecibosPago:
                    bIsValidAppID = true;
                    break;
                default:
                    bIsValidAppID = false;
                    break;
            }
            return bIsValidAppID;
        }
    }
}
