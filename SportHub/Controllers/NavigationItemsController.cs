using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SportHub.Domain.Models;
using SportHub.Domain.ViewModel;
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
        private readonly INavigationItemService _navigationService;
        public NavigationItems(ILogger<NavigationItems> logger, INavigationItemService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }

        [HttpPost("/save")]
        public async Task<IActionResult> SaveItems(Dictionary<string, List<NavigationItemForSave>> data)
        {
            bool result = await _navigationService.AddNewItems(data["date"]);
            return result ? new ObjectResult("OK") : BadRequest("Errore");
        }

        [HttpGet("/GetTree")]
        public async Task<IActionResult> GetTree(int itemId)
        {
            var result = await _navigationService.GetRecusiveTree(itemId);
            return result != null ? new ObjectResult(result) : BadRequest("Errore");
        }

        [HttpGet("/GetRoot")]
        public async Task<IActionResult> GetRoot()
        {
            var result = await _navigationService.GetTopCategories();
            return result != null ? new ObjectResult(result) : BadRequest("Errore");
        }

        [HttpGet("/GetChildren")]
        public async Task<IActionResult> GetChildren(int itemId)
        {
            var result = await _navigationService.GetChildrenOfItem(itemId);
            return result != null ? new ObjectResult(result) : BadRequest("Errore");
        }
    }
}