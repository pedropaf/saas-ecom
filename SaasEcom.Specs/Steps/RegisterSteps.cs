using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SaasEcom.Specs.Base;
using TechTalk.SpecFlow;

namespace SaasEcom.Specs.Steps
{
    [Binding]
    public class RegisterSteps
    {
        private const string BaseUrl = "http://localhost:52337/";

        [Given(@"I have the homepage open")]
        public void GivenIHaveTheHomepageOpen()
        {
            WebBrowser.Current.Navigate().GoToUrl(BaseUrl);
            Assert.AreEqual("Home - SAAS Ecom", WebBrowser.Current.Title);
        }

        [When(@"I click on ""(.*)""")]
        public void WhenIClickOn(string buttonId)
        {
            WebBrowser.Current.FindElement(new ByIdOrName(buttonId)).Click();
        }

        [Then(@"I should see ""(.*)"" on the screen")]
        public void ThenIShouldSeeOnTheScreen(string p0)
        {
            Assert.AreEqual(p0, WebBrowser.Current.FindElement(By.XPath("/html/body/div[2]/div[1]/p[1]")).Text);
        }

        [Then(@"I see the registration form")]
        public void ThenISeeTheRegistrationForm()
        {
            Assert.AreEqual("Register - SAAS Ecom", WebBrowser.Current.Title);
        }

        [Given(@"I am at the registration page")]
        public void GivenIAmAtTheRegistrationPage()
        {
            WebBrowser.Current.Navigate().GoToUrl(BaseUrl + "Account/Register");
            Assert.AreEqual("Register - SAAS Ecom", WebBrowser.Current.Title);
        }

        [When(@"I fill the registration form")]
        public void WhenIFillTheRegistrationForm(Table table)
        {
            ScenarioContext.Current.Pending();

            //foreach (var row in table.Rows)
            //{
            //    var field = row["Field"];
            //    var value = row["Value"];
            //    switch (field)
            //    {
            //        case "SubscriptionPlan":  // Put exceptions here for drop downs, date pickers, radio buttons, etc.
            //            WebBrowser.Current.SelectDropDownByText(By.Id(field), value);
            //            break;
            //        default:
            //            WebBrowser.Current.SetTextForControl(By.Id(field), value);
            //            break;
            //    }
            //}

        }

        [When(@"I fill the registration form with invalid data")]
        public void WhenIFillTheRegistrationFormWithInvalidData()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I see validation errors")]
        public void ThenISeeValidationErrors()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I see thank you flash message")]
        public void ThenISeeThankYouFlashMessage()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
