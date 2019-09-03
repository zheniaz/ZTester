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
        public void GetZTesterConfigData<T>(ref List<T> networkSettings, string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            TextReader textReader = new StreamReader(path);
            //List<T> networkSettings;
            networkSettings = (List<T>)deserializer.Deserialize(textReader);
            textReader.Close();
            //return networkSettings;
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
