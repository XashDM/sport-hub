using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using SportHub.Services.Exceptions.ArticleServiceExceptions;
using System;
using System.Collections.Generic;

namespace SportHub.Services.ArticleServices
{
    public class GetArticleService: IGetArticleService
    {
        private readonly SportHubDBContext _context;
        private readonly IImageService _imageService;
        NavigationItem NavigationItem;

        public GetArticleService(SportHubDBContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }


        public async Task<Article> GetArticle(int id)
        {
            try
            {
                var article = await _context
                    .Articles
                    .Include(image => image.ImageItem)
                    .FirstOrDefaultAsync(idArticle => idArticle.Id == id);
                if (article is not null)
                {
                    article.ImageItem.ImageLink = await _imageService.GetImageLinkByNameAsync(article.ImageItem.ImageLink);
                }
                return article;
            }
            catch { return null; }
        }

        public string GetArticlesTeam(int? id)
        {
            var Article = _context.Articles.First(Article => Article.Id == id);
            if (Article.ReferenceItemId == null)
            {
                return null;
            }
            try
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == Article.ReferenceItemId);
            }
            catch
            {
                return null;
            }
            if (NavigationItem.Type != "Team") return null;
            return NavigationItem.Name;
        }

        public string GetArticlesSubcategory(int? id)
        {
            var Article = _context.Articles.First(Article => Article.Id == id);
            if (Article.ReferenceItemId == null)
            {
                return null;
            }
            try
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == Article.ReferenceItemId);
                if (NavigationItem.Type == "Subcategory") return NavigationItem.Name;
            }
            catch
            {
                return null;
            }
            if (NavigationItem.Type == "Category")
            {
                return null;
            }
            if (NavigationItem.Type == "Team")
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == NavigationItem.ParentsItemId);
                if (NavigationItem.Type != "Subcategory") return null;
            }
            return NavigationItem.Name;
        }

        public string GetArticlesCategory(int? id)
        {
            var Article = _context.Articles.First(Article => Article.Id == id);
            if (Article.ReferenceItemId == null)
            {
                return "All Category";
            }
            try
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == Article.ReferenceItemId);
            }
            catch
            {
                return null;
            }
            int count = 0;
            while (NavigationItem.Type != "Category" && count < 3)
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == NavigationItem.ParentsItemId);
                count++;
            }
            if (NavigationItem.Type != "Category") return null;
            return NavigationItem.Name;
        }

        public IQueryable<NavigationItem> GetAllCategoriesQueryable()
        {
            var categories = _context.NavigationItems
                .AsNoTracking()
                .Where(navigationItem => navigationItem.Type.Equals("Category"));

            return categories;
        }

        public IQueryable<NavigationItem> GetAllSubcategoriesByCategoryIdQueryable(int categoryId)
        {
            var subcategories = _context.NavigationItems
                .AsNoTracking()
                .Where(navigationItem => navigationItem.Type.Equals("Subcategory"))
                .Where(navigationItem => navigationItem.ParentsItemId.Equals(categoryId));

            return subcategories;
        }

        public IQueryable<NavigationItem> GetAllTeamsByParentIdQueryable(int parentId)
        {
            var teams = _context.NavigationItems
                .AsNoTracking()
                .Where(navigationItem => navigationItem.Type.Equals("Team"))
                .Where(navigationItem => navigationItem.ParentsItemId.Equals(parentId));

            return teams;
        }

        public IQueryable<Article> GetAllArticlesByParentIdQueryable(int parentId)
        {
            var articles = _context.Articles
                .AsNoTracking()
                .Include(article => article.ReferenceItem)
                .ThenInclude(refItem => refItem.ParentsItem)
                .ThenInclude(refItem => refItem.ParentsItem)
                .Where(article => article.ReferenceItemId.Equals(parentId));

            return articles;
        }

        public async Task SaveMainArticles(Dictionary<int, bool> articlesToSave)
        {
            var itemsToReset = _context.DisplayItems
                .Where(displayItem => displayItem.Type.Equals("Article"))
                .Where(displayItem => displayItem.DisplayLocation.Equals("MainSection"));

            _context.RemoveRange(itemsToReset);
            await _context.SaveChangesAsync();

            List<DisplayItem> itemsToSave = new List<DisplayItem>();
            foreach (var item in articlesToSave)
            {
                DisplayItem displayItem = new DisplayItem()
                {
                    DisplayLocation = "MainSection",
                    IsDisplayed = item.Value,
                    Type = "Article",
                    ArticleId = item.Key,
                };

                itemsToSave.Add(displayItem);
            }

            await _context.AddRangeAsync(itemsToSave);
            await _context.SaveChangesAsync();
        }

        // doesn't include ImageItem, text only
        public async Task<DisplayItem[]> GetMainArticles()
        {
            var articlesToReturn = _context.DisplayItems
                .Where(displayItem => displayItem.Type.Equals("Article"))
                .Where(displayItem => displayItem.DisplayLocation.Equals("MainSection"))
                .Include(displayItem => displayItem.Article)
                .ThenInclude(article => article.ReferenceItem)
                .ThenInclude(team => team.ParentsItem)
                .ThenInclude(subcategory => subcategory.ParentsItem);

            foreach (var item in articlesToReturn)
            {
                item.Article.DisplayItems = null;
            }

            return await articlesToReturn.ToArrayAsync();
        }

        //returns only displayed articles list with ImageItems
        public async Task<DisplayItem[]> GetDisplayedMainArticles()
        {
            var articlesToReturn = _context.DisplayItems
                .Where(displayItem => displayItem.Type.Equals("Article"))
                .Where(displayItem => displayItem.DisplayLocation.Equals("MainSection"))
                .Where(displayItem => displayItem.IsDisplayed.Equals(true))
                .Include(displayItem => displayItem.Article)
                    .ThenInclude(article => article.ReferenceItem)
                        .ThenInclude(team => team.ParentsItem)
                            .ThenInclude(subcategory => subcategory.ParentsItem)
                .Include(displayItem => displayItem.Article)
                    .ThenInclude(imageItem => imageItem.ImageItem);

            foreach (var item in articlesToReturn)
            {
                item.Article.DisplayItems = null;
                item.Article.ImageItem.ImageLink = await _imageService.GetImageLinkByNameAsync(item.Article.ImageItem.ImageLink);
            }

            return await articlesToReturn.ToArrayAsync();
        }

        public (IQueryable<T>, int, int) Paginate<T>(IQueryable<T> items, int pageSize, int pageNumber)
        {
            (int toSkip, int toTake, int totalPages, int totalItemsAmount) = GetPaginationValues(items, pageSize, pageNumber);

            items = items
                .Skip(toSkip)
                .Take(toTake);

            return (items, totalPages, totalItemsAmount);
        }

        private (int, int, int, int) GetPaginationValues<T>(IQueryable<T> items, int pageSize, int pageNumber)
        {
            int totalItemsAmount = items.Count();

            if (pageNumber < 1 || pageSize < 1)
            {
                throw new InvalidPageArgumentsException();
            }

            int totalPages = (int)Math.Ceiling(totalItemsAmount / (double)pageSize);

            int toSkip = (pageNumber - 1) * pageSize;
            int toTake = pageSize;

            return (toSkip, toTake, totalPages, totalItemsAmount);
        }

        public async Task UploadPhotoOfTheDay(ImageItem image)
        {
            var itemToUpdate = await _context.DisplayItems
                .Where(displayItem => displayItem.DisplayLocation.Equals("PhotoOfTheDay"))
                .Include(item => item.ImageItem)
                .FirstOrDefaultAsync();
            //creating a new photo of the day, if doesn't exist
            if (itemToUpdate is null)
            {
                itemToUpdate = new DisplayItem()
                {
                    DisplayLocation = "PhotoOfTheDay",
                    IsDisplayed = false,
                    Type = "PhotoOfTheDay"
                };

                if (image.ImageLink is null)
                {
                    return;
                }
                else
                {
                    itemToUpdate.ImageItem = image;
                }

                await _context.AddAsync(itemToUpdate);
                await _context.SaveChangesAsync();
                return;
            }

            if (image.ImageLink != null)
            {
                if (itemToUpdate.ImageItem is not null)
                {
                    _imageService.DeleteImageFromStorage(itemToUpdate.ImageItem.ImageLink);
                    _context.Remove<ImageItem>(itemToUpdate.ImageItem);
                    await _context.SaveChangesAsync();
                }
                itemToUpdate.ImageItem = image;
                await _context.SaveChangesAsync();
                return;
            }
            else
            {
                image.ImageLink = itemToUpdate.ImageItem.ImageLink;
            }

            itemToUpdate.ImageItem.Alt = image.Alt;
            itemToUpdate.ImageItem.PhotoTitle = image.PhotoTitle;
            itemToUpdate.ImageItem.ShortDescription = image.ShortDescription;
            itemToUpdate.ImageItem.Author = image.Author;
            //await _context.AddAsync(itemToUpdate);
            await _context.SaveChangesAsync();
        }

        //admin only, returns hidden article 
        public async Task<DisplayItem> GetPhotoOfTheDay()
        {
            var item = await _context.DisplayItems
                .Where(displayItem => displayItem.DisplayLocation.Equals("PhotoOfTheDay"))
                .Include(displayItem => displayItem.ImageItem).FirstOrDefaultAsync();
            if (item is null)
            {
                return null;
            }

            if (item.ImageItem is not null)
            {
                item.ImageItem.ImageLink = await _imageService.GetImageLinkByNameAsync(item.ImageItem.ImageLink);
            }
            return item;
        }
        public async Task<DisplayItem> GetDisplayedPhotoOfTheDay()
        {
            var item = await _context.DisplayItems
                .Where(displayItem => displayItem.DisplayLocation.Equals("PhotoOfTheDay"))
                .Where(displayItem => displayItem.IsDisplayed.Equals(true))
                .Include(displayItem => displayItem.ImageItem).FirstOrDefaultAsync();
            if (item is null)
            {
                return null;
            }

            if (item.ImageItem is not null)
            {
                item.ImageItem.ImageLink = await _imageService.GetImageLinkByNameAsync(item.ImageItem.ImageLink);
            }
            return item;
        }

        public async Task DisplayPhotoOfTheDay()
        {
            var item = await _context.DisplayItems
                .Where(displayItem => displayItem.DisplayLocation.Equals("PhotoOfTheDay")).FirstOrDefaultAsync();
            if (item is not null)
            {
                if (!item.IsDisplayed)
                {
                    item.IsDisplayed = true;
                    _context.SaveChanges();
                }
            }
        }

        public async Task HidePhotoOfTheDay()
        {
            var item = await _context.DisplayItems
                .Where(displayItem => displayItem.DisplayLocation.Equals("PhotoOfTheDay")).FirstOrDefaultAsync();
            if (item is not null)
            {
                if (item.IsDisplayed)
                {
                    item.IsDisplayed = false;
                    _context.SaveChanges();
                }
            }
        }
    }
}
