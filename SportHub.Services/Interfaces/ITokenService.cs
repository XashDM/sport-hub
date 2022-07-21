using Microsoft.IdentityModel.Tokens;
using SportHub.Domain.Models;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface ITokenService
    {
        Task<RefreshToken> CreateRefreshTokenAsync(string jwtTokenId, int userId);
        Task<RefreshToken?> GetRefreshTokenByTokenValueAndAccessTokenId(string refreshTokenValue, string accessTokenId);
        Task<bool> RevokeAllUserTokensByUserId(int userId);
        Task UpdateRefreshTokenById(int tokenId, bool isUsed = true, bool isRevoked = false);
        Task<string?> ValidateTokenPair(TokenValidationParameters validationParameters, string accessToken, string refreshToken);
    }
}