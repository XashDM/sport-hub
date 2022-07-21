let amountOfArticlesInSearchFieldSearchPage = 0;
let startElementPositionSearchPage = 10;
let amountOfElementsSearchPage = 2;

$(document).ready(function () {
    $('.search-page-search-article-bottom-content').map(function () {
        let fieldFromArticle = $(this).text().replace(/(\r\n|\n|\r)/gm, "");
        let searchValue = $(".search-page-result-name").text().replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "");
        let articleContentText = "";
        for (var j = 0; j < fieldFromArticle.length; j++) {
            if (fieldFromArticle.substr(j, searchValue.length) == searchValue) {
                articleContentText += `<span class="find-words-search-article-top-info"><b>${fieldFromArticle.substr(j, searchValue.length)}</b></span>`;
                j += searchValue.length - 1;
            }
            else {
                articleContentText += fieldFromArticle[j];
            }
        }
        $(this).html(articleContentText);
    });
});

function searchScrollSearchPage() {
    let hiddenDivSize = $('.search-page-search-result-articles')[0].scrollHeight;
    let visibleDivSize = $('.search-page-search-result-articles').height();
    let scrollHeight = hiddenDivSize - visibleDivSize;
    let scrollPosition = $('.search-page-search-result-articles').scrollTop();
    if ((visibleDivSize + scrollPosition) / hiddenDivSize > 0.8) {
        updateSearchAfterScrollingSearchPage();
        hiddenDivSize = $('.search-page-search-result-articles')[0].scrollHeight;
        scrollHeight = visibleDivSize / hiddenDivSize * visibleDivSize;
    }
}

function updateSearchAfterScrollingSearchPage() {
    let searchValue = $(".search-page-result-name").text();
    searchValue = searchValue.replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "");
    let searchParameters = {
        searchValue: searchValue,
        startPosition: startElementPositionSearchPage,
        amountArticles: amountOfElementsSearchPage,
    };
    $.ajax({
        method: 'post',
        url: '/api/Articles/ArticlesRange',
        contentType: 'application/json',
        data: JSON.stringify(searchParameters),
        success: function (articles) {
            if (articles.length != 0) {
                amountOfArticles = articles.length;
                amountToDelete = amountOfArticles;
                for (var i = 0; i < amountOfArticles; i++) {
                    var articleSearchField = $('.search-page-search-article-info:first')
                        .clone().attr('id', `article-with-id-${articles[i].id}`)
                        .appendTo('.search-page-search-result-articles');
                    //add category, subcategory, team fields
                    articleSearchField.find('#article-href').attr('href', `/Articles/Details?id=${articles[i].id}`);
                    articleSearchField.find('.search-page-search-article-category-name').text(articles[i].referenceItem.name);
                    if (articles[i].referenceItem.parentsItem != null) {
                        articleSearchField.find('.search-page-search-article-subcategory-name').text(articles[i].referenceItem.parentsItem.name);
                        if (articles[i].referenceItem.parentsItem.parentsItem != null) {
                            articleSearchField.find('.search-page-search-article-team-name').text(articles[i].referenceItem.parentsItem.parentsItem.name);
                            articleSearchField.find('.search-page-search-article-team-name').css('color', '#D72130');
                            articleSearchField.find('.search-page-search-article-subcategory-name').css('color', '');
                            articleSearchField.find('.search-page-search-article-category-name').css('color', '');
                        }
                        else {
                            articleSearchField.find('.search-page-search-article-subcategory-name').css('color', '#D72130');
                            articleSearchField.find('.search-page-search-article-team-name').text('');
                        }
                    }
                    else {
                        articleSearchField.find('.search-page-search-article-category-name').css('color', '#D72130');
                        articleSearchField.find('.search-page-search-article-team-name').text('');
                        articleSearchField.find('.search-page-search-article-subcategory-name').text('');
                        articleSearchField.find('#search-page-image-in-search-between-category-subcategory').css('display', 'none');
                    }

                    let fieldFromArticle = articles[i].contentText;
                    let articleContentText = "";
                    for (var j = 0; j < fieldFromArticle.length; j++) {
                        if (fieldFromArticle.substr(j, searchValue.length) == searchValue) {
                            articleContentText += `<span class="find-words-search-article-top-info"><b>${fieldFromArticle.substr(j, searchValue.length)}</b></span>`;
                            j += searchValue.length - 1;
                        }
                        else {
                            articleContentText += fieldFromArticle[j];
                        }
                    }
                    articleSearchField.find('.search-page-search-article-bottom-content').html(articleContentText.substr(0, 300));
                }
                if (amountOfArticles == 0) {
                    var articleSearchField = $('.search-page-search-article-info:first')
                        .clone().appendTo('.search-page-search-result-articles');
                    articleSearchField.find('.search-page-search-article-category-name').text('No matches found.');
                    articleSearchField.find('.search-page-search-article-bottom-content').text('Please try another search.');
                }
            }
        }
    });
    startElementPositionSearchPage += amountOfElementsSearchPage;
}