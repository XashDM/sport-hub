using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class FacebookLogin : IExternalAuthHandler
    {
        public async Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner)
        {
            var existingUser = _userService.GetUserByEmail(externalAuthArgs.Email);
            var authToken = _jwtSigner.FetchToken(existingUser);

            return authToken;
        }
    }
}
