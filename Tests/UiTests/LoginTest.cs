using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Interfaces;
using Serilog;

using Models;
using Pages;
using Helpers;

namespace Automation.Tests.UiTests
{
    public class LoginTesting
    {
        private IWebDriver driver;

        private const string UsersDataPath = "Resources/Users.json";
        public static IEnumerable<UserTestData> Users => TestDataLoader.LoadUsers(UsersDataPath);

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

        }

        [Test, TestCaseSource(nameof(Users))]
        [Category("UI")]
        public void LoginTest(UserTestData data)
        {
            Logger.Info($"Тест логина: {data.UserName}, ожидаем успех = {data.ShouldSucceed}");

            Logger.Info("Starting Basic Login tests");
            try
            {

                Logger.Info("Opening login page");
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

                Logger.Info("Specifiying credentials");
                var loginPage = new LoginPage(driver);
                loginPage.Login(data.UserName, data.Password);


                if (data.ShouldSucceed)
                {
                    Logger.Info("Cheking if after-login success message exists");
                    var securePage = new SecureAreaPage(driver);
                    string successMessage = securePage.GetPostLoginMessage();
                    Assert.That(successMessage, Does.Contain("You logged into a secure area!"));
                }
                else
                {
                    Logger.Info("Cheking if login failed message exists");
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
            Logger.Info("Closing browser");

            driver.Dispose();
        }
        [OneTimeTearDown]
        public void FlushLogger() => Serilog.Log.CloseAndFlush();
    }
}