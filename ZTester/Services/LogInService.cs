namespace ZTester.Services
{
    class LogInService
    {
        CMDService _cmdService = new CMDService();

        public void EnableAutoLogIn()
        {
            _cmdService.RunCMDCommand($"REG ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v AutoAdminLogon /t REG_SZ /d 1 /f");
            _cmdService.RunCMDCommand($"REG ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v DefaultDomainName /t REG_SZ /d INTOWINDOWS /f");
            _cmdService.RunCMDCommand($"REG ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v DefaultUserName /t REG_SZ /d admin /f");
            _cmdService.RunCMDCommand($"REG ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v DefaultPassword /t REG_SZ /d 1 /f");
        }

        public void DisableAutoLogIn()
        {
            _cmdService.RunCMDCommand($"REG ADD {"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"} /v AutoAdminLogon /t REG_SZ /d 0 /f");
        }
    }
}
