
  Feature: Profile Page
  As a user, I want to view my languages and skills
  So that others can see my details

  Scenario: View Languages and Skills
    Given I am logged in 
    And  I am in my profile page
    Then I should see my languages listed
    When I switch to the skills tab
    Then I should see my skills listed