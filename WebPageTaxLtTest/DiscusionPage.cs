using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace WebPageTaxLtTest
{
    public class DiscusionPage
    {
        private IWebDriver driver;
        public DiscusionPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        public string RandomTopicNumber()
        {
            Random rnd = new Random();
            int num = rnd.Next(1, 35);
            string rndNum = num.ToString();
            return rndNum;
        }
        public void WaitElementExists(IWebDriver driver, string xpath)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(30);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Message = "Element to be searched not found";
            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
        }
        public string GetElementTexByXpath(IWebDriver driver, string xpath)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(30);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Message = "Element to be searched not found";
            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            return searchResult.Text;
        }
        //Compare if clicked topic hase the same name as the redirected page topic name
        public void TopicAndHeaderNameCompare(IWebDriver driver)
        {
            string xpath = "(//div[@class=\"span8\"]/ul/li/a)[" + RandomTopicNumber() + "]";
            WaitElementExists(driver, xpath);
            By by = By.XPath(xpath);
            var element = driver.FindElement(by);
            string topicName = GetElementTexByXpath(driver, xpath);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Perform();
            element.Click();
            WaitElementExists(driver, "//div[@class=\"page-header\"]");
            string headerName = GetElementTexByXpath(driver, "//div[@class=\"page-header\"]");
            if (!topicName.Equals(headerName))
            {
                Assert.Fail(topicName + "!=" + headerName);
            }
        }
        public void GoToAllDiscussions(IWebDriver driver)
        {
            string xpath = "//a[@href=\"/temos\"]";
            WaitElementExists(driver, xpath);
            var element = driver.FindElement(By.XPath(xpath));
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Perform();
            element.Click();
        }
    }
}
