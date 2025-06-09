using OpenQA.Selenium;
using System;
using System.IO;

namespace MyTests.Helpers
{
    public static class ScreenshotHelper
    {
        public static void TakeScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                var dir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(dir);

                var filePath = Path.Combine(dir, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(filePath);
                Console.WriteLine($"Screenshot saved to: {filePath}");
                }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not take screenshot: {ex.Message}");
            }
        }
    }
}