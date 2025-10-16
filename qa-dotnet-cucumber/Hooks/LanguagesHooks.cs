using AventStack.ExtentReports;
using static AventStack.ExtentReports.MediaEntityBuilder;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using qa_dotnet_cucumber.Helpers;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using Reqnroll.BoDi;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.Collections.Generic;
using System.IO;

[Binding]
public class LanguagesHooks
{
    private readonly IObjectContainer _objectContainer;
    private readonly ScenarioContext _scenarioContext;
    private ExtentTest _test;
    private IWebDriver _driver;

    public LanguagesHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario("@Languages")]
    public void BeforeLanguagesScenario()
    {
        try
        {

            var extent = typeof(Hooks)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.GetValue(null) as ExtentReports;

            lock (extent)
            {
                _test = extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
            }

            _scenarioContext["ExtentTest"] = _test;
            _test.Log(Status.Info, $"Starting scenario '{_scenarioContext.ScenarioInfo.Title}'");


            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            _driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();

            _objectContainer.RegisterInstanceAs(_driver);
            _scenarioContext["Driver"] = _driver;

            // Login using helper
            var navigationHelper = new NavigationHelper(_driver);
            LoginHelper.LoginAsTestUser(navigationHelper, _driver);
            _test.Log(Status.Pass, "Login successful");

            // Navigate to Languages tab
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var tab = wait.Until(d => d.FindElement(By.XPath("//a[@data-tab='first' and text()='Languages']")));
            tab.Click();
            _test.Log(Status.Pass, "Languages tab opened successfully");

            // Cleanup existing data before scenario
            LanguagesAndSkillsHelper.CleanupAllTestLanguages(_driver);
            _test.Log(Status.Info, "Cleaned up existing test languages before scenario");

            _scenarioContext["AddedLanguages"] = new List<string>();
        }
        catch (Exception ex)
        {
            _test?.Log(Status.Fail, $"Error during BeforeScenario: {ex.Message}");
            _driver?.Quit();
            throw;
        }
    }

    [AfterStep("@Languages")]
    public void AfterStep()
    {
        var extent = typeof(Hooks)
            .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.GetValue(null) as ExtentReports;

        var test = _scenarioContext["ExtentTest"] as ExtentTest;
        var stepText = _scenarioContext.StepContext.StepInfo.Text;

        if (_scenarioContext.TestError == null)
        {
            lock (extent)
            {
                test.Log(Status.Pass, stepText);
            }
        }
        else
        {
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"Screenshot_{DateTime.Now.Ticks}.png");
            screenshot.SaveAsFile(path);

            lock (extent)
            {
                test.Log(Status.Fail, _scenarioContext.TestError.Message,
                    CreateScreenCaptureFromPath(path).Build());
            }
        }
    }

    [AfterScenario("@Languages")]
    public void AfterLanguagesScenario()
    {
        try
        {
            if (_scenarioContext.ContainsKey("AddedLanguages"))
            {
                var addedLanguages = (List<string>)_scenarioContext["AddedLanguages"];
                foreach (var lang in addedLanguages)
                {
                    LanguagesAndSkillsHelper.CleanupScenarioLanguage(_driver, lang);
                    _test.Log(Status.Info, $"Deleted language '{lang}' added by this scenario");
                }
            }

            _test.Log(Status.Info, "Completed cleanup after scenario");
        }
        catch (Exception ex)
        {
            _test.Log(Status.Fail, $"Error during AfterScenario cleanup: {ex.Message}");
        }
        finally
        {
            _driver.Quit();
            _test.Log(Status.Info, "Closed browser for @Languages scenario");
        }
    }
}
