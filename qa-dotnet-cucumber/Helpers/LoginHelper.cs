using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using qa_dotnet_cucumber.Pages;
using SeleniumExtras.WaitHelpers;
using System;

namespace qa_dotnet_cucumber.Helpers
{
    public static class LoginHelper
    {
        public static void LoginAsTestUser(NavigationHelper navigationHelper, IWebDriver driver)
        {
            string email = "newuser2@test.com";
            string password = "Pass@123";

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            navigationHelper.NavigateTo("");

            try
            {
                var signInButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[text()='Sign In']")));
                signInButton.Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")));
                var emailInput = driver.FindElement(By.Name("email"));
                var passwordInput = driver.FindElement(By.Name("password"));

                emailInput.Clear();
                emailInput.SendKeys(email);
                passwordInput.Clear();
                passwordInput.SendKeys(password);

                var loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Login']")));
                loginButton.Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'Languages')]")));

                Console.WriteLine("Logged in successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                throw;
            }
        }


        public static void SignOut(IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));



                // Click on the Sign Out button 
                var signOutButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@class='item']/button[text()='Sign Out']")));
                signOutButton.Click();
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[text()='Sign In']"))); // wait for login page
                Console.WriteLine("User signed out successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignOut failed: {ex.Message}");
            }
        }

    }
}
