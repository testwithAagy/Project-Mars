@Profile @Languages
Feature: Add Languages in Profile
As a user, I want to manage languages in my profile

  # --------------------------
  # ADD LANGUAGE SCENARIO
  # --------------------------



  # Positive Test - Valid Input

  @Positive 
  Scenario Outline: Successfully adding a new language
    Given I am on the Languages tab
    When I add a language "<Language>" with level "<Level>"
    Then I should see the language success message"<ExpectedMessage>"

    Examples:
      | Language | Level  | ExpectedMessage                          |
      | English  | Fluent | English has been added to your languages |



  # Negative Tests - Invalid Input

  @Negative
  Scenario Outline: Adding language with invalid inputs
   Given I am on the Languages tab
   When I try to add a language with invalid input "<Language>" and level "<Level>"
   Then I should see the language error message "<ExpectedError>"

  Examples:
    | Language                        | Level  | ExpectedError                         |
    |                                 | Fluent | Please enter language and level       |
    | Spanish                         |        | Please enter language and level       |
    | 12345                           | Fluent | Please enter a valid language         |
    |                                 |        | Please enter language and level       |
    |@#$%                             | Fluent | Please enter a valid language         |
    |"; DROP TABLE Languages; --      | Fluent | Please enter a valid language         |
    |<script>alert('hacked')</script> | Fluent | Please enter a valid language         |


    # Negative Test - Valid input

    @Negative
    Scenario Outline: Adding a duplicate language
     Given I am on the Languages tab
     When I add a language "English" with level "Fluent"
     And I try to add a language "<Language>" with level "<Level>" again
     Then I should see the language error message "<ExpectedError>"

    Examples:
      | Language | Level          | ExpectedError                                             |
      | English  | Fluent         | This language is already exist in your language list.     |
      | English  | Conversational | Duplicated data                                           |
      | ENGLISH  | Basic          | This language is already exist in your language list.     |
  
  
  # Negative Test - Destructive or Malicious Input

  @Destructive @Negative
  Scenario Outline: Adding a language with destructive or malicious input
   Given I am on the Languages tab
   When I add a destructive input for language "<Language>" with level "<Level>"
   Then I should see the language error message "<ExpectedError>"
   And the system should not crash or display any HTML/script output

  Examples:
    | Language       | Level  | ExpectedError                 |
    | <LONG_STRING>  | Fluent | Please enter a valid language |
    | <IMG_SCRIPT>   | Fluent | Please enter a valid language |


  # --------------------------
  # UPDATE LANGUAGE SCENARIO
  # --------------------------



 # Positive Test - Valid Input

 @Positive
 Scenario Outline: Successfully updating a language
  Given I am on the Languages tab
  When I add a language "<OldLanguage>" with level "<OldLevel>"
  And I update the language "<OldLanguage>" to "<NewLanguage>" with level "<NewLevel>"
  Then I should see the language success message"<ExpectedMessage>"

  Examples:
    | OldLanguage | OldLevel       | NewLanguage | NewLevel        | ExpectedMessage                            |
    | English     | Fluent         | Spanish     | Conversational  | Spanish has been updated to your languages |
    | Spanish     | Basic          | Spanish     | Native/Bilingual| Spanish has been updated to your languages |
    | French      | Conversational | French      | Fluent          | French has been updated to your languages  |



 # Negative Test - Valid input

  @Negative
  Scenario Outline: Attempt to update a non-existing language
    Given I am on the Languages tab
    When I update the language "Italian" to "French" with level "Fluent"
    Then nothing happens because the language does not exist


  # --------------------------
  # DELETE LANGUAGE SCENARIO
  # --------------------------


  # Positive Test - Valid Input

  @Positive 
  Scenario Outline: Successfully deleting a language
    Given I am on the Languages tab
    When I add a language "<Language>" with level "<Level>"
    And I delete the language "<Language>"
    Then I should see the language deletion success message "<ExpectedMessage>"

    Examples:
      | Language | Level  | ExpectedMessage                          |
      | English  | Fluent | English has been deleted from your languages |


 # Negative Test - Valid input

@Negative 
Scenario Outline: Attempt to delete a non-existing language
    Given I am on the Languages tab
    When I delete the language "Italian"
    Then nothing happens because the language does not exist
