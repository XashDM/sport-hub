using Microsoft.IdentityModel.Tokens;
using SportHub.Domain.Models;

namespace SportHub.Config.JwtAuthentication
{
    public interface IJwtSigner
    {
        RsaSecurityKey FetchPKey();
        string FetchToken(User user);
    }
}