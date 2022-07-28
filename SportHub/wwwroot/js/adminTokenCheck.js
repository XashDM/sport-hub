function checkAdminToken() {
    const token = localStorage.getItem('Jwt Token');
    if (token) {
        const parseToken = JSON.parse(atob(token.split('.')[1]));

        const today = new Date();
        const currentTime = today.getTime() / 1000 >> 0;
        if (!parseToken.role.includes("Admin") || parseToken.exp < currentTime) {
            window.location.replace("/");
        }
    }
    else {
        window.location.replace("/");
    }
}