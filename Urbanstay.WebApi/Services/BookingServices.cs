using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Urbanstay.WebApi.Services
{
    public class BookingServices
    {
        private readonly IConfiguration _configuration;

        public BookingServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task BookingNotifyEmail(string fromName, string fromemail, string toName, string toemail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromName, fromemail));
            emailMessage.To.Add(new MailboxAddress(toName, toemail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EmailSettings:SMTPServer"], int.Parse(_configuration["EmailSettings:SMTPPort"]), false);
                await client.AuthenticateAsync(_configuration["EmailSettings:SMTPUsername"], _configuration["EmailSettings:SMTPPassword"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
