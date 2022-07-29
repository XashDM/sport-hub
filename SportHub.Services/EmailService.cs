using SportHub.Domain.Models;
using System;
using System.Globalization;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using SportHub.Services.Config;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using SportHub.Services.Interfaces;

namespace SportHub.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfig _emailServiceConfig;

        public EmailService(EmailServiceConfig emailServiceConfig)
        {
            _emailServiceConfig = emailServiceConfig;
        }

        public async Task SendSignUpEmail(string userEmailAddress)
        {
            var loginPage = ConfigurationManager.AppSettings.Get("loginPage");
            var loginEmailPath = ConfigurationManager.AppSettings.Get("loginEmailPath");

            var emailBody = File.ReadAllText($"{Directory.GetCurrentDirectory()}{loginEmailPath}")
                .Replace("{DateRegistered}", DateTime.Now.ToString("MMMM dd, yyyy"))
                .Replace("{loginpage}", loginPage);

            var email = GenerateEmail(userEmailAddress, "Signup verification", emailBody);

            await SendEmail(email);
        }
        public async Task SendResetPasswordEmail(string userEmailAddress, string token)
        {
            var resetPasswordPage = ConfigurationManager.AppSettings.Get("resetPasswordPage");
            var resetPasswordEmailPath = ConfigurationManager.AppSettings.Get("resetPasswordEmailPath");

            var emailBody = File.ReadAllText($"{Directory.GetCurrentDirectory()}{resetPasswordEmailPath}")
                .Replace("{resetPasswordPage}", $"{resetPasswordPage}{token}");

            var email = GenerateEmail(userEmailAddress, "Password reset", emailBody);

            await SendEmail(email);
        }

        private MimeMessage GenerateEmail(string userEmailAddress, string subject, string emailBody)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailServiceConfig.DisplayName, _emailServiceConfig.From));
            email.To.Add(MailboxAddress.Parse(userEmailAddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            return email;
        }

        private async Task SendEmail(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_emailServiceConfig.SmtpServer, _emailServiceConfig.Port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_emailServiceConfig.UserName, _emailServiceConfig.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
