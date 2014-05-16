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

        [Given(@"I fill in the registration form")]
        public void GivenIFillInTheRegistrationForm()
        {
            ScenarioContext.Current.Pending();
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

        [Then(@"I see thank you page")]
        public void ThenISeeThankYouPage()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
