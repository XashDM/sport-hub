using Microsoft.IdentityModel.Tokens;
using SportHub.Domain.Models;
using SportHub.Models;

namespace SportHub.Config.JwtAuthentication
{
    public interface IJwtSigner
    {
        RsaSecurityKey FetchPKey();
        AccessToken FetchToken(User user);
        TokenValidationParameters GetTokenValidationParameters();
    }
}