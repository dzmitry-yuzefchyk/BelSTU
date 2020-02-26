using OpenQA.Selenium;

namespace PageObjectLab.PageObjects
{
    public class ComparisonPage
    {
        private IWebDriver driver;

        private string productsSelector = "a[class='product']";

        public ComparisonPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public int CountProudcts()
        {
            var products = driver.FindElements(By.CssSelector(productsSelector));
            return products.Count;
        }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://www.ttn.by/shop/product-compare/compare");
        }
    }
}
