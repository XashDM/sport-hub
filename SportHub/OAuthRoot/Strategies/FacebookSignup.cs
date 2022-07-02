using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using System;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class FacebookSignup : IExternalAuthHandler
    {
        public async Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner)
        {
            if (externalAuthArgs.Email == null || externalAuthArgs.FirstName == null || externalAuthArgs.LastName == null)
            {
                throw new ArgumentNullException();
            }

            var createdUser = _userService.CreateUser(externalAuthArgs.Email, null, externalAuthArgs.FirstName, externalAuthArgs.LastName, true);
            var authToken = _jwtSigner.FetchToken(createdUser);

            return authToken;
        }
    }
}
