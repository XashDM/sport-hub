using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace SportHub.Services.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly SportHubDBContext _context;

        public LanguageService(SportHubDBContext context)
        {
            _context = context;
        }

        public async Task AddDisplayedLanguage(DisplayedLanguage language)
        {
            _context.DisplayedLanguages.Add(language);
            await _context.SaveChangesAsync();
        }

        public async Task AddDisplayedLanguageRange(List<Language> languages)
        {
            var languagesToAdd = languages.Select(p => new DisplayedLanguage
            {
                IsEnabled = p.IsEnabled,
                LanguageName = p.LanguageName,
            }).ToList();

            await _context.DisplayedLanguages.AddRangeAsync(languagesToAdd);
            await _context.SaveChangesAsync();
        }
                
        public async Task<List<Language>> GetLanguageRangeById(List<int> languages)
        {
            List<Language> SelectedLanguageList = new List<Language>();
            
            foreach (int element in languages)
            {
                if (element == null) 
                {

                    throw new ArgumentNullException(nameof(element));

                }
                
                SelectedLanguageList.Add(await GetLanguageById(element));
            }

            return SelectedLanguageList;
        }

        public async Task DeleteDisplayedLanguage(int id)
        {
            var language = await GetDisplayedLanguageById(id);
            _context.DisplayedLanguages.Remove(language);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDisplayedLanguage(int id, bool isEnable)
        {
            var language = await GetDisplayedLanguageById(id);
            language.IsEnabled = isEnable;
            _context.DisplayedLanguages.Update(language);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Language>> GetAllLanguages()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<List<DisplayedLanguage>> GetAllDisplayedLanguages()
        {
            return await _context.DisplayedLanguages.ToListAsync();
        }

        private async Task<Language> GetLanguageById(int id)
        {
            var languages = await _context.Languages.ToListAsync();

            var language = languages.FirstOrDefault(x => x.Id == id);

            return language;
        }

        private async Task<DisplayedLanguage> GetDisplayedLanguageById(int id)
        {
            var language = (await _context.DisplayedLanguages.ToListAsync()).FirstOrDefault(x => x.Id == id);

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            return language;
        }

    }
}
