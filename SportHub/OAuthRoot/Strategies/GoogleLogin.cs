using Google.Apis.Auth;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using System;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class GoogleLogin : IExternalAuthHandler
    {
        public async Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner)
        {
            if (externalAuthArgs.Email == null)
            {
                throw new ArgumentNullException();
            }

            var validatedToken = await GoogleJsonWebSignature.ValidateAsync(externalAuthArgs.UserToken);

            if (validatedToken != null)
            {
                var email = validatedToken.Email;

                var existingUser = _userService.GetUserByEmail(email);
                var authToken = _jwtSigner.FetchToken(existingUser);

                return authToken;
            }

            return null;
        }
    }
}
