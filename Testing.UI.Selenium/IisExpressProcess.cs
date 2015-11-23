using System.Diagnostics;

namespace Affecto.Testing.UI.Selenium
{
    internal class IisExpressProcess : Process
    {
        public IisExpressProcess(string webSiteProjectName)
        {
            IisExpressExecutable executable = new IisExpressExecutable();
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                ErrorDialog = false,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = executable.FullFileName,
                Arguments = string.Format("/site:{0}", webSiteProjectName)
            };
            StartInfo = processStartInfo;
        }
    }
}
