using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Interfaces;
using System.Net;
using Serilog;


using Pages;
using Helpers;
using Models;


namespace Automation.Tests.E2ETests
{
    public class E2EBookingLogin
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

        }

        [TestCase("BratokTesta", "Password1!", TestName = "Login and Logout test", Category = "E2E")]
        [Order(1)]
        public void LoginTest(string username, string password)
        {
            Helpers.Logger.Info("Starting Login and Logout test");
            try
            {

                Helpers.Logger.Info("Creating User via API");
                var status = CreateUserApi.CreateUser(username, password);
                Assert.That(status, Is.EqualTo(HttpStatusCode.Created));

                Helpers.Logger.Info("Opening demoqa login page");
                driver.Navigate().GoToUrl("https://demoqa.com/login");

                Helpers.Logger.Info($"Specifiying credentials: {username} and {password}");
                var loginPage = new BookLoginPage(driver);
                loginPage.Login(username, password);

                Helpers.Logger.Info("Verifying that profile page is displayed");
                var profilePage = new ProfilePage(driver);
                Assert.That(profilePage.IsUsenameVisible(), Is.EqualTo(username));

                Helpers.Logger.Info("User is logged out");
                profilePage.IsLogoutButtonInteractable();
            }
            
            catch(Exception ex)
            {
                Helpers.Logger.Error(ex, "Couldn't create user or login failed");
                throw;
            }
        }

        [TestCase("BratokTesta", "Password1!", TestName = "Delete Account Test", Category = "E2E")]
        [Order(2)]
        public void DeleteAccountTest(string username, string password)
        {
            Helpers.Logger.Info("Starting Delete Account Test");
            try
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
                profilePage.IsDeleteAccButtonInteractable();
                Helpers.Logger.Info("Account is Deleted");
            }
            catch (Exception ex)
            {
                Helpers.Logger.Error(ex, "Couldn't delete user or login failed");
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




//VladVampire / Password1!