using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stoca.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "ErrorSettings")]
    public class BaseConfigElement
        {
        [XmlElement("IsLogError")]
        public bool IsLogError { get; set; }

        [XmlElement("Pathlog")]
        public string Pathlog { get; set; }

        [XmlElement("FileName")]
        public string FileName { get; set; }

    }
}
