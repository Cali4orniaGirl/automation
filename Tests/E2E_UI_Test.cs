using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;
using RestSharp;
using System.Net;
using System.Text.Json;

using Pages;
using Helpers;
using Models;


namespace Tests
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
                driver.Manage().Window.Maximize(); 
            }
            catch (Exception ex)
            {
                Helpers.Logger.Error("Не удалось открыть браузер", ex);
                throw;
            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

        }

        [TestCase("BratokTesta", "Password1!", TestName = "Login and Logout test", Category = "E2E")]
        [Order(1)]
        public void LoginTest(string username, string password)
        {

            var status = CreateUserApi.CreateUser(username, password);
            Assert.That(status, Is.EqualTo(HttpStatusCode.Created));

            Helpers.Logger.Info("Opening demoqa login page");
            driver.Navigate().GoToUrl("https://demoqa.com/login");

            Helpers.Logger.Info($"Specifiying credentials: {username} and {password}");
            var loginPage = new BookLoginPage(driver);
            loginPage.Login(username, password);

            Helpers.Logger.Info($"Verifying that profile page is displayed");
            var profilePage = new ProfilePage(driver);
            Assert.That(profilePage.IsUsenameVisible(), Is.EqualTo(username));

            Helpers.Logger.Info($"User is logged out");
            profilePage.IsLogoutButtonInteractable();
        }

        [TestCase("BratokTesta", "Password1!", TestName = "Delete Account Test", Category = "E2E")]
        [Order(2)]
        public void DeleteAccountTest(string username, string password)
        {
            Helpers.Logger.Info("Opening demoqa login page");
            driver.Navigate().GoToUrl("https://demoqa.com/login");

            Helpers.Logger.Info($"Relogin with the same user: {username} and {password}");
            var loginPage = new BookLoginPage(driver);
            loginPage.Login(username, password);

            Helpers.Logger.Info($"Verifying that profile page is displayed");
            var profilePage = new ProfilePage(driver);
            Assert.That(profilePage.IsUsenameVisible(), Is.EqualTo(username));

            Helpers.Logger.Info("Searching for delete button");
            Thread.Sleep(3000);
            profilePage.IsDeleteAccButtonInteractable();
            Helpers.Logger.Info("Account is Deleted");

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