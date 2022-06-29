using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using SportHub.Services.Exceptions.UserServiceExceptions;
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

        public UsersController(IUserService userService, IJwtSigner jwtSigner)
        {
            _userService = userService;
            _jwtSigner = jwtSigner;
        }

        [HttpPost(nameof(HandleExternalAuth))]
        [AllowAnonymous]
        public async Task<IActionResult> HandleExternalAuth(ExternalAuthArgs externalAuthArgs)
        {
            try
            {
                var validatedToken = await GoogleJsonWebSignature.ValidateAsync(externalAuthArgs.UserToken);

                if (validatedToken != null)
                {
                    var firstname = validatedToken.GivenName;
                    var lastname = validatedToken.FamilyName;
                    var email = validatedToken.Email;

                    if (externalAuthArgs.IsCreationRequired == true)
                    {
                        var createdUser = _userService.CreateUser(email, null, firstname, lastname, true);
                        var authToken = _jwtSigner.FetchToken(createdUser);

                        return Ok(authToken);
                    }
                    else
                    {
                        var existingUser = _userService.GetUserByEmail(email);
                        var authToken = _jwtSigner.FetchToken(existingUser);

                        return Ok(authToken);
                    }
                }

                return BadRequest();
            }
            catch (UserServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
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