var counter = 0;
var mainArticles;
var mainTeams;

function setMainArticlesList(articles, teams) {
    $("#num-0").toggleClass("red-accent");
    mainArticles = articles;
    mainTeams = teams;
    console.log(mainArticles);
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
