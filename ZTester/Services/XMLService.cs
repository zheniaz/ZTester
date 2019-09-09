using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Util;
using ZTester.models;

namespace ZTester.Services
{
    class XMLService
    {
        FileService _fileService = new FileService();
        string ZTesterConfigFilePath = "";

        public XMLService()
        {
            ZTesterConfigFilePath = $"{_fileService.GetFullFilePath(Constants.ZTesterConfigName)}";
        }

        public List<NetworkSettingsModel> GetNetworkSettingsList()
        {
            List<NetworkSettingsModel> networkSettingsList = new List<NetworkSettingsModel>();

            XmlSerializer deserializer = new XmlSerializer(typeof(List<NetworkSettingsModel>));
            TextReader textReader = new StreamReader(ZTesterConfigFilePath);
            networkSettingsList = (List<NetworkSettingsModel>)deserializer.Deserialize(textReader);
            textReader.Close();

            return networkSettingsList;
        }

        public NetworkSettingsModel GetNetworkSettings(TestType testType)
        {
            List<NetworkSettingsModel> networkSettingsList = new List<NetworkSettingsModel>();
            networkSettingsList = GetNetworkSettingsList();
            NetworkSettingsModel networkSettings = networkSettingsList.Find(n => n.SettingName == testType.ToString());
            return networkSettings;
        }

        public void SerializeToXML<NetworkSettingsModel>(List<NetworkSettingsModel> list, string targetPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<NetworkSettingsModel>));
            TextWriter textWriter = new StreamWriter(targetPath);
            serializer.Serialize(textWriter, list);
            textWriter.Close();
        }
    }
}
