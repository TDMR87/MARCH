function isSmallScreen() {
    return window.matchMedia("(max-width: 768px)").matches;
}

document.body.addEventListener('htmx:responseError', function (event) {
    document.getElementById('error').innerHTML = event.detail.xhr.response;
});