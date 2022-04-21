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
                .Replace("{DateRegistered}", DateTime.Now.ToString("D",
                  CultureInfo.CreateSpecificCulture("en-US")));

            var email = mailer
                .To(user.Email, user.FirstName)
                .Subject("Signup verification")
                .UsingTemplate(emailBody, new { });

            await email.SendAsync();
        }
    }
}
