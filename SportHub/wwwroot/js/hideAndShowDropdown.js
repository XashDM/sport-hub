let startElementPosition = 10;
let amountOfElements = 5;

$(document).ready(function () {
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
    let selectedSubcategory = $('.get-admins-articles-subcategory-select').find(":selected").text();
    if (selectedSubcategory == 'All Subcategories') {
        selectedSubcategory = '';
    }
    let selectedTeam = $('.get-admins-articles-team-select').find(":selected").text();
    if (selectedTeam == 'All Teams') {
        selectedTeam = '';
    }
    var selectedPublish = $('.get-admins-articles-publish-unpublish-select').find(":selected").text();
    if (selectedPublish == 'All') {
        selectedPublish = '';
    }

    $.ajax({
        dataType: "json",
        method: "post",
        url: `/articles?startPosition=${startElementPosition}&amountArticles=${amountOfElements}&publishValue=${selectedPublish}&category=${category}&subcategory=${selectedSubcategory}&team=${selectedTeam}`,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        data: {
            'startPosition': startElementPosition,
            'amountArticles': amountOfElements,
            'publishValue': 'Published'
        },
        success: function (result) {
            amountOfArticles = result.length;
            console.log(result[0]);
            
            for (var i = 0; i < amountOfArticles; i++) {
                var articleField = $('.get-admins-articles-elements:first')
                    .clone().attr('id', `article-with-id-${result[i].id}`)
                    .insertAfter("div.get-admins-articles-elements:last");
                articleField.find('.get-admins-articles-elements').attr('id', `article-with-id-${result[i].id}`);
                articleField.find('.get-admins-articles-image').attr('src', result[i].imageLink);
                articleField.find('.get-admins-articles-image-container-a').attr('href', `/Articles/Details?id=${result[i].id}`);
                //console.log(articleField.find('.get-admins-articles-image-container-a'));
                articleField.find('.get-admins-articles-title').text(result[i].title);
                articleField.find('.get-admins-articles-content-text').text(result[i].contentText);
                articleField.find('.get-admins-articles-subcategory-team').text(result[i].referenceItemId);
                articleField.find('.get-admins-articles-dropdown-content').attr('id', `${result[i].id}`);
                articleField.find('.get-admins-articles-drop-btn')
                    .attr('onclick', `openDropdownFunction(${result[i].id})`);
                articleField.find('.get-admins-articles-publish-button')
                    .attr('onclick', `publishUnpublish(${result[i].id})`);
                articleField.find('.get-admins-articles-publish-button').attr(
                    'id', `isPublishedButton-${result[i].id}`);
                articleField.find('.get-admins-articles-published-info-outside').attr('id', `articlePublishFooter-${result[i].id}`);
                articleField.find('.get-admins-articles-published-info').attr('id', `publishedForUnpublished-${result[i].id}`);
                articleField.find('.get-admins-articles-delete').attr('onclick', `deleteArticleFunction(${result[i].id})`);
                if (result[i].isPublished == false) {
                    articleField.find('.get-admins-articles-published-info').css("display", "none");
                    articleField.find('.get-admins-articles-publish-button div')
                        .text('Publish');
                    articleField.find('.get-admins-articles-edit').attr('id', `edit-${result[i].id}`);
                    articleField.find('.get-admins-articles-edit').css("display", "");
                }
                else {
                    articleField.find('.get-admins-articles-published-info').css("display", "");
                    articleField.find('.get-admins-articles-publish-button div')
                        .text('Unpublish');
                    articleField.find('.get-admins-articles-edit').attr('id', `edit-${result[i].id}`);
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