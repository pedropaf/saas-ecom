Feature: Register
	I can visit the website
	As a user
	I want to register

@UI
Scenario: Open homepage
	Given I have the homepage open
	Then I should see "This is a template that can be used as a startup point to build your SAAS subscription website using ASP.NET MVC 5." on the screen

@UI
Scenario: Navigate to register
	Given I have the homepage open
	When I click on "hero-btn"
	Then I see the registration form
	
@UI
Scenario: Register valid data
    Given I am at the registration page 
    When I fill the registration form
    | field            | value         |
    | Email            | test@test.com |
    | Password         | pass01        |
    | ConfirmPassword  | pass01        |
    | SubscriptionPlan | Premium       |
    And I click on "Register"
    Then I should see "This is a template that can be used as a startup point to build your SAAS subscription website using ASP.NET MVC 5." on the screen

@UI
Scenario: Register with invalid data
    Given I am at the registration page
    When I fill the registration form with invalid data
    And I click on "Register"
    Then I see validation errors
