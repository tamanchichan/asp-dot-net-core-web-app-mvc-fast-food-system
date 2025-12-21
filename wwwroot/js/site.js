function keepScrollPosition() {
    // Save scroll position before leaving or reloading the page
    window.addEventListener("beforeunload", () => {
        sessionStorage.setItem("scrollPosition", window.scrollY);
    });

    // Restore scroll position after page load
    window.addEventListener("DOMContentLoaded", () => {
        const scrollY = sessionStorage.getItem("scrollPosition");
        if (scrollY !== null) {
            window.scrollTo(0, parseInt(scrollY, 10));
            sessionStorage.removeItem("scrollPosition");
        }
    });
}

// Call the function to activate it to work throughout the site
// keepScrollPosition();

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".save-scroll-position").forEach((link) => {
        link.addEventListener("click", () => {
            sessionStorage.setItem("scrollPosition", window.scrollY);
        });
    });

    const scrollY = sessionStorage.getItem("scrollPosition");
    if (scrollY !== null) {
        window.scrollTo(0, parseInt(scrollY, 10));
        sessionStorage.removeItem("scrollPosition");
    }
});
