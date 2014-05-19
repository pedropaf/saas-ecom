Feature: Dashboard

@Admin
@AdminDashboard
Scenario: Stripe setup panel
	Given I am in the admin dashboard
	And Stripe details have not been setup
	When I click Setup Stripe
	Then I see the stripe settings form

@Admin
@AdminDashboard
Scenario: Subscription Plans banner
	Given I am in the admin dashboard
	And There are no subscription plans in the database
	When I click Create Subscription Plans
	Then I see the form to add a new plan

@Admin
@AdminDashboard
Scenario: Stripe is setup -> no banner
	Given I am in the admin dashboard
	And Stripe is setup
	Then I can't see Stripe setup banner

@Admin 
@AdminDashboard
Scenario: Subscription plans list
	Given I am in the admin dashboard
	And Subscriptions plans saved in the database
	Then I can see the list of plans
