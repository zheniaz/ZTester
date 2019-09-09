using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZTester.models
{
    public class NetworkSettingsModel
    {
        [XmlAttribute("SettingName")]
        public string SettingName { get; set; }

        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }

        [XmlElement("URL")]
        public string URL { get; set; }

        [XmlElement("TestFileLocation")]
        public string TestFileLocation { get; set; }

        [XmlElement("Domain")]
        public string Domain { get; set; }

        [XmlElement("NetworkName")]
        public string NetworkName { get; set; }

        [XmlElement("IPAdress")]
        public string IPAdress { get; set; }
    }
}