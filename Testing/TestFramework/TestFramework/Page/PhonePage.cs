using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using TestFramework.Constants;
using TestFramework.Test.Base;

namespace TestFramework.Page
{
    public class PhonePage : PageBase
    {
        private const string _addToWishButton = "//*[@id='main']/div/div[1]/div[2]/div[1]/h1";
        private const string _saveToWishButton = "//*[@id='add-to-wishlist']";

        private const string _addToCartButton = "//*[@id='main']/div/div[1]/div[3]/div[1]/form/div/div/button";

        private const string _goToCartButton = "//*[@id='site-header-cart']/li/a";

        public PhonePage(IWebDriver driver)
            : base(driver)
        {
            Url = PageUrl.Phones;
        }

        public PhonePage AddToWishlist()
        {
            Thread.Sleep(1000);
            Actions build = new Actions(_driver);
            build.MoveToElement(_driver.FindElement(By.XPath(_addToWishButton)))
                .MoveByOffset(350, 0)
                .Click()
                .Build()
                .Perform();
            //var addToWishButton = _driver.FindElement(By.XPath(_addToWishButton));
            //addToWishButton.Submit();

            var saveToWishButton = _driver.FindElement(By.XPath(_saveToWishButton));
            saveToWishButton.Click();

            return this;
        }

        public PhonePage AddToCart()
        {
            var addToCartButton = _driver.FindElement(By.XPath(_addToCartButton));
            addToCartButton.Click();
            Thread.Sleep(3000);

            return this;
        }

        public CartPage GoToCarPage()
        {
            var goToCatrButton = _driver.FindElement(By.XPath(_goToCartButton));
            goToCatrButton.Click();

            return new CartPage(_driver);
        }
    }
}
