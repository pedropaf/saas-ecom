using System;
using NUnit.Core;
using NUnit.Framework;
using SaasEcom.Specs.Base;
using TechTalk.SpecFlow;

namespace SaasEcom.Specs.Steps.Billing
{
    [Binding]
    public class CustomersSteps
    {
        [Given(@"I am in the customers section of the Admin panel")]
        public void GivenIAmInTheCustomersSectionOfTheAdminPanel()
        {
            WebBrowser.Current.Navigate().GoToUrl(StepsHelpers.BaseUrl + "Billing/Customers");
        }

        [When(@"There are no customers registered")]
        public void WhenThereAreNoCustomersRegistered()
        {
            Assert.True(false);
        }

        [Then(@"I can see an empty list of customers \(placeholder\)")]
        public void ThenICanSeeAnEmptyListOfCustomersPlaceholder()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
