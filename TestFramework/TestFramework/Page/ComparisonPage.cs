using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestFramework.Test.Base;

namespace TestFramework.Page
{
    public class ComparisonPage : PageBase
    {
        private const string productsSelector = "h3[class='product-title']";

        public ComparisonPage(IWebDriver driver)
            : base(driver) 
        { }

        public List<string> GetProductNames()
        {
            Thread.Sleep(5000);
            var products = _driver.FindElements(By.CssSelector(productsSelector));
            var productNames = products.Select(x => x.GetProperty("innerHTML")).ToList();

            return productNames;
        }
    }
}
