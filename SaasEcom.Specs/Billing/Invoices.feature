Feature: Invoices

@Admin
@AdminInvoices
Scenario: I can see an empty list of invoices
	Given I am in the invoices section of the Admin panel
	When There are no invoices in the system
	Then I can see an empty list of invoices (placeholder)

@Admin
@AdminInvoices
Scenario: I can see a list of invoices
	Given I am in the invoices section of the Admin panel
	When  There are invoices in the system
	Then I can see the list of invoices

@Admin
@AdminInvoices
Scenario: I can see a paginated list of invoices and navigate forward
	Given I am in the invoices section of the Admin panel
	And There are too many invoices in the system
	When I navigate to the next page
	Then I can see the second page of invoices

@Admin
@AdminInvoices
Scenario: I can see a paginated list of invoices and navigate back
	Given There are too many invoices
	And I am in the second page of invoices
	When I navigate to the previous page
	Then I can see the first page of invoices
