﻿let amountOfArticlesInSearchFieldSearchPage = 0;
let startElementPosition = 10;
let amountOfArticlesAdded = 5;

function scrollCheck() {
    let hiddenDivSize = $('.user-articles-bottom-articles')[0].scrollHeight;
    let visibleDivSize = $('.user-articles-bottom-articles').height();
    let scrollHeight = hiddenDivSize - visibleDivSize;
    let scrollPosition = $('.user-articles-bottom-articles').scrollTop();
    if ((visibleDivSize + scrollPosition) / hiddenDivSize > 0.9) {
        updateAfterScrolling();
        hiddenDivSize = $('.user-articles-bottom-articles')[0].scrollHeight;
        scrollHeight = visibleDivSize / hiddenDivSize * visibleDivSize;
        console.log(hiddenDivSize, visibleDivSize, scrollPosition);
    }
}

function updateAfterScrolling() {
    let subcategory = $('#subcategory').text()
    if (subcategory == "") {
        subcategory = null;
    }

    let team = $('#team').text()
    if (team == "") {
        team = null;
    }

    let searchParameters = {
        startPosition: startElementPosition,
        amountOfArticles: amountOfArticlesAdded,
        Category: $('#category').text(),
        Subcategory: subcategory,
        Team: team
    };
    $.ajax({
        method: 'post',
        url: '/api/Articles/SearchUserArticlesRange',
        contentType: 'application/json',
        data: JSON.stringify(searchParameters),
        success: function (articles) {
            if (articles.length != 0) {
                amountOfArticles = articles.length;
                for (var i = 0; i < amountOfArticles; i++) {
                    console.log(articles[i].title);
                    var articleCopied = $('.user-articles-elements:first')
                        .clone().attr('id', `article-with-id-${articles[i].id}`)
                        .attr('href', `/Articles/Details?id=${articles[i].id}`)
                        .appendTo('.user-articles-scroll-position');
                    articleCopied.find('.user-articles-title').text(articles[i].title);
                    articleCopied.find('.user-articles-content-text').text(articles[i].contentText);
                    articleCopied.find('.user-articles-bottom-image').attr('src', articles[i].imageItem.imageLink);
                    articleCopied.find('.user-articles-bottom-image').attr('alt', articles[i].imageItem.alt);
                }
            }
        }
    });
    startElementPosition += amountOfArticlesAdded;
}