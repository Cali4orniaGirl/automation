using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Interfaces;
using Serilog;

using Pages;
using Helpers;

namespace Automation.Tests.UiTests
{
    public class LoginTesting
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

        }

        [TestCase("tomsmith", "SuperSecretPassword!", true, TestName = "Basic Login test", Category = "UI")]
        [TestCase("123", "SuperSecretPassword!", false, TestName = "Wrong Username test", Category = "UI")]
        [TestCase("tomsmith", "`123", false, TestName = "Wrong Password test", Category = "UI")]

        public void LoginTest(string username, string password, bool isPassed)
        {
            Helpers.Logger.Info("Starting Basic Login tests");
            try
            {

                Helpers.Logger.Info("Opening login page");
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

                Helpers.Logger.Info("Specifiying credentials");
                var loginPage = new LoginPage(driver);
                loginPage.Login(username, password);


                if (isPassed)
                {
                    Helpers.Logger.Info("Cheking if after-login success message exists");
                    var securePage = new SecureAreaPage(driver);
                    string successMessage = securePage.GetPostLoginMessage();
                    Assert.That(successMessage, Does.Contain("You logged into a secure area!"));
                }
                else
                {
                    Helpers.Logger.Info("Cheking if login failed message exists");
                    var message = loginPage.GetMessage();
                    Assert.That(message, Does.Contain("Your password is invalid!").Or.Contains("Your username is invalid!"));
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Couldn't login or verify message");
                throw;
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
            Helpers.Logger.Info("Closing browser");

            driver.Dispose();
        }
        [OneTimeTearDown]
        public void FlushLogger() => Serilog.Log.CloseAndFlush();
    }
}