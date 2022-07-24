using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.OAuthRoot;
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
        private readonly IExternalAuthHandlerFactory _externalAuthHandlerFactory;
        private readonly IEmailService _emailService;

        public AuthController(IRefreshTokenHandler refreshTokenHandler, IUserService userService, IJwtSigner jwtSigner, 
            ITokenService tokenService, IExternalAuthHandlerFactory externalAuthHandlerFactory, IEmailService emailService)
        {
            _refreshTokenHandler = refreshTokenHandler;
            _userService = userService;
            _jwtSigner = jwtSigner;
            _tokenService = tokenService;
            _externalAuthHandlerFactory = externalAuthHandlerFactory;
            _emailService = emailService;
        }

        [HttpPost(nameof(RefreshToken))]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] AccessTokenJwt accessToken)
        {
            try
            {
                var refreshToken = Request.Cookies["RefreshToken"];
                if (refreshToken != null)
                {
                    var tokenRequest = new TokenRequest() { AccessToken = accessToken.Token, RefreshToken = refreshToken };
                    var authTokenPair = await _refreshTokenHandler.GetAuthTokenPair(tokenRequest, _userService, _jwtSigner, _tokenService);

                    Response.Cookies.Append("RefreshToken", authTokenPair.RefreshToken, new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });

                    return Ok(authTokenPair.AccessToken);
                }

                return BadRequest("Refresh token cookie was not found");
            }
            catch (TokenServiceException e)
            {
                return StatusCode(e.StatusCode, e.Args);
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

        [HttpPost(nameof(HandleExternalAuth))]
        [AllowAnonymous]
        public async Task<IActionResult> HandleExternalAuth(ExternalAuthArgs externalAuthArgs)
        {
            try
            {
                var temp = Request.Cookies;
                var externalAuthHandler = _externalAuthHandlerFactory.GetAuthHandler(externalAuthArgs.IsCreationRequired, externalAuthArgs.AuthProvider);
                var userTokens = await externalAuthHandler.HandleExternalAuth(externalAuthArgs, _userService, _tokenService, _jwtSigner, _emailService);

                if (userTokens != null)
                {
                    Response.Cookies.Append("RefreshToken", userTokens.RefreshToken, new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });

                    return Ok(userTokens.AccessToken);
                }

                return StatusCode(400, "Cannot authenticate user");
            }
            catch (UserServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (ExternalAuthException e)
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
