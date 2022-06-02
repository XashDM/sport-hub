using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using SportHub.Services.ViewModels;
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
            LanguageList = (await languageService.GetAllLanguages()).ToList();
        }

        public async Task OnPost(List<int> checkboxIdSet)
        {
            List<Language> selectedLanguageList = await languageService.GetLanguageRangeById(checkboxIdSet);

            await languageService.AddDisplayedLanguageRange(selectedLanguageList);

            await OnGet();
        }
    }
}
