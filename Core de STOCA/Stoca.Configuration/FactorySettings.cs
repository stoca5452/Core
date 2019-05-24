using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stoca.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "Factories")]
    public class FactorySettings
    {
        [XmlElement("ApplicationID")]
        public string ApplicationID { get; set; }

        [XmlElement("ConnectionType")]
        public string ConnectionType { get; set; }
        
        [XmlElement("ConnectionTimeOut")]
        public int ConnetionTimeOut { get; set; }
    }
}
