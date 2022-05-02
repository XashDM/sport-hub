function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

function validatePassword(value) {
    return /^[A-Za-z0-9\d=!\-@._*]*$/.test(value)
        && /[a-z]/.test(value)
        && /\d/.test(value)
}

function applySignupResponse(responseText, cssClass) {
    $('#signup-response')
        .text(responseText)
        .css('visibility', 'visible')
        .addClass(cssClass);
}

function setBorderColor(elemId, color) {
    $(elemId).css('border', `${color} 1px solid`);
}

function resetBorderColors() {
    setBorderColor('#firstname-input', 'var(--fieldframe-light)');
    setBorderColor('#lastname-input', 'var(--fieldframe-light)');
    setBorderColor('#email-input', 'var(--fieldframe-light)');
    setBorderColor('#password-input', 'var(--fieldframe-light)');
}

$('#signup-form').submit((event) => {
    event.preventDefault();

    const firstName = $('#firstname-input').val().toString();
    const lastName = $('#lastname-input').val().toString();
    const emailAddress = $('#email-input').val().toString();
    const password = $('#password-input').val().toString();

    if (!(firstName && lastName && emailAddress && password)) {
        applySignupResponse('Marked fields cannot be empty.', 'response-danger-text');
        setBorderColor('#firstname-input', 'var(--red-main)');
        setBorderColor('#lastname-input', 'var(--red-main)');
        setBorderColor('#email-input', 'var(--red-main)');
        setBorderColor('#password-input', 'var(--red-main)');
        return;
    }

    if (!validateEmail(emailAddress)) {
        applySignupResponse('Provided email is incorrect.', 'response-danger-text');
        resetBorderColors();
        setBorderColor('#email-input', 'var(--red-main)');
        return;
    }

    if (!validatePassword(password)) {
        applySignupResponse('Provided password is incorrect.', 'response-danger-text');
        resetBorderColors();
        setBorderColor('#password-input', 'var(--red-main)');
        return;
    }

    let passwordHash = sha256(password).then((res) => {
        passwordHash = res;
        signUp(firstName, lastName, emailAddress, passwordHash);
    });
})

function signUp(firstName, lastName, email, passwordHash) {
    $.ajax({
        headers:
        {
            'RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        async: true,
        url: '/Signup',
        type: 'post',
        data: {
            'FirstName': firstName,
            'LastName': lastName,
            'Email': email,
            'PasswordHash': passwordHash
        },
        success: function (response) {
            window.location.href = '/Login';
        },
        error: function (response) {
            applySignupResponse(response.responseJSON['error'], 'response-danger-text');
            resetBorderColors();
            setBorderColor('#email-input', 'var(--red-main)');
        }
    });
};