export function clearCartProducts() {
    fetch("/Cart/ClearCartProducts", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.redirected) {
                localStorage.clear();
                window.location.href = response.url;
            } else {
                window.location.reload();
            }
        })
        .catch(error => {
            console.error("Error clearing cart:", error);
        });
}