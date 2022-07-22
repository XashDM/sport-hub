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

$(".switch-arrow").on({
    mouseenter: function () {
        $('.message-box-with-arrow').show();
    },
    mouseleave: function () {
        $('.message-box-with-arrow').hide();
    }
});

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

    startElementPosition = 10;
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

                            let fieldFromArticle = articles[articleIndex].contentText;
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
                            $(this).find('.search-article-bottom-content').html(articleContentText.substr(0, articleContentTextLength));

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
                        articleSearchField.find('.search-article-bottom-content').html(articleContentText.substr(0, articleContentTextLength));
                    }
                    $('.search-result-articles').slideDown('');
                }
                else {
                    amountOfArticles = 0;
                    console.log("no result");
                    let categoryName = $('.search-article-info:first').find('.search-article-category-name').text().replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "");
                    console.log(categoryName, noResultTop.replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, ""));
                    let availableArticles = 0;
                    if (categoryName == noResultTop.replace(/ /g, '').replace(/(\r\n|\n|\r)/gm, "")) {
                        console.log("asdasd");
                    }
                    else {
                        if (displayedArticles == 0) {
                            var articleSearchField = $('.search-article-info:first')
                                .clone().css('display', 'block')
                                .appendTo('.search-result-articles');
                            articleSearchField.find('.search-article-category-name').text(noResultTop);
                            articleSearchField.find('.search-article-subcategory-name').text('');
                            articleSearchField.find('.search-article-team-name').text('');
                            articleSearchField.find('.search-article-bottom-content').text(noResultBottom);
                            $('.search-result-articles').show();
                            console.log("add some");
                        }
                        else {
                            $('.search-result-articles').find('.search-article-info').map(function () {
                                if (availableArticles == 0) {
                                    $(this).css('display', 'block');
                                    $(this).find('.search-article-category-name').text(noResultTop);
                                    $(this).find('.search-article-subcategory-name').text('');
                                    $(this).find('.search-article-team-name').text('');
                                    $(this).find('.search-article-bottom-content').text(noResultBottom);
                                    console.log($(this));
                                }
                                else {
                                    $(this).remove();
                                }
                                availableArticles++;
                            });
                            console.log(availableArticles);
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
                    articleSearchField.find('.search-article-bottom-content').html(articleContentText.substr(0, articleContentTextLength));
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

function checkIsInputActive() {
    let searchValue = $(".lable").val();
    if (searchValue != "" && $('#search-field-tag').css('display') == 'none') {
        console.log("Working(no)");
        console.log($('#search-field-tag').css('display'));
    }
}

function moveToSearchPage() {
    const searchValue = $('#header-search-field').val();
    console.log(searchValue);
    $(location).attr('href', `/search?searchValue=${searchValue}`);
}

var input = document.getElementById("header-search-field");
input.addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        moveToSearchPage();
    }
});


function logoutUser() {
    localStorage.removeItem('Jwt Token');
    location.reload();
};

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