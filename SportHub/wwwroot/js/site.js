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

$(document).ajaxSend(function (event, jqxhr, settings) {
    if (settings.url == '/api/Auth/RefreshToken') {
        return;
    }

    refreshToken();
});

function refreshToken () {
    const jwtToken = localStorage.getItem('Jwt Token');
    if (jwtToken) {
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: false,
            url: '/api/Auth/RefreshToken',
            type: 'post',
            data: JSON.stringify({
                'Token': jwtToken,
            }),
            success: function (token) {
                localStorage.setItem('Jwt Token', token);
            },
            error: function (response) {
                console.error(response);
                if (response.isReloginRequired == true) {
                    window.location.href = window.location.origin + '/Login';
                }
            }
        });
    }   
};

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
        success(response) {
            localStorage.setItem('Jwt Token', response.token);
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

function logoutUser() {
    localStorage.removeItem('Jwt Token');
    location.reload();
};

function SendResetMail(email) {
    forgotPassword =
    {
        'Email': email
    }
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        async: false,
        url: '/ForgotPassword',
        type: 'post',
        data: {
            'forgotPassword': forgotPassword,
        },
        success() {
            $("#forgotPassword").hide();
            $("#email").text("Check your email " + email)
            $("#checkYourMail").show();
        },
        error(errorThrown) {
            console.log(errorThrown);
        }
    });
};

$("#setNewPassword").submit((event) => {
    event.preventDefault();
    const password1 = $("#field_newPassword").val().toString();
    const password2 = $("#field_confirmPassword").val().toString();
    if (password1 === password2) {
        let passwordHash = sha256(password1).then((res) => {
            passwordHash = res;
            ResetPassword(passwordHash);
        });
    }
    else {
        $("#reset-result").html('Passwords do not match');
    }
});

function ResetPassword(passwordHash) {
    let urlParams = new URLSearchParams(window.location.search);
    let token = urlParams.get("token");
    $.ajax({
        headers:
        {
            "Authorization": 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: false,
        url: '/api/Users/ResetPassword?passwordHash=' + passwordHash,
        type: 'post',
        data: {
            "passwordHash": passwordHash
        },
        success() {
            $("#reset-result").html('Your password has been updated.').css('color', '#57A902');
        },
        error(errorThrown) {
            console.log(errorThrown);
            $("#reset-result").html('Passwords do not match');

        }
    });
};

$("#requestResetPasswordEmail").submit((event) => {
    event.preventDefault();
    const email = $("#field_email").val().toString();
    SendResetMail(email);
});

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

let activeCategory = null;
let activeSubcategory = null;
let path = "/user/Articles";
function CreateCatgorySidebar() {
    let List = $("#category-sidebar")
    List.empty();
    List.css({
        display: "block"
    })
    let path = "/user/Articles";
    let refElement = $("<a href='/'>Home</a>").css("color", 'rgb(215, 33, 48)');
    $(`<li/>`).mouseenter(function () {
        HideSubcategory();
    })
    .append(refElement).appendTo(List);
    let date = DateGetClass.getCategory();
    console.log(date);
    for (let e in date) {
        let element = date[e];
        let refElement = $(`<a href='${path}/${element.name}'>${element.name}</a>`)
            .mouseenter(function () {
                CreateSubcatgorySidebar(element);
            });
        var li = $(`<li/>`)
            .append(refElement)
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
    activeCategory = e; 
    let date = DateGetClass.getSubcategoryofCategory(e);
    for (let e in date) {
        let element = date[e];
        let refElement = $(`<a href='${path}/${activeCategory.name}/${element.name}'>${element.name}</a>`);
        var li = $(`<li/>`)
            .mouseenter(function () {
                CreateTeamSidebar(element);
            })
            .addClass('item-sidebar')
            .append(refElement)
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
    activeSubcategory = e;
    let date = DateGetClass.getTeamofSubcategory(e);
    for (let e in date) {
        let element = date[e];
        let refElement = $(`<a href='${path}/${activeCategory.name}/${activeSubcategory.name}/${element.name}'>${element.name}</a>`);
        var li = $(`<li/>`)
            .addClass('item-sidebar')
            .append(refElement)
            .appendTo(List);
    }
}
CreateCatgorySidebar();