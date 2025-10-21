@Profile @Skills
Feature: Add Skills in Profile
As a user, I want to manage Skills in my profile

  # --------------------------
  # ADD SKILLS SCENARIO
  # --------------------------

  # Positive Test - Valid Input
  
  @Positive
  Scenario Outline: Successfully adding a new skill
    Given I am on the Skills tab
    When I add a skill "<Skill>" with level "<Level>"
    Then I should see the skill success message "<ExpectedMessage>"

    Examples:
      | Skill       | Level          | ExpectedMessage                            |
      | Automation  | Expert         | Automation has been added to your skills   |
      | C#          | Intermediate   | C# has been added to your skills           |


  # Negative Tests - Invalid Input

  @Negative
  Scenario Outline: Adding Skills with invalid inputs
   Given I am on the Skills tab
   When I try to add a Skill with invalid input "<Skills>" and level "<Level>"
   Then I should see that invalid skill is not accepted "<ExpectedError>"

  Examples:
    | Skills                          | Level    | ExpectedError                               |
    |                                 | Expert   | Please enter skill and experience level     |
    | Java                            |          | Please enter skill and experience level     |
    |                                 |          | Please enter skill and experience level     |
    |@#$%                             | Expert   | Please enter a Valid Skill                  |

 # Negative Test - Valid Input (Duplicate Skill)
 
 @Negative
 Scenario Outline: Adding duplicate skills should not be allowed
  Given I am on the Skills tab
  When I add a skill "<Skill>" with level "<Level>"
  And I try to add the same skill "<Skill>" with level "<Level>" again
  Then I should see a duplication warning "<ExpectedMessage>"

  Examples:
    | Skill  | Level        | ExpectedMessage                                |
    | Java   | Expert       | This skill is already exist in your skill list.|
    | JAVA   | Expert       | This skill is already exist in your skill list.|
    | java   | Expert       | This skill is already exist in your skill list.|
    | Java   | Intermediate | This skill is already exist in your skill list.|


@Destructive @Negative
Scenario Outline: Adding a skill with destructive or malicious input
  Given I am on the Skills tab
  When I add a destructive input for skill "<Skill>" with level "<Level>"
  Then I should see that invalid skill is not accepted "<ExpectedError>"



  Examples:
    | Skill                                           | Level   | ExpectedError                |
    | <LONG_STRING>                                   | Expert  | Please enter a valid skill   |
    | <script>alert('XSS')</script>                   | Expert  | Please enter a valid skill   |
    | ' OR 1=1--                                      | Expert  | Please enter a valid skill   |
    | 漢字                                             | Expert  | Please enter a valid skill   |



# --------------------------
# UPDATE SKILL SCENARIO
# --------------------------

# Positive Test - Valid Input
@Positive
Scenario Outline: Successfully updating a skill
  Given I am on the Skills tab
  When I add a skill "<OldSkill>" with level "<OldLevel>"
  And I update the skill "<OldSkill>" to "<NewSkill>" with level "<NewLevel>"
  Then I should see the skill success message "<ExpectedMessage>"

Examples:
  | OldSkill    | OldLevel       | NewSkill    | NewLevel       | ExpectedMessage                            |
  | Automation  | Intermediate   | Automation  | Expert         | Automation has been updated to your skills |
  | Java        | Expert         | Python      | Expert         | Python has been updated to your skills     |
  | C#          | Intermediate   | C++         | Expert         | C++ has been updated to your skills        |



  # Negative Test - Valid input

  @Negative
  Scenario Outline: Attempt to update a non-existing skill
  Given I am on the Skills tab
  When I update the skill "Ruby" to "Python" with level "Expert"
  Then nothing happens because the skill does not exist



# --------------------------
# DELETE SKILL SCENARIO
# --------------------------

@Positive
Scenario Outline: Successfully deleting a skill
  Given I am on the Skills tab
  When I add a skill "<Skill>" with level "<Level>"
  And I delete the skill "<Skill>"
  Then I should see the skill deletion success message "<ExpectedMessage>"

  Examples:
    | Skill      | Level   | ExpectedMessage         |
    | Java       | Expert  | Java has been deleted   |

@Negative
Scenario Outline: Attempt to delete a non-existing skill
    Given I am on the Skills tab
    When I delete the skill "Sports"
    Then nothing happens because the skill does not exist

   