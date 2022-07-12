using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.OAuthRoot;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtSigner _jwtSigner;
        private readonly IExternalAuthHandlerFactory _externalAuthHandlerFactory;

        public UsersController(IUserService userService, IJwtSigner jwtSigner, IExternalAuthHandlerFactory externalAuthHandlerFactory)
        {
            _userService = userService;
            _jwtSigner = jwtSigner;
            _externalAuthHandlerFactory = externalAuthHandlerFactory;
        }

        [HttpPost(nameof(HandleExternalAuth))]
        [AllowAnonymous]
        public async Task<IActionResult> HandleExternalAuth(ExternalAuthArgs externalAuthArgs)
        {
            try
            {
                var externalAuthHandler = _externalAuthHandlerFactory.GetAuthHandler(externalAuthArgs.IsCreationRequired, externalAuthArgs.AuthProvider);
                var authToken = await externalAuthHandler.HandleExternalAuth(externalAuthArgs, _userService, _jwtSigner);

                if (authToken != null)
                {
                    return Ok(authToken);
                }

                return StatusCode(400, "Cannot authenticate user");
            }
            catch (UserServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Not supplied with requested data");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid Auth provider");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost(nameof(ResetPassword))]
        [Authorize]
        public IActionResult ResetPassword([FromQuery] string passwordHash)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = "";
            
            if (identity != null)
            {
                email = identity.Claims.ElementAt(0).Value;
            }
            
            var user = _userService.ChangePassword(email, passwordHash);

            return Ok();
        }
    }
}