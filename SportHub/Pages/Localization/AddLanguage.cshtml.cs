using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using SportHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Pages.Localization
{
    public class AddLanguageModel : PageModel
    {
        private ILanguageService languageService { get; set; }

        public List<LanguageViewModel> LanguageList { get; set; }
        
        public AddLanguageModel(ILanguageService Service)
        {
            languageService = Service;
        }

        public async Task OnGet()
        {
            var languages = await languageService.GetAllLanguages();

            var languagesViewModels = languages.Select(x =>
            {
                var languageViewModel = new LanguageViewModel();
                languageViewModel.Id = x.Id;
                languageViewModel.LanguageName = x.LanguageName;
                languageViewModel.IsEnabled = x.IsEnabled;
                return languageViewModel;
            }).ToList();

            LanguageList = languagesViewModels;
        }

        public async Task OnPost(List<int> checkboxIdSet)
        {
            List<Language> selectedLanguageList = await languageService.GetLanguageRangeById(checkboxIdSet);

            await languageService.AddDisplayedLanguageRange(selectedLanguageList);

            await OnGet();
        }
    }
}
