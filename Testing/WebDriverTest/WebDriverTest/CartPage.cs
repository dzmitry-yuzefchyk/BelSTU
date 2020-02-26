using OpenQA.Selenium;
using System.Threading;

namespace WebDriverTest
{
    public static class CartPage
    {
        public static string pageUrl = "https://www.ttn.by/shop/cart?issubmit=0";
        public static string searchInputTagId = "woocommerce-product-search-field-0";
        public static string firstProductPath = "//*[@class='search-popup-ad']/div[2]/div[1]/a";
        public static string addToCartButtonPath = "//*[@class='single_add_to_cart_button button alt wc-variation-selection-needed to-cart-for-ajax']";
        public static string firstProductInCartPath = "//*[@id='cart-table']/tbody/tr[1]/td[2]/div/div/a";

        public static void AddToCart(string productName, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(MainPage.pageUrl);
            IWebElement searchElement = driver.FindElement(By.Id(searchInputTagId));
            SlowType(searchElement, productName);
            Thread.Sleep(3000);

            driver.FindElement(By.XPath(firstProductPath)).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.XPath(addToCartButtonPath)).Click();
            Thread.Sleep(3000);
        }

        public static string CheckAddingToCart(string productName, IWebDriver driver)
        {
            AddToCart(productName, driver);
            driver.Navigate().GoToUrl(pageUrl);
            string productTitle = driver.FindElement(By.XPath(firstProductInCartPath)).Text;
            return productTitle;
        }

        public static void SlowType(IWebElement webElement, string text)
        {
            webElement.Click();
            webElement.Clear();
            foreach (var key in text)
            {
                webElement.SendKeys(key.ToString());
                Thread.Sleep(150);
            }
        }
    }
}
