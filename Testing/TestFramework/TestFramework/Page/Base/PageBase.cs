using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TestFramework.Page;

namespace TestFramework.Test.Base
{
    public class PageBase
    {
        private const int _incrementScrollHeight = 100;
        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (string.IsNullOrEmpty(_url))
                {
                    _url = value;
                }
            }
        }
        protected IWebDriver _driver;

        public PageBase(IWebDriver driver)
        {
            _driver = driver;
        }

        public PageBase Go()
        {
            _driver.Navigate().GoToUrl(_url);
            return this;
        }

        public HomePage GoToHomePage()
        {
            return new HomePage(_driver);
        }

        public virtual void ScrollToElement(IWebElement element, int startScroll = 100)
        {
            try
            {
                Actions actions = new Actions(_driver);
                actions.MoveToElement(element);
                actions.Perform();
            }
            catch
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript($"scroll(0,{startScroll});");
                this.ScrollToElement(element, startScroll + _incrementScrollHeight);
            }
        }
    }
}
