using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WebDriverTest
{
    class WebDriverTest
    {
        private string _homeURL;
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _homeURL = "https://www.ttn.by/";
            _driver = new ChromeDriver();
        }

        [Test]
        public void CheckSearchField()
        {
            string product = "Смартфон Xiaomi Mi A1 4GB/64GB (черный)";
            string searchTagId = "woocommerce-product-search-field-0";

            _driver.Navigate().GoToUrl(_homeURL);
            IWebElement searchElement = _driver.FindElement(By.Id(searchTagId));
            SlowType(searchElement, product);

            Thread.Sleep(3000);
            string phoneTitle = _driver.FindElement(By.XPath("//*[@class='search-popup-ad']/div[2]/div[1]/a/div[1]")).Text;
            Assert.IsTrue(phoneTitle == product);
        }

        [Test]
        public void CheckAddingToCart()
        {
            string product = "Смартфон Xiaomi Mi A1 4GB/64GB (черный)";
            string searchTagId = "woocommerce-product-search-field-0";

            _driver.Navigate().GoToUrl(_homeURL);
            IWebElement searchElement = _driver.FindElement(By.Id(searchTagId));
            SlowType(searchElement, product);
            
            Thread.Sleep(3000);
            _driver.FindElement(By.XPath("//*[@class='search-popup-ad']/div[2]/div[1]/a")).Click();
            Thread.Sleep(3000);
            
            _driver.FindElement(By.XPath("//*[@class='single_add_to_cart_button button alt wc-variation-selection-needed to-cart-for-ajax']")).Click();
            Thread.Sleep(3000);
            
            _driver.Navigate().GoToUrl("https://www.ttn.by/shop/cart?issubmit=0");
            string phoneTitle = _driver.FindElement(By.XPath("//*[@id='cart-table']/tbody/tr[1]/td[2]/div/div/a")).Text;
            Assert.IsTrue(phoneTitle == product);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Close();
        }

        private void SlowType(IWebElement webElement, string text)
        {
            webElement.Click();
            webElement.Clear();
            foreach (var key in text)
            {
                webElement.SendKeys(key.ToString());
                Thread.Sleep(150);
            }
        }
    }
}
