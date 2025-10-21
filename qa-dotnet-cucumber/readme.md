# QA-DotNet-Cucumber Framework

A .NET-based test automation framework using Reqnroll (Cucumber for .NET), Selenium WebDriver, and NUnit. This framework
is designed to test web applications with a clean, maintainable structure.

## Overview

This framework provides automated functional testing for web applications with the following features:

- **Reqnroll**: Implements Cucumber's Gherkin syntax for readable tests
- **Selenium WebDriver**: Handles browser interactions
- **NUnit**: Manages test execution and assertions
- **ExtentReports**: Generates HTML test reports
- **Page Object Model (POM)**: Separates test logic from page interactions

## Prerequisites

- **.NET SDK**: Version 8.0 or higher (install from [dotnet.microsoft.com](https://dotnet.microsoft.com))
- **IDE**: Visual Studio Code or Visual Studio (recommended)
- **Chrome Browser**: Required for Selenium WebDriver (ChromeDriver version must match your browser version via
  WebDriverManager)

## Project Structure

```
├── Features/           # Gherkin feature files
├── Steps/              # C# step implementations
├── Pages/              # Page Object Model classes
├── Hooks/              # Setup and teardown logic
├── Config/             # Configuration classes
├── Tests/              # NUnit test runner
└── settings.json       # Configuration file
```
## Overview of My Implementation

I have added the framework to automate login, profile, and home page features of the application. Key points of my implementation:

* **Feature-driven testing**: Added multiple `.feature` files covering home page, login, and profile management scenarios including languages and skills feature, including sign in and join workflows.
* **Step definitions**: Implemented step classes in `StepDefinitions/` to map Gherkin steps to actions using Page Objects.
* **Page Object Model (POM)**: Created `HomePage.cs`, `LoginPage.cs`, `LanguagesPage.cs' and 'SkillsPage.cs`  with methods to encapsulate page interactions.
* **Hooks**: Added `Hooks.cs', 'LanguagesHooks.cs' and 'SkillsHooks.cs` to initialize WebDriver, Global clean up, maximize the window, manage waits, and capture screenshots on test failure to avoid test dependencies.
* * **Reporting**: Integrated ExtentReports to generate HTML test reports for all scenarios.

## Key Features Implemented

### Home Page Automation

* Verified the home page loads correctly.
* Implemented validation for **Sign In** and **Join** buttons.
* Ensured navigation to login page after clicking **Sign In** and to registration page after clicking **Join**.

### Login Automation

* Verified successful login redirects to profile page 
* Handled unsuccessful login with email verification error and frontend error validation.
* Added assertions to check UI elements after login.

* flows tested:
Positive / Successful login
Negative / Invalid input login
Negative / Valid input edge cases

### Profile Page - Languages feature  Automation


* Verified default selection of the profile tab and visibility of languages.
* Implemented methods to add, update, and delete languages with various inputs.
* Tested positive scenarios with valid inputs.
* Tested negative scenarios including invalid input, valid input edge cases (like duplicates and case sensitivity), and destructive/malicious inputs.
* Ensured validation messages are displayed correctly and system does not crash.




### Profile Page - Skills feature  Automation

* Verified visibility of skills on the profile page on switching.
* Implemented methods to add, update, and delete skills with various inputs.
* Tested positive scenarios with valid inputs.
* Tested negative scenarios including invalid input, valid input edge cases (like duplicates and case sensitivity), and destructive/malicious inputs.
* Ensured validation messages are displayed correctly and system does not crash.


### Test Flow

* Each scenario begins with `Given I am on the home page` or `Given I am logged in`.
* Actions are executed via Page Objects.
* Assertions verify page elements, text, and navigation correctness.
* Screenshots captured for reporting and evidence of test execution.

Tests → Contains CucumberRunner.cs, an NUnit-based test runner that allows ReqnRoll to automatically discover and execute all feature files. Parallel execution is enabled.

## Screenshots for Submission

All screenshots are placed in the `screenshots/` folder:


## Running the Tests

1. Restore dependencies:

```bash
dotnet restore
```

2. Build the solution:

```bash
dotnet build
```

3. Run tests:

```bash
dotnet test
```

4. Open the HTML report (`TestReport.html`) in your browser to view detailed results.




KNOWN BUGS/ISSUES

The following issues were identified during testing of the languages and skills features:

1.Premature account lock / login field blocked on first invalid attempt

Test Case: Multiple Invalid Login Attempts Should Lock The Account

Expected Behavior:

User should be able to attempt login up to 3 times with incorrect password.
Only after the 3rd invalid attempt should the account be temporarily locked and a message displayed:
“Your account has been temporarily locked due to multiple failed attempts.”

Actual Behavior:

On the first invalid login attempt, the password field disappears / becomes inaccessible.
User cannot retry login.
Selenium test fails with:
NoSuchElementException: Unable to locate element: *[name="password"]
Account is effectively “blocked” after the first wrong password attempt.


2.Invalid skill input not validated

Test Case: AddingSkillsWithInvalidInputs("@#$%", "Expert")

Actual Behavior: The system accepts invalid input @#$% and adds it to the skills list.

Expected Behavior: The system should reject invalid input and show error message: "Please enter a Valid Skill".


3.Duplicate language addition not blocked (case insensitive check missing)

Test Case: AddingADuplicateLanguage("ENGLISH", "Basic", "This language is already exist in your language list.")

Actual Behavior: The system allows adding a language that is the same as an existing one but with different letter casing (ENGLISH) and shows:
"ENGLISH has been added to your languages".

Expected Behavior: The system should reject duplicate languages regardless of case and show the error message:
"This language is already exist in your language list."


4.Invalid language input not handled

Test Case: AddingLanguageWithInvalidInputs("@#$%", "Fluent", "Please enter a valid language")

Actual Behavior: The system allows an invalid language input (@#$%) to be added and shows:
"@#$% has been added to your languages"

Expected Behavior: The system should reject invalid inputs and show an error message:
"Please enter a valid language"


5.Destructive/malicious language input not handled

Test Case: AddingALanguageWithDestructiveOrMaliciousInput("<IMG_SCRIPT>", "Fluent", "Please enter a valid language")

Actual Behavior:

Adding a malicious input like <IMG_SCRIPT> triggers an unexpected alert:
OpenQA.Selenium.UnhandledAlertException : unexpected alert open: {Alert text : 1}
The system does not handle the input properly and does not show the expected validation error.

Expected Behavior:

The system should reject malicious or destructive inputs and display:
"Please enter a valid language"
No unexpected alerts should appear.


6.System accepts excessively long language input

Test Case: AddingALanguageWithDestructiveOrMaliciousInput("<LONG_STRING>", "Fluent", "Please enter a valid language")

Actual Behavior:

Adding a very long string (e.g., 1000+ characters) as a language is accepted.
The system displays the full string instead of the validation error:
"AAAAAAAAAAA...."

The expected validation message "Please enter a valid language" is not shown.
No crash occurred in this case, but the system fails to handle the input properly.


7.SQL-like input accepted as a language

Test Case: AddingLanguageWithInvalidInputs("\"; DROP TABLE Languages; --", "Fluent", "Please enter a valid language")

Actual Behavior:

Inputting SQL-like text ("; DROP TABLE Languages; --) is accepted by the system.
The system displays:
"; DROP TABLE Languages; -- has been added to your languages"

No validation error "Please enter a valid language" appears.
Eventhough the SQL-like input does not actually execute any scripts or cause a system crash, it is still incorrectly accepted as a valid language.


8.HTML/Script input accepted as a language

Test Case: AddingLanguageWithInvalidInputs("<script>alert('hacked')</script>", "Fluent", "Please enter a valid language")

Actual Behavior:

Inputting an HTML/script tag (<script>alert('hacked')</script>) is accepted by the system.
The system shows: "has been added to your languages"
No validation error "Please enter a valid language" appears.
Eventhough the input does not execute any scripts or alert, it is still incorrectly accepted as a valid language.

Expected Behavior:

Input validation should reject HTML/script tags or malicious strings.
The system should display: "Please enter a valid language"
No script execution or HTML injection should be allowed.


9.Numeric input accepted as a language

Test Case: AddingLanguageWithInvalidInputs("12345", "Fluent", "Please enter a valid language")

Actual Behavior:

Entering numeric-only input ("12345") is accepted by the system.
The system shows: "12345 has been added to your languages"
No validation error "Please enter a valid language" appears.

Expected Behavior:

Input validation should reject numeric-only values as invalid language names.
The system should display: "Please enter a valid language"


10.SQL injection-like input accepted as a skill

Test Case: AddingASkillWithDestructiveOrMaliciousInput("' OR 1=1--", "Expert", "Please enter a valid skill")

Actual Behavior:

Entering SQL-like input (' OR 1=1--) is accepted by the system.
The system shows: "' OR 1=1-- has been added to your skills"
System did not fail or crash, but the input should not have been accepted.

Expected Behavior:

Input validation should reject destructive or malicious patterns as invalid skill names.
The system should display: "Please enter a valid skill"
System should not crash or execute any SQL/script operations.


11.Excessively long skill input accepted

Test Case: AddingASkillWithDestructiveOrMaliciousInput("<LONG_STRING>", "Expert", "Please enter a valid skill")

Actual Behavior:

Entering a very long string for a skill (e.g., hundreds of characters) is accepted by the system.
The system shows: 'AutomationAutomationAutomation... has been added to your skills'
System did not fail or crash, but the input should not have been accepted.

Expected Behavior:

Input validation should reject excessively long or malformed skill names.
The system should display: "Please enter a valid skill"
System should not crash or produce unintended behavior.


12.Bug Title: Destructive XSS input for skill is accepted

Test Case: AddingASkillWithDestructiveOrMaliciousInput("<script>alert('XSS')</script>", "Expert", "Please enter a valid skill")

Actual Behavior:

Entering a destructive input with a script tag is accepted by the system.
The system shows: "has been added to your skills"
System did not fail or crash, and no XSS alert was executed.
Even though the system did not fail, it still accepts invalid/malicious input, which is incorrect behavior.

Expected Behavior:

Input validation should reject HTML/script tags in skill names.
The system should display: "Please enter a valid skill"
System should not execute any scripts or produce unintended behavior.


13.Non-Latin input for skill is accepted

Test Case: AddingASkillWithDestructiveOrMaliciousInput("漢字", "Expert", "Please enter a valid skill")

Actual Behavior:

Entering a skill name using non-Latin characters (漢字) is accepted by the system.
The system shows: "漢字 has been added to your skills"
System did not fail or crash, and no errors occurred.
Even though the system did not fail, it still accepts invalid input, which violates the expected input validation rules.

Expected Behavior:

Input validation should reject skill names with unsupported characters.
The system should display: "Please enter a valid skill"
System should not allow invalid inputs to be added.



## Notes

* The WebDriver is configured via `settings.json`; Chrome is used by default.
* The Page Objects encapsulate element locators and actions, ensuring maintainable test code.
* Hooks manage initialization, cleanup, and screenshot capture.
* Feature files clearly define BDD scenarios and are linked to step definitions
* Each test is independent and does not rely on data from other tests.
* Before starting any test , all existing languages and skills are deleted to reset the state.
* After each test, only the data created by that test is removed, ensuring other tests are unaffected.
* This approach ensures:Tests can run in any order or in parallel, No false positives or negatives occur due to leftover data, Language and skill updates/deletions first create the required test data before performing actions.
* Implemented via LanguagesHooks.cs and SkillsHooks.cs alongside Hooks.cs for login and profile flows.


This implementation demonstrates an end-to-end BDD test automation setup using the QA-DotNet-Cucumber framework,  including home page validation for both Sign In and Join , login and profile automation scenarios.