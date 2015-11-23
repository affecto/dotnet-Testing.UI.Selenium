using System;

namespace Affecto.Testing.UI.Selenium
{
    internal class IisExpressExecutable
    {
        private readonly string path;

        public string FullFileName
        {
            get { return string.Format(@"{0}\IIS Express\iisexpress.exe", path); }
        }

        public IisExpressExecutable()
        {
            path = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("programfiles"))
                ? Environment.GetEnvironmentVariable("programfiles(x86)")
                : Environment.GetEnvironmentVariable("programfiles");
        }
    }
}