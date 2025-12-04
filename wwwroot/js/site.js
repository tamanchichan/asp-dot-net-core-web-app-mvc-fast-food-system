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

// Call the function to activate it
keepScrollPosition();
