using SportHub.Domain.Models;
using System.Threading.Tasks;

namespace SportHub.Services
{
    public interface IEmailService
    {
        Task SendSignUpEmail(User user);
        Task SendResetPasswordEmail(User user, string token);
    }
}