using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement UsernameInput => driver.FindElement(By.Id("username"));
        private IWebElement PasswordInput => driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => driver.FindElement(By.CssSelector("#login > button"));

        public void Login(string username, string password)
        {
            UsernameInput.SendKeys(username);
            PasswordInput.SendKeys(password);
            LoginButton.Click();
        }

        public string GetMessage()
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(By.Id("flash"))).Text;
        }
    }
}