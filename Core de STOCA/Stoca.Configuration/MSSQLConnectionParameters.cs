using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stoca.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "MSQLConnectionParameters")]
    /// Clase que retorna los parametros de conexion para Gestor de DB de Microsoft
    public class MSSQLConnectionParameters
    {
        [XmlElement("User")]
        public string MSQLUser { get; set; }

        [XmlElement("Password")]
        public string MSQLPassword { get; set; }

        [XmlElement("SeverName")]
        public string MSQLServerName { get; set; }

        [XmlElement("DataBase")]
        public string MSQLDataBase { get; set; }


    }
}
