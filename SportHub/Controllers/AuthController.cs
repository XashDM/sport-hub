using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IJwtSigner _jwtSigner;
        private readonly IUserService _userService;

        public AuthController(ITokenService tokenService, IJwtSigner jwtSigner, IUserService userService)
        {
            _tokenService = tokenService;
            _jwtSigner = jwtSigner;
            _userService = userService;
        }


        [HttpPost(nameof(RefreshToken))]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var userEmail = await _tokenService.ValidateTokenPair(_jwtSigner.GetTokenValidationParameters(), 
                tokenRequest.AccessToken, tokenRequest.RefreshToken);
            if (userEmail != null)
            {
                var user = _userService.GetUserByEmail(userEmail);
                var accessToken = _jwtSigner.FetchToken(user);
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, user.Id);
                var response = new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token };

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
