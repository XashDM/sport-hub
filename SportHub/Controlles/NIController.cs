using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Threading.Tasks;
namespace SportHub.Controlles
{

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

        [HttpPost("/save")]
        public async Task<IActionResult> SaveItems(Dictionary<string, List<NavigationItem>> data)
        {
            /*
            var result = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(data);
            var result1 = System.Text.Json.JsonSerializer.Deserialize<List<NavigationItem>>((JsonElement)data["date"]);
                     */
            _servise.AddNewItems(data["date"]);
   
            //var result = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<NavigationItem>>>(data);
            return new ObjectResult("");
        }
    }
}