﻿var token = localStorage.getItem('Jwt Token');
if (token) {
    var parseToken = JSON.parse(atob(token.split('.')[1]));
}

if (!parseToken.role.includes("Admin")) {
    window.location.replace("/");
}