using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string FromEmail = _configuration["EmailSettings:User"];
            string password = _configuration["EmailSettings:Password"];
            string smtp = _configuration["EmailSettings:SMTP"];
            int port = Int32.Parse(_configuration["EmailSettings:PORT"]);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(FromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var bodybuilder = new BodyBuilder();
            bodybuilder.HtmlBody = body;
            email.Body = bodybuilder.ToMessageBody();

            using (var smtpCl = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpCl.Connect(smtp, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                smtpCl.Authenticate(FromEmail, password);
                await smtpCl.SendAsync(email);
                smtpCl.Disconnect(true);
            }
        }
    }
}
