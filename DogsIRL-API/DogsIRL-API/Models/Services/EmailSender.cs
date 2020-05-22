using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DogsIRL_API.Models.Services
{
    public class EmailSender : IEmailSender
    {

        private IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Composes and sends the html message to the given email with the given subject via Sendgrid
        /// </summary>
        /// <param name="email">The email address to send the message to</param>
        /// <param name="subject">The subject to put on the email</param>
        /// <param name="htmlMessage">The message to put in the email, with html formatting</param>
        /// <returns>The completed task</returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridClient client = new SendGridClient(_configuration["SendGrid-Key"]);
            SendGridMessage msg = new SendGridMessage();

            msg.SetFrom("admin@DogsIRL.com", "Teddy & Carrington & Andrew");
            msg.AddTo(email);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, htmlMessage);

            await client.SendEmailAsync(msg);
        }
    }
}
