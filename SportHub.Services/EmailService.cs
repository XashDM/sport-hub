using FluentEmail.Core;
using FluentEmail.Smtp;
using SportHub.Domain.Models;
using System;
using System.Net;
using System.Net.Mail;

namespace SportHub.Services
{
    public class EmailService : IEmailService
    {

        public async void SendSignUpEmail(User user)
        {
            if (user == null)
            {
                throw new Exception("User was not provided.");
            }

            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential("nickyr.beast@gmail.com", "***"),
                EnableSsl = true
            });

            Email.DefaultSender = sender;

            var email = Email
                .From("nickyr.beast@gmail.com", "SportHub SignUp")
                .To(user.Email, user.FirstName)
                .Subject("SignUp verification")
                .Body("Just testing out");

            await email.SendAsync();
        }
    }
}
