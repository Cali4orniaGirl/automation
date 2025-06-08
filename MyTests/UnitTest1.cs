using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MyTests
{
    public class LoginTest
    {
        private IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }

        [TestCase("tomsmith", "SuperSecretPassword!", true)]
        [TestCase("tomsmith","123", false)]
        [TestCase("123","SuperSecretPassword!", false)]
        public void LoginTests(string username, string password, bool isPassed)
        {

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.CssSelector("#login > button")).Click();

            string message = driver.FindElement(By.Id("flash")).Text;
            if (isPassed)
            {
                Assert.That(message, Does.Contain("You logged into a secure area!"));
            }
            else
                Assert.That(message, Does.Contain("Your password is invalid!").Or.Contains("Your username is invalid!"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Dispose();
        }
    }
}