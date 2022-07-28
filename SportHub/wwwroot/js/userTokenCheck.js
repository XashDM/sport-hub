function checkUserToken() {
    const token = localStorage.getItem('Jwt Token');

    if (token) {
        const parseToken = JSON.parse(atob(token.split('.')[1]));
        const today = new Date();
        const currentTime = today.getTime() / 1000 >> 0;

        if (parseToken.exp < currentTime) {
            localStorage.removeItem('Jwt Token');
            location.reload();
        }
    }
}