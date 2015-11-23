using Affecto.Testing.UI.Selenium;

namespace Affecto.Testing.Ui.Selenium.TestRunner
{
    internal class Program
    {
        static void Main()
        {
            using (WebHostDriver driver = new WebHostDriver("PhantomJS"))
            {
                driver.NavigateTo("www.google.com");                
            }
        }
    }
}