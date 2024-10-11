function isSmallScreen() {
    return window.matchMedia("(max-width: 768px)").matches;
}

function getRouteContentsFromServer(evt) {
    const targetElement = document.getElementById('mainContentArea');
    if (targetElement && window.location.pathname !== '/') {
        htmx.ajax('GET', window.location.pathname, {
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