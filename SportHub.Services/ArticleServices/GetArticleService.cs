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
            var Article = _context.Articles.FirstOrDefault(Article => Article.Id == id);
            if (Article is not null)
            {
                if (Article.ReferenceItem is not null)
                {
                    NavigationItem = _context.NavigationItems.FirstOrDefault(Item => Item.Id == Article.ReferenceItemId);
                    if (NavigationItem is not null)
                    {
                        if (NavigationItem.Type != "Team")
                        {
                            return "";
                        }
                        return NavigationItem.Name;
                    }
                    //return error_page, idk how, but return
                    return "error_page";
                }
                return "";
            }
            return "error_page_404";
        }

        public string GetArticlesSubcategory(int? id)
        {
            var Article = _context.Articles.FirstOrDefault(article => article.Id == id);
            if (Article is not null)
            {
                if (Article.ReferenceItemId is not null)
                {
                    NavigationItem itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == Article.ReferenceItemId);
                    int count = 0;
                    while (itemReference.Type != "Subcategory" && count < 3)
                    {
                        itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == itemReference.ParentsItemId);
                        if (itemReference is null)
                        {
                            return "item reference wasn't found(no subcategory)";
                        }
                        if (itemReference.Type == "Category")
                        {
                            return "All subcategory";
                        }
                        count++;
                    }
                    if (itemReference.Type == "Subcategory")
                    {
                        return itemReference.Name;
                    }
                    return "subcategory wasn't found";
                }
                return "articles reference id is null";
            }
            return "error_page_404";
        }

        public string GetArticlesCategory(int? id)
        {
            var Article = _context.Articles.FirstOrDefault(article => article.Id == id);
            if (Article is not null)
            {
                if (Article.ReferenceItemId is not null)
                {
                    NavigationItem itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == Article.ReferenceItemId);
                    int count = 0;
                    while (itemReference.Type != "Category" && count < 3)
                    {
                        itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == itemReference.ParentsItemId);
                        if (itemReference is null)
                        {
                            return "item reference wasn't found";
                        }
                        count++;
                    }
                    if (itemReference.Type == "Category")
                    {
                        return itemReference.Name;
                    }
                    return "category wasn't found";
                }
                return "articles reference id is null";
            }
            return "error_page_404";
        }

        public IQueryable<NavigationItem> GetAllCategoriesQueryable()
        {
            var categories = _context.NavigationItems
                .AsNoTracking()
                .Where(navigationItem => navigationItem.Type.Equals("Category"));

            return categories;
        }

        public Task<NavigationItem[]> GetAllCategoriesArrayAsync()
        {
            return GetAllCategoriesQueryable().ToArrayAsync();
        }

        public IQueryable<NavigationItem> GetAllSubcategoriesByCategoryIdQueryable(int categoryId)
        {
            var subcategories = _context.NavigationItems
                .AsNoTracking()
                .Where(navigationItem => navigationItem.Type.Equals("Subcategory"))
                .Where(navigationItem => navigationItem.ParentsItemId.Equals(categoryId));

            return subcategories;
        }

        public Task<NavigationItem[]> GetAllSubcategoriesByCategoryIdArrayAsync(int categoryId)
        {
            return GetAllSubcategoriesByCategoryIdQueryable(categoryId).ToArrayAsync();
        }

        public IQueryable<NavigationItem> GetAllTeamsByParentIdQueryable(int parentId)
        {
            var teams = _context.NavigationItems
                .AsNoTracking()
                .Where(navigationItem => navigationItem.Type.Equals("Team"))
                .Where(navigationItem => navigationItem.ParentsItemId.Equals(parentId));

            return teams;
        }

        public Task<NavigationItem[]> GetAllTeamsByParentIdArrayAsync(int parentId)
        {
            return GetAllTeamsByParentIdQueryable(parentId).ToArrayAsync();
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

        public async Task<Article[]> GetArticlesByParentIdPaginatedArrayAsync(int parentId, int pageSize, int pageNumber)
        {
            var allArticles = GetAllArticlesByParentIdQueryable(parentId);

            var paginatedArticles = await Paginate(allArticles, pageSize, pageNumber)
                .Item1
                .ToArrayAsync();

            return paginatedArticles;
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

        public async Task<bool> UploadPhotoOfTheDay(ImageItem image)
        {
            try
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
                        return false;
                    }
                    else
                    {
                        itemToUpdate.ImageItem = image;
                    }

                    await _context.AddAsync(itemToUpdate);
                    await _context.SaveChangesAsync();
                    return true;
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
                    return true;
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ImageItem> UploadArticlePhoto(ImageItem image)
        {
            if (image.ImageLink != null)
            {
                await _context.AddAsync(image);
                await _context.SaveChangesAsync();
                return image;
            }
            return null; 
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
        public async Task<bool> SaveArticle(Article article)
        {
            if (article.ReferenceItemId != null)
            {
               NavigationItem item = await _context.NavigationItems.FirstOrDefaultAsync(item => item.Id == article.ReferenceItemId);
               if(item == null)
                    return false;
            }

            article.PostedDate = DateTime.Now;

            _context.Add(article);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
