using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TestFramework.Driver;
using TestFramework.Environment;
using TestFramework.Page;
using TestFramework.Test.Base;
using TestFramework.Test.Services;

namespace TestFramework.Test.Tests
{
    [TestFixture]
    public class WishlistTests : TestBase
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
        public void AddToWishlistTest()
        {
            Test(() =>
            {
                var user = UserCreator.CreateUser(_configuration);
                var wishlist = WishlistCreator.CreateWishlist(_configuration);
                var homePage = new HomePage(_driver);
                var phoneTitle = homePage
                    .GoTo()
                    .GoToLoginPage()
                    .WriteCredentials(user)
                    .GoToProfilePage()
                    .GoToWishlistPage()
                    .CreateWishlist(wishlist)
                    .GoToHomePage()
                    .GoToPhonesPage()
                    .SearchProduct(wishlist.Phone)
                    .OpenPhone()
                    .AddToWishlist()
                    .GoToHomePage()
                    .GoTo()
                    .GoToProfilePage()
                    .GoToWishlistPage()
                    .GetWishedPhone();

                var wishlistPage = new WishlisthPage(_driver);
                wishlistPage.RemoveWishlist();

                Assert.IsTrue(phoneTitle.Contains(wishlist.Phone));
            });
        }

        [Test]
        public void CreateWishlistTest()
        {
            Test(() =>
            {
                var user = UserCreator.CreateUser(_configuration);
                var wishlist = WishlistCreator.CreateWishlist(_configuration);
                var homePage = new HomePage(_driver);
                var wishlistTitle = homePage
                    .GoTo()
                    .GoToLoginPage()
                    .WriteCredentials(user)
                    .GoToProfilePage()
                    .GoToWishlistPage()
                    .CreateWishlist(wishlist)
                    .GetWishlistTitle();

                var wishlistPage = new WishlisthPage(_driver);
                wishlistPage.RemoveWishlist();

                Assert.IsTrue(wishlistTitle.Contains(wishlist.Title));
            });
        }

        [Test]
        public void RemoveWishlistTest()
        {
            Test(() =>
            {
                var user = UserCreator.CreateUser(_configuration);
                var wishlist = WishlistCreator.CreateWishlist(_configuration);
                var homePage = new HomePage(_driver);
                var wishlistsCount = homePage
                    .GoTo()
                    .GoToLoginPage()
                    .WriteCredentials(user)
                    .GoToProfilePage()
                    .GoToWishlistPage()
                    .CreateWishlist(wishlist)
                    .RemoveWishlist()
                    .CountWishlists();

                Assert.Zero(wishlistsCount);
            });
        }
    }
}
