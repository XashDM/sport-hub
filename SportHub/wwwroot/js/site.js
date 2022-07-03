// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

// Write your JavaScript code.


async function sha256(message) {
    const msgBuffer = new TextEncoder().encode(message);
    const hashBuffer = await crypto.subtle.digest('SHA-256', msgBuffer);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map((b) => b.toString(16).padStart(2, '0')).join('');
    return hashHex;
}

$("#loginForm").submit((event) => {
    event.preventDefault();
    const emailAddress = $("#field_email").val().toString();
    let passwordHash = sha256($('#field_password').val().toString()).then((res) => {
        passwordHash = res;
        Login(emailAddress, passwordHash);
    });
})

function Login(email, passwordHash) {
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        async: false,
        url: '/Login',
        type: 'post',
        data: {
            'Email': email,
            'PasswordHash': passwordHash
        },
        success(token) {
            localStorage.setItem('Jwt Token', token);
            window.location.href = '/Index'
        },
        error(errorThrown) {
            console.log(errorThrown);
            $("#form-result").html('Incorrect user ID or password. Try again.');
        }
    });
};

function userData() {
    var token = localStorage.getItem('Jwt Token');
    if (token) {
        var parseToken = JSON.parse(atob(token.split('.')[1]));
    }
    return parseToken;
}

function displayData() {
    console.log(userData());
    document.getElementById("fullName").textContent = userData().name + ' ' + userData().family_name;
    document.getElementById("email").textContent = userData().email;
    document.getElementById("username").textContent = userData().name + ' ' + userData().family_name;
}

function displayLogInOut() {
    var token = localStorage.getItem('Jwt Token');
    if (token) {
        var parseToken = JSON.parse(atob(token.split('.')[1]));
        if (Date.now() < parseToken['exp'] * 1000) {
            $("#buttons").hide();
            $("#profile").show();
        }
        else {
            window.clearInterval(timer);
            $("#buttons").show();
            $("#profile").hide();
        }
    }
    else {
        $("#buttons").show();
        $("#profile").hide();
        window.localStorage.removeItem("Jwt Token");
    }
}

//search-results
let startElementPosition = 10;
let amountOfElements = 2;
function searchField() {
    if ($('.search-result-articles').css('display') != "block") {
        startElementPosition = 10;
        let searchValue = $(".lable").val();
        $(".search-result-articles").empty();

        let searchParameters = {
            searchValue: searchValue,
            startPosition: 0,
            amountArticles: 10,
        };
        if (searchValue != "") {
            $.ajax({
                method: 'post',
                url: '/api/Articles/SearchArticlesRange',
                contentType: 'application/json',
                data: JSON.stringify(searchParameters),
                success: function (articles) {
                    amountOfArticles = articles.length;

                    for (var i = 0; i < amountOfArticles; i++) {
                        var articleSearchField = $('.search-article-info:first')
                            .clone().attr('id', `article-with-id-${articles[i].category}`)
                            .appendTo('.search-result-articles');

                        //add category, subcategory, team fields

                        articleSearchField.find('.search-article-category-name').text(articles[i].category);
                        articleSearchField.find('.search-article-subcategory-name').text(articles[i].subcategory);
                        if (articles[i].subcategory == "") {
                            articleSearchField.find('.search-article-category-name').css('color', '#D72130');
                        }
                        articleSearchField.find('.search-article-team-name').text(articles[i].team);
                        if (articles[i].team == "") {
                            articleSearchField.find('.search-article-subcategory-name').css('color', '#D72130');
                        }
                        if (articles[i].team != "") {
                            articleSearchField.find('.search-article-team-name').css('color', '#D72130');
                            articleSearchField.find('.search-article-subcategory-name').css('color', '');
                            articleSearchField.find('.search-article-category-name').css('color', '');
                        }
                        articleSearchField.find('.search-article-bottom-content').text(articles[i].contentText);
                        $('.search-result-articles').show();
                    }
                }
            });
        }
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
        url: '/api/Articles/SearchArticlesRange',
        contentType: 'application/json',
        data: JSON.stringify(searchParameters),
        success: function (articles) {
            amountOfArticles = articles.length;

            for (var i = 0; i < amountOfArticles; i++) {
                var articleSearchField = $('.search-article-info:first')
                    .clone().attr('id', `article-with-id-${articles[i].category}`)
                    .appendTo('.search-result-articles');

                //add category, subcategory, team fields

                articleSearchField.find('.search-article-category-name').text(articles[i].category);
                articleSearchField.find('.search-article-subcategory-name').text(articles[i].subcategory);
                if (articles[i].subcategory == "") {
                    articleSearchField.find('.search-article-category-name').css('color', '#D72130');
                }
                articleSearchField.find('.search-article-team-name').text(articles[i].team);
                if (articles[i].team == "") {
                    articleSearchField.find('.search-article-subcategory-name').css('color', '#D72130');
                }
                if (articles[i].team != "") {
                    articleSearchField.find('.search-article-team-name').css('color', '#D72130');
                    articleSearchField.find('.search-article-subcategory-name').css('color', '');
                    articleSearchField.find('.search-article-category-name').css('color', '');
                }
                articleSearchField.find('.search-article-bottom-content').text(articles[i].contentText);
                $('.search-result-articles').show();
            }
        }
    });
    startElementPosition += amountOfElements;
}

var ignoreClickOnMeElement = document.getElementsByClassName('search-result-articles')[0];

document.addEventListener('click', function (event) {
    var isClickInsideElement = ignoreClickOnMeElement.contains(event.target);
    if (!isClickInsideElement) {
        $('.search-result-articles').hide();
    }
});

function moveToSearchPage() {
    const searchValue = $('#header-search-field').val();
    console.log(searchValue);
    $(location).prop('href', `/search?value=${searchValue}`);
}

// for sidebar
let timer1;
let timer2;
let DateGetClass = new ExtendDateClass();
$("#team-side, #subcategory-sidebar").mouseleave(function () {
    timer1 = setTimeout(HideTeam, 10);
}).mouseenter(function () {
    clearTimeout(timer1);
});
$("#category-sidebar, #subcategory-side, #team-side").mouseleave(function () {
    timer2 = setTimeout(HideSubcategory, 10);
}).mouseenter(function () {
    clearTimeout(timer2);
});
function HideTeam() {
    console.log('Team left');
    $("#team-side").css({
        display: "none"
    });
}
function HideSubcategory() {
    console.log('Subcategory left');
    $("#subcategory-side").css({
        display: "none"
    });
    $("#blureid").css({
        display: "none"
    });
}
function CreateCatgorySidebar() {
    let List = $("#category-sidebar")
    List.empty();
    List.css({
        display: "block"
    })
    let date = DateGetClass.getCategory();
    console.log(date);
    for (let e in date) {
        let element = date[e];
        var li = $(`<li/>`)
            .text(element.name)
            .mouseenter(function () {
                CreateSubcatgorySidebar(element);
            })
            .appendTo(List);
    }
}
function CreateSubcatgorySidebar(e) {
    let List = $("#subcategory-sidebar");
    List.empty();
    $("#subcategory-side").css({
        display: "block"
    });
    $("#blureid").css({
        display: "block"
    });
    let date = DateGetClass.getSubcategoryofCategory(e);
    for (let e in date) {
        let element = date[e];
        var li = $(`<li/>`)
            .text(element.name)
            .addClass('item-sidebar')
            .mouseenter(function () {
                CreateTeamSidebar(element);
            })
            .appendTo(List);
    }
}
function CreateTeamSidebar(e) {
    let List = $("#team-sidebar");
    List.empty();
    $("#team-side").css({
        display: "block"
    })
    $("#blureid").css({
        display: "block"
    });
    let date = DateGetClass.getTeamofSubcategory(e);
    for (let e in date) {
        let element = date[e];
        var li = $(`<li/>`)
            .text(element.name)
            .addClass('item-sidebar')
            .appendTo(List);
    }
}
CreateCatgorySidebar();