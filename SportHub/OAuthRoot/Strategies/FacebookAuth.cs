using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.ExternalAuthExceptions;
using System;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class FacebookAuth : IExternalAuthHandler
    {
        public async Task<string?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, IJwtSigner _jwtSigner)
        {
            if (externalAuthArgs.IsCreationRequired)
            {
                if (externalAuthArgs.Email == null || externalAuthArgs.FirstName == null || externalAuthArgs.LastName == null)
                {
                    throw new InvalidAuthDataException();
                }

                var createdUser = _userService.CreateUser(externalAuthArgs.Email, null, externalAuthArgs.FirstName, externalAuthArgs.LastName, 
                                                            ExternalAuthProvidersEnum.Facebook.ToString(), true);
                var authToken = _jwtSigner.FetchToken(createdUser);

                return authToken;
            }
            else
            {
                if (externalAuthArgs.Email == null)
                {
                    throw new InvalidAuthDataException();
                }

                var existingUser = _userService.GetUserByEmail(externalAuthArgs.Email);

                if (existingUser.AuthProvider.Equals(ExternalAuthProvidersEnum.Facebook.ToString()))
                {
                    var authToken = _jwtSigner.FetchToken(existingUser);

                    return authToken;
                }

                return null;
            }
        }
    }
}
