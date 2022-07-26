var token = localStorage.getItem('Jwt Token');

if (token) {
    var parseToken = JSON.parse(atob(token.split('.')[1]));
    var today = new Date();
    var currentTime = today.getTime() / 1000 >> 0;

    if (parseToken.exp < currentTime) {
        localStorage.removeItem('Jwt Token');
        location.reload();
    }
}