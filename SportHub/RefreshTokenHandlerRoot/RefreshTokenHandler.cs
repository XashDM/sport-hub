using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services.Exceptions.TokenServiceExceptions;
using SportHub.Services.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportHub.RefreshTokenHandlerRoot
{
    public class RefreshTokenHandler : IRefreshTokenHandler
    {
        public async Task<AuthTokenResponse> GetAuthTokenPair(TokenRequest tokenRequest, IUserService _userService, IJwtSigner _jwtSigner, ITokenService _tokenService)
        {
            var validAccessTokenPrincipal = await _tokenService.ValidateTokenPair(_jwtSigner.GetTokenValidationParameters(),
                tokenRequest.AccessToken, tokenRequest.RefreshToken);

            var userEmailClaim = validAccessTokenPrincipal.Claims.SingleOrDefault(claim => claim.Type.Equals(ClaimTypes.Email));

            if (userEmailClaim == null)
            {
                throw new TokenClaimNotFoundOrInvalidException();
            }

            var user = _userService.GetUserByEmail(userEmailClaim.Value);
            var accessToken = _jwtSigner.FetchToken(user);
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, user.Id);

            return new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token };
        }
    }
}
