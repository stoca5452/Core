using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stoca.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "OracleConnectionParameters")]
    /// Clase que retorna los parametros de conexion para Gestor de DB de Microsoft
    public class ORAConnectionParameters
    {
        [XmlElement("User")]
        public string ORAUser { get; set; }

        [XmlElement("Password")]
        public string ORAPassword { get; set; }

        [XmlElement("SeverName")]
        public string ORAServerName { get; set; }

        [XmlElement("DataBase")]
        public string ORADataBase { get; set; }

    }
}
