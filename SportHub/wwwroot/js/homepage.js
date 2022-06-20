var counter = 0;
var displayItems;

function getMainArticles() {
    $.get(
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: true,
            type: 'GET',
            url: '/api/Articles/GetDisplayedMainArticles',
            dataType: "json",
            success: function (data) {
                setMainArticlesList(data);
            },
            error: function (response) {
                console.error(response);
            }
        });
}

function setMainArticlesList(articles) {
    console.log(articles);
    displayItems = articles;
    var newsbox = $("#main-news-0");
    for (i = 1; i < displayItems.length; i++) {
        newsbox.clone().appendTo($("#main-article-list")).attr("id", `main-news-${i}`);
    }

    for (i = 1; i <= displayItems.length; i++) {
        $("#right-button").before(`<p id="num-${i-1}">0${i}</p>`);
    }
    $("#num-0").toggleClass("red-accent");
    for (i = 0; i < displayItems.length; i++) {
        var currentElement = $(`#main-news-${i}`)
        currentElement.find("a").attr("href", `/Articles/Details?id=${displayItems[i].article.id}`);
        currentElement.find(".medium-news-img").attr("src", `${displayItems[i].article.imageItem.imageLink}`);
        currentElement.find(".h3-heading-text").text(displayItems[i].article.title);
        currentElement.find(".caption").text(displayItems[i].article.contentText.substring(0, 50));
    }
    setMainArticle();
}

function setMainArticle() {
    $(".main-article-photo").finish().fadeOut("5", function () {
        $(this).attr("src", displayItems[counter].article.imageItem.imageLink)
    }).fadeIn("5");
    $("#main-article-team-tag").html("<p>" + displayItems[counter].article.referenceItem.name + "</p>");
    $("#banner-published-date").text(displayItems[counter].article.postedDate);
    $("#banner-title").text(displayItems[counter].article.title);
    $(".main-bottom-info").text(displayItems[counter].article.contentText);
    $("#banner-more-button").attr("href", `/Articles/Details?id=${displayItems[counter].article.id}`)
}

function nextImage() {
    $("#num-" + counter).toggleClass("red-accent");
    if (counter < displayItems.length - 1) {
        counter += 1;
    }
    else {
        counter = 0;
    }
    $("#num-" + counter).toggleClass("red-accent");
    setMainArticle();
}

function prevImage() {
    $("#num-" + counter).toggleClass("red-accent");
    if (counter > 0) {
        counter -= 1;
    }
    else {
        counter = displayItems.length - 1;
    }
    $("#num-" + counter).toggleClass("red-accent");
    setMainArticle();
}

