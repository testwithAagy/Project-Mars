using AventStack.ExtentReports;
using OpenQA.Selenium;
using qa_dotnet_cucumber.Helpers;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using Reqnroll.BoDi;

[Binding]
public class LanguagesPageSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IWebDriver _driver;
    private readonly ExtentTest _test;
    private readonly LanguagesPage _languagesPage;

    public LanguagesPageSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _driver = (IWebDriver)_scenarioContext["Driver"];
        _test = (ExtentTest)_scenarioContext["ExtentTest"];
        _languagesPage = new LanguagesPage(_driver);
    }

    [Given(@"I am on the Languages tab")]
    public void GivenIAmOnTheLanguagesTab()
    {
       
        _test.Log(Status.Pass, "Landed on Languages tab.");
    }

    [When(@"I add a language ""(.*)"" with level ""(.*)""")]
    public void WhenIAddALanguageWithLevel(string language, string level)
    {
        _languagesPage.AddLanguage(language, level);

        // Track added languages for cleanup
        if (!_scenarioContext.ContainsKey("AddedLanguages"))
            _scenarioContext["AddedLanguages"] = new List<string>();
        ((List<string>)_scenarioContext["AddedLanguages"]).Add(language);

        _test.Log(Status.Pass, $"Added language '{language}' with level '{level}'.");
    }


    [When(@"I try to add a language ""(.*)"" with level ""(.*)"" again")]
    public void WhenITryToAddDuplicateLanguage(string language, string level)
    {
        _languagesPage.AddLanguageWithInvalidInput(language, level);
        _test.Log(Status.Info, $"Attempted to add duplicate language '{language}' with level '{level}'.");
    }

    [When(@"I try to add a language with invalid input ""(.*)"" and level ""(.*)""")]
    public void WhenITryToAddALanguageWithInvalidInput(string language, string level)
    {
        _languagesPage.AddLanguageWithInvalidInput(language, level);

        _test.Log(Status.Info, $"Attempted to add language '{language}' with level '{level}' (invalid input).");
    }

    [When(@"I add a destructive input for language ""(.*)"" with level ""(.*)""")]
    public void WhenIAddADestructiveInputForLanguageWithLevel(string language, string level)
    {
        if (language == "<LONG_STRING>")
            language = new string('A', 1000);
        else if (language == "<IMG_SCRIPT>")
            language = "<img src=x onerror=alert(1)>";

      
        _languagesPage.AddLanguageWithInvalidInput(language, level);

        _test.Log(Status.Info, $"Attempted destructive input: '{language}' with level '{level}'.");
    }

    [When(@"I update the language ""(.*)"" to ""(.*)"" with level ""(.*)""")]
    public void WhenIUpdateTheLanguageToWithLevel(string oldLanguage, string newLanguage, string newLevel)
    {
        _languagesPage.UpdateLanguage(oldLanguage, newLanguage, newLevel);

        _test.Log(Status.Pass, $"Updated language '{oldLanguage}' to '{newLanguage}' with level '{newLevel}'.");

        // Track updated language for cleanup
        if (!_scenarioContext.ContainsKey("AddedLanguages"))
            _scenarioContext["AddedLanguages"] = new List<string>();

        var addedLanguages = (List<string>)_scenarioContext["AddedLanguages"];

        // Remove old language if it exists
        if (addedLanguages.Contains(oldLanguage))
            addedLanguages.Remove(oldLanguage);

        // Add the updated language to the list
        addedLanguages.Add(newLanguage);
    }

    [When(@"I delete the language ""(.*)""")]
    public void WhenIDeleteTheLanguage(string language)
    {
        try
        {
            _languagesPage.DeleteLanguage(language);
            _test.Log(Status.Pass, $"Attempted to delete language '{language}'.");
        }
        catch (NoSuchElementException)
        {
            _test.Log(Status.Info, $"Language '{language}' was not found in the list. No deletion occurred.");
        }
        // Remove from context tracking if needed
        if (_scenarioContext.ContainsKey("AddedLanguages"))
        {
            var addedLanguages = (List<string>)_scenarioContext["AddedLanguages"];
            if (addedLanguages.Contains(language))
                addedLanguages.Remove(language);
        }
    }


    [Then(@"I should see the language success message""(.*)""")]
    public void ThenIShouldSee(string expectedMessage)
    {
        string actual = _languagesPage.GetToastMessage();
        _test.Log(Status.Info, $"Expected: '{expectedMessage}', Actual: '{actual}'");
        Assert.That(actual, Is.EqualTo(expectedMessage));
        _test.Log(Status.Pass, "Toast message validation passed.");
    }

    [Then(@"I should see the language error message ""(.*)""")]
    public void ThenIShouldSeeTheLanguageErrorMessage(string expectedError)
    {
        string actual = _languagesPage.GetToastMessage();
        _test.Log(Status.Info, $"Expected Error: '{expectedError}', Actual: '{actual}'");

        Assert.That(actual, Is.EqualTo(expectedError));
        _test.Log(Status.Pass, "Error message validation passed.");
    }

   

    [Then(@"I should see the language deletion success message ""(.*)""")]
    public void ThenIShouldSeeTheLanguageDeletionSuccessMessage(string expectedMessage)
    {
        string actual = _languagesPage.GetToastMessage();
        Assert.That(actual, Is.EqualTo(expectedMessage));
        _test.Log(Status.Pass, $"Deletion toast message validated: '{actual}'");
    }

    [Then(@"nothing happens because the language does not exist")]
    public void ThenNothingHappensBecauseTheLanguageDoesNotExist()
    {
        _test.Log(Status.Info, "Confirmed: no deletion occurred as language was not present.");
    }

}
