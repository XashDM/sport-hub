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
        if (Date.now() < parseToken['exp']* 1000) {
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

