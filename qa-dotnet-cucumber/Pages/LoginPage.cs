using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace qa_dotnet_cucumber.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private By EmailField => By.Name("email");       
        private By PasswordField => By.Name("password"); 
        private By LoginButton => By.XPath("//button[text()='Login']");   
        private By ErrorMessage => By.CssSelector(".error-message"); 
        private By SignOutButton => By.XPath("//button[normalize-space()='Sign Out']"); 
        
        public void EnterEmail(string email)
        {
            var emailElement = _wait.Until(ExpectedConditions.ElementIsVisible(EmailField));
            emailElement.Clear();
            emailElement.SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            var passwordElement = _wait.Until(ExpectedConditions.ElementIsVisible(PasswordField));
            passwordElement.Clear();
            passwordElement.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            var loginBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(LoginButton));
            loginBtn.Click();
        }

        public bool IsAtLoginPage()
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(LoginButton)).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsUserLoggedIn()
        {
            try
            {
                // SignOutButton appears only when logged in
                return _wait.Until(ExpectedConditions.ElementIsVisible(SignOutButton)).Displayed;
            }
            catch
            {
                return false;
            }
        }
        public string GetInlineErrorMessage()
        {
            try
            {
              
                var errorElement = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(d => d.FindElement(By.CssSelector(".ui.basic.red.pointing.prompt.label.transition.visible")));

                return errorElement.Text.Trim();
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty; 
            }
        }

     
    }
}

