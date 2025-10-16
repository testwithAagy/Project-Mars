using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace qa_dotnet_cucumber.Helpers
{
    public static class LanguagesAndSkillsHelper
    {
        // Deletes all test languages added before each scenario

 
        public static void CleanupAllTestLanguages(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                while (true)
                {
                    // Wait for the table inside Languages tab
                    var table = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//div[@data-tab='first']//table")));

                    // Find the first row (if any)
                    var row = table.FindElements(By.XPath(".//tbody//tr")).FirstOrDefault();
                    if (row == null)
                        break; // No more rows

                    // Find delete icon in the row
                    var deleteIcon = row.FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));
                    deleteIcon.Click();

                    // Wait until the row is removed
                    wait.Until(ExpectedConditions.StalenessOf(row));
                }

                Console.WriteLine("Global cleanup complete: all languages deleted.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No languages found for global cleanup.");
            }
        }

        // Deletes only the language added by the scenario

        public static void CleanupScenarioLanguage(IWebDriver driver, string language)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                // Wait for the table inside the Languages tab
                var table = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[@data-tab='first']//table")));

                // Find all rows relative to the table
                var rows = table.FindElements(By.XPath(".//tbody//tr"));

                // Find the row that contains the language 
                var targetRow = rows.FirstOrDefault(row =>
                    row.Text.IndexOf(language, StringComparison.OrdinalIgnoreCase) >= 0);

                if (targetRow != null)
                {
                    // Find the delete icon relative to the row
                    var deleteIcon = targetRow.FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));
                    deleteIcon.Click();

                    // Wait until the target row disappears after deletion
                    wait.Until(ExpectedConditions.StalenessOf(targetRow));

                    Console.WriteLine($"Scenario cleanup: '{language}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"No matching row found for '{language}', skipping deletion.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Timeout while cleaning up language '{language}'.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Delete icon not found for '{language}'.");
            }
        }



        // Delete all test skills added before each scenario
        public static void CleanupAllTestSkills(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                while (true)
                {
                    // Wait for the table inside Skills tab
                    var table = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//div[@data-tab='second']//table")));

                    // Find the first row (if any)
                    var row = table.FindElements(By.XPath(".//tbody//tr")).FirstOrDefault();
                    if (row == null)
                        break; // No more rows

                    // Find delete icon in the row
                    var deleteIcon = row.FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));
                    deleteIcon.Click();

                    // Wait until the row is removed
                    wait.Until(ExpectedConditions.StalenessOf(row));
                }

                Console.WriteLine("Global cleanup complete: all skills deleted.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No skills found for global cleanup.");
            }
        }


        //  Delete only a specific skill added by the scenario
        public static void CleanupScenarioSkill(IWebDriver driver, string skill)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                //  Skills tab
                var tab = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@data-tab='second' and text()='Skills']")));
                tab.Click();

                var table = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[@data-tab='second']//table")));

                var rows = table.FindElements(By.XPath(".//tbody//tr"));

                var targetRow = rows.FirstOrDefault(row =>
                    row.Text.IndexOf(skill, StringComparison.OrdinalIgnoreCase) >= 0);

                if (targetRow != null)
                {
                    var deleteIcon = targetRow.FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));
                    deleteIcon.Click();
                    wait.Until(ExpectedConditions.StalenessOf(targetRow));

                    Console.WriteLine($"Scenario cleanup: '{skill}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"No matching row found for '{skill}', skipping deletion.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Timeout while cleaning up skill '{skill}'.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Delete icon not found for '{skill}'.");
            }
        }

    }
}
