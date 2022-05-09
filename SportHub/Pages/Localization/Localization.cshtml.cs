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
    public class LocalizationModel : PageModel
    {
        private ILanguageService languageService { get; set; }

        public List<LanguageViewModel> LangList { get; set; }

        public LocalizationModel(ILanguageService Service)
        {
            languageService = Service;
        } 

        public async Task OnGet()
        {
            LangList = (await languageService.GetAllLanguages()).ToList();
        }


        public async Task OnPostDelete(int id)
        {
           await languageService.DeleteLanguage(id);
           await OnGet();
        }


        /*
        public async Task OnPost(int id, string languageName, bool isEnabled)
        {
            var language = new Language
            {
                Id = id,
                LanguageName = languageName,
                IsEnabled = isEnabled
            };
            await languageService.AddLanguage(language);
            await OnGet();
        }
        */
    }
}


