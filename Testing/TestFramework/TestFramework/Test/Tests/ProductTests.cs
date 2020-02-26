using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using TestFramework.Driver;
using TestFramework.Environment;
using TestFramework.Page;
using TestFramework.Test.Base;
using TestFramework.Test.Services;

namespace TestFramework.Test.Tests
{
    [TestFixture]
    public class ProductTests : TestBase
    {
        private IWebDriver _driver;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = Settings.GetConfiguration();
            _driver = DriverController.Driver(Settings.Browser);
            _driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void CloseBrowser()
        {
            DriverController.CloseBrowser();
        }

        [Test]
        public void SearchTest()
        {
            Test(() =>
            {
                var product = ProductCreator.CreateProduct(_configuration);
                var homePage = new HomePage(_driver);
                var products = homePage
                    .GoTo()
                    .GoToPhonesPage()
                    .SearchProduct(product.Search)
                    .GetTitles();

                Assert.IsTrue(products.Any(x => x.Contains(product.Search)));
            });
        }

        [Test]
        public void PriceFilterTest()
        {
            Test(() =>
            {
                var product = ProductCreator.CreateProduct(_configuration);
                var homePage = new HomePage(_driver);
                var prices = homePage
                    .GoTo()
                    .GoToPhonesPage()
                    .ApplyMaxPriceFilter(product.MaxPrice)
                    .GetPrices();

                Assert.IsTrue(!prices.Any(x => x > product.MaxPrice));
            });
        }

        [Test]
        public void ComparisonTest()
        {
            Test(() =>
            {
                var product = ProductCreator.CreateProduct(_configuration);
                var homePage = new HomePage(_driver);
                var products = homePage
                    .GoTo()
                    .GoToPhonesPage()
                    .SearchProduct(product.Compare[0])
                    .AddFirstProduct()
                    .SearchProduct(product.Compare[1])
                    .AddFirstProduct()
                    .GoToComparisonPage()
                    .GetProductNames();

                Assert.IsTrue(!products.Any(x => product.Compare.Any(u => u.ToLower().Contains(x.ToLower()))));
            });
        }

        [Test]
        public void AddToCartTest()
        {
            Test(() =>
            {
                var product = ProductCreator.CreateProduct(_configuration);
                var homePage = new HomePage(_driver);
                var productTitle = homePage
                    .GoTo()
                    .GoToPhonesPage()
                    .SearchProduct(product.AddToCart)
                    .OpenPhone()
                    .AddToCart()
                    .GoToCarPage()
                    .GetTitle();

                Assert.IsTrue(productTitle.ToLower().Contains(product.AddToCart.ToLower()));
            });
        }

        [Test]
        public void ClearCartTest()
        {
            Test(() =>
            {
                var product = ProductCreator.CreateProduct(_configuration);
                var homePage = new HomePage(_driver);
                var isCartEmpty = homePage
                    .GoTo()
                    .GoToPhonesPage()
                    .SearchProduct(product.AddToCart)
                    .OpenPhone()
                    .AddToCart()
                    .GoToCarPage()
                    .ClearAll()
                    .IsCartEmpty();

                Assert.IsTrue(isCartEmpty);
            });
        }
    }
}
