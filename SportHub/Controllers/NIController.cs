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
            bool result = await _servise.AddNewItems(data["date"]);
            if (result)
            {
                return new ObjectResult("OK");
            }
            else
            {
                return BadRequest("Errore");
            }
        }

        [HttpGet("/GetTree")]
        public async Task<IActionResult> OnGetTree(int ItemId)
        {
            var result = await _servise.GetRecusiveTree(ItemId);
            if (result != null)
            {
                return new ObjectResult(result);
            }
            else
            {
                return BadRequest("Errore");
            }
        }

        [HttpGet("/GetRoot")]
        public async Task<IActionResult> OnGetRoot()
        {
            var result = await _servise.GetTopCategories();
            if (result != null)
            {
                return new ObjectResult(result);
            }
            else
            {
                return BadRequest("Errore");
            }
        }

        [HttpGet("/GetChildren")]
        public async Task<IActionResult> OnGetChildren(int ItemId)
        {
            var result = await _servise.GetChildrenOfItem(ItemId);
            if (result != null)
            {
                return new ObjectResult(result);
            }
            else
            {
                return BadRequest("Errore");
            }
        }

        [HttpGet("/GetAddItem")]
        public async Task<IActionResult> OnGetAddItem(string name, int? ParentsItemId, string Type)
        {
            NavigationItem Item = new NavigationItem
            {
                Type = Type,
                Name = name,
                ParentsItemId = ParentsItemId,
            };
            await _servise.AddNewItem(Item);
            return new OkObjectResult(Item);
        }
    }
}