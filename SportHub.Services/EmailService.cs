using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;
using System;
using System.Globalization;
using System.IO;

namespace SportHub.Services
{
    public class EmailService : IEmailService
    {
        public async void SendSignUpEmail(User user, [FromServices] IFluentEmail mailer)
        {
            var emailBody = File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/signupEmail.html")
                .Replace("{DateRegistered}", DateTime.Now.ToString("MMMM dd, yyyy"))
                .Replace("{loginpage}", "https://localhost:7128/login");

            var email = mailer
                .To(user.Email, user.FirstName)
                .Subject("Signup verification")
                .UsingTemplate(emailBody, new { });
            //I have problem with gmail
            await email.SendAsync();
        }
        public async void SendResetPasswordEmail(User user, [FromServices] IFluentEmail mailer, string token)
        {
            var emailBody = File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/resetPasswordEmail.html")
                .Replace("{resetPasswordPage}", $"https://localhost:7128/ResetPassword?token={token}");

            var email = mailer
                .To(user.Email, user.FirstName)
                .Subject("Reset Password")
                .UsingTemplate(emailBody, new { });
      
            await email.SendAsync();
        }
    }
}
