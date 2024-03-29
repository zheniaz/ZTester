﻿using System.Xml.Serialization;

namespace ZTester.models
{
    public class ZTestSettingModel
    {
        [XmlAttribute("TestName")]
        public string TestName { get; set; }

        [XmlElement("NeedToRunTimes")]
        public int NeedToRunTimes { get; set; }

        [XmlElement("TestFileFullPath")]
        public string TestFileFullPath { get; set; }

        [XmlElement("IsSettedEnvironment")]
        public bool IsSettedEnvironment { get; set; }

        [XmlElement("Priority")]
        public int Priority { get; set; }
    }
}
