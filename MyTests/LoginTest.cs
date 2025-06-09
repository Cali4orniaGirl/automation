using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;

using MyTests.Pages;
using MyTests.Helpers;

namespace MyTests
{
    public class LoginTesting
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [TestCase("tomsmith", "SuperSecretPassword!", true, TestName = "Basic Login test", Category = "UI")]
        [TestCase("123", "SuperSecretPassword!", false, TestName = "Wrong Username test", Category = "UI")]
        [TestCase("tomsmith", "`123", false, TestName = "Wrong Password test", Category = "UI")]

        public void LoginTest(string username, string password, bool isPassed)
        {

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            var loginPage = new LoginPage(driver);
            loginPage.Login(username, password);


            if (isPassed)
            {
                var securePage = new SecureAreaPage(driver);
                string successMessage = securePage.GetPostLoginMessage();
                Assert.That(successMessage, Does.Contain("You logged into a secure area!"));
            }
            else
            {
                var message = loginPage.GetMessage();
                Assert.That(message, Does.Contain("Your password is invalid!").Or.Contains("Your username is invalid!"));
            }
        }
        
        [TearDown]
        public void Teardown()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                ScreenshotHelper.TakeScreenshot(driver, testName);
            }

            driver.Dispose();
        }
    }
}