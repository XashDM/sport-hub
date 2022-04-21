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

function ValidateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

function ValidatePassword(value) {
    return /^[A-Za-z0-9\d=!\-@._*]*$/.test(value) // consists of only these
        && /[a-z]/.test(value) // has a lowercase letter
        && /\d/.test(value) // has a digit
}

$("#signup-form").submit((event) => {
    event.preventDefault();
    console.log("dhfjah");
    const firstName = $("#firstname-input").val().toString();
    const lastName = $("#lastname-input").val().toString();
    const emailAddress = $("#email-input").val().toString();
    const password = $('#password-input').val().toString();

    if (!firstName) {
        console.error("Firstname is null");
        return;
    }

    if (!lastName) {
        console.error("Lastname is null");
        return;
    }

    if (!(ValidateEmail(emailAddress) && emailAddress != null)) {
        console.error("Invalid email");
        return;
    }

    if (!ValidatePassword(password)) {
        console.error("Invalid password");
        return;
    }

    let passwordHash = sha256(password).then((res) => {
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