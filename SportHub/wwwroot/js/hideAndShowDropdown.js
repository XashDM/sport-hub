let startElementPosition = 10;
let amountOfElements = 5;

function test() {
    var template = $("#template")
        .clone().removeAttr("id")
        .removeClass("hidden")
        .html();

    console.log(template);

    // re-define `template`
    template = $(template).filter("#username")
        .attr("id", "username-" + $("[id^=username]").length)
        .html("new user");

    console.log(template[0].outerHTML);

    $("#container").prepend(template)
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