using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using SportHub.Services.Services;
using SportHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Pages.Localization
{
    public class LocalizationModel : PageModel
    {
        private ILanguageService languageService { get; set; }

        public List<LanguageViewModel> DispLanguageList { get; set; }

        public LocalizationModel(ILanguageService Service)
        {
            languageService = Service;
        } 

        public async Task OnGet()
        {
            var displanguages = await languageService.GetAllDisplayedLanguages();

            var displanguagesViewModels = displanguages.Select(x =>
            {
                var displanguageViewModel = new LanguageViewModel();
                displanguageViewModel.Id = x.Id;
                displanguageViewModel.LanguageName = x.LanguageName;
                displanguageViewModel.IsEnabled = x.IsEnabled;
                return displanguageViewModel;
            }).ToList();

            DispLanguageList = displanguagesViewModels;
        }

        public async Task OnPost(int id, bool isEnable)
        {
            await languageService.UpdateDisplayedLanguage(id, isEnable);
            await OnGet();
        }

        public async Task OnPostDelete(int id)
        {
           await languageService.DeleteDisplayedLanguage(id);
           await OnGet();
        }
    }
}


