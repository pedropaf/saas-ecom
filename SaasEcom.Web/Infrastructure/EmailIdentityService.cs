using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace SaasEcom.Web.Infrastructure
{
    public class EmailIdentityService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Email service here to send an email.
            // Create the mail message
            var mailMessage = new MailMessage(
                "myapp@myapp.com",
                message.Destination,
                message.Subject,
                message.Body
                );

            // Send the message
            var client = new SmtpClient();
            client.SendAsync(mailMessage, null);

            return Task.FromResult(true);
        }
    }
}
