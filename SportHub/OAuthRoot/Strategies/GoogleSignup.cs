using Google.Apis.Auth;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class GoogleSignup : IExternalAuthHandler
    {
        public async Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner)
        {
            var validatedToken = await GoogleJsonWebSignature.ValidateAsync(externalAuthArgs.UserToken);

            if (validatedToken != null)
            {
                var email = validatedToken.Email;
                var firstname = validatedToken.GivenName;
                var lastname = validatedToken.FamilyName;

                var createdUser = _userService.CreateUser(email, null, firstname, lastname, true);
                var authToken = _jwtSigner.FetchToken(createdUser);

                return authToken;
            }

            return null;
        }
    }
}
