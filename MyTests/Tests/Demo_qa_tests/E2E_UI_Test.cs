using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;
using RestSharp;
using System.Net;
using System.Text.Json;

using MyTests.Pages;
using MyTests.Helpers;
using MyTests.Models;


namespace MyTests.Tests
{
    public class BookingLogin
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            try
            {
                driver = new ChromeDriver();
            }
            catch (Exception ex)
            {
                Helpers.Logger.Error("Не удалось открыть браузер", ex);
                throw;
            }
        }

        [TestCase("tomsmith", "SuperSecretPassword!", true, TestName = "Basic Login test", Category = "UI")]

        public void LoginTest(string username, string password, bool isPassed)
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
    }
}




//VladVampire / Password1!