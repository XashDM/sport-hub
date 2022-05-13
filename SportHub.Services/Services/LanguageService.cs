﻿using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using SportHub.Services.ViewModels;
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


        public async Task AddLanguage(Language language)
        {
            _context.Languages.Add(language);
            await _context.SaveChangesAsync();
        }


        public async Task ChangeLanguageDisplayValue(Language language)
        {
            _context.Languages.Attach(language);
            await _context.SaveChangesAsync();
        }

        public async Task AddDisplayedLanguage(DisplayedLanguage language)
        {
            _context.DisplayedLanguages.Add(language);
            await _context.SaveChangesAsync();
        }

        public async Task AddDisplayedLanguageRange(IEnumerable<DisplayedLanguage> languages)
        {
            await _context.DisplayedLanguages.AddRangeAsync(languages);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDisplayedLanguage(int? id)
        {
            var language = await GetDisplayedLanguageById(id);
            _context.DisplayedLanguages.Remove(language);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDisplayedLanguage(int? id, bool isEnable)
        {
            var language = await GetDisplayedLanguageById(id);
            language.IsEnabled = isEnable;
            _context.DisplayedLanguages.Update(language);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LanguageViewModel>> GetAllLanguages()
        {
            var languages = (await _context.Languages.ToListAsync()).Select(x =>
            {
                var languageViewModel = new LanguageViewModel();
                languageViewModel.Id = x.Id;
                languageViewModel.LanguageName = x.LanguageName;
                languageViewModel.IsEnabled = x.IsEnabled;
                return languageViewModel;
            }).ToList();
            return languages;
        }


        public async Task<List<DisplayedLanguageViewModel>> GetAllDisplayedLanguages()
        {
            var displanguages = (await _context.DisplayedLanguages.ToListAsync()).Select(x =>
            {
                var displanguageViewModel = new DisplayedLanguageViewModel();
                displanguageViewModel.Id = x.Id;
                displanguageViewModel.LanguageName = x.LanguageName;
                displanguageViewModel.IsEnabled = x.IsEnabled;
                return displanguageViewModel;
            }).ToList();
            return displanguages;
        }

        private async Task<Language> GetLanguageById(int? id)
        {
            var language = (await _context.Languages.ToListAsync()).FirstOrDefault(x => x.Id == id);
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
            return language;
        }

        private async Task<DisplayedLanguage> GetDisplayedLanguageById(int? id)
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
