using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace qa_dotnet_cucumber.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private readonly By SignInButton = By.XPath("//a[text()='Sign In']");
        private readonly By JoinButton = By.XPath("//button[contains(text(),'Join')]");
        private readonly By LoginForm = By.Name("email"); 
        private readonly By JoinForm = By.Name("firstName");
        
        
      
        public void ClickSignIn()
        {
           
            var element = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                      .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SignInButton));
            element.Click();
        }

        public bool IsSignInVisible()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(SignInButton)).Displayed;
        }

        public bool IsLoginFormVisible()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(LoginForm)).Displayed;
        }

        public void ClickJoin()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(JoinButton)).Click();
        }

        public bool IsJoinVisible()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(JoinButton)).Displayed;
        }

        public bool IsJoinFormVisible()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(JoinForm)).Displayed;
        }
    }
}

