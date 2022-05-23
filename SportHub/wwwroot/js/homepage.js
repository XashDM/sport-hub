var counter = 0;
var mainArticles;
var mainTeams;

function setMainArticlesList(articles, teams) {
    mainArticles = articles;
    mainTeams = teams;
    console.log(mainArticles);
    var newsbox = $("#main-news-0");
    for (i = 1; i < mainArticles.length; i++) {
        newsbox.clone().appendTo($("#main-article-list")).attr("id", `main-news-${i}`);
    }

    for (i = 1; i <= mainArticles.length; i++) {
        $("#right-button").before(`<p id="num-${i-1}">0${i}</p>`);
    }
    $("#num-0").toggleClass("red-accent");
    for (i = 0; i < mainArticles.length; i++) {
        var currentElement = $(`#main-news-${i}`)
        currentElement.find("a").attr("href", `/Articles/Details?id=${mainArticles[i].id}`);
        currentElement.find(".medium-news-img").attr("src", `${mainArticles[i].imageLink}`);
        currentElement.find(".h3-heading-text").text(mainArticles[i].title);
        currentElement.find(".caption").text(mainArticles[i].contentText.substring(0, 50));
    }
    setMainArticle();
}

function setMainArticle() {
    $(".main-article-photo").finish().fadeOut("5", function () {
        $(this).attr("src", mainArticles[counter].imageLink)
    }).fadeIn("5");
    $("#main-article-team-tag").html("<p>" + mainTeams[counter] + "</p>");
    $("#banner-published-date").text(mainArticles[counter].postedDate);
    $("#banner-title").text(mainArticles[counter].title);
    $(".main-bottom-info").text(mainArticles[counter].contentText);
}

function nextImage() {
    $("#num-" + counter).toggleClass("red-accent");
    if (counter < mainArticles.length - 1) {
        counter += 1;
    }
    else {
        counter = 0;
    }
    $("#num-" + counter).toggleClass("red-accent");
    setMainArticle();
    // console.log(counter);
}

function prevImage() {
    $("#num-" + counter).toggleClass("red-accent");
    if (counter > 0) {
        counter -= 1;
    }
    else {
        counter = mainArticles.length - 1;
    }
    $("#num-" + counter).toggleClass("red-accent");
    setMainArticle();
    // console.log(counter);
}
