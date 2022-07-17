using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot
{
    public interface IExternalAuthHandler
    {
        Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner);
    }
}