using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace qa_dotnet_cucumber.Pages
{
    public class ProfilePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public ProfilePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Locators
        private By LanguageTab => By.XPath("//h3[text()='Languages']/ancestor::div[contains(@class,'column')]");
        private IReadOnlyCollection<IWebElement> LanguageRows =>
            _driver.FindElements(By.XPath("//table[@class='ui fixed table']//tr"));
        private By SkillsTab => By.CssSelector("a[data-tab='second']");
        private IReadOnlyCollection<IWebElement> SkillRows =>
           _driver.FindElements(By.XPath("//th[text()='Skill']/ancestor::table//tbody/tr"));

        public bool AreLanguagesDisplayed()
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementIsVisible(LanguageTab));
                return LanguageRows.Count > 0; // At least one language listed
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void SwitchToSkillsTab()
        {
            // Wait for the tab to be clickable, then click
            var tab = _wait.Until(ExpectedConditions.ElementToBeClickable(SkillsTab));
            tab.Click();

            // Wait for the Skills section to be visible after clicking
            _wait.Until(ExpectedConditions.ElementIsVisible(SkillsTab));
        }

        public bool AreSkillsDisplayed()
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementIsVisible(SkillsTab));
                return SkillRows.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
