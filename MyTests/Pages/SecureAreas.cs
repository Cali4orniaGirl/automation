using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace MyTests.Pages
{
    public class SecureAreaPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public SecureAreaPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement Message => driver.FindElement(By.Id("flash"));


        public string GetPostLoginMessage()
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(By.Id("flash"))).Text;
        }
    }
}