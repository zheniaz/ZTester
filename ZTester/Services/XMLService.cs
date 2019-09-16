using System.Collections.Generic;
using System.IO;
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
            if (_fileService.CheckIfFileExists(ZTestSettingConfigFilePath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<ZTestSettingModel>));
                TextReader textReader = new StreamReader(ZTestSettingConfigFilePath);
                testSettingList = (List<ZTestSettingModel>)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            

            return testSettingList;
        }

        public ZTestSettingModel GetZTestSetting(TestType testType)
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            ZTestSettingModel networkSettings = testSettingList.Find(n => n.TestName == testType.ToString());
            return networkSettings;
        }

        public void EditZTestSetting(ZTestSettingModel updatedTestSetting)
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            foreach (var item in testSettingList)
            {
                if (item.TestName == updatedTestSetting.TestName)
                {
                    item.NeedToRunTimes = updatedTestSetting.NeedToRunTimes;
                    item.IsSettedEnvironment = updatedTestSetting.IsSettedEnvironment;
                }
            }
            SerializeToXML(testSettingList);
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
            SerializeToXML(testSettingList);
        }

        public void SafeTestSettings(List<ZTestSettingModel> zTestSettingModels)
        {
            if (_fileService.CheckIfFileExists(ZTestSettingConfigFilePath))
            {
                List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
                testSettingList = GetZTestSettingsList();
                testSettingList.AddRange(zTestSettingModels);
                SerializeToXML(testSettingList);
            }
            else
            {
                SerializeToXML(zTestSettingModels);
            }
        }

        public void RemoveZtestSetting(TestType testType)
        {
            List<ZTestSettingModel> testSettingList = new List<ZTestSettingModel>();
            testSettingList = GetZTestSettingsList();
            if (testSettingList.Count == 1)
            {
                _fileService.RemoveFile(_fileService.AppPath, Constants.ZTestSettingConfigName, _fileService.IsZTestSettingXMLExists);
            }
            else
            {
                testSettingList.Remove(testSettingList.Find(t => t.TestName == testType.ToString()));
                SerializeToXML(testSettingList);
            }
        }

        #endregion

        public void SerializeToXML<T>(List<T> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter textWriter = new StreamWriter(ZTestSettingConfigFilePath);
            serializer.Serialize(textWriter, list);
            textWriter.Close();
        }
    }
}
