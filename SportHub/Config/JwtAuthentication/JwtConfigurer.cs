using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;

namespace SportHub.Config.JwtAuthentication
{
    public class JwtConfigurer : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IJwtSigner _jwtSigner;

        public JwtConfigurer(IJwtSigner jwtSigner)
        {
            _jwtSigner = jwtSigner;
        }

        public void Configure(string token, JwtBearerOptions options)
        {
            options.IncludeErrorDetails = true;
            options.SaveToken = true;
            options.TokenValidationParameters = _jwtSigner.GetTokenValidationParameters();
        }

        public void Configure(JwtBearerOptions options) { throw new NotImplementedException(); }
    }
}

