using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using SportHub.Domain.Models;

namespace SportHub.Config.JwtAuthentication
{
    public class JwtSigner : IJwtSigner
    {
        private readonly RsaSecurityKey _privateRsaKey;

        public JwtSigner()
        {
            string rsaPrivateKeyBase64 = System.IO.File.ReadAllText(@"D:\SSH-keys\private-key.txt");
            RSA rsa = RSA.Create();
            byte[] rawKey = Convert.FromBase64String(rsaPrivateKeyBase64);
            rsa.ImportRSAPrivateKey(rawKey, out _);
            _privateRsaKey = new RsaSecurityKey(rsa);
        }

        public string FetchToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "SportHub",
                Audience = "SportHub",
                Subject = new ClaimsIdentity(CreateTokenClaims(user)),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(45),
                SigningCredentials = new SigningCredentials(_privateRsaKey, SecurityAlgorithms.RsaSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RsaSecurityKey FetchPKey()
        {
            return _privateRsaKey;
        }

        private List<Claim> CreateTokenClaims(User user)
        {
            List<Claim> claimsList = new List<Claim>();
            claimsList.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var userRole in user.Roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, userRole.RoleName));
            }

            return claimsList;
        }
    }
}
