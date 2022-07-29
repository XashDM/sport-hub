//search-results
let noResultTop = 'No matches found.';
let noResultBottom = 'Please try another search.';
let variable;
let checkTyping = false;
function searchFieldWithTimeLimit() {
    checkTyping = true;
    variable = setTimeout(function () {
        if (checkTyping == true) {
            searchField();
            checkTyping = false;
        }
    }, 1000);
}

$('#header-search-field').on('click', function () {
    if ($('.search-result-articles').css('display') == 'none' && $(".lable").val() != '') {
        $('main').css('z-index', '-1');
        $('.search-result-articles').slideDown();
    }
});

let amountOfArticlesInSearchField = 0;
let startElementPosition = 10;
let amountOfElements = 2;
let amountToDelete;
let displayedArticles = 0;
let articleContentTextLength = 300;
function searchField() {
    clearTimeout(variable);

    let amountOfArticles;
    let searchValue = $(".lable").val();

    let searchParameters = {
        searchValue: searchValue,
        startPosition: 0,
        amountArticles: 10,
    };

    if (searchValue != "") {

        $.ajax({
            method: 'post',
            url: '/api/Articles/ArticlesRange',
            contentType: 'application/json',
            data: JSON.stringify(searchParameters),
            success: function (articles) {
                if (articles.length != 0) {
                    $('main').css('z-index', '-1');
                    amountOfArticles = articles.length;
                    let articleIndex = 0;
                    $('.search-result-articles').find('.search-article-info').map(function () {
                        if (amountOfArticles > articleIndex) {
                            $(this).find('#article-href').attr('href', `/Articles/Details?id=${articles[articleIndex].id}`);
                            $(this).find('.search-article-category-name').text(articles[articleIndex].referenceItem.name);
                            if (articles[articleIndex].referenceItem.parentsItem != null) {
                                $(this).find('.search-article-subcategory-name').text(articles[articleIndex].referenceItem.parentsItem.name);
                                if (articles[articleIndex].referenceItem.parentsItem.parentsItem != null) {
                                    $(this).find('.search-article-team-name').text(articles[articleIndex].referenceItem.parentsItem.parentsItem.name);
                                    $(this).find('.search-article-team-name').css('color', '#D72130');
                                    $(this).find('.search-article-subcategory-name').css('color', '');
                                    $(this).find('.search-article-category-name').css('color', '');
                                }
                                else {
                                    $(this).find('.search-article-subcategory-name').css('color', '#D72130');
                                    $(this).find('.search-article-team-name').text('');
                                }
                            }
                            else {
                                $(this).find('.search-article-category-name').css('color', '#D72130');
                                $(this).find('.search-article-team-name').text('');
                                $(this).find('.search-article-subcategory-name').text('');
                                $(this).find('#image-in-search-between-category-subcategory').css('display', 'none');
                            }

                            let fieldFromArticle = articles[articleIndex].contentText.replace(/<[^>]*>?/gm, '').substr(0, articleContentTextLength);
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
                            $(this).find('.search-article-bottom-content').html(articleContentText);

                            articleIndex++;
                            $(this).show();
                            $(this).appendTo('.search-result-articles');
                        }
                        else {
                            $(this).remove();
                        }
                    });
                    for (var i = articleIndex; i < amountOfArticles; i++) {
                        var articleSearchField = $('.search-article-info:first')
                            .clone().attr('id', `article-with-id-${articles[i].id}`).css('display', 'block')
                            .appendTo('.search-result-articles');
                        //add category, subcategory, team fields
                        articleSearchField.find('#article-href').attr('href', `/Articles/Details?id=${articles[i].id}`);
                        articleSearchField.find('.search-article-category-name').text(articles[i].referenceItem.name);
                        if (articles[i].referenceItem.parentsItem != null) {
                            articleSearchField.find('.search-article-subcategory-name').text(articles[i].referenceItem.parentsItem.name);
                            if (articles[i].referenceItem.parentsItem.parentsItem != null) {
                                articleSearchField.find('.search-article-team-name').text(articles[i].referenceItem.parentsItem.parentsItem.name);
                                articleSearchField.find('.search-article-team-name').css('color', '#D72130');
                                articleSearchField.find('.search-article-subcategory-name').css('color', '');
                                articleSearchField.find('.search-article-category-name').css('color', '');
                            }
                            else {
                                articleSearchField.find('.search-article-subcategory-name').css('color', '#D72130');
                                articleSearchField.find('.search-article-team-name').text('');
                            }
                        }
                        else {
                            articleSearchField.find('.search-article-category-name').css('color', '#D72130');
                            articleSearchField.find('.search-article-team-name').text('');
                            articleSearchField.find('.search-article-subcategory-name').text('');
                            articleSearchField.find('#image-in-search-between-category-subcategory').css('display', 'none');
                        }

                        let fieldFromArticle = articles[i].contentText.replace(/<[^>]*>?/gm, '').substr(0, articleContentTextLength);
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
                        articleSearchField.find('.search-article-bottom-content').html(articleContentText);
                    }
                    $('.search-result-articles').slideDown('');
                }
                else {
                    amountOfArticles = 0;
                    let categoryName = $('.search-article-info:first').find('.search-article-category-name').text().replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "");
                    noResultTop.replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "");
                    let availableArticles = 0;
                    if (categoryName != noResultTop.replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "")) {
                        if (displayedArticles == 0) {
                            var articleSearchField = $('.search-article-info:first')
                                .clone().css('display', 'block')
                                .appendTo('.search-result-articles');
                            articleSearchField.find('.search-article-category-name').text(noResultTop);
                            articleSearchField.find('.search-article-subcategory-name').text('');
                            articleSearchField.find('.search-article-team-name').text('');
                            articleSearchField.find('.search-article-bottom-content').text(noResultBottom);
                            $('.search-result-articles').show();
                        }
                        else {
                            $('.search-result-articles').find('.search-article-info').map(function () {
                                if (availableArticles == 0) {
                                    $(this).css('display', 'block');
                                    $(this).find('.search-article-category-name').text(noResultTop);
                                    $(this).find('.search-article-subcategory-name').text('');
                                    $(this).find('.search-article-team-name').text('');
                                    $(this).find('.search-article-bottom-content').text(noResultBottom);
                                }
                                else {
                                    $(this).remove();
                                }
                                availableArticles++;
                            });
                        }
                    }


                }
                displayedArticles = amountOfArticles;
            }
        });
        //for (var i = 0; i < amountOfArticlesInSearchField; i++) {
        //    $('.search-result-articles').find('.search-article-info').first().remove();
        //}
        amountOfArticlesInSearchField = amountToDelete;
    }
    else {
        displayedArticles = 0;
        let variableForDelete = 0;
        $(".search-result-articles").slideUp('', function () {
            let unfoundedElements = $('.search-result-articles').find('.search-article-info').last()
            $('.search-result-articles').empty();
            $('.search-result-articles').hide();
            //unfoundedElements.appendTo('.search-result-articles');
        });
    }
}

function searchScroll() {
    let hiddenDivSize = $('.search-result-articles')[0].scrollHeight;
    let visibleDivSize = $('.search-result-articles').height();
    let scrollHeight = hiddenDivSize - visibleDivSize;
    let scrollPosition = $('.search-result-articles').scrollTop();
    if ((visibleDivSize + scrollPosition) / hiddenDivSize > 0.8) {
        updateSearchAfterScrolling();
        hiddenDivSize = $('.search-result-articles')[0].scrollHeight;
        scrollHeight = visibleDivSize / hiddenDivSize * visibleDivSize;
    }
}

function updateSearchAfterScrolling() {
    let searchValue = $(".lable").val();

    let searchParameters = {
        searchValue: searchValue,
        startPosition: startElementPosition,
        amountArticles: amountOfElements,
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
                    var articleSearchField = $('.search-article-info:first')
                        .clone().attr('id', `article-with-id-${articles[i].id}`)
                        .appendTo('.search-result-articles');
                    //add category, subcategory, team fields
                    articleSearchField.find('#article-href').attr('href', `/Articles/Details?id=${articles[i].id}`);
                    articleSearchField.find('.search-article-category-name').text(articles[i].referenceItem.name);
                    if (articles[i].referenceItem.parentsItem != null) {
                        articleSearchField.find('.search-article-subcategory-name').text(articles[i].referenceItem.parentsItem.name);
                        if (articles[i].referenceItem.parentsItem.parentsItem != null) {
                            articleSearchField.find('.search-article-team-name').text(articles[i].referenceItem.parentsItem.parentsItem.name);
                            articleSearchField.find('.search-article-team-name').css('color', '#D72130');
                            articleSearchField.find('.search-article-subcategory-name').css('color', '');
                            articleSearchField.find('.search-article-category-name').css('color', '');
                        }
                        else {
                            articleSearchField.find('.search-article-subcategory-name').css('color', '#D72130');
                            articleSearchField.find('.search-article-team-name').text('');
                        }
                    }
                    else {
                        articleSearchField.find('.search-article-category-name').css('color', '#D72130');
                        articleSearchField.find('.search-article-team-name').text('');
                        articleSearchField.find('.search-article-subcategory-name').text('');
                        articleSearchField.find('#image-in-search-between-category-subcategory').css('display', 'none');
                    }

                    let fieldFromArticle = articles[i].contentText.replace(/(\r\n|\n|\r)/gm, '').substr(0, articleContentTextLength);
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
                    articleSearchField.find('.search-article-bottom-content').html(articleContentText);
                    $('.search-result-articles').show();
                }
                if (amountOfArticles == 0) {
                    var articleSearchField = $('.search-article-info:first')
                        .clone().appendTo('.search-result-articles');
                    articleSearchField.find('.search-article-category-name').text('No matches found.');
                    articleSearchField.find('.search-article-bottom-content').text('Please try another search.');
                }
            }
        }
    });
    startElementPosition += amountOfElements;
}

let ignoreClickOnMeElement = document.getElementsByClassName('search-result-articles')[0];
let ignoreClickOnMeElement2 = document.getElementById('header-search-field');
document.addEventListener('click', function (event) {
    let isClickInsideElement = ignoreClickOnMeElement.contains(event.target);
    let isClickInsideElement2 = ignoreClickOnMeElement2.contains(event.target);
    if (!isClickInsideElement && !isClickInsideElement2) {
        $('#search-field-tag').slideUp('fast', function () {
            $('main').css('z-index', '');
        });
    }
});

function moveToSearchPage() {
    const searchValue = $('#header-search-field').val();
    $(location).attr('href', `/search?searchValue=${searchValue}`);
}

var input = document.getElementById("header-search-field");
input.addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        moveToSearchPage();
    }
});