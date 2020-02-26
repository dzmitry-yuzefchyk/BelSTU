using OpenQA.Selenium;
using System.Threading;
using TestFramework.Constants;
using TestFramework.Test.Base;

namespace TestFramework.Page
{
    public class ProfilePage : PageBase
    {
        private const string _nameField = "//*[@id='user-first_name']";

        private const string _wishlistButton = "//*[@id='main']/div/div/div/div/div/div/div[2]/a[3]";

        private const string _submitButton = "//*[@id='profile-form']/div[2]/div/div/button";

        public ProfilePage(IWebDriver driver)
            : base(driver)
        {
            Url = PageUrl.Profile;
        }

        public string GetLoggedUser()
        {
            var nameField = _driver.FindElement(By.XPath(_nameField));
            var name = nameField.GetAttribute("value");
            return name;
        }

        public WishlisthPage GoToWishlistPage()
        {
            var wishlistButton = _driver.FindElement(By.XPath(_wishlistButton));
            wishlistButton.Click();
            Thread.Sleep(1000);
            return new WishlisthPage(_driver);
        }

        public ProfilePage ChangeName(string name)
        {
            var nameField = _driver.FindElement(By.XPath(_nameField));
            nameField.Clear();
            nameField.SendKeys(name);

            var submitButton = _driver.FindElement(By.XPath(_submitButton));
            submitButton.Click();
            Thread.Sleep(1000);

            return this;
        }
    }
}
