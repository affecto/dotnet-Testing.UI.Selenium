using Protractor;

namespace Affecto.Testing.UI.Selenium
{
    public class AngularWebHostDriver : WebHostDriver
    {
        public AngularWebHostDriver(string browserName)
            : base(browserName)
        {
            Value = new NgWebDriver(Value);
        }
    }
}
