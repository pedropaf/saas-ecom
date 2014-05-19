Feature: Logout

@Admin
@AdminCustomers
Scenario: I can logout of the admin area
	Given I am in the Admin panel
	When  I click logout 
	Then I am logged out of the admin panel
