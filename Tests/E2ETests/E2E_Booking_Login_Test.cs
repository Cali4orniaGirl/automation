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
        private const string UsersDataPath = "Resources/E2EUsers.json";
        public static IEnumerable<E2EUsersTestData> Users => TestDataLoader.LoadE2EUsers(UsersDataPath);

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

        }

        [Test, TestCaseSource(nameof(Users))]
        [Category("E2E")]
        [Order(1)]
        public void LoginTest(E2EUsersTestData data)
        {
            Logger.Info("Starting Login and Logout test");
            try
            {

                Logger.Info("Creating User via API");
                var status = CreateUserApi.CreateUser(data.UserName, data.Password);
                Assert.That(status, Is.EqualTo(HttpStatusCode.Created));

                Logger.Info("Opening demoqa login page");
                driver.Navigate().GoToUrl("https://demoqa.com/login");

                Logger.Info($"Specifiying credentials: {data.UserName} and {data.Password}");
                var loginPage = new BookLoginPage(driver);
                loginPage.Login(data.UserName, data.Password);

                Logger.Info("Verifying that profile page is displayed");
                var profilePage = new ProfilePage(driver);
                Assert.That(profilePage.IsUsenameVisible(), Is.EqualTo(data.UserName));

                Logger.Info("User is logged out");
                profilePage.IsLogoutButtonInteractable();
            }
            
            catch(Exception ex)
            {
                Logger.Error(ex, "Couldn't create user or login failed");
                throw;
            }
        }

        [Test, TestCaseSource(nameof(Users))]
        [Category("E2E")]
        [Order(2)]
        public void DeleteAccountTest(E2EUsersTestData data)
        {
            Logger.Info("Starting Delete Account Test");
            try
            {
                Logger.Info("Opening demoqa login page");
                driver.Navigate().GoToUrl("https://demoqa.com/login");

                Logger.Info($"Relogin with the same user: {data.UserName} and {data.Password}");
                var loginPage = new BookLoginPage(driver);
                loginPage.Login(data.UserName, data.Password);

                Logger.Info($"Verifying that profile page is displayed");
                var profilePage = new ProfilePage(driver);
                Assert.That(profilePage.IsUsenameVisible(), Is.EqualTo(data.UserName));

                Logger.Info("Searching for delete button");
                profilePage.IsDeleteAccButtonInteractable();
                Logger.Info("Account is Deleted");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Couldn't delete user or login failed");
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
            Logger.Info("Closing browser");

            driver.Dispose();
        }
        [OneTimeTearDown]
        public void FlushLogger() => Log.CloseAndFlush();
    }
}




//VladVampire / data.Password1!