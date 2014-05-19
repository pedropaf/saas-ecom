Feature: SubscriptionPlans

@Admin
@AdminSubscriptionPlans
Scenario: I can see an empty list of subscription plans
	Given I am in the plans section of the Admin panel
	When  There are no plans in the system
	Then I can see an empty list of plans

@Admin
@AdminSubscriptionPlans
Scenario: I can add a subscription plan
	Given I am in the plans section of the Admin panel
	And I click on Start creating a subscription plan
	When I fill in the plan details
	| field | value |
	| FriendlyId | test_plan_id |
	| Name | Test plan name |
	| Price | 30.45 |
	| Interval | Monthly |
	| TrialPeriodInDays | 14 |
	Then I can see the plan added in the list

@Admin
@AdminSubscriptionPlans
Scenario: Add a subscription plan validation
	Given I am in the plans section of the Admin panel
	And I click on Start creating a subscription plan
	When I click on save plan
	Then I can see the plan form validation

@Admin
@AdminSubscriptionPlans
Scenario: I can see a list of subscription plans
	When I am in the plans section of the Admin panel
	And There are plans in the system
	Then I can see a list of plans

@Admin
@AdminSubscriptionPlans
Scenario: I can edit the name of a subscription plans
	Given I am in the plans section of the Admin panel
	And There are plans in the system
	When I click on edit
	And Fill in the form with a valid name
	And I click on save
	Then I can see a list of plans with the plan updated

@Admin
@AdminSubscriptionPlans
Scenario: I can disable a subscription plan
	Given I am in the plans section of the Admin panel
	And There are plans in the system
	When I click on disable plan and confirm
	Then The plan is disabled and no more customers can join

@Admin
@AdminSubscriptionPlans
Scenario: I can delete a subscription plan
	Given I am in the plans section of the Admin panel
	And There are plans in the system
	And No customers have signed up for the plan yet
	When I click on delete plan and confirm
	Then The plan is deleted from the system
