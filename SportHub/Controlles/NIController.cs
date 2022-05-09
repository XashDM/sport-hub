using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;
namespace SportHub.Controlles
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationItems : ControllerBase
    {

        private readonly ILogger<NavigationItems> _logger;
        private readonly Domain.SportHubDBContext _context;
        private INavigationItemService _servise;
        public NavigationItems(ILogger<NavigationItems> logger, Domain.SportHubDBContext context, INavigationItemService servise)
        {
            _logger = logger;
            _context = context;
            _servise = servise;
        }
        [HttpPost]
        public async Task<IActionResult> SaveItem([FromBody] dynamic data)
        {
            return new ObjectResult("dfdf");
        }
    }
}
