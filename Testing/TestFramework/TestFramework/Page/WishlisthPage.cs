using OpenQA.Selenium;
using TestFramework.Constants;
using TestFramework.Test.Base;
using TestFramework.Test.Models;

namespace TestFramework.Page
{
    public class WishlisthPage : PageBase
    {
        private const string _createButton = "//*[@id='new-wishlist']";
        private const string _titleField = "//*[@id='wishlist-title']";
        private const string _submitButton = "//*[@id='wishlist-create']/button";

        private const string _phoneTitle = "//*[@id='main']/div/div[2]/div/div/div/table/tbody/tr/td[3]/a";

        private const string _deleteButton = "//*[@id='main']/div/div[2]/div/div/div/div/div[1]/div[2]/a";

        private const string _wishlistTitle = "//*[@id='main']/div/div[2]/div/div/div/div/div[1]/h4";

        public WishlisthPage(IWebDriver driver)
            : base(driver)
        {
            Url = PageUrl.Wishlihst;
        }

        public WishlisthPage CreateWishlist(Wishlist wishlist)
        {
            var createButton = _driver.FindElement(By.XPath(_createButton));
            createButton.Click();

            var titleField = _driver.FindElement(By.XPath(_titleField));
            titleField.SendKeys(wishlist.Title);

            var submitButton = _driver.FindElement(By.XPath(_submitButton));
            submitButton.Click();

            return this;
        }

        public WishlisthPage RemoveWishlist()
        {
            var deleteButton = _driver.FindElement(By.XPath(_deleteButton));
            deleteButton.Click();

            return this;
        }

        public string GetWishlistTitle()
        {
            var wishlistTitle = _driver.FindElement(By.XPath(_wishlistTitle));
            return wishlistTitle.Text;
        }

        public int CountWishlists()
        {
            var wishlists = _driver.FindElements(By.XPath(_wishlistTitle));
            return wishlists.Count;
        }

        public string GetWishedPhone()
        {
            var phoneField = _driver.FindElement(By.XPath(_phoneTitle));
            var phoneTitle = phoneField.Text;
            return phoneTitle;
        }
    }
}
