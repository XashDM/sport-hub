using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using SportHub.Domain.Models;
using SportHub.Models;

namespace SportHub.Config.JwtAuthentication
{
    public class JwtSigner : IJwtSigner
    {
        private readonly RsaSecurityKey _privateRsaKey;

        public JwtSigner()
        {
            string rsaPrivateKeyBase64 = "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";
            RSA rsa = RSA.Create();
            byte[] rawKey = Convert.FromBase64String(rsaPrivateKeyBase64);
            rsa.ImportRSAPrivateKey(rawKey, out _);
            _privateRsaKey = new RsaSecurityKey(rsa);
        }

        public AccessToken FetchToken(User user)
        {
            var tokenGuid = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "SportHub",
                Audience = "SportHub",
                Subject = new ClaimsIdentity(CreateTokenClaims(user, tokenGuid)),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(_privateRsaKey, SecurityAlgorithms.RsaSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessToken() { Id = tokenGuid, TokenJwt = tokenHandler.WriteToken(token) };
        }
        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = FetchPKey(),
                ValidateIssuer = true,
                ValidIssuer = "SportHub",
                ValidAlgorithms = new string[] { SecurityAlgorithms.RsaSha256.ToString() },
                ValidateAudience = true,
                ValidAudience = "SportHub",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                }
            };
        }

        public RsaSecurityKey FetchPKey()
        {
            return _privateRsaKey;
        }

        private List<Claim> CreateTokenClaims(User user, string tokenId)
        {
            List<Claim> claimsList = new List<Claim>();
            claimsList.Add(new Claim(ClaimTypes.Email, user.Email));
            claimsList.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claimsList.Add(new Claim(ClaimTypes.Name, user.FirstName));
            claimsList.Add(new Claim(ClaimTypes.Sid, tokenId));
            claimsList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            foreach (var userRole in user.Roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, userRole.RoleName));
            }

            return claimsList;
        }
    }
}
