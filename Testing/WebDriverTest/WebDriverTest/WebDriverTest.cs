using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebDriverTest
{
    class WebDriverTest
    {
        private IWebDriver _driver;
        private string _productName;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _productName = "Смартфон Xiaomi Mi A1 4GB/64GB (черный)";
        }

        [Test]
        public void CheckSearchField()
        {
            string searchFieldValue = MainPage.GetSearchFieldValue(_productName, _driver);
            bool expected = searchFieldValue == _productName;
            Assert.IsTrue(expected);
        }

        [Test]
        public void CheckAddingToCart()
        {
            string productTitle = CartPage.CheckAddingToCart(_productName, _driver);
            bool expected = productTitle == _productName;
            Assert.IsTrue(expected);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Close();
        }
    }
}
