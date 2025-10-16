@Login @requiresTestUser
Feature: Login Functionality
  As a user, I want to log in to the application to access restricted content.

  # -----------------------------
  # Positive Test
  # -----------------------------
  @Positive
  Scenario: Successful login with valid credentials
    Given I am on the landing page
    When I click the "Sign In" button for login
    And I enter email " newuser2@test.com" and password "Pass@123"
    And I click the login button
    Then I should see "Secure area displayed"


  # --------------------------------------------
  # Negative Tests - Valid & Invalid Credentials
  # --------------------------------------------
  
  @Negative
  Scenario Outline: Login attempts with invalid credentials
    Given I am on the landing page
    When I click the "Sign In" button
    And I enter email "<Email>" and password "<Password>"
    And I click the login button
    Then I should see "<ExpectedMessage>"

    Examples:
      | Email                    | Password       | ExpectedMessage                               |
      | wrong@test               | Pass123!       | Please enter a valid email address            |
      | newuser2@test.com        | wrongpass      | Send Verification Email                       |
      |                          | Pass1234       | Please enter a valid email address            |
      |username@test.com         |                | Password must be at least 6 characters        |
      |unregistereduser@test.com | Pass@123       | User does not exist                           |


  # -------------------------------------------------------
  # Negative Test - Multiple Failed Attempts (Account Lock)
  # -------------------------------------------------------
 
 @Negative
  Scenario: Multiple invalid login attempts should lock the account
    Given I am on the landing page
    When I click the "Sign In" button
    And I attempt to log in with email "newuser2@test.com" and invalid password "Wrong@123" three times
    Then I should see "Your account has been temporarily locked due to multiple failed attempts"