using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ZTester.Services
{
    class LogInService
    {
        CMDService _cmdService = new CMDService();
        RegistryKeyService _registryKeyService = new RegistryKeyService();

        public void EnableAutoLogIn()
        {
            


            string keyPath = @"Software\Microsoft\Windows NT\CurrentVersion\Winlogon";
            _registryKeyService.AddRegistryKey(keyPath, "AutoAdminLogon", "1");


            //WriteDefaultLogin("admin", "1");
            _cmdService.RunCMDCommand($"REG ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v AutoAdminLogon /t REG_SZ /d 1 /f");
            _cmdService.RunCMDCommand($"ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v AutoAdminLogon /t REG_SZ /d 1 /f", fileName: "reg");
            //_cmdService.RunCMDCommand($@"SET HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon /v AutoAdminLogonCount  /t REG_DWORD /d 1 /f", fileName: "reg");
            //_cmdService.RunCMDCommand($"ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v DefaultDomainName /t REG_SZ /d INTOWINDOWS /f", fileName: "reg");
            //_cmdService.RunCMDCommand($"ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v DefaultUserName /t REG_SZ /d admin /f", fileName: "reg");
            //_cmdService.RunCMDCommand($"ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v DefaultPassword /t REG_SZ /d 1 /f", fileName: "reg");
        }

        public static void WriteDefaultLogin(string usr, string pwd)
    {
        //creates or opens the key provided.Be very careful while playing with 
        //windows registry.
        RegistryKey rekey = Registry.LocalMachine.CreateSubKey
            ("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");

        if (rekey == null)
            Console.WriteLine
                ("There has been an error while trying to write to windows registry");
        else
        {
            //these are our hero like values here
            //simply use your RegistryKey objects SetValue method to set these keys
            rekey.SetValue("AutoAdminLogon", "1");
            rekey.SetValue("DefaultUserName", usr);
            rekey.SetValue("DefaultPassword", pwd);
        }
        //close the RegistryKey object
        rekey.Close();
    }

        public void DisableAutoLogIn()
        {
            _cmdService.RunCMDCommand($"SET {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v AutoAdminLogon /t REG_SZ /d 0 /f", fileName: "reg");
        }

        public void LoginAsGuest()
        {
            RegistrySecurity userSecurity = new RegistrySecurity();
            RegistryAccessRule userRule = new RegistryAccessRule("Everyone", RegistryRights.FullControl, AccessControlType.Allow);
            userSecurity.AddAccessRule(userRule);

            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);

            if (key == null)
            {
                //eventLogger.WriteEntry("Error accessing the registry key");
            }
            else
            {
                try
                {
                    key.SetValue("AutoAdminLogon", "1", RegistryValueKind.String);
                    key.SetValue("DefaultUserName", "admin", RegistryValueKind.String);
                    key.SetValue("DefaultPassword", "1", RegistryValueKind.String);
                }
                catch (Exception exception)
                {
                    //eventLogger.WriteEntry("Problem setting up keys: " + exception);
                }
            }
            key.Close();
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
}
