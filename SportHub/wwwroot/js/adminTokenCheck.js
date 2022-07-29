var token = localStorage.getItem('Jwt Token');
if (token) {
    var parseToken = JSON.parse(atob(token.split('.')[1]));
}
else {
    window.location.replace("/");
}

var today = new Date();
var currentTime = today.getTime() / 1000 >> 0;
if (!parseToken.role.includes("Admin") || parseToken.exp < currentTime) {
    window.location.replace("/");
}