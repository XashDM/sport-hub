#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportHub.Domain.Models;

namespace SportHub.Pages.Articles
{
    public class IndexModel : PageModel
    {
        private readonly Domain.SportHubDBContext _context;

        public IndexModel(Domain.SportHubDBContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }

        public async Task OnGetAsync()
        {
            Article = await _context.Articles.ToListAsync();
        }
    }
}
