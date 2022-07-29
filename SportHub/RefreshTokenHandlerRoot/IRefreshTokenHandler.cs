using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.RefreshTokenHandlerRoot
{
    public interface IRefreshTokenHandler
    {
        Task<AuthTokenResponse> GetAuthTokenPair(TokenRequest tokenRequest, IUserService _userService, IJwtSigner _jwtSigner, ITokenService _tokenService);
    }
}