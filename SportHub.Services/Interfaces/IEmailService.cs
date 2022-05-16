using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;

namespace SportHub.Services
{
    public interface IEmailService
    {
        void SendSignUpEmail(User user, [FromServices] IFluentEmail mailer);
    }
}