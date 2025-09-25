using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using SeleniumExtras.WaitHelpers;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly HomePage _homePage;
        private readonly NavigationHelper _navigationHelper;

        public LoginSteps(IWebDriver driver,  LoginPage loginPage, NavigationHelper navigationHelper)
        {  
            _driver = driver;
            _loginPage = loginPage;
            _homePage = new HomePage(_driver);
            _navigationHelper = navigationHelper;
            
        }

        [Given("I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _navigationHelper.NavigateTo("");

            // Click Sign In to open login form
            _homePage.ClickSignIn();
            Assert.That(_homePage.IsLoginFormVisible(), Is.True, "Login form should be visible after clicking Sign In");
        }

        [When("I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            _loginPage.Login("aagyannapaul98@gmail.com", "Aagy@12345");
        }

        [When("I enter an invalid username and valid password")]
        public void WhenIEnterAnInvalidUsernameAndValidPassword()
        {
            _loginPage.Login("aagy@gmail.com", "Aagy@12345");
        }

        [When("I enter a valid username and invalid password")]
        public void WhenIEnterAValidUsernameAndInvalidPassword()
        {
            _loginPage.Login("aagyannapaul98@gmail.com", "wrongpassword");
        }

        [When("I enter empty credentials")]
        public void WhenIEnterEmptyCredentials()
        {
            _loginPage.Login("", "");
        }

      
        [Then(@"I should see the secure area")]
        public void ThenIShouldSeeTheSecureArea()
        {
            Assert.That(_loginPage.IsUserLoggedIn(), Is.True,
                "User should be redirected to /Account/Profile and Sign Out button should be visible after login");
        }

        [Then("I should see an email verification error")]
        public void ThenIShouldSeeAnEmailVerificationError()
        {
            string errorMessage = _loginPage.GetEmailVerificationError();
            Assert.That(errorMessage, Does.Contain("Confirm your email"),
                "Expected email verification error not shown");
        }

        [Then(@"I should see an error message ""(.*)""")]
        public void ThenIShouldSeeAnErrorMessage(string expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Find the error div with exact text match
            var error = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath($"//div[normalize-space(text())='{expectedMessage}']")
            ));

            Assert.That(error.Text, Is.EqualTo(expectedMessage),
                $"Expected error message '{expectedMessage}' but got '{error.Text}'");
        }
    }
}