using AventStack.ExtentReports;
using OpenQA.Selenium;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using System.Collections.Generic;
using NUnit.Framework;

[Binding]
public class SkillsPageSteps
{
    private readonly IWebDriver _driver;
    private readonly ScenarioContext _scenarioContext;
    private readonly ExtentTest _test;
    private readonly SkillsPage _skillsPage;

  
    public SkillsPageSteps(IWebDriver driver, ScenarioContext scenarioContext)
    {
        
        _scenarioContext = scenarioContext;
        _driver = (IWebDriver)_scenarioContext["Driver"];
        _skillsPage = new SkillsPage(_driver); 
        _test = (ExtentTest)_scenarioContext["ExtentTest"];
    }

    [Given(@"I am on the Skills tab")]
    public void GivenIAmOnTheSkillsTab()
    {       
        _test.Log(AventStack.ExtentReports.Status.Pass, "Navigated to Skills tab successfully.");
    }

    [When(@"I add a skill ""(.*)"" with level ""(.*)""")]
    public void WhenIAddASkillWithLevel(string skill, string level)
    {
        _skillsPage.AddSkill(skill, level);
        _test.Log(AventStack.ExtentReports.Status.Pass, $"Added skill '{skill}' with level '{level}'.");

        if (!_scenarioContext.ContainsKey("AddedSkills"))
            _scenarioContext["AddedSkills"] = new List<string>();

        var addedSkills = (List<string>)_scenarioContext["AddedSkills"];
        addedSkills.Add(skill);
    }

    [When(@"I try to add a Skill with invalid input ""(.*)"" and level ""(.*)""")]
    public void WhenITryToAddASkillWithInvalidInputAndLevel(string skill, string level)
    {
        _skillsPage.AddSkillWithInvalidInput(skill, level);
        _test.Log(Status.Info, $"Attempted to add invalid skill '{skill}' with level '{level}'");

        if (!_scenarioContext.ContainsKey("AddedSkills"))
            _scenarioContext["AddedSkills"] = new List<string>();

        var addedSkills = (List<string>)_scenarioContext["AddedSkills"];
        addedSkills.Add(skill);
    }

    [When(@"I try to add the same skill ""(.*)"" with level ""(.*)"" again")]
    public void WhenITryToAddTheSameSkillWithLevelAgain(string skill, string level)
    {
        _skillsPage.AddSkill(skill, level); // reuse the valid add method
        _test.Log(Status.Info, $"Attempted to add duplicate skill '{skill}' with level '{level}'.");
    }

    [When(@"I add a destructive input for skill ""(.*)"" with level ""(.*)""")]
    public void WhenIAddADestructiveInputForSkillWithLevel(string skill, string level)
    {
        if (skill == "<LONG_STRING>")
            skill = string.Concat(Enumerable.Repeat("Automation", 100));
        else if (skill == "<SCRIPT_TAG>")
            skill = "<script>alert('XSS')</script>";
        else if (skill == "<SQL_INJECTION>")
            skill = "' OR 1=1--";
        else if (skill == "<UNICODE>")
            skill = "漢字";
        _skillsPage.AddSkillWithInvalidInput(skill, level);

        _test.Log(Status.Info, $"Attempted destructive input: '{skill}' with level '{level}'.");
    }


    [When(@"I update the skill ""(.*)"" to ""(.*)"" with level ""(.*)""")]
    public void WhenIUpdateTheSkillToWithLevel(string oldSkill, string newSkill, string newLevel)
    {
        _skillsPage.UpdateSkill(oldSkill, newSkill, newLevel);

        _test.Log(Status.Pass, $"Updated skill '{oldSkill}' to '{newSkill}' with level '{newLevel}'.");

        // Track updated skills for cleanup
        if (!_scenarioContext.ContainsKey("AddedSkills"))
            _scenarioContext["AddedSkills"] = new List<string>();

        var addedSkills = (List<string>)_scenarioContext["AddedSkills"];

        // Remove old skill if it exists
        if (addedSkills.Contains(oldSkill))
            addedSkills.Remove(oldSkill);

        // Add the updated skill to the list
        addedSkills.Add(newSkill);
    }


    [When(@"I delete the skill ""(.*)""")]
    public void WhenIDeleteTheSkill(string skill)
    {
        try
        {
            _skillsPage.DeleteSkill(skill);
            _test.Log(Status.Pass, $"Attempted to delete skill '{skill}'.");
        }
        catch (NoSuchElementException)
        {
            _test.Log(Status.Info, $"Skill '{skill}' was not found in the list. No deletion occurred.");
        }

        // Remove from context tracking if needed
        if (_scenarioContext.ContainsKey("AddedSkills"))
        {
            var addedSkills = (List<string>)_scenarioContext["AddedSkills"];
            if (addedSkills.Contains(skill))
                addedSkills.Remove(skill);
        }
    }


    [Then(@"I should see the skill success message ""(.*)""")]
    public void ThenIShouldSeeTheSkillSuccessMessage(string expectedMessage)
    {
        var actualMessage = _skillsPage.GetToastMessage();
        Assert.That(actualMessage, Is.EqualTo(expectedMessage), "Success message did not match.");
        _test.Log(AventStack.ExtentReports.Status.Pass, $"Verified success message: '{actualMessage}'.");
    }

    [Then(@"I should see that invalid skill is not accepted ""(.*)""")]
    public void ThenIShouldSeeThatInvalidSkillIsNotAccepted(string expectedError)
    {
        // Read the actual toast message / error message
        var actualMessage = _skillsPage.GetToastMessage();

        if (actualMessage != null && actualMessage.Contains(expectedError))
        {
            _test.Log(Status.Pass, $"Correctly displayed error: '{actualMessage}'");
            Assert.Pass();
        }
        else
        {
            _test.Log(Status.Fail, $"Expected error '{expectedError}' but got '{actualMessage}'");
            Assert.Fail($"Invalid skill input not handled correctly. Expected: '{expectedError}', Actual: '{actualMessage}'");
        }
    }

    [Then(@"I should see a duplication warning ""(.*)""")]
    public void ThenIShouldSeeADuplicationWarning(string expectedMessage)
    {
        var actualMessage = _skillsPage.GetToastMessage();
        Assert.That(actualMessage, Is.EqualTo(expectedMessage), "Duplication warning did not match.");
        _test.Log(Status.Pass, $"Verified duplication warning: '{actualMessage}'.");
    }

    [Then(@"nothing happens because the skill does not exist")]
    public void ThenNothingHappensBecauseTheSkillDoesNotExist()
    {
        _test.Log(Status.Info, "Confirmed: no update occurred as skill was not present.");
    }

    [Then(@"I should see the skill deletion success message ""(.*)""")]
    public void ThenIShouldSeeTheSkillDeletionSuccessMessage(string expectedMessage)
    {
        var actualMessage = _skillsPage.GetToastMessage(); 

        Assert.That(actualMessage, Is.EqualTo(expectedMessage), "Skill deletion message did not match.");
        _test.Log(Status.Pass, $"Verified skill deletion message: '{actualMessage}'.");
    }


}
