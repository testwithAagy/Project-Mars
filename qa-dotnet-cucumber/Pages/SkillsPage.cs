using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace qa_dotnet_cucumber.Pages
{
    public class SkillsPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;


        public SkillsPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        private By AddNewButton => By.XPath("//div[contains(@class,'ui teal button') and text()='Add New']");
        private By SkillsTab => By.XPath("//a[text()='Skills']");
        private By SkillInput => By.XPath("//input[@placeholder='Add Skill']");
        private By LevelDropdown => By.Name("level");
        private By AddButton => By.XPath("//input[@value='Add']");
        private By ToastMessage => By.XPath("//div[contains(@class,'ns-box-inner')]");


        // Add a skill with a given level

        public void AddSkill(string skill, string level)
        {
            
            var addNewButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//div[@data-tab='second']//div[contains(@class,'ui teal button') and text()='Add New']")));
            addNewButton.Click();

            // Wait for skill input to appear
            var skillField = _wait.Until(ExpectedConditions.ElementIsVisible(SkillInput));
            skillField.Clear();
            skillField.SendKeys(skill);

            // Select level
            var dropdown = _wait.Until(ExpectedConditions.ElementIsVisible(LevelDropdown));
            var select = new SelectElement(dropdown);
            select.SelectByText(level);

            // Click Add button
            var addBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(AddButton));
            addBtn.Click();

           
            _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
        }

        public void AddSkillWithInvalidInput(string skill, string level)
        {
          

            // Click on "Add New" button 
            var addNewButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//div[@data-tab='second']//div[contains(@class,'ui teal button') and text()='Add New']")));
            addNewButton.Click();

            try
            {
                // Fill skill only if provided
                if (!string.IsNullOrWhiteSpace(skill))
                {
                    var skillField = _wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//input[@placeholder='Add Skill']")));
                    skillField.Clear();
                    skillField.SendKeys(skill);
                }

                // Select level only if provided
                if (!string.IsNullOrWhiteSpace(level))
                {
                    var dropdown = _wait.Until(ExpectedConditions.ElementIsVisible(By.Name("level")));
                    var select = new SelectElement(dropdown);
                    select.SelectByText(level);
                }

                // Try clicking Add button
                var addBtn = _driver.FindElement(By.XPath("//input[@value='Add']"));
                if (addBtn.Enabled && addBtn.Displayed)
                {
                    addBtn.Click();
                }
                else
                {
                    Console.WriteLine("Add button not interactable for invalid input — expected behavior.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Expected missing element for invalid skill input.");
            }
            catch (ElementNotInteractableException)
            {
                Console.WriteLine("Element not interactable — this is expected for invalid input.");
            }
        }


        public void UpdateSkill(string oldSkill, string newSkill, string newLevel)
        {
            try
            {
                // Locate the row by old skill
                var row = _driver.FindElement(By.XPath($"//td[text()='{oldSkill}']/.."));

                // Click edit icon
                row.FindElement(By.XPath(".//i[contains(@class,'write icon')]")).Click();

                // Update skill name
                var skillInput = _wait.Until(d => d.FindElement(By.Name("name")));
                skillInput.Clear();
                skillInput.SendKeys(newSkill);

                // Update level
                var dropdown = new SelectElement(_driver.FindElement(By.Name("level")));
                dropdown.SelectByText(newLevel);

                // Click Update
                _driver.FindElement(By.XPath("//input[@value='Update']")).Click();

              
                _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Skill '{oldSkill}' does not exist, so update was skipped.");
            }
        }

        public void DeleteSkill(string skill)
        {
            // Ensure we are on the Skills tab
            _driver.FindElement(By.CssSelector("a[data-tab='second']")).Click();

            try
            {
                // Locate the row by skill name
                var row = _driver.FindElement(By.XPath($"//td[text()='{skill}']/.."));

                // Click the delete (remove) icon
                row.FindElement(By.XPath(".//i[contains(@class,'remove icon')]")).Click();

                // Wait for the toast message to appear
                _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Skill '{skill}' does not exist, so delete was skipped.");
            }
        }

      
        // Get the toast message 

        public string GetToastMessage()
        {
            try
            {
                var message = _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
                return message.Text.Trim();
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}
