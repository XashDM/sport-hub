using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;

namespace SportHub.Services.Interfaces
{
    public interface IEmailService
    {
        void SendSignUpEmail(User user, [FromServices] IFluentEmail mailer);
        void SendResetPasswordEmail(User user, [FromServices] IFluentEmail mailer, string token);
    }
}