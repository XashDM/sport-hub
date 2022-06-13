using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;
using System;
using System.Globalization;
using System.IO;
using System.Configuration;

namespace SportHub.Services
{
    public class EmailService : IEmailService
    {
        public async void SendSignUpEmail(User user, [FromServices] IFluentEmail mailer)
        {
            var loginPage = ConfigurationManager.AppSettings.Get("loginPage");
            var loginEmailPath = ConfigurationManager.AppSettings.Get("loginEmailPath");

            var emailBody = File.ReadAllText($"{Directory.GetCurrentDirectory()}{loginEmailPath}")
                .Replace("{DateRegistered}", DateTime.Now.ToString("MMMM dd, yyyy"))
                .Replace("{loginpage}", loginPage);

            var email = mailer
                .To(user.Email, user.FirstName)
                .Subject("Signup verification")
                .UsingTemplate(emailBody, new { });
            //I have problem with gmail
            await email.SendAsync();
        }
        public async void SendResetPasswordEmail(User user, [FromServices] IFluentEmail mailer, string token)
        {
            var resetPasswordPage = ConfigurationManager.AppSettings.Get("resetPasswordPage");
            var resetPasswordEmailPath = ConfigurationManager.AppSettings.Get("resetPasswordEmailPath");

            var emailBody = File.ReadAllText($"{Directory.GetCurrentDirectory()}{resetPasswordEmailPath}")
                .Replace("{resetPasswordPage}", $"{resetPasswordPage}{token}");

            var email = mailer
                .To(user.Email, user.FirstName)
                .Subject("Reset Password")
                .UsingTemplate(emailBody, new { });
      
            await email.SendAsync();
        }
    }
}
