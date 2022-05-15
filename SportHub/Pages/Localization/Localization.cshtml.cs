using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using SportHub.Services.Services;
using SportHub.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Pages.Localization
{
    public class LocalizationModel : PageModel
    {
        private ILanguageService languageService { get; set; }

        public List<DisplayedLanguageViewModel> DispLanguageList { get; set; }

        public LocalizationModel(ILanguageService Service)
        {
            languageService = Service;
        } 

        public async Task OnGet()
        {
            DispLanguageList = (await languageService.GetAllDisplayedLanguages()).ToList();
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


