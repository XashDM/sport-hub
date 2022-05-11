using SportHub.Domain.Models;
using SportHub.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface ILanguageService
    {
        
        public Task AddLanguage(Language language);

        public Task DeleteDisplayedLanguage(int? id);

        public Task AddDisplayedLanguage(DisplayedLanguage language);
        public Task AddDisplayedLanguageRange(IEnumerable<DisplayedLanguage> language);

        public Task<List<LanguageViewModel>> GetAllLanguages();

        public Task<List<DisplayedLanguageViewModel>> GetAllDisplayedLanguages();

        public Task<LanguageViewModel> GetLanguage(int? id);

        
    }
}
