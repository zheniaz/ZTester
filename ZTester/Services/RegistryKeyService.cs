using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ZTester.Services
{
    class RegistryKeyService
    {
        public void AddRegistryKey(string keyPath, string value, string data)
        {
            RegistryKey registryKey = Registry.LocalMachine;

            using (RegistryKey myRegKey = registryKey.OpenSubKey(keyPath, true))
            {



                //myRegKey.SetValue(value, data);

                //if (myRegKey.GetValueNames().Contains(value))
                //{
                //    myRegKey.SetValue(value, data);
                //}
                //else
                //{
                //    Console.WriteLine((string)registryKey.GetValue("id", "ID not found."));
                //}

                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions | RegistryRights.ReadKey);
                RegistrySecurity sec = new RegistrySecurity();
                
                sec.AddAccessRule(new RegistryAccessRule("Administrator", RegistryRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                rk.SetAccessControl(sec);
                rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
                sec.SetOwner(new NTAccount("Administrators"));
                rk.SetAccessControl(sec);
                rk.SetValue("AutoAdminLogon", "1");
                rk.SetValue("DefaultUserName", "admin");
                rk.SetValue("DefaultPassword", "1");
                rk.SetValue("DefaultDomainName", "INTOWINDOWS");
            }
        }

        public void DeleteRegistryKey(string keyPath, string key)
        {
            RegistryKey registryKey = Registry.LocalMachine;

            using (RegistryKey myRegKey = registryKey.OpenSubKey(keyPath, true))
            {
                if (myRegKey.GetValueNames().Contains(key))
                {
                    myRegKey.DeleteValue(key);
                }
                else
                {
                    Console.WriteLine((string)registryKey.GetValue("id", "ID not found."));
                }
            }
        }
    }
}
