using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MyTests.Pages;

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

        [TestCase("tomsmith", "SuperSecretPassword!", true)]
        [TestCase("123", "SuperSecretPassword!", false)]
        [TestCase("tomsmith", "`123", false)]

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


            // Assert.That(message, Does.Contain("Your password is invalid!").Or.Contains("Your username is invalid!"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Dispose();
        }
    }
}