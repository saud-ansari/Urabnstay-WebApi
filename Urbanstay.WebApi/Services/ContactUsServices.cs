using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;
using Urbanstay.WebApi.ViewModels;

namespace Urbanstay.WebApi.Services
{
    public class ContactUsServices
    {
        private readonly IConfiguration _configuration;

        public ContactUsServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ContactUs(ContactModel contactModel)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(contactModel.Name, contactModel.Email));
            email.To.Add(new MailboxAddress(_configuration["emailsettings:ToName"], _configuration["emailsettings:Toemail"]));
            email.Subject = _configuration["emailsettings:Subject"];

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <h2>Contact Us</h2>
                <p><strong>Name:</strong> {contactModel.Name}</p>
                <p><strong>Email:</strong> {contactModel.Email}</p>
                <p><strong>Message:</strong></p>
                <p>{contactModel.Message}</p>"
            };

            email.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["emailsettings:SMTPServer"], int.Parse(_configuration["emailsettings:SMTPPort"]), false);
                await client.AuthenticateAsync(_configuration["emailsettings:SMTPUsername"], _configuration["emailsettings:SMTPPassword"]);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
    }

}
