function isSmallScreen() {
    return window.matchMedia("(max-width: 768px)").matches;
}

function getRouteContentsFromServer(evt) {
    const route = window.location.pathname !== "/" ? window.location.pathname : "/home";
    const targetElement = document.getElementById('mainContentArea');
    if (targetElement) {
        htmx.ajax('GET', route, {
            target: targetElement,
            swap: 'innerHTML',
            error: (err) => console.error('Failed to load content:', err)
        });
    }
    document.body.removeEventListener('htmx:afterSwap', getRouteContentsFromServer);
}

document.body.addEventListener('htmx:afterSwap', getRouteContentsFromServer);

document.body.addEventListener('htmx:responseError', function (event) {
    document.getElementById('error').innerHTML = event.detail.xhr.response;
});