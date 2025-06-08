using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MyTests.Pages;

namespace MyTests
{
    public class LoginTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void isLoginSuccessfull()
        {

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            var loginPage = new LoginPage(driver);
            loginPage.Login("tomsmith", "SuperSecretPassword!");
            var message = loginPage.GetMessage();

            Assert.That(message, Does.Contain("You logged into a secure area!"));


            // Assert.That(message, Does.Contain("Your password is invalid!").Or.Contains("Your username is invalid!"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Dispose();
        }
    }
}