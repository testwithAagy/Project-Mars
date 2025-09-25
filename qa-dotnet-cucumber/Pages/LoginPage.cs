using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace qa_dotnet_cucumber.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        public IWebDriver Driver => _driver;  

        // Locators
        private readonly By UsernameField = By.Name("email");
        private readonly By PasswordField = By.Name("password");
        private readonly By LoginButton = By.XPath("//button[text()='Login']");
        private By SignOutButton => By.XPath("//button[normalize-space()='Sign Out']");
        private readonly By EmailVerificationError = By.XPath("//div[@class='ns-box-inner' and normalize-space()='Confirm your email']");

        public LoginPage(IWebDriver driver) 
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); 
        }
        
        public void Login(string username, string password)
        {
            var usernameElement = _wait.Until(ExpectedConditions.ElementIsVisible(UsernameField));
            usernameElement.SendKeys(username);

            var passwordElement = _wait.Until(d => d.FindElement(PasswordField));
            passwordElement.SendKeys(password);

            var loginButtonElement = _wait.Until(ExpectedConditions.ElementToBeClickable(LoginButton));
            loginButtonElement.Click();
        }


        // Verification: checks redirect + sign out button visibility
        public bool IsUserLoggedIn()
        {
            try
            {
                // Wait for Sign Out button to appear
                var signOut = _wait.Until(ExpectedConditions.ElementIsVisible(SignOutButton));
                return signOut.Displayed && _driver.Url.Contains("/Account/Profile");
            }
            catch
            {
                return false;
            }
        }
        public string GetEmailVerificationError()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(EmailVerificationError)).Text;
        }

        
        public bool IsAtLoginPage()
        {
            try
            {
                // Wait until the login form is visible
                _wait.Until(ExpectedConditions.ElementIsVisible(UsernameField));
                _wait.Until(ExpectedConditions.ElementIsVisible(PasswordField));
                _wait.Until(ExpectedConditions.ElementIsVisible(LoginButton));

                return true; // Login form is visible
            }
            catch (WebDriverTimeoutException)
            {
                return false; // Login form did not appear in time
            }
        }

    }
}