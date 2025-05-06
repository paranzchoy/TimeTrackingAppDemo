using System.Diagnostics;

namespace TimeTrackingApp.E2E.Playwright
{
    public class Hooks
    {
        [Before(TestSession)]
        public static void InstallPlaywright()
        {
            if (Debugger.IsAttached)
            {
                Environment.SetEnvironmentVariable("PWDEBUG", "1");
            }

            Microsoft.Playwright.Program.Main(["install"]);
        }
    }
}