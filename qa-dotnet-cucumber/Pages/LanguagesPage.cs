using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace qa_dotnet_cucumber.Pages
{
    public class LanguagesPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LanguagesPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

     
        private By AddNewButton => By.XPath("//div[contains(@class,'ui teal button') and text()='Add New']");
        private By LanguageInput => By.Name("name");
        private By LevelDropdown => By.Name("level");
        private By SaveButton => By.XPath("//input[@value='Add']");
        private By ToastMessage => By.XPath("//div[contains(@class,'ns-box-inner')]");

        private string lastToastText = string.Empty;



        // Add a language
        public void AddLanguage(string language, string level)
        {
          
            _wait.Until(d => d.FindElement(AddNewButton)).Click();
          
            var langInput = _wait.Until(d => d.FindElement(LanguageInput));
            langInput.Clear();
            langInput.SendKeys(language);
        
            var dropdown = new SelectElement(_driver.FindElement(LevelDropdown));
            dropdown.SelectByText(level);

            _driver.FindElement(SaveButton).Click();
       
            _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
           
        }

        public void AddLanguageWithInvalidInput(string language, string level)
        {
            _wait.Until(d => d.FindElement(AddNewButton)).Click();

            var langInput = _wait.Until(d => d.FindElement(LanguageInput));
            langInput.Clear();
            langInput.SendKeys(language);

            if (!string.IsNullOrEmpty(level))
            {
                var dropdown = new SelectElement(_driver.FindElement(LevelDropdown));
                dropdown.SelectByText(level);
            }
           
            _driver.FindElement(SaveButton).Click();
        }

        // Update language 
        public void UpdateLanguage(string oldLanguage, string newLanguage, string newLevel)
        {
            try
            {
                // Locate the row by old language
                var row = _driver.FindElement(By.XPath($"//td[text()='{oldLanguage}']/.."));

          
            row.FindElement(By.XPath(".//i[contains(@class,'write icon')]")).Click();

            var langInput = _wait.Until(d => d.FindElement(By.Name("name")));
            langInput.Clear();
            langInput.SendKeys(newLanguage);

            var dropdown = new SelectElement(_driver.FindElement(By.Name("level")));
            dropdown.SelectByText(newLevel);

            _driver.FindElement(By.XPath("//input[@value='Update']")).Click();

            _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
            }
            catch (NoSuchElementException)
            {
                
                Console.WriteLine($"Language '{oldLanguage}' does not exist, so update was skipped.");
            }

        }


        public void DeleteLanguage(string language)
        {
            // Locate the row by language
            var row = _driver.FindElement(By.XPath($"//td[text()='{language}']/.."));
           
            row.FindElement(By.XPath(".//i[contains(@class,'remove icon')]")).Click();
           
            _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));
           
        }

        public string GetToastMessage()
           {
               
               IWebElement toast = _wait.Until(ExpectedConditions.ElementIsVisible(ToastMessage));

               string newToastText = toast.Text.Trim();
               int retryCount = 0;

               while (newToastText == lastToastText && retryCount < 5)
               {
                   Thread.Sleep(500);
                   newToastText = toast.Text.Trim();
                   retryCount++;
               }

               lastToastText = newToastText;
               return newToastText;
           }
    }
}
