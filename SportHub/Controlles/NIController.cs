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
        [HttpGet("/GetTree")]
        public async Task<IActionResult> OnGetTree(int ItemId)
        {
            return new OkObjectResult(await _servise.GetRecusiveTree(ItemId));
        }
        [HttpGet("/GetRoot")]
        public async Task<IActionResult> OnGetRoot()
        {
            return new OkObjectResult(await _servise.GetTopCategories());
        }
        [HttpGet("/GetChildren")]
        public async Task<IActionResult> OnGetChildren(int ItemId)
        {

            return new OkObjectResult(await _servise.GetChildrenOfItem(ItemId));
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