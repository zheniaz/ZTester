using System;
using System.IO;
using IWshRuntimeLibrary;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Util;

namespace ZTester.Services
{
    class FileService
    {
        public bool CheckIfFileExists(string filePath)
        {
            bool isFileExists = System.IO.File.Exists(filePath);
            return isFileExists;
        }

        public void MoveFileToFolder(string filePath, string destination)
        {
            if (CheckIfFileExists(filePath))
            {
                System.IO.File.Move(filePath, destination);
            }
        }

        public void RemoveFile(string fileFolder, string fileName, bool isFileExists)
        {
            try
            {
                if (isFileExists)
                {
                    System.IO.File.Delete(Path.Combine(fileFolder, fileName));
                    //Console.WriteLine($"Deleted {fileName} from {fileFolder}");
                }
                else
                {
                    Console.WriteLine($"File {fileName} not found");
                }
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
            Thread.Sleep(1000);
        }

        public string GetStartupFolderPath()
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString() + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup";
            }
            return path;
        }

        public void CreateShortcut(string directoryFullPath, string fileName, bool isFileExists)
        {
            if (!isFileExists)
            {
                WshShell shell = new WshShell();
                string shortcutPathLink = directoryFullPath + $@"\{fileName}.lnk";
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(directoryFullPath + $@"\{fileName}.lnk");
                shortcut.Description = "New shortcut for a RebootLoop";
                shortcut.WorkingDirectory = directoryFullPath;
                shortcut.TargetPath = directoryFullPath + $@"\{Constants.AppName}";
                shortcut.Save();
            }
        }

        public void RenameFile(string oldFileName, string newFileName)
        {
            System.IO.File.Move(oldFileName, newFileName);
            Thread.Sleep(500);
        }

        public List<string> GetAllFilesWithExectExtantion(string sourcedirectory, string extension)
        {
            List<string> files = Directory.GetFiles(sourcedirectory, $"*.{extension}", SearchOption.AllDirectories).ToList();

            return files;
        }

        public List<string> GetDirectories(string path)
        {
            List<string> directories = Directory.GetDirectories(path).ToList();
            return directories;
        }

        public void CreateAndWriteXMLFile(string path, string fileName, Dictionary<string, string> linesList, string testName, bool isSpecific)
        {
            XmlWriter writer = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                writer = XmlWriter.Create(path, settings);

                writer.WriteStartElement("Area");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                writer.WriteAttributeString("Name", testName);
                writer.WriteAttributeString("ContextName", "Scripting VBSTest");

                writer.WriteStartElement("TCInfo");

                foreach (var item in linesList)
                {
                    writer.WriteElementString(item.Key, item.Value);
                }

                writer.WriteElementString("ProductType", "JScript 5.0");
                writer.WriteElementString("ProductType", "JScript 5.5");
                writer.WriteElementString("ProductType", "JScript 5.6");
                writer.WriteElementString("ProductType", "JScript 7.0");
                writer.WriteElementString("ProductType", "JScript 7.1");
                if (!isSpecific)
                {
                    writer.WriteElementString("ProductType", "Scripting %28WSH + JS + VBS%29");
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}
