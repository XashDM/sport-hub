using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    [ApiController]
    public class NavigationItems : Controller
    {
        private readonly ILogger<NavigationItems> _logger;
        private readonly Domain.SportHubDBContext _context;
        private INavigationItemService _servise;
        public NavigationItems( ILogger<NavigationItems> logger, Domain.SportHubDBContext context, INavigationItemService servise)
        {
            _logger = logger;
            _context = context;
            _servise = servise;
        }

        [HttpPost]
        [Route("/save/page")]
        public async Task<IActionResult> SaveItem([FromBody] dynamic data)
        {
            return new ObjectResult("dfdf");
        }
    }
}
