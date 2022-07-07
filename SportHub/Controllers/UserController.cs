using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
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

        [HttpPost(nameof(BlockUserById))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUserById([FromBody] UserIdArgs args)
        {
            var result = await _userService.BlockUserByIdAsync(args.UserId);
            if (result)
            {
               return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ActivateUserById))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUserById([FromBody] UserIdArgs args)
        {
            var result = await _userService.ActivateUserByIdAsync(args.UserId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(DeleteUserById))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserById([FromBody] UserIdArgs args)
        {
            var result = await _userService.DeleteUserByIdAsync(args.UserId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}