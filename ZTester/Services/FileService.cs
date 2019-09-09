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
        public string AppPath { get { return Path.GetDirectoryName(Path.GetFullPath(Constants.AppName)); } set { } }
        public string AppFullPath { get { return this.AppPath + "\\" + Constants.AppName; } set { } }

        #region Directory Region

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

        public List<string> GetAllFilesWithExectExtantion(string sourcedirectory, string extension)
        {
            List<string> files = Directory.GetFiles(sourcedirectory, $"*.{extension}", SearchOption.AllDirectories).ToList();

            return files;
        }

        public List<string> GetDirectories(string path)
        {
            List<string> directories = null;
            try
            {
                if (Directory.Exists(path))
                {
                    directories = Directory.GetDirectories(path).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

            return directories;
        }

        public void CreateDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        public bool CheckIfDirectoryExists(string path)
        {
            bool isDirectoryExists = Directory.Exists(path);
            return isDirectoryExists;
        }

        public void RemoveDirectory(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                string[] dirs = Directory.GetDirectories(path);

                foreach (string file in files)
                {
                    System.IO.File.SetAttributes(file, FileAttributes.Normal);
                    System.IO.File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    RemoveFilesAndFoldersFromDirectory(dir);
                }

                Directory.Delete(path, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.Message);
            }
        }

        #endregion

        #region File Region

        public bool CheckIfFileExists(string filePath, string errorMessage = "")
        {
            bool isFileExists = System.IO.File.Exists(filePath);
            if (!isFileExists && errorMessage != "")
            {
                Console.WriteLine(errorMessage);
            }
            return isFileExists;
        }

        public void RenameFile(string oldFileName, string newFileName)
        {
            System.IO.File.Move(oldFileName, newFileName);
            Thread.Sleep(500);
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

        public void RemoveFilesAndFoldersFromDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                RemoveFilesAndFoldersFromDirectory(dir);
            }
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

        public string GetPathRoot(string str)
        {
            return Path.GetPathRoot(str);
        }

        public string GetFullFilePath(string path)
        {
            return Path.GetFullPath(path);
        }

        public void CopyFile(string sourcePath, string targetPath)
        {
            try
            {
                if (Directory.Exists(sourcePath))
                {
                    System.IO.File.Copy(sourcePath, targetPath, true);
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CopyFiles(string sourcePath, string targetPath)
        {
            try
            {
                Console.WriteLine("Directory.Exists(sourcePath) exists: " + Directory.Exists(sourcePath));
                Console.WriteLine("Directory.Exists(targetPath) exists: " + Directory.Exists(targetPath));
                if (Directory.Exists(sourcePath) && Directory.Exists(targetPath))
                {
                    string[] files = Directory.GetFiles(sourcePath);

                    foreach (string file in files)
                    {
                        Console.WriteLine("file: " + file);
                        string fileName = Path.GetFileName(file);
                        string destFile = Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(file, destFile, true);
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        public List<string> GetFiles(string path)
        {
            List<string> files = null;
            try
            {
                if (Directory.Exists(path))
                {
                    files = Directory.GetFiles(path).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

            return files;
        }

        #endregion
    }
}
