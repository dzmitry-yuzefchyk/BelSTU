using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestFramework.Constants;
using TestFramework.Page.Base;

namespace TestFramework.Page
{
    public class PhonesPage : SearchBarPageBase<PhonesPage>
    {
        private const string _phoneCard = "//*[@id='grid-extended']/div/div/div[1]/a[2]";

        private const string _phoneCards = "a[class='woocommerce-LoopProduct-link woocommerce-loop-product__link woocommerce-loop-product__title title__a']";

        private const string _filterId = "filter-price-end";

        private const string _productPricesSelector = "span[class='woocommerce-Price-amount amount']";

        private const string _productsComparisonSelector = "div[class='div-add-to-compare-link']";
        private const string _comparisonXPath = "//*[@id='masthead']/div[1]/div[2]/ul[1]/li/a";

        private const string _checkboxSelector = "div[class='checkbox-item']";

        public PhonesPage(IWebDriver driver)
            : base(driver)
        {
            Url = PageUrl.Phones;
        }

        public PhonePage OpenPhone()
        {
            var phoneCard = _driver.FindElement(By.XPath(_phoneCard));
            phoneCard.Click();
            Thread.Sleep(500);

            return new PhonePage(_driver);
        }

        public List<string> GetTitles()
        {
            var phoneCards = _driver.FindElements(By.CssSelector(_phoneCards));
            var titles = phoneCards.Select(x => x.Text).ToList();
            return titles;
        }

        public PhonesPage ApplyMaxPriceFilter(double max)
        {
            Thread.Sleep(5000);
            var filter = _driver.FindElement(By.Id(_filterId));
            filter.Click();
            filter.SendKeys(max.ToString());
            filter.Submit();

            return this;
        }

        public PhonesPage ApplyManufacturerFilter(string manufacturer)
        {
            Thread.Sleep(5000);
            var checkboxes = _driver.FindElements(By.CssSelector(_checkboxSelector));
            foreach (var checkbox in checkboxes)
            {
                var checkboxElement = checkbox.FindElement(By.XPath("//label"));
                if (checkboxElement.GetAttribute("#text").ToLower().Contains(manufacturer.ToLower()))
                {
                    checkboxElement.Click();
                    break;
                }
            }

            return this;
        }

        public List<double> GetPrices()
        {
            Thread.Sleep(5000);
            var productPricesBlocks = _driver.FindElements(By.CssSelector(_productPricesSelector));
            var productPrices = new List<double>();
            foreach (var productPriceBlock in productPricesBlocks)
            {
                var productPriceString = productPriceBlock.GetAttribute("innerHTML");
                productPriceString = productPriceString.Replace(" р.", "");
                productPriceString = productPriceString.Replace('.', ',');
                productPrices.Add(double.Parse(productPriceString));
            }

            return productPrices;
        }

        public PhonesPage AddFirstProduct()
        {
            Thread.Sleep(5000);
            var products = _driver.FindElements(By.CssSelector(_productsComparisonSelector));
            products[0].Click();

            return this;
        }

        public ComparisonPage GoToComparisonPage()
        {
            Thread.Sleep(1000);
            var comparison = _driver.FindElement(By.XPath(_comparisonXPath));
            comparison.Click();
            return new ComparisonPage(_driver);
        }
    }
}
