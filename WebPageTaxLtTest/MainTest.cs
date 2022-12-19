using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Security.Cryptography.X509Certificates;
namespace WebPageTaxLtTest
{
    public class MainTest
    {
        //Testing webpage https://www.tax.lt/
        BrowserControler brCtrl;
        DiscusionPage dicusionPage;
        LoginPage loginPage;
        SelaryTaxCalc selaryTax;
        IWebDriver driver;
        public string TestName = "";

        [SetUp]
        public void BuildUp()
        {
            driver = new ChromeDriver();
            brCtrl = new BrowserControler(driver);
            dicusionPage = new DiscusionPage(driver);
            loginPage = new LoginPage(driver);
            selaryTax = new SelaryTaxCalc(driver);
            brCtrl.NavUrl("https://www.tax.lt");
            brCtrl.ClickElementByXpath("//button[@aria-label=\"Sutinku\"]");
        }

        [TearDown]
        public void BuilDown()
        {
            string[] files = Directory.GetFiles("C:\\Users\\mbp10\\Documents\\TestScreenShots\\");

            if (files.Length > 10)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"{file} is deleted.");
                }
            }
            try
            {
                Screenshot TakeScreenshot = ((ITakesScreenshot)driver).GetScreenshot();
                DateTime Time = new DateTime();
                TakeScreenshot.SaveAsFile("C:\\Users\\mbp10\\Documents\\TestScreenShots\\"+ TestName + DateTime.Now.ToString("h-mm") + ".png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            driver.Close();
        }

        [Test]//Test Case nr.003 
        public void TopicAndHeaderCompareTest()
        {
            TestName = "TopicAndHeaderCompareTest"; //This name will be present in test screenshot file
            dicusionPage.GoToAllDiscussions(driver);
            dicusionPage.TopicAndHeaderNameCompare(driver);
        }

        [Test]//Test Case nr.002
        public void InvalidSignInTest()
        {
            TestName = "InvalidSignInTest"; //This name will be present in test screenshot file
            brCtrl.ClickElementByXpath("//div/a[@href=\"/login\"]");
            loginPage.TypeEmail("mdb1020@gmail.com");
            loginPage.TypePasword("kotletas123");
            loginPage.ClickLoginButton();
            loginPage.CheckMessagePresentByXpath("//div[contains(@class, 'alert-error')]");
        }

        [Test]//Test Case nr.001
        public void SelaryCalculatorTest()
        {
            TestName = "SelaryCalculatorTest"; //This name will be present in test screenshot file
            brCtrl.ClickElementByXpath("(//li/a/b[@class=\"caret\"])[1]");
            brCtrl.ClickElementByXpath("//a[contains(@href, 'skaiciuokles/atlyginimo')]");
            selaryTax.EnterSalaryBrutto();
            selaryTax.CheckOutputGood();
        
        }
    }
}