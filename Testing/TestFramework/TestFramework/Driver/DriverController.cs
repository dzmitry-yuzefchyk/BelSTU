using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TestFramework.Driver
{
    public class DriverController
    {
        private static IWebDriver _driver;

        public static IWebDriver Driver(string browser = "")
        { 
            if (_driver == null)
            {
                switch (browser)
                {
                    case "firefox":
                        new DriverManager().SetUpDriver(new FirefoxConfig());
                        _driver = new FirefoxDriver();
                        break;

                    case "chrome":
                        new DriverManager().SetUpDriver(new ChromeConfig());
                        _driver = new ChromeDriver();
                        break;

                    default:
                        new DriverManager().SetUpDriver(new ChromeConfig());
                        _driver = new ChromeDriver();
                        break;
                }
            }

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return _driver;
        }

        public static void CloseBrowser()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
