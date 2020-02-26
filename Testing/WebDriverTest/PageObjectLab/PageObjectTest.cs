using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PageObjectLab.PageObjects;

namespace PageObjectLab
{
    public class PageObjectTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }

        [Test]
        public void AddingFirstTwoProductsToComparisonTest()
        {
            var productsPage = new ProductsPage(driver);
            productsPage.GoToPage();
            productsPage.AddTwoProducts();
            var comparisonPage = productsPage.GoToComparisonPage();
            var productsAmount = comparisonPage.CountProudcts();
            Assert.IsTrue(productsAmount == 2);
        }

        [Test]
        public void CheckMaxPriceFilter()
        {
            var productsPage = new ProductsPage(driver);
            productsPage.GoToPage();
            productsPage.ApplyMaxPriceFilter(40);
            var isPriceMax = productsPage.CheckIfPriceIsMax(40);
            Assert.IsTrue(isPriceMax);
        }
    }
}