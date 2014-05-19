Feature: StripeSettings

@Admin
@AdminSettings
Scenario: Stripe change account credentials in test
	Given I am logged in to the dashboard
	And I am in settings section
	When I fill in the details for stripe
	| field | value |
	| StripeAccount_StripeTestSecretApiKey | bdd_StripeTestSecretApiKey_test |
	| StripeAccount_StripeTestPublicApiKey | bdd_StripeTestPublicApiKey_test |
	| StripeAccount_StripeLiveSecretApiKey | bdd_StripeLiveSecretApiKey_test |
	| StripeAccount_StripeLivePublicApiKey | bdd_StripeLivePublicApiKey_test |
	| test | true |
	And I click on save
	Then The details are updated

@Admin
@AdminSettings
Scenario: Stripe change account credentials in live
	Given I am logged in to the dashboard
	And I am in settings section
	When I fill in the details for stripe
	| field | value |
	| StripeAccount_StripeTestSecretApiKey | bdd_StripeTestSecretApiKey_live |	
	| StripeAccount_StripeTestPublicApiKey | bdd_StripeTestPublicApiKey_live |
	| StripeAccount_StripeLiveSecretApiKey | bdd_StripeLiveSecretApiKey_live |
	| StripeAccount_StripeLivePublicApiKey | bdd_StripeLivePublicApiKey_live |
	| live | true |
	And I click on save
	Then The details are updated
