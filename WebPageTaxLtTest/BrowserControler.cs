using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace WebPageTaxLtTest
{
    public class BrowserControler 
    {
        public IWebDriver driver;
        public BrowserControler(IWebDriver driver)
        {
            this.driver = driver;
        }
        MainTest main = new MainTest();
        public void NavUrl(string newUrl)
        {
            driver.Manage().Window.Maximize();
            driver.Url = newUrl;
        }

        public void ClickElementByXpath(string xpath)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(30);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Message = "Element to be searched not found";
            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            searchResult.Click();
        }
    }
}
