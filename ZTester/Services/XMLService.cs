using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ZTester.models;

namespace ZTester.Services
{
    class XMLService
    {
        public List<NetworkSettings> GetZTesterConfigData(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<NetworkSettings>));
            TextReader textReader = new StreamReader(path);
            List<NetworkSettings> networkSettings;
            networkSettings = (List<NetworkSettings>)deserializer.Deserialize(textReader);
            textReader.Close();
            return networkSettings;
        }

        public void SerializeToXML<T>(List<T> list, string targetPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter textWriter = new StreamWriter(targetPath);
            serializer.Serialize(textWriter, list);
            textWriter.Close();
        }
    }
}
