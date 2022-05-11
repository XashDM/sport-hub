using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using SportHub.Services.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Pages.Localization
{
    public class AddLanguageModel : PageModel
    {
        private ILanguageService languageService { get; set; }

        public List<LanguageViewModel> LanguageList { get; set; }

        public List<LanguageViewModel> SelectedLanguageList { get; set; }

        public AddLanguageModel(ILanguageService Service)
        {
            languageService = Service;
        }

        public async Task OnGet()
        {
            LanguageList = (await languageService.GetAllLanguages()).ToList();
        }

        public async Task OnPost()
        {
            var languagesToAdd = SelectedLanguageList.Where(x => x.IsEnabled).Select(x =>
            {
                var language = new DisplayedLanguage();
                language.IsEnabled = x.IsEnabled;
                language.LanguageName = x.LanguageName;
                return language;
            });
            await languageService.AddDisplayedLanguageRange(languagesToAdd);
        }
    }
}
