using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportHub.Services
{
    public class TokenService : ITokenService
    {
        private readonly SportHubDBContext _context;

        public TokenService(SportHubDBContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(string jwtTokenId, int userId)
        {
            var refreshToken = new RefreshToken()
            {
                JwtTokenId = jwtTokenId,
                UserId = userId,
                IsRevoked = false,
                IsUsed = false,
                IssuedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Token = GenerateRefreshTokenString()
            };

            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenValueAndAccessTokenId(string refreshTokenValue, string accessTokenId)
        {
            var refreshToken = await _context.RefreshTokens
                .AsNoTracking()
                .Where(token => token.Token.Equals(refreshTokenValue) && token.JwtTokenId.Equals(accessTokenId))
                .SingleOrDefaultAsync();

            return refreshToken;
        }

        public async Task<bool> RevokeAllUserTokensByUserId(int userId)
        {
            var tokensToRevoke = await _context.RefreshTokens
                .Where(token => token.UserId.Equals(userId))
                .ToArrayAsync();

            foreach (var token in tokensToRevoke)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateRefreshTokenById(int tokenId, bool isUsed = true, bool isRevoked = false)
        {
            var token = await _context.RefreshTokens
                .Where(token => token.Id.Equals(tokenId))
                .SingleOrDefaultAsync();

            token.IsUsed = isUsed;
            token.IsRevoked = isRevoked;

            await _context.SaveChangesAsync();
        }

        public async Task<string?> ValidateTokenPair(TokenValidationParameters validationParameters, string accessToken, string refreshToken)
        {
           
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            validationParameters.ValidateLifetime = false;
            var tokenInValidation = jwtTokenHandler.ValidateToken(accessToken, validationParameters, out var validatedToken);

            if (validatedToken.ValidTo > DateTime.UtcNow)
            {
                return null;
            }

            var accessTokenId = tokenInValidation.Claims.SingleOrDefault(claim => claim.Type.Equals(ClaimTypes.Sid)).Value;
            var storedRefreshToken = await GetRefreshTokenByTokenValueAndAccessTokenId(refreshToken, accessTokenId);

            if (storedRefreshToken == null)
            {
                return null;
            }

            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return null; // relogin
            }

            if (storedRefreshToken.IsRevoked)
            {
                return null; // relogin
            }

            var userId = Convert.ToInt32(tokenInValidation.Claims.SingleOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                
            if (storedRefreshToken.IsUsed)
            {
                await RevokeAllUserTokensByUserId(userId);

                return null; // Security breach
            }
            else
            {
                storedRefreshToken.IsUsed = true;
            }

            await UpdateRefreshTokenById(storedRefreshToken.Id, storedRefreshToken.IsUsed, storedRefreshToken.IsRevoked);

            var userEmail = tokenInValidation.Claims.SingleOrDefault(claim => claim.Type.Equals(ClaimTypes.Email)).Value;

            return userEmail;
        }

        private string GenerateRefreshTokenString() // Generates a 128 char long random string
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 92)
                .Select(x => x[random.Next(x.Length)]).ToArray()) + Guid.NewGuid().ToString();
        }
    }
}
