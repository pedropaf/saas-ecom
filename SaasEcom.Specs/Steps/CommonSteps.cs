using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SaasEcom.Specs.Base;
using TechTalk.SpecFlow;

namespace SaasEcom.Specs.Steps
{
    [Binding]
    public class CommonSteps
    {
        [Given(@"I am logged in to the admin panel")]
        public void GivenIAmLoggedInToTheAdminPanel()
        {
            WebBrowser.Current.Navigate().GoToUrl(StepsHelpers.BaseUrl + "Account/Login");
            WebBrowser.Current.SetTextForControl(By.Id("Email"), "admin@admin.com");
            WebBrowser.Current.SetTextForControl(By.Id("Password"), "password");
            WebBrowser.Current.ClickSubmitButton();
        }
    }
}
