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
        url: '/api/Auth/HandleExternalAuth',
        type: 'post',
        data: JSON.stringify({
            'UserToken': token,
            'AuthProvider': authProvider,
            'IsCreationRequired': isSignup,
            'Email': email,
            'FirstName': firstname,
            'LastName': lastname
        }),
        success: function (token) {
            localStorage.setItem('Jwt Token', token);
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