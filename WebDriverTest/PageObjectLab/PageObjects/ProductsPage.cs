using OpenQA.Selenium;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace PageObjectLab.PageObjects
{
    public class ProductsPage
    {
        private IWebDriver driver;

        public ProductsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private string productsComparisonSelector = "div[class='div-add-to-compare-link']";
        private string comparisonXPath = "//*[@id='masthead']/div[1]/div[2]/ul[1]/li/a";
        private string productPricesSelector = "span[class='woocommerce-Price-amount amount']";

        public void AddTwoProducts()
        {
            Thread.Sleep(5000);
            var products = driver.FindElements(By.CssSelector(productsComparisonSelector));
            products[0].Click();
            products[1].Click();
        }

        public void ApplyMaxPriceFilter(double max)
        {
            Thread.Sleep(5000);
            string filterId = "filter-price-end";
            var filter = driver.FindElement(By.Id(filterId));
            filter.Click();
            filter.SendKeys(max.ToString());
            filter.Submit();
        }

        public bool CheckIfPriceIsMax(double max)
        {
            Thread.Sleep(5000);
            var productPricesBlocks = driver.FindElements(By.CssSelector(productPricesSelector));
            var productPrices = new List<double>();
            foreach (var productPriceBlock in productPricesBlocks)
            {
                var productPriceString = productPriceBlock.GetAttribute("innerHTML");
                productPriceString = productPriceString.Replace(" р.", "");
                productPriceString = productPriceString.Replace('.', ',');
                productPrices.Add(double.Parse(productPriceString));
            }

            return !productPrices.Any(x => x > max);
        }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://www.ttn.by/electronics/telephony_and_communication/smartphones");
        }

        public ComparisonPage GoToComparisonPage()
        {
            var comparison = driver.FindElement(By.XPath(comparisonXPath));
            comparison.Click();
            return new ComparisonPage(driver);
        }
    }
}
