using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Logging;
using SportHub.Models;
using Microsoft.AspNetCore.Localization;

namespace SportHub.Controllers
{

    public class LocalizationController : Controller
    {
        private readonly IHtmlLocalizer<LocalizationController> _localizer;
        private readonly ILogger<LocalizationController> _logger;

        public LocalizationController(ILogger<LocalizationController> logger, IHtmlLocalizer<LocalizationController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult LocalizatonIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, 
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = System.DateTimeOffset.Now.AddDays(30)});

            return LocalRedirect(returnUrl);
        }
    }
}
