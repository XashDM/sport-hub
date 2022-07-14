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

function handleGoogleCredentialResponseSignup(googleUser) {
    let token = googleUser.credential;
    sendExternalAuthAjaxRequest(token, true, 'Google');
};

function handleGoogleCredentialResponseSignin(googleUser) {
    let token = googleUser.credential;
    sendExternalAuthAjaxRequest(token, false, 'Google');
};

function sendExternalAuthAjaxRequest(token, isSignup, authProvider, email = null, firstname = null, lastname = null) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true,
        url: '/api/Users/HandleExternalAuth',
        type: 'post',
        data: JSON.stringify({
            'UserToken': token,
            'AuthProvider': authProvider,
            'IsCreationRequired': isSignup,
            'Email': email,
            'FirstName': firstname,
            'LastName': lastname
        }),
        success: function (jwtToken) {
            localStorage.setItem('Jwt Token', jwtToken);
            window.location.href = '/';
        },
        error: function (response) {
            console.error(response);
            if (isSignup) {
                if (response.statusCode === 500) {
                    applySignupResponse('Something went wrong. Try again later', 'responce-danger-text');
                    resetBorderColors();
                    return;
                }
                applySignupResponse(response.responseJSON, 'response-danger-text');
                resetBorderColors();
            }
            else {
                if (response.statusCode === 500) {
                    $('#form-result').text('Something went wrong. Try again later');
                    $('#form-result').show();
                    return;
                }

                $('#form-result').text(response.responseJSON);
                $('#form-result').show();
            }
        }
    });
}

window.fbAsyncInit = function () {
    FB.init({
        appId: '646098686739937',
        oauth: true,
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true // parse XFBML
    });

};

function handleFacebookResponseSignup() {
    FB.login(function (response) {
        if (response.authResponse) {
            const userToken = response.authResponse.accessToken;
            FB.api(
                "/me?fields=name,email",
                function (response) {
                    if (response) {
                        const firstname = response.name.split(' ')[0];
                        const lastname = response.name.split(' ')[1];
                        sendExternalAuthAjaxRequest(userToken, true, 'Facebook', response.email, firstname, lastname);
                    }
                }
            );
        } 
    }, {
        scope: 'public_profile,email'
    });
}

function handleFacebookResponseSignin() {
    FB.login(function (response) {
        if (response.authResponse) {
            const userToken = response.authResponse.accessToken;
            FB.api(
                "/me?fields=name,email",
                function (response) {
                    if (response) {
                        sendExternalAuthAjaxRequest(userToken, false, 'Facebook', response.email);
                    }
                }
            );
        }
    }, {
        scope: 'public_profile,email'
    });
}
(function () {
    var e = document.createElement('script');
    e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
    e.async = true;
    document.getElementById('fb-root').appendChild(e);
}());

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

function logoutUser(){
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