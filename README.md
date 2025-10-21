Project Mars – Manual & Automation Testing Overview

Project Mars is a sample web-based application used to manage user profiles, including modules such as Login, Languages, and Skills.
This repository includes both manual testing and BDD automation testing of these core functionalities.

Manual Testing

The Mars_Onboarding_Test_Cases.xlsx file contains detailed test cases and execution results for:

Login feature – verifying valid/invalid login scenarios and UI validations

Languages feature – verifying add, update, and delete functionalities

Skills feature – verifying add, update, and delete functionalities

The test cases cover:

Positive and negative test scenarios

Boundary and destructive testing

Expected vs actual results with pass/fail status



Automation Testing (BDD Framework)

Folder: qa-dotnet-cucumber

Developed using C#, Reqnroll (Cucumber for .NET), Selenium WebDriver, and NUnit.

Implements BDD-style feature files for:

Login

Languages (add, update, delete)

Skills (add, update, delete)

Each feature includes:

Scenario Outlines with Examples for parameterized testing

Positive, Negative, and Destructive test coverage

Hooks for state management (cleanup before/after each test)

Tests are fully independent and parallel-execution ready.

Known Bugs / Issues

All known defects identified during testing are listed in the qa-dotnet-cucumber/readme.md file under the KNOWN BUGS/ISSUES section.

Tools & Technologies

Language: C#

Framework: Reqnroll (Cucumber for .NET), NUnit

Automation Tool: Selenium WebDriver

IDE: Visual Studio

Version Control: Git & GitHub

This project demonstrates a complete end-to-end testing approach for Project Mars, combining manual and BDD automation testing using Reqnroll, Selenium, and NUnit. It showcases practical testing strategies—covering positive, negative, and destructive scenarios—along with effective state management and test data cleanup to ensure reliable, independent, and maintainable test execution.
