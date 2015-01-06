SAAS Ecommerce
==============

Robust subscription support for ASP.NET MVC 5 apps using [Stripe](https://stripe.com), including out-of-the-box pricing pages, payment pages, and  subscription management for your customers. Also makes it easy to manage logic related to new subscriptions, cancellations, payment failures, and streamlines hooking up notifications, etc. [**Available on NuGet**](https://www.nuget.org/packages/SaasEcom.FrontEnd)

You can see [more information about the project in this blog post](http://www.pedroalonso.net/blog/2014/04/28/saas-ecom-open-source-for-net-mvc-5-stripe/)

[![Build status](https://ci.appveyor.com/api/projects/status/qt875ktp3lsv89pg?svg=true)](https://ci.appveyor.com/project/pedropaf/saas-ecom)

![Web](http://www.pedroalonso.net/images/posts/2014/04/01-SAAS-Ecom.png)

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

## NuGet

This project is available in NuGet, the recommended way to install it is:

Create a new MVC 5 project that uses Individual accounts. Then install this NuGet Package:

    PM> Install-Package SaasEcom.FrontEnd -Pre

After installing the package, edit the file "IdentityModels.cs"

    public class ApplicationUser : SaasEcomUser
    {
        // default code ...
    }

    public class ApplicationDbContext : SaasEcomDbContext<ApplicationUser>
    {
        // default code ...
    }

Now the solution should compile successfully, but you should complete the following additional steps.

### Additional Steps

1. Register in Stripe.com, and get your API Keys.
2. Add your API Keys to Web.config.
3. Configure [Stripe Webhooks](https://manage.stripe.com/account/webhooks), the URL will be something like: http://yourdomain.com/StripeWebhooks
4. Enable Entity Framework Migrations, add the first migration and update your database.

## Contribute

I'm open to suggestions and pull requests. 

Get in touch if you'd like to use this project and need help, I also do **contract work**.