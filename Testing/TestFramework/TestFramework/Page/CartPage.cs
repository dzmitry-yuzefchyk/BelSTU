using OpenQA.Selenium;
using TestFramework.Test.Base;

namespace TestFramework.Page
{
    public class CartPage : PageBase
    {
        private const string _productInCart = "//*[@id='cart-table']/tbody/tr[1]/td[2]/div/div/a";

        private const string _removeAll = "//*[@id='cart-table']/thead/tr/th/a";
        private const string _emptyCart = "//*[@id='primary']/p";

        public CartPage(IWebDriver driver)
            : base(driver)
        { }

        public string GetTitle()
        {
            var productTitle = _driver.FindElement(By.XPath(_productInCart));
            return productTitle.Text;
        }

        public CartPage ClearAll()
        {
            var clearAllButton = _driver.FindElement(By.XPath(_removeAll));
            clearAllButton.Click();
            return this;
        }

        public bool IsCartEmpty()
        {
            var emptyCartLabel = _driver.FindElement(By.XPath(_emptyCart));
            return emptyCartLabel.Text == "Ваша корзина пуста.";
        }
    }
}
