var token = localStorage.getItem('Jwt Token');
if (token) {
    var parseToken = JSON.parse(atob(token.split('.')[1]));
}

if (parseToken.role == "Admin") {
    console.log("u a an admin");
    $('.switch-button-to-admin-view').show();
    console.log($('.switch-button-to-admin-view').css('display'));
}