using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebPageTaxLtTest
{
    public class SelaryTaxCalc
    {
        private IWebDriver driver;
        public SelaryTaxCalc(IWebDriver driver)
        {
            this.driver = driver;
        }
        public string Input()
        {
            string input = "5000";
            return input;
        }
        public string CalculateSalary()
        {
            double brutto = System.Convert.ToDouble(Input());
            string salaryString = "";
            
            double npd;
            double rounded;
            double salaryNotFormated;

            if (brutto <= 540)
            {
                salaryNotFormated = brutto - (brutto * 0.0698) - (brutto * 0.1252);
                
            }         
            else if (brutto <= 730)
            {
                npd = 540;
                salaryNotFormated = brutto - ((brutto - npd) * 0.2)-(brutto * 0.0698) - (brutto * 0.1252);                                       
            }
            else if (brutto<= 1704)
            {
                npd = 540 - 0.34 * (brutto - 730);
                salaryNotFormated = brutto - ((brutto-npd) * 0.2) - (brutto * 0.0698) - (brutto * 0.1252);                                       
            }
            else if (brutto<= 2864)
            {
                npd = 400 - 0.18 * (brutto - 642);
                salaryNotFormated = brutto - ((brutto - npd) * 0.2) - (brutto * 0.0698) - (brutto * 0.1252);                                        
            }
            else
            {
                npd = 0;
                salaryNotFormated = brutto - ((brutto - npd) * 0.2) - (brutto * 0.0698) - (brutto * 0.1252);                                          
            }
            Console.WriteLine(salaryNotFormated);
            rounded = Math.Round(salaryNotFormated, 2);        
            salaryString = string.Format("{0:0.00}", rounded);

            return salaryString;
        }
        public string FormatSalaryString()
        {
            string salary;
            salary = Regex.Replace(CalculateSalary(), @"\.", ",");
            string pattern = ".*(.{3})$";
            Match match = Regex.Match(salary, pattern);
            string lastThreeChar = match.Groups[1].Value;
            string firstChars = Regex.Replace(salary, ".{3}$", "");
            string salaryComaDelimited = System.Convert.ToInt32(firstChars).ToString("0,0", CultureInfo.InvariantCulture); //from ##,###,###
            string salarySpaceDelimited = Regex.Replace(salaryComaDelimited, ",", " "); //returns ## ### ###
            string clculatedFormatedSalary = salarySpaceDelimited + lastThreeChar + " €"; //combine strings
            string regex = @"^0(.*)";
            string replacement = @"$1"; 
            clculatedFormatedSalary = Regex.Replace(clculatedFormatedSalary, regex, replacement);
            return clculatedFormatedSalary;
        }
 
        public void EnterSalaryBrutto()
        {
            string xpath = "//input[@id=\"atlyginimas\"]";
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(30);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Message = "Element to be searched not found";
            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            searchResult.SendKeys(Input());
        }
        public void WaitElementPresent(string xpath)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(5);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Message = "Element to be searched not found";
            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
        }
        public void GoToElementByXpath(IWebDriver driver, string xpath)
        {
            WaitElementPresent(xpath);
            var element = driver.FindElement(By.XPath(xpath));
            Actions actions = new Actions(driver);
            actions.MoveToElement(element);
        }
        public string getTextByXpath()
        {
            string xpath = "//td[@id=\"i_rankas\"]";
            GoToElementByXpath(driver, xpath);
            string xpathText = "";
            while (xpathText == "")
            {
                By element = By.XPath(xpath);
                xpathText = driver.FindElement(element).Text;
            }
            return xpathText;
        }
        public void CheckOutputGood()
        {
            if (getTextByXpath() != FormatSalaryString())
            {
                Assert.Fail(getTextByXpath() + " != " + FormatSalaryString());
            }
        }
    }
}
