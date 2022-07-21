using Google.Apis.Auth;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using System;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class GoogleAuth : IExternalAuthHandler
    {
        public async Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner)
        {
            if (externalAuthArgs.IsCreationRequired)
            {
                var validatedToken = await GoogleJsonWebSignature.ValidateAsync(externalAuthArgs.UserToken);

                if (validatedToken != null)
                {
                    var email = validatedToken.Email;
                    var firstname = validatedToken.GivenName;
                    var lastname = validatedToken.FamilyName;

                    var createdUser = await _userService.CreateUser(email, null, firstname, lastname, ExternalAuthProvidersEnum.Google.ToString(), true);
                    var authToken = _jwtSigner.FetchToken(createdUser);

                    return authToken;
                }

                return null;
            }
            else
            {                
                var validatedToken = await GoogleJsonWebSignature.ValidateAsync(externalAuthArgs.UserToken);

                if (validatedToken != null)
                {
                    var email = validatedToken.Email;

                    var existingUser = _userService.GetUserByEmail(email);

                    if (existingUser.AuthProvider.Equals(ExternalAuthProvidersEnum.Google.ToString()))
                    {
                        var authToken = _jwtSigner.FetchToken(existingUser);

                        return authToken;
                    }       
                }

                return null;
            }
        }
    }
}
