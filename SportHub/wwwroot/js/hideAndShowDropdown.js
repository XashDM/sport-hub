﻿let startElementPosition = 10;
let amountOfElements = 5;

$(document).ready(function () {
    let category = $(location).attr('pathname');
    category = category.substring(16, category.length);
    if (category != "") {
        $('#displayed-category-name').text(decodeURI(category));
    }
    else {
        $('#displayed-category-name').text("All categories");
    }
    let hiddenDivSize = $('.get-admins-articles-scroll-position').height();
    let visibleDivSize = $('.get-admins-articles-container-body').height();
    let scrollHeight = hiddenDivSize - visibleDivSize;
    $(".get-admins-articles-container-body").scroll(function () {
        let scrollPosition = $(".get-admins-articles-container-body").scrollTop();
        if ((scrollHeight + scrollPosition) / hiddenDivSize > 0.95) {
            updateArticlesAfterScrolling();
            hiddenDivSize = $(".get-admins-articles-scroll-position").height();
            scrollHeight = visibleDivSize / hiddenDivSize * visibleDivSize;
        }
    });
});


function updateArticlesAfterScrolling() {
    let category = $(location).attr('pathname');
    category = category.substring(16, category.length);
    if (category == "") {
        category = null;
    }
    let selectedSubcategory = $('.get-admins-articles-subcategory-select').find(":selected").text();
    if (selectedSubcategory == 'All Subcategories') {
        selectedSubcategory = null;
    }
    let selectedTeam = $('.get-admins-articles-team-select').find(":selected").text();
    if (selectedTeam == 'All Teams') {
        selectedTeam = null;
    }
    var selectedPublish = $('.get-admins-articles-publish-unpublish-select').find(":selected").text();
    if (selectedPublish == 'All') {
        selectedPublish = null;
    }

    let articleDisplayParameters = {
        startPosition: startElementPosition,
        amountArticles: amountOfElements,
        publishValue: selectedPublish,
        category: category,
        subcategory: selectedSubcategory,
        team: selectedTeam
    };

    $.ajax({
        method: 'post',
        url: '/articles',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(articleDisplayParameters),
        success: function (articles) {
            amountOfArticles = articles.length;
            console.log(articles[0]);
            
            for (var i = 0; i < amountOfArticles; i++) {
                var articleField = $('.get-admins-articles-elements:first')
                    .clone().attr('id', `article-with-id-${articles[i].id}`)
                    .insertAfter("div.get-admins-articles-elements:last");
                articleField.find('.get-admins-articles-elements').attr('id', `article-with-id-${articles[i].id}`);
                articleField.find('.get-admins-articles-image').attr('src', articles[i].imageLink);
                articleField.find('.get-admins-articles-image-container-a').attr('href', `/Articles/Details?id=${articles[i].id}`);
                articleField.find('.get-admins-articles-title').text(articles[i].title);
                articleField.find('.get-admins-articles-content-text').text(articles[i].contentText);

                let articleNavigation = articles[i].referenceItem;
                let articleInfo = " / ";
                if (articleNavigation != null) {
                    if (articleNavigation.type == "Team") {
                        articleInfo = articleInfo + articleNavigation.name;
                        let referenceItem1 = articleNavigation.parentsItem;
                        if (referenceItem1.type == "Subcategory") {
                            articleInfo = referenceItem1.name + articleInfo;
                        }
                    }
                    if (articleNavigation.type == "Subcategory") {
                        articleInfo = articleNavigation.name + articleInfo;
                    }
                }
                articleField.find('.get-admins-articles-subcategory-team').text(articleInfo);

                articleField.find('.get-admins-articles-dropdown-content').attr('id', `${articles[i].id}`);
                articleField.find('.get-admins-articles-drop-btn')
                    .attr('onclick', `openDropdownFunction(${articles[i].id})`);
                articleField.find('.get-admins-articles-publish-button')
                    .attr('onclick', `publishUnpublish(${articles[i].id})`);
                articleField.find('.get-admins-articles-publish-button').attr(
                    'id', `isPublishedButton-${articles[i].id}`);
                articleField.find('.get-admins-articles-published-info-outside').attr('id', `articlePublishFooter-${articles[i].id}`);
                articleField.find('.get-admins-articles-published-info').attr('id', `publishedForUnpublished-${articles[i].id}`);
                articleField.find('.get-admins-articles-delete').attr('onclick', `deleteArticleFunction(${articles[i].id})`);
                if (articles[i].isPublished == false) {
                    articleField.find('.get-admins-articles-published-info').css("display", "none");
                    articleField.find('.get-admins-articles-publish-button div')
                        .text('Publish');
                    articleField.find('.get-admins-articles-edit').attr('id', `edit-${articles[i].id}`);
                    articleField.find('.get-admins-articles-edit').css("display", "");
                }
                else {
                    articleField.find('.get-admins-articles-published-info').css("display", "");
                    articleField.find('.get-admins-articles-publish-button div')
                        .text('Unpublish');
                    articleField.find('.get-admins-articles-edit').attr('id', `edit-${articles[i].id}`);
                    articleField.find('.get-admins-articles-edit').css("display", "none");
                }  
            } 
            startElementPosition += amountOfElements;
        }
    });
}

function openDropdownFunction(articleId) {
    document.getElementById(articleId.toString()).classList.toggle("show");
}

function findHideSearchField() {
    console.log("Yes");
    $("#search-field").toggle();
    $("#search-field").focus();
}

function deleteArticleFunction(articleId) {
    $.ajax({
        dataType: "json",
        method: "delete",
        url: `/article/delete/${articleId}`,
        success: function (result) {
            document.getElementById(`article-with-id-${articleId}`).style.display = "none";
        }
    });
}

function publishUnpublish(articleId) {
    $.ajax({
        dataType: "json",
        method: "put",
        url: `/article/publishunpublish/${articleId}`,
        success: function (result) {
            console.log(result.isPublished);
            if (result.isPublished) {
                $(`#isPublishedButton-${articleId}`).html("<div>Unpublish</div>");
                $(`#articlePublishFooter-${articleId}`).show();
                $(`#publishedForUnpublished-${articleId}`).show();
                $(`#edit-${articleId}`).hide();
                var publishDropdown = document.getElementById("publish-dropdown-list");
                var publishText = publishDropdown.options[publishDropdown.selectedIndex].text;
                console.log(publishText);
                if (publishText == "Unpublished") {
                    $(`#article-with-id-${articleId}`).hide();
                }
                if (publishText == "Published") {
                    $(`#article-with-id-${articleId}`).show();
                }
            }
            else {
                $(`#isPublishedButton-${articleId}`).html("<div>Publish</div>");
                $(`#articlePublishFooter-${articleId}`).hide();
                $(`#edit-${articleId}`).show();
                var publishDropdown = document.getElementById("publish-dropdown-list");
                var publishText = publishDropdown.options[publishDropdown.selectedIndex].text;
                console.log(publishText);
                if (publishText == "Published") {
                    $(`#article-with-id-${articleId}`).hide();
                }
                if (publishText == "Unpublished") {
                    $(`#article-with-id-${articleId}`).show();
                }
            }
        }
    });
}