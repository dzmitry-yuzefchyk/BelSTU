using OpenQA.Selenium;
using System.Threading;
using TestFramework.Test.Base;

namespace TestFramework.Page.Base
{
    public class SearchBarPageBase<T> : PageBase
        where T : SearchBarPageBase<T>
    {
        public SearchBarPageBase(IWebDriver driver)
            : base(driver)
        {
            _driver = driver;
        }

        private const string _searchBar = "//*[@id='search-header']";

        public T SearchProduct(string productTitle)
        {
            var searchBar = _driver.FindElement(By.XPath(_searchBar));
            searchBar.Clear();
            searchBar.SendKeys(productTitle);
            searchBar.Submit();
            Thread.Sleep(500);

            return (T)this;
        }
    }
}
