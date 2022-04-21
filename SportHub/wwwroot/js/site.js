// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

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
            console.log(token);
            localStorage.setItem('Jwt Token', token);
            window.location.href = '/Index';
        },
        error(errorThrown) {
            console.log(errorThrown);
        }
    });
};


