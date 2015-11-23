using System.Collections.Generic;
using System.Diagnostics;

namespace Affecto.Testing.UI.Selenium
{
    public class IisExpressWebServer
    {
        private readonly List<Process> processes;

        public IisExpressWebServer()
        {
            processes = new List<Process>();
        }
        
        public void Start(string webSiteProjectName)
        {
            Process process = new IisExpressProcess(webSiteProjectName);
            try
            {
                process.Start();
                processes.Add(process);
            }
            catch
            {
                process.Dispose();
            }
        }

        public void Stop()
        {
            foreach (Process process in processes)
            {
                process.Kill();
                process.Dispose();                
            }
        }
    }
}