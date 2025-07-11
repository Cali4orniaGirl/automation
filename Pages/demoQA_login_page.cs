using OpenQA.Selenium;

namespace Pages
{
    public class BookLoginPage
    {
        private readonly IWebDriver driver;

        public BookLoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IWebElement UsernameInput => driver.FindElement(By.Id("userName"));
        private IWebElement PasswordInput => driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => driver.FindElement(By.Id("login"));

        public void Login(string username, string password)
        {
            UsernameInput.SendKeys(username);
            PasswordInput.SendKeys(password);
            LoginButton.Click();
        }
    }
}