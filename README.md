SAAS Ecom
=========

Robust subscription support for ASP.NET MVC 5 apps using [Stripe](https://stripe.com), including payment pages and subscription management for your customers. Also makes it easy to manage logic related to new subscriptions, cancellations, payment failures, and streamlines hooking up notifications, etc. [**Available on NuGet**](https://www.nuget.org/packages/SaasEcom.FrontEnd)

* [More information about the project in this website](http://www.saasecom.org).
* Detailed tutorial on [Building a Note-Taking Software-as-a-Service Using ASP.NET MVC 5, Stripe, and Azure] (http://code.tutsplus.com/tutorials/building-a-note-taking-software-as-a-service-using-aspnet-mvc-5-stripe-and-azure--cms-22922)

[![Build status](https://ci.appveyor.com/api/projects/status/qt875ktp3lsv89pg?svg=true)](https://ci.appveyor.com/project/pedropaf/saas-ecom)

See an example of [SAAS Ecom Demo](http://saas-ecom.azurewebsites.net/). It's connected to Stripe test gateway, so you can add one of their [test credit cards](https://stripe.com/docs/testing).

* **Test Card Number:** 4242 4242 4242 4242 
* **Expiry date:** Any future date.
* **CVC:** 3 digits

## Features

*  **Integrated with Stripe:** Use stripe as your gateway. This billing application is nicely integrated.
*  **Trials supported:** You have the option to let your customers to try your application first. Don't even ask them for a credit card to register.
*  **Invoicing:** The invoices are created automatically, and Stripe will try to get them paid.
*  **Billing view helpers:** View helpers for your application, to let your customers manage their subscription / password / credit card.
*  **Only SSL needed:** Your customers' credit card number don't hit your server (stripe.js). You only need SSL to deploy this app.

## Is it easy to integrate SAAS Ecom in my application?
SAAS Ecom uses the most popular technologies in the .NET world. The main requirements for your application is that you need to use ASP.NET Identity 2.1 for membership, and also Entity Framework 6 Code First. Although, if you’re not using EF Code First currently, you can use a second database for membership and SAAS Ecom, and another one for your application data.

## NuGet

This project is available in NuGet, the recommended way to install it is:

Create a new MVC 5 project that uses Individual accounts. Then install this NuGet Package:

    PM> Install-Package SaasEcom.FrontEnd

After installing the package, edit the file "IdentityModels.cs"

    public class ApplicationUser : SaasEcomUser
    {
        // default code ...
    }

    public class ApplicationDbContext : SaasEcomDbContext<ApplicationUser>
    {
        // default code ...
    }

The SaasEcomUser class defines non-nullable RegistrationDate and LastLoginDate properties. Anyone installing saas-ecom into their project will need to update their Registration action to define the user as below:

    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, RegistrationDate = DateTime.UtcNow, LastLoginTime = DateTime.UtcNow  };    

Now the solution should compile successfully, but you should complete the following additional steps.

### Additional Steps

1. Register in Stripe.com, and get your API Keys.
2. Add your API Keys to Web.config.
3. Configure [Stripe Webhooks](https://manage.stripe.com/account/webhooks), the URL will be something like: http://yourdomain.com/StripeWebhooks
4. Enable Entity Framework Migrations, add the first migration and update your database.
5. Integrate your SAAS with the provided view helpers:
    - Create a Subscription (free or paid) on user register.
    - Integrate SAAS View Helpers in the account management section for your customers.


## Frequently Asked Questions

### I have a freemium SAAS, can I still use SAAS Ecom?
Yes, in this case the recommended approach would be to not create the users in Stripe until a user is a paying customer. You should create a Subscription plan in the database, and use "free" as PlanId, that way you can differentiate the users in a free plan from the users in a paid plan.

### I am not using Entity Framework Code First, can I use SAAS Ecom?
Yes, you can create a second database using Entity Framework Code first for billing. This is not an ideal solution and hopefully this dependency will become optional in a future release.

### I want to give my customers a Free trial, but I need to collect their credit card on sign up, how can I do it?
This view helper is not provided, but you just need to add the credit card collection details to your register form and submit that to stripe using their js script. Similar to the upgrade form.

### Do you support Paypal or Braintree?
Not yet, it might come on a future release but nothing planned so far.

### Can I integrate SAAS Ecom with my invoicing application?
Yes, you can do that integration on the controller handling the WebHooks from Stripe.

### Is SAAS Ecom being used by any Real Project?
Yes at the moment SAAS Ecom is used by:

- [Photonube](https://www.photonube.com): Fremium SAAS for professional photographers.
- [Fluxifi](https://www.fluxifi.com): Twitter analytics SAAS
- If you're using SAAS in any project, let me know to add you here.

## Contribute

I'm open to suggestions and pull requests. 

Get in touch if you'd like to use this project and need help, I also do **contract work**.