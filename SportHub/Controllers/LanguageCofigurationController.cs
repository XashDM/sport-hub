using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageCofigurationController : ControllerBase
    {
        private ILanguageService languageService {get; set;}

        public LanguageCofigurationController(ILanguageService Service)
        {
            languageService = Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLanguages()
        {
            var languages = await languageService.GetAllLanguages();
            return Ok(languages.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddLanguage([FromBody] Language language)
        {
            if (language == null)
            {
                return BadRequest("language was null");
            }

            await languageService.AddLanguage(language);

            return StatusCode(201);
        }
    }
}
