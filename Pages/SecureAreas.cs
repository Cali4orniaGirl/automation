using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Pages
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
        
        public string GetPostLoginMessage()
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(By.Id("flash"))).Text;
        }
    }
}