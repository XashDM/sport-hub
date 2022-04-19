// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const { error } = require("jquery");

// Write your JavaScript code.


async function sha256(message) {
    const msgBuffer = new TextEncoder().encode(message);
    const hashBuffer = await crypto.subtle.digest('SHA-256', msgBuffer);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map((b) => b.toString(16).padStart(2, '0')).join('');
    return hashHex;
}

$("#signup-form").submit((event) => {
    event.preventDefault();

    const firstName = $("#firstname-input").val().toString();
    const lastName = $("#lastname-input").val().toString();
    const emailAddress = $("#email-input").val().toString();

    let passwordHash = sha256($('#password-input').val().toString()).then((res) => {
        passwordHash = res;
        SignUp(firstName, lastName, emailAddress, passwordHash);
    });
})

function SignUp(firstName, lastName, email, passwordHash) {
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        async: false,
        url: '/Signup',
        type: 'post',
        data: {
            'FirstName': firstName,
            'LastName': lastName,
            'Email': email,
            'PasswordHash': passwordHash
        }
    });
};

function successCallback(responce) {
    localStorage.setItem("efqf", responce);
}