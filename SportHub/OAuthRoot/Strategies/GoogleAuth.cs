using Google.Apis.Auth;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.OAuthRoot.Strategies
{
    public class GoogleAuth : IExternalAuthHandler
    {
        public async Task<AuthTokenResponse?> HandleExternalAuth(ExternalAuthArgs externalAuthArgs, IUserService _userService, ITokenService _tokenService, 
            IJwtSigner _jwtSigner, IEmailService _emailService)
        {
            if (externalAuthArgs.IsCreationRequired)
            {
                var validatedToken = await GoogleJsonWebSignature.ValidateAsync(externalAuthArgs.UserToken);

                if (validatedToken != null)
                {
                    var email = validatedToken.Email;
                    var firstname = validatedToken.GivenName;
                    var lastname = validatedToken.FamilyName;

                    await _emailService.SendSignUpEmail(email);

                    var createdUser = await _userService.CreateUser(email, null, firstname, lastname, ExternalAuthProvidersEnum.Google.ToString(), true);
                    var accessToken = _jwtSigner.FetchToken(createdUser);
                    var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, createdUser.Id);

                    return new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token };
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
                        var accessToken = _jwtSigner.FetchToken(existingUser);
                        var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, existingUser.Id);

                        return new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token };
                    }       
                }

                return null;
            }
        }
    }
}
