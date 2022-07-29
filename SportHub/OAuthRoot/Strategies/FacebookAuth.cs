using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services;
using SportHub.Services.Exceptions.ExternalAuthExceptions;
using SportHub.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class FacebookAuth : IExternalAuthHandler
    {
        public async Task<AuthTokenResponse?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, ITokenService _tokenService, 
            IJwtSigner _jwtSigner, IEmailService _emailService)
        {
            if (externalAuthArgs.IsCreationRequired)
            {
                if (externalAuthArgs.Email == null || externalAuthArgs.FirstName == null || externalAuthArgs.LastName == null)
                {
                    throw new InvalidAuthDataException();
                }

                await _emailService.SendSignUpEmail(externalAuthArgs.Email);

                var createdUser = await _userService.CreateUser(externalAuthArgs.Email, null, externalAuthArgs.FirstName, externalAuthArgs.LastName,                                      ExternalAuthProvidersEnum.Facebook.ToString(), true);
                var accessToken = _jwtSigner.FetchToken(createdUser);
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, createdUser.Id);

                return new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token };
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
                    var accessToken = _jwtSigner.FetchToken(existingUser);
                    var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, existingUser.Id);

                    return new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token };
                }

                return null;
            }
        }
    }
}
