using SportHub.Domain.Models;
using SportHub.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface ILanguageService
    {
        public Task DeleteDisplayedLanguage(int id);
        public Task UpdateDisplayedLanguage(int id, bool isEnable);
        public Task AddDisplayedLanguage(DisplayedLanguage language);
        public Task AddDisplayedLanguageRange(List<Language> language);
        public Task<List<Language>> GetLanguageRangeById(List<int> languages);
        public Task<List<LanguageViewModel>> GetAllLanguages();
        public Task<List<LanguageViewModel>> GetAllDisplayedLanguages();
    }
}
