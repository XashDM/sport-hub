using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;
using System;
using System.Globalization;
using System.IO;
using SportHub.Services;
using SportHub.Services.Interfaces;
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
    }
}
