using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace Affecto.Testing.UI.Selenium
{
    public class WebHostDriver : IDisposable
    {
        private const int MaximumWaitInSecondsWhenFindingPageContent = 3;
        private const int MaxWaitDefaultInMilliseconds = 2000;

        public IWebDriver Value { get; protected set; }

        public WebHostDriver(string browserName)
        {
            switch (browserName)
            {
                case "Internet Explorer":
                    Value = new InternetExplorerDriver();
                    break;
                case "Mozilla Firefox":
                    FirefoxProfile profile = new FirefoxProfile();
                    profile.SetPreference("network.automatic-ntlm-auth.trusted-uris", "localhost");
                    Value = new FirefoxDriver(profile);
                    break;
                case "Google Chrome":
                    Value = new ChromeDriver();
                    break;
                case "PhantomJS":
                    Value = new PhantomJSDriver();
                    break;
                default:
                    throw new ArgumentException(string.Format("Browser '{0}' not supported.", browserName));
            }
            Value.Manage().Window.Maximize();
            Value.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, MaximumWaitInSecondsWhenFindingPageContent));
        }

        public void NavigateTo(string url)
        {
            Value.Navigate().GoToUrl(url);
        }

        public void SelectDropDownListItem(string elementId, string itemText)
        {
            IWebElement selectDocumentTypeDropDown = Value.FindElement(By.Id(elementId));
            SelectItem(selectDocumentTypeDropDown, itemText);
        }

        public void SelectDropDownListItemWithWait(string elementId, string itemText, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement selectDocumentTypeDropDown = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            SelectItem(selectDocumentTypeDropDown, itemText);
        }

        public void SelectDropDownListItem(string labelAndDropDownListElementId, string elementLabel, string itemText)
        {
            IWebElement dropDownListElement = GetDropDownListElement(labelAndDropDownListElementId, elementLabel);
            SelectItem(dropDownListElement, itemText);
        }

        public void TypeText(string inputElementId, string text)
        {
            IWebElement inputElement = Value.FindElement(By.Id(inputElementId));
            ClearAndTypeText(text, inputElement);
        }

        public void TypeTextWithWait(string inputElementId, string text, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement inputElement = FindElementByIdWithWait(inputElementId, maxWaitInMilliseconds);
            ClearAndTypeText(text, inputElement);
        }

        public void ClickElement(string elementId)
        {
            Value.FindElement(By.Id(elementId)).Click();
        }

        public void ClickElementWithWait(string elementId, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            FindElementByIdWithWait(elementId, maxWaitInMilliseconds).Click();
        }

        public void ClickLink(string linkText)
        {
            Value.FindElement(By.LinkText(linkText)).Click();
        }

        public List<string> GetDropDownListContent(string elementId)
        {
            IWebElement dropDownListElement = FindElementAndSleep(elementId);
            return GetDropDownListContent(dropDownListElement);
        }

        public List<string> GetDropDownListContent(string labelAndDropDownListElementId, string label)
        {
            IWebElement dropDownListElement = GetDropDownListElement(labelAndDropDownListElementId, label);
            return GetDropDownListContent(dropDownListElement);
        }

        public string GetDropDownListSelection(string labelAndDropDownListElementId, string label)
        {
            IWebElement dropDownListElement = GetDropDownListElement(labelAndDropDownListElementId, label);
            return dropDownListElement.FindElement(By.XPath(".//option[@selected=\"selected\"]")).Text;
        }

        public string GetDropDownListSelection(string elementId)
        {
            IWebElement dropDownListElement = Value.FindElement(By.Id(elementId));
            return dropDownListElement.FindElement(By.XPath(".//option[@selected=\"selected\"]")).Text;
        }

        public string GetDropDownListSelectionWithWait(string elementId, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement dropDownListElement = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            return dropDownListElement.FindElement(By.XPath(".//option[@selected=\"selected\"]")).Text;
        }

        public bool IsDropDownListEnabled(string labelAndDropDownListElementId, string elementLabel)
        {
            IWebElement dropDownListElement = GetDropDownListElement(labelAndDropDownListElementId, elementLabel);
            return dropDownListElement.Enabled;
        }

        public bool ElementHasText(string elementId, string textToFind)
        {
            IWebElement element = Value.FindElement(By.Id(elementId));
            return element.Text.Contains(textToFind);
        }

        public bool ElementHasTextWithWait(string elementId, string textToFind, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement element = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            return element.Text.Contains(textToFind);
        }

        public bool InputHasText(string elementId, string textToFind)
        {
            IWebElement element = FindElementByIdWithWait(elementId);
            return element.GetAttribute("value") == textToFind;
        }

        public bool InputHasTextWithWait(string elementId, string textToFind, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement element = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            return element.GetAttribute("value") == textToFind;
        }

        public bool ElementHasAnyText(string elementId)
        {
            IWebElement element = FindElementAndSleep(elementId);
            return !string.IsNullOrWhiteSpace(element.Text);
        }

        public bool ElementHasAnyTextWithWait(string elementId, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement element = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            return !string.IsNullOrWhiteSpace(element.Text);
        }

        public bool IsElementPresent(string elementId)
        {
            ReadOnlyCollection<IWebElement> elements = FindElementsAndSleep(elementId);
            return VerifyIsSingleElementPresent(elementId, elements);
        }

        public bool IsElementPresentWithWait(string elementId, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            ReadOnlyCollection<IWebElement> elements = FindElementsByIdWithWait(elementId, maxWaitInMilliseconds);
            return VerifyIsSingleElementPresent(elementId, elements);
        }

        public bool IsTableCellLinkPresent(string tableElementId, string linkText)
        {
            IWebElement tableElement = FindElementByIdWithWait(tableElementId);
            ReadOnlyCollection<IWebElement> cellLinks = tableElement.FindElements(By.XPath(string.Format("//td[a=\"{0}\"]", linkText)));
            return cellLinks.Count == 1;
        }

        public bool IsElementDisplayed(string elementId)
        {
            IWebElement element = FindElementAndSleep(elementId);
            return element.Displayed;
        }

        public bool IsElementDisplayedWithWait(string elementId, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement element = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            return element.Displayed;
        }

        public bool IsElementEnabled(string elementId)
        {
            IWebElement element = FindElementAndSleep(elementId);
            return element.Enabled;
        }

        public bool IsElementEnabledWithWait(string elementId, int maxWaitInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            IWebElement element = FindElementByIdWithWait(elementId, maxWaitInMilliseconds);
            return element.Enabled;
        }

        public IWebElement FindElementByIdWithWait(string elementId, int maxWaitTimeInMilliseconds = MaxWaitDefaultInMilliseconds)
        {
            WebDriverWait wait = new WebDriverWait(Value, TimeSpan.FromMilliseconds(maxWaitTimeInMilliseconds));
            IWebElement resultElement = wait.Until(d => d.FindElement(By.Id(elementId)));
            return resultElement;
        }

        public void Dispose()
        {
            Value.Quit();
        }

        private static void SelectItem(IWebElement dropDownElement, string itemText)
        {
            SelectElement selector = new SelectElement(dropDownElement);
            selector.SelectByText(itemText);
        }

        private static bool VerifyIsSingleElementPresent(string elementId, ReadOnlyCollection<IWebElement> elements)
        {
            if (elements.Count > 1)
            {
                throw new MultipleMatchingElementsException(string.Format("More than one matching element found with id '{0}'.", elementId));
            }
            return elements.Count == 1;
        }

        private IWebElement GetDropDownListElement(string labelAndDropDownListElementId, string label)
        {
            foreach (IWebElement labelAndDropDownList in Value.FindElements(By.Id(labelAndDropDownListElementId)))
            {
                IWebElement labelElement = labelAndDropDownList.FindElement(By.TagName("label"));
                if (labelElement.Text.Equals(label))
                {
                    return labelAndDropDownList.FindElement(By.TagName("select"));
                }
            }
            throw new ArgumentException(string.Format("Label '{0}' not found in element '{1}'.", label, labelAndDropDownListElementId));
        }

        private IWebElement FindElementAndSleep(string elementId)
        {
            IWebElement element = Value.FindElement(By.Id(elementId));
            Thread.Sleep(200);
            return element;
        }

        private ReadOnlyCollection<IWebElement> FindElementsAndSleep(string elementId)
        {
            ReadOnlyCollection<IWebElement> elements = Value.FindElements(By.Id(elementId));
            Thread.Sleep(200);
            return elements;
        }

        private ReadOnlyCollection<IWebElement> FindElementsByIdWithWait(string elementId, int maxWaitTimeInMilliseconds)
        {
            WebDriverWait wait = new WebDriverWait(Value, TimeSpan.FromMilliseconds(maxWaitTimeInMilliseconds));
            ReadOnlyCollection<IWebElement> resultElements = wait.Until(d => d.FindElements(By.Id(elementId)));
            return resultElements;
        }

        private static List<string> GetDropDownListContent(ISearchContext selectDocumentTypeDropDown)
        {
            return selectDocumentTypeDropDown.FindElements(By.TagName("option")).Select(option => option.Text).ToList();
        }

        private static void ClearAndTypeText(string text, IWebElement inputElement)
        {
            inputElement.Clear();
            inputElement.SendKeys(text);
        }
    }
}