using SportHub.Domain.Models;

namespace SportHub.Services
{
    public interface IEmailService
    {
        void SendSignUpEmail(User user);
    }
}