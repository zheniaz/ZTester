using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Util;
using ZTester.models;

namespace ZTester.Services
{
    class XMLService
    {
        FileService _fileService = new FileService();
        string ZTesterConfigFilePath = "";
        string ZTestSettingConfigFilePath = "";

        public XMLService()
        {
            ZTesterConfigFilePath = $"{_fileService.GetFullFilePath(Constants.ZTesterConfigName)}";
            ZTestSettingConfigFilePath = $"{_fileService.GetFullFilePath(Constants.ZTestSettingConfigName)}";
        }

        #region NetworkSettingsModel XML Region

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

        #endregion

        #region ZTestSettingModel XML Region

        public List<ZTestSettingModel> GetZTestSettingsList()
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();

            XmlSerializer deserializer = new XmlSerializer(typeof(List<ZTestSettingModel>));
            TextReader textReader = new StreamReader(ZTestSettingConfigFilePath);
            testSettingList = (List<ZTestSettingModel>)deserializer.Deserialize(textReader);
            textReader.Close();

            return testSettingList;
        }

        public ZTestSettingModel GetZTestSetting(TestType testType)
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            ZTestSettingModel networkSettings = testSettingList.Find(n => n.TestName == testType.ToString());
            return networkSettings;
        }

        public int GetZTestSettingCount()
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            return testSettingList.Count;
        }

        public void AddToExistingTestSettingConfigFile(List<ZTestSettingModel> newTestSettings)
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            testSettingList.AddRange(newTestSettings);

            SerializeToXML(testSettingList, ZTestSettingConfigFilePath);
        }

        public void RemoveZtestSetting(TestType testType)
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            testSettingList.Remove(testSettingList.Find(t => t.TestName == testType.ToString()));
            SerializeToXML(testSettingList, ZTestSettingConfigFilePath);
        }

        #endregion

        public void SerializeToXML<T>(List<T> list, string targetPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter textWriter = new StreamWriter(targetPath);
            serializer.Serialize(textWriter, list);
            textWriter.Close();
        }
    }
}
