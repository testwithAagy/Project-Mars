using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using SeleniumExtras.WaitHelpers;
using System;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver _driver;
        private readonly HomePage _homePage;
        private readonly LoginPage _loginPage;
        private readonly NavigationHelper _navigationHelper;

        public LoginSteps(IWebDriver driver, NavigationHelper navigationHelper)
        {
            _driver = driver;
            _navigationHelper = navigationHelper;
            _homePage = new HomePage(driver);
            _loginPage = new LoginPage(driver);
        }

       
        [Given(@"I am on the landing page")]
        public void GivenIAmOnTheLandingPageForLogin()
        {
            _navigationHelper.NavigateTo("");
            
        }

     
        [When(@"I click the ""(.*)"" button for login")]
        public void WhenIClickTheButton(string buttonName)
        {
            if (buttonName.Equals("Sign In", StringComparison.OrdinalIgnoreCase))
            {
                _homePage.ClickSignIn();
                Assert.That(_loginPage.IsAtLoginPage(), Is.True, "Login page did not load after clicking Sign In.");
            }
            else if (buttonName.Equals("Login", StringComparison.OrdinalIgnoreCase))
            {
                _loginPage.ClickLoginButton();
            }
            else
            {
                throw new ArgumentException($"Unknown button name: {buttonName}");
            }
        }

        [When(@"I enter email ""(.*)"" and password ""(.*)""")]
        public void WhenIEnterEmailAndPassword(string email, string password)
        {
            _loginPage.EnterEmail(email.Trim());
            _loginPage.EnterPassword(password.Trim());
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            _loginPage.ClickLoginButton();
        }

        //Multiple failed login attempts
        
        [When(@"I attempt to log in with email ""(.*)"" and invalid password ""(.*)"" three times")]
        public void WhenIAttemptToLogInMultipleTimes(string email, string password)
        {
            for (int i = 0; i < 3; i++)
            {
                _loginPage.EnterEmail(email.Trim());
                _loginPage.EnterPassword(password.Trim());
                _loginPage.ClickLoginButton();

                // Wait briefly between attempts to simulate user behavior
                System.Threading.Thread.Sleep(1000);
            }
        }


        
        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            if (expectedMessage.Equals("Secure area displayed", StringComparison.OrdinalIgnoreCase))
            {
                Assert.That(_loginPage.IsUserLoggedIn(), Is.True,
                    "Expected secure area, but user is not logged in.");
            }
           
            else if (expectedMessage.Equals("Please enter a valid email address", StringComparison.OrdinalIgnoreCase) ||
              expectedMessage.Equals("Incorrect password", StringComparison.OrdinalIgnoreCase) ||
              expectedMessage.Equals("This email has already been used to register an account", StringComparison.OrdinalIgnoreCase) ||
              expectedMessage.Equals("Password must be at least 6 characters", StringComparison.OrdinalIgnoreCase))
            {
                string actualError = _loginPage.GetInlineErrorMessage();
                Assert.That(actualError, Does.Contain(expectedMessage).IgnoreCase,
                    $"Expected error message '{expectedMessage}' but got '{actualError}'");
            }

            else if (expectedMessage.Equals("Send Verification Email", StringComparison.OrdinalIgnoreCase) ||
                expectedMessage.Equals("User does not exist", StringComparison.OrdinalIgnoreCase))
            {
                var popupButtonLocator = By.Id("submit-btn");
                var isDisplayed = wait.Until(driver => driver.FindElement(popupButtonLocator).Displayed);
                Assert.That(isDisplayed, Is.True,
                    $"Expected popup/button '{expectedMessage}' is not displayed.");
            }

            else if (expectedMessage.Equals("Your account has been temporarily locked due to multiple failed attempts", StringComparison.OrdinalIgnoreCase))
            {
                string actualError = _loginPage.GetInlineErrorMessage();
                Assert.That(actualError, Does.Contain("locked").IgnoreCase,
                    $"Expected lockout message but got '{actualError}'");
            }

            else
            {
                // Fallback for unexpected messages
                Assert.Fail($"Test does not handle expected message: '{expectedMessage}'");
            }
        }
    }
}
