using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.RefreshTokenHandlerRoot;
using SportHub.Services.Exceptions.RootExceptions;
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
        private readonly IRefreshTokenHandler _refreshTokenHandler;
        private readonly IUserService _userService;
        private readonly IJwtSigner _jwtSigner;
        private readonly ITokenService _tokenService;

        public AuthController(IRefreshTokenHandler refreshTokenHandler, IUserService userService, IJwtSigner jwtSigner, ITokenService tokenService)
        {
            _refreshTokenHandler = refreshTokenHandler;
            _userService = userService;
            _jwtSigner = jwtSigner;
            _tokenService = tokenService;
        }

        [HttpPost(nameof(RefreshToken))]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                var authTokenPair = await _refreshTokenHandler.GetAuthTokenPair(tokenRequest, _userService, _jwtSigner, _tokenService);

                return Ok(authTokenPair);
            }
            catch (TokenServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (UserServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
