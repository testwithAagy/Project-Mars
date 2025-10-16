@HomePage
Feature: Landing Page/ Home Page
  As a user, I want to access the Mars application landing page / Home page.
  So that I can navigate to login or join

  Scenario: Landing page loads successfully
    Given I am on the Mars application landing page
    Then I should see the "Sign In" button
    And I should see the "Join" button

  Scenario: Click Sign In opens login form
    Given I am on the Mars application landing page
    When I click the "Sign In" button
    Then the login form should be displayed

  Scenario: Click Join opens join form
    Given I am on the Mars application landing page
    When I click the "Join" button
    Then the join form should be displayed
