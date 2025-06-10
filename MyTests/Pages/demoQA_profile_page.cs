using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace MyTests.Pages
{
    public class ProfilePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public ProfilePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public IWebElement UsernameValue => driver.FindElement(By.CssSelector("#userName-value"));
        private IWebElement LogoutButton => driver.FindElement(By.XPath("/html/body/div[2]/div/div/div/div[2]/div[2]/div[1]/div[3]/button"));
        private IWebElement DeleteAccButton => driver.FindElement(By.XPath("/html/body/div[2]/div/div/div/div[2]/div[2]/div[3]/div[2]/button"));
        private IWebElement ConfirmButton => driver.FindElement(By.Id("closeSmallModal-ok"));

        public void IsLogoutButtonInteractable()
        {
            LogoutButton.Click();
        }

        public void IsDeleteAccButtonInteractable()
        {
            DeleteAccButton.Click();
            Thread.Sleep(3000);
            ConfirmButton.Click();
        }
        public string IsUsenameVisible()
        {
            return UsernameValue.Text;
        }

    }
}