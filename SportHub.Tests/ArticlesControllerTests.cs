using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SportHub.Controllers;
using SportHub.Domain.Models;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.ArticleServiceExceptions;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Tests
{
    [TestFixture]
    public class ArticleControllerTests
    {
        private NavigationItem[] categories;
        private NavigationItem[] subcategories;
        private NavigationItem[] teams;
        private Article[] articles;
        private DisplayItem[] mainArticles;
        private ImageItem[] images;

        [SetUp]
        public void Setup()
        {
            categories = new NavigationItem[] {
                new NavigationItem { Id = 1, Name = "NBL", ParentsItem = null, ParentsItemId = null, Type = "ñategory" },
                new NavigationItem { Id = 2, Name = "NFL", ParentsItem = null, ParentsItemId = null, Type = "ñategory" }
            };

            subcategories = new NavigationItem[] {
                new NavigationItem { Id = 3, Name = "Soccer", ParentsItem = null, ParentsItemId = 1, Type = "Subcategory" },
                new NavigationItem { Id = 4, Name = "Basketball", ParentsItem = null, ParentsItemId = 1, Type = "Subcategory" },
                new NavigationItem { Id = 5, Name = "Basketball", ParentsItem = null, ParentsItemId = 2, Type = "Subcategory" },
            };

            teams = new NavigationItem[]
            {
                new NavigationItem { Id = 6, Name = "Ukraine", ParentsItem = null, ParentsItemId = 1, Type = "Team" },
                new NavigationItem { Id = 7, Name = "NAVI", ParentsItem = null, ParentsItemId = 1, Type = "Team" },
                new NavigationItem { Id = 8, Name = "Poland", ParentsItem = null, ParentsItemId = 3, Type = "Team" },
                new NavigationItem { Id = 9, Name = "Storm", ParentsItem = null, ParentsItemId = 3, Type = "Team" },
            };

            images = new ImageItem[]
            {
                new ImageItem {Id = 1, Alt = "Alt", Author = "Author", ImageLink = "link", PhotoTitle = "title", ShortDescription = "description"},
                new ImageItem {Id = 2, Alt = "Alt", Author = "Author", ImageLink = "link", PhotoTitle = "title", ShortDescription = "description"},
                new ImageItem {Id = 3, Alt = "Alt", Author = "Author", ImageLink = "link", PhotoTitle = "title", ShortDescription = "description"}
            };

            articles = new Article[]
            {
                new Article { Id = 1, ContentText = "Text", ImageItem = images[0], ImageItemId = images[0].Id, PostedDate = DateTime.Now, ReferenceItem = teams[0], ReferenceItemId = teams[0].Id, Title = "None1"},
                new Article { Id = 2, ContentText = "Text", ImageItem = images[1], ImageItemId = images[1].Id, PostedDate = DateTime.Now, ReferenceItem = teams[0], ReferenceItemId = teams[0].Id, Title = "None2"},
                new Article { Id = 3, ContentText = "Text", ImageItem = images[2], ImageItemId = images[2].Id, PostedDate = DateTime.Now, ReferenceItem = teams[0], ReferenceItemId = teams[0].Id, Title = "None3"},
            };

            mainArticles = new DisplayItem[]
            {
                new DisplayItem { Id = 1, Article = articles[0], ArticleId = articles[0].Id, DisplayLocation = "MainSection", Type = "Article", IsDisplayed = false },
                new DisplayItem { Id = 2, Article = articles[1], ArticleId = articles[1].Id, DisplayLocation = "MainSection", Type = "Article", IsDisplayed = true },
            };
        }

        [Test]
        public async Task GetAllCategories_ReturnsStatusCode200()
        {
            // arrange

            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetAllCategoriesArrayAsync())
                .Returns(Task.FromResult(categories));

            var imageService = new Mock<IImageService>();

            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.GetAllCategories() as OkObjectResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, categories);
        }

        [Test]
        public async Task GetAllSubcategoriesByCategoryId_TakesCategoryId_ReturnsStatusCode200()
        {
            // arrange
            
            int categoryId = 1;
            var subcategoriesToReturn = subcategories.Where(subcategory => subcategory.ParentsItemId.Equals(categoryId)).ToArray();
            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetAllSubcategoriesByCategoryIdArrayAsync(categoryId))
                .Returns(Task.FromResult(subcategoriesToReturn));

            var imageService = new Mock<IImageService>();
            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.GetAllSubcategoriesByCategoryId(categoryId) as OkObjectResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, subcategoriesToReturn);
        }

        [Test]
        public async Task GetAllTeamsByParentId_TakesIdOfACategoryOrSubcategory_ReturnsStatusCode200()
        {
            // arrange

            int parentId = 1;
            var teamsToReturn = teams.Where(team => team.ParentsItemId.Equals(parentId)).ToArray();
            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetAllTeamsByParentIdArrayAsync(parentId))
                .Returns(Task.FromResult(teamsToReturn));
            
            var imageService = new Mock<IImageService>();
            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.GetAllTeamsByParentId(parentId) as OkObjectResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, teamsToReturn);
        }

        [Test]
        public async Task GetAllArticlesByParentIdPaginated_TakesIdOfAParentItem_ReturnsStatusCode200()
        {
            // arrange
            PageArgs pageArgs = new PageArgs { PageNumber = 1, PageSize = 3 };
            ArticleArgs articleArgs = new ArticleArgs { ArticleParentId = teams[0].Id, PageArgs = pageArgs };
            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetArticlesByParentIdPaginatedArrayAsync(articleArgs.ArticleParentId, pageArgs.PageSize, pageArgs.PageNumber))
                .Returns(Task.FromResult(articles));

            var imageService = new Mock<IImageService>();
            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.GetAllArticlesByParentIdPaginated(articleArgs) as OkObjectResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, articles);
        }

        [Test]
        public async Task GetAllArticlesByParentIdPaginated_TakesIdOfAParentItem_ReturnsStatusCode500()
        {
            // arrange

            PageArgs pageArgs = new PageArgs { PageNumber = 0, PageSize = -5 };
            ArticleArgs articleArgs = new ArticleArgs { ArticleParentId = teams[0].Id, PageArgs = pageArgs };
            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetArticlesByParentIdPaginatedArrayAsync(articleArgs.ArticleParentId, pageArgs.PageSize, pageArgs.PageNumber))
                .ThrowsAsync(new InvalidPageArgumentsException());

            var imageService = new Mock<IImageService>();
            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.GetAllArticlesByParentIdPaginated(articleArgs) as ObjectResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        public async Task SaveMainArticles_TakesDictionaryOfArticleIdAndTheirDisplayValue_ReturnsStatusCode201()
        {
            // arrange

            MainArticlesToSaveArgs mainArticlesToSaveArgs = new MainArticlesToSaveArgs();
            
            mainArticlesToSaveArgs.ArticlesDisplayValues = new Dictionary<int, bool>()
            {
                { 1, true },
                { 2, true },
                { 3, false },
            };

            var articleService = new Mock<IGetArticleService>();
            var imageService = new Mock<IImageService>();

            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.SaveMainArticles(mainArticlesToSaveArgs) as StatusCodeResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 201);
        }

        [Test]
        public async Task GetMainArticles_ReturnsStatusCode200()
        {
            // arrange

            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetMainArticles()).Returns(Task.FromResult(mainArticles));

            var imageService = new Mock<IImageService>();
            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);
            // act

            var result = await articleController.GetMainArticles() as OkObjectResult;

            // assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, mainArticles);
        }

        [Test]
        public async Task GetDisplayedMainArticles_ReturnsStatusCode200()
        {
            // arrange
            var articlesToReturn = mainArticles.Where(article => article.IsDisplayed.Equals(true)).ToArray();
            var articleService = new Mock<IGetArticleService>();
            articleService.Setup(articleService => articleService.GetDisplayedMainArticles()).Returns(Task.FromResult(articlesToReturn));

            var imageService = new Mock<IImageService>();
            var adminArticleService = new Mock<IGetAdminArticlesService>();
            var searchService = new Mock<ISearchService>();
            var commentService = new Mock<ICommentService>();

            var articleController = new ArticlesController(articleService.Object, adminArticleService.Object, imageService.Object, searchService.Object, commentService.Object);

            // act

            var result = await articleController.GetDisplayedMainArticles() as OkObjectResult;

            // assert
            foreach (var article in articlesToReturn)
            {
                Assert.AreEqual(article.IsDisplayed, true);
            }
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(articlesToReturn, result.Value);
        }
    }
}