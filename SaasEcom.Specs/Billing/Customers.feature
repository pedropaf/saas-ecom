Feature: Customers section in admin dashboard

@Admin
@AdminCustomers
Scenario: I can see an empty list of registered customers
	Given I am logged in to the admin panel
	And I am in the customers section of the Admin panel
	When There are no customers registered
	Then I can see an empty list of customers (placeholder)

@Admin
@AdminCustomers
Scenario: I can see a list of registered customers
	Given I am in the customers section of the Admin panel
	When  There are customers registered
	Then I can see the list of customers

@Admin
@AdminCustomers
Scenario: I can see a paginated list of registered customers and navigate forward
	Given I am in the customers section of the Admin panel
	And There are too many customers registered
	When I navigate to the next page
	Then I can see the second page of the list of customers

@Admin
@AdminCustomers
Scenario: I can see a paginated list of registered customers and navigate back
	Given There are too many customers registered
	And I am in the second page of customers section of the Admin panel
	When I navigate to the previous page
	Then I can see the first page of the list of customers 