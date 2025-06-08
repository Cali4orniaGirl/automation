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
        }

        [Test]
        public void isLoginSuccessfull()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("#login > button")).Click();

            string message = driver.FindElement(By.Id("flash-messages")).Text;

            Assert.That(message, Does.Contain("You logged into a secure area!"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Dispose();
        }
    }
}