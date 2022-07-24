using SportHub.Domain.Models;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSignUpEmail(string userEmailAddress);
        Task SendResetPasswordEmail(string userEmailAddress, string token);
    }
}