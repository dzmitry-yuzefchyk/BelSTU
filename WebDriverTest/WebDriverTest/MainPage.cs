using OpenQA.Selenium;
using System.Threading;

namespace WebDriverTest
{
    public static class MainPage
    {
        public static string pageUrl = "https://www.ttn.by/";
        public static string searchInputTagId = "woocommerce-product-search-field-0";
        public static string firstProductPath = "//*[@class='search-popup-ad']/div[2]/div[1]/a/div[1]";

        public static string GetSearchFieldValue(string productName, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(pageUrl);
            IWebElement searchElement = driver.FindElement(By.Id(searchInputTagId));
            CartPage.SlowType(searchElement, productName);
            Thread.Sleep(3000);

            string prductTitle = driver.FindElement(By.XPath(firstProductPath)).Text;
            return prductTitle;
        }
    }
}
