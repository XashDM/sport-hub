using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot
{
    public interface IExternalAuthHandler
    {
        Task<AuthTokenResponse?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, ITokenService _tokenService, IJwtSigner _jwtSigner, IEmailService _emailService);
    }
}