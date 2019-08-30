using System;
using System.Collections.Generic;
using Util;
using ZTester.Services;

namespace ZTester.Tests
{
    class WSHTester
    {
        private InputService _inputService = new InputService();
        private FileService _fileService = new FileService();
        private CMDService _cmdService = new CMDService();

        private string scriptLocation = @"C:\work";
        private string sourceXMLFileName = "";
        private enum ScriptType { None, JScript, VBScript };
        public bool IsXMLFileExists { get { return _fileService.CheckIfFileExists(scriptLocation); } set { } }

        public void StartTest()
        {
            List<string> xmlFiles = _fileService.GetAllFilesWithExectExtantion(scriptLocation, "xml");
            List<string> scriptFolders = null;
            bool isXMLExists = false;
            bool isScriptExists = false;
            string testName = "";
            string testPath = "";
            string sourceTestFile = "";
            string scriptName = "";
            ScriptType testType = ScriptType.None;
            Dictionary<string, string> xmlDictionary = new Dictionary<string, string>();
            string TCInfoXmlPath = "";

            Console.WriteLine("Type test name (Example: JS-Math): ");
            do
            {
                testName = _inputService.ReadValueFromConsole();
                testPath = "C:\\work\\" + testName + ".xml";
                isXMLExists = xmlFiles.Exists(item => item == testPath);
                if (!isXMLExists)
                {
                    Console.WriteLine("Test with this name does not exist, please type again: ");
                }
                else
                {
                    sourceTestFile = "C:\\work\\" + testName + " - source";
                    if (testName.Contains("JS"))
                    {
                        testType = ScriptType.JScript;
                    }
                    else if (testName.Contains("VBS"))
                    {
                        testType = ScriptType.VBScript;
                    }
                }
            } while (!isXMLExists);

            string scriptPath = @"C:\WSH\TC\Scripting VBSTest";
            scriptFolders = _fileService.GetDirectories($@"{scriptPath}\{testType}\{testName}");
            Console.WriteLine("Type script name (Example: Cos001): "); // need to implement for Cos001,Cos002,Cos003
            do
            {
                scriptName = _inputService.ReadValueFromConsole();
                scriptPath = $"C:\\WSH\\TC\\Scripting VBSTest\\{testType}\\{testName}\\{scriptName}";
                foreach (var item in scriptFolders)
                {
                    if (item == scriptPath)
                    {
                        isScriptExists = true;
                    }
                }
                if (!isScriptExists)
                {
                    Console.WriteLine("Script with this name does not exist, please type again: ");
                }
            } while (!isScriptExists);

            xmlDictionary.Add("TCName", scriptName);
            if (testType == ScriptType.JScript)
            {
                TCInfoXmlPath = $"\\Scripting VBSTest\\JScript\\{testName}\\{scriptName}\\TCInfo.xml";
            }
            else if (testType == ScriptType.VBScript)
            {
                TCInfoXmlPath = $"\\Scripting VBSTest\\VBScript\\{testName}\\{scriptName}\\TCInfo.xml";
            }

            bool isSpecific = (testName == "JS-Math" && scriptName[scriptName.Length - 1] == 'i');
            xmlDictionary.Add("TCInfoXmlPath", TCInfoXmlPath);
            if (isSpecific)
            {
                xmlDictionary.Add("VailOS", "Itanium OSes");
            }
            else
            {
                xmlDictionary.Add("VailOS", "All Operating Systems");
            }

            if (_fileService.CheckIfFileExists($"{sourceTestFile}.xml"))
            {
                _fileService.RemoveFile($"{scriptLocation}", testName, _fileService.CheckIfFileExists($"{scriptLocation}\\{testName}.xml"));
            }
            else
            {
                _fileService.RenameFile($"C:\\work\\{testName}.xml", $"{sourceTestFile}.xml");
            }


            _fileService.CreateAndWriteXMLFile($"C:\\work\\{testName}.xml", $"{testName}.xml", xmlDictionary, testName, isSpecific);

            string arguments = $"{scriptName} {testName} 5.6";
            string fileName = "WSHTester.exe";
            string workDirectory = "C:\\work";
            _cmdService.RunCMDCommand(arguments, fileName, workDirectory, "", true);

            _fileService.RemoveFile(scriptLocation, $"{testName}.xml", isXMLExists);
            _fileService.RenameFile($"{sourceTestFile}.xml", $"{testPath}");
        }
    }
}
