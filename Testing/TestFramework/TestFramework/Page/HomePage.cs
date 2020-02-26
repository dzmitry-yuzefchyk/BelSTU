using OpenQA.Selenium;
using System.Threading;
using TestFramework.Constants;
using TestFramework.Page.Base;

namespace TestFramework.Page
{
    public class HomePage : SearchBarPageBase<HomePage>
    {
        private const string _loginButton = "//*[@id='page']/div[1]/div/ul[2]/li[3]/a";
        private const string _profileButton = "//*[@id='page']/div[1]/div/ul[2]/li[3]/a";

        public HomePage(IWebDriver driver)
            : base(driver)
        {
            Url = PageUrl.Home;
        }

        public HomePage GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
            return this;
        }

        public LoginPage GoToLoginPage()
        {
            var loginButton = _driver.FindElement(By.XPath(_loginButton));
            loginButton.Click();
            Thread.Sleep(500);
            return new LoginPage(_driver);
        }

        public ProfilePage GoToProfilePage()
        {
            Thread.Sleep(1000);
            var profileButton = _driver.FindElement(By.XPath(_profileButton));
            Thread.Sleep(500);
            profileButton.Click();
            Thread.Sleep(1000);
            return new ProfilePage(_driver);
        }

        public PhonesPage GoToPhonesPage()
        {
            _driver.Navigate().GoToUrl(PageUrl.Phones);
            Thread.Sleep(1000);
            return new PhonesPage(_driver);
        }
    }
}
