using SportHub.Domain.Models;
using SportHub.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface ILanguageService
    {

        public Task AddLanguage(Language language);

        public Task DeleteLanguage(int? id);

        public Task<List<LanguageViewModel>> GetAllLanguages();

        public Task<LanguageViewModel> GetLanguage(int? id);


    }
}
