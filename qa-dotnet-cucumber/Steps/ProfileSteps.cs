using OpenQA.Selenium;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class ProfilePageSteps
    {
        private readonly IWebDriver _driver;
        private readonly NavigationHelper _navigationHelper;
        private LoginPage _loginPage = null!;
        private ProfilePage _profilePage = null!;

        public ProfilePageSteps(IWebDriver driver, NavigationHelper navigationHelper)
        {
            _driver = driver;
            _navigationHelper = navigationHelper;
        }

        [Given("I am logged in")]
        public void GivenIAmLoggedIn()
        {
            _navigationHelper.NavigateTo("");  

            // Click Sign In on HomePage
            var homePage = new HomePage(_driver);
            homePage.ClickSignIn();

            // Initialize LoginPage
            _loginPage = new LoginPage(_driver);

            // Wait until login form is visible
            Assert.That(_loginPage.IsAtLoginPage(), Is.True, "Login form did not appear after clicking Sign In.");

            // Perform login
            _loginPage.Login("aagyannapaul98@gmail.com", "Aagy@12345");

            // Verify login succeeded
            Assert.That(_loginPage.IsUserLoggedIn(), Is.True, "Login failed or profile not visible.");
        }

        
        [Given("I am in my profile page")]
        public void GivenIAmInMyProfilePage()
        {
            _navigationHelper.NavigateTo("account/profile");
            _profilePage = new ProfilePage(_driver);
        }

        
        [Then("I should see my languages listed")]
        public void ThenIShouldSeeLanguagesListed()
        {
            Assert.That(_profilePage.AreLanguagesDisplayed(),
                        Is.True,
                        "Languages section/table is not displayed.");
        }

       
        [When("I switch to the skills tab")]
        public void WhenISwitchToSkillsTab()
        {
            _profilePage.SwitchToSkillsTab();
        }

        
        [Then("I should see my skills listed")]
        public void ThenIShouldSeeSkillsListed()
        {
            Assert.That(_profilePage.AreSkillsDisplayed(),
                        Is.True,
                        "Skills section/table is not displayed.");
        }
    }
}
