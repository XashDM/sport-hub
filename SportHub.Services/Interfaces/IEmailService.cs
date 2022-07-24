using SportHub.Domain.Models;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSignUpEmail(User user);
        Task SendResetPasswordEmail(User user, string token);
    }
}