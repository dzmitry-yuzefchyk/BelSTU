using OpenQA.Selenium;
using System.Threading;
using TestFramework.Constants;
using TestFramework.Test.Base;
using TestFramework.Test.Models;

namespace TestFramework.Page
{
    public class LoginPage : PageBase
    {
        private const string _emailField = "//*[@id='loginform-email']";
        private const string _passwordField = "//*[@id='loginform-password']";
        private const string _submitButton = "//*[@id='enter-item']/p[2]/input";

        public LoginPage(IWebDriver driver)
            : base(driver)
        {
            Url = PageUrl.Login;
        }

        public HomePage WriteCredentials(User user) 
        {
            var emailField = _driver.FindElement(By.XPath(_emailField));
            ScrollToElement(emailField);
            emailField.SendKeys(user.Email);

            var passwordField = _driver.FindElement(By.XPath(_passwordField));
            ScrollToElement(passwordField);
            passwordField.SendKeys(user.Password);

            var submitButton = _driver.FindElement(By.XPath(_submitButton));
            ScrollToElement(submitButton);
            submitButton.Click();

            Thread.Sleep(1000);
            return new HomePage(_driver);
        }
    }
}
