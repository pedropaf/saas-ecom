SAAS Ecommerce
==============

Robust subscription support for ASP.NET MVC 5 apps using [Stripe](https://stripe.com), including out-of-the-box pricing pages, payment pages, and  subscription management for your customers. Also makes it easy to manage logic related to new subscriptions, cancellations, payment failures, and streamlines hooking up notifications, etc.

See an example of [SAAS Ecom Demo](http://saas-ecom.azurewebsites.net/). It's connected to Stripe test gateway, so you can add one of their test credit cards.

* **Test Card Number:** 4242 4242 4242 4242 
* **Expiry date:** Any valid
* **CVC:** 3 digits

## Features

*  **Integrated with Stripe:** Use stripe as your gateway. This billing application is nicely integrated.
*  **Trials supported:** You have the option to let your customers to try your application first. Don't even ask them for a credit card to register.
*  **Invoicing:** The invoices are created automatically, and Stripe will try to get them paid.
*  **Customers dashboard:** Dashboard area for your customers, where they can manage their subscription / password / credit card.
*  **Only SSL needed:** Your customers' credit card number don't hit your server (stripe.js). You only need SSL to deploy this app.

## Installation

1. Register in Stripe.com, and get your API Keys.
2. Add your API Keys to Web.config.
3. Add your email server details to Web.config. Recommended to use [Mandrill](http://www.mandrill.com) / [Sendgrid](http://www.sendgrid.com).
4. Configure [Stripe Webhooks](https://manage.stripe.com/account/webhooks), the URL will be something like: http://yourdomain.com/StripeWebhooks
5. Create your database, and add the connection string to Web.config. By default is using SQL Compact.

**Contributions and feedback is very welcome.**

## Roadmap

For future releases these is a list of features that it would be nice to support:

* Paypal subscriptions
* Add unit tests.
* Support multiple subscriptions per user.
* Cancel subscriptions individually.
* Upgrade / downgrade subscription.
* Social Logins: Register using your facebook / twitter / google account?