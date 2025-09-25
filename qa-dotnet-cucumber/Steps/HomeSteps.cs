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
    public class LandingPageSteps
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly HomePage _homePage;
        private readonly NavigationHelper _navigationHelper;


        public LandingPageSteps(IWebDriver driver, NavigationHelper navigationHelper)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _homePage = new HomePage(_driver);
            _navigationHelper = navigationHelper;


        }


        //hitting the URL on the browser and landing on the home page/Landing page
        
        [Given(@"I am on the Mars application landing page")]
        public void GivenIAmOnTheMarsApplicationLandingPage()
        {
            
            _navigationHelper.NavigateTo("");


        }


        // Check that Sign In or Join button is visible

        [Then(@"I should see the ""(.*)"" button")]
        public void ThenIShouldSeeTheButton(string buttonName)
        {
            switch (buttonName)
            {
                case "Sign In":
                    Assert.That(_homePage.IsSignInVisible(), Is.True, "Sign In button not visible on home page");
                    break;
                case "Join":
                    Assert.That(_homePage.IsJoinVisible(), Is.True, "Join button not visible on home page");
                    break;
                default:
                    throw new ArgumentException($"Button {buttonName} not found");
            }
        }



        // Click buttons

        [When(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string buttonName)
        {
            switch (buttonName)
            {
                case "Sign In":
                    _homePage.ClickSignIn();
                    break;
                case "Join":
                    _homePage.ClickJoin();
                    break;
                default:
                    throw new ArgumentException($"Button {buttonName} not found");
            }
        }



        // Verify login form

        [Then(@"the login form should be displayed")]
        public void ThenTheLoginFormShouldBeDisplayed()
        {
            Assert.That(_homePage.IsLoginFormVisible(), Is.True, "Login form is not visible after clicking Sign In");
        }


        // Verify join form

        [Then(@"the join form should be displayed")]
        public void ThenTheJoinFormShouldBeDisplayed()
        {
            Assert.That(_homePage.IsJoinFormVisible(), Is.True, "Join form is not visible after clicking Join");
        }
    }
}
