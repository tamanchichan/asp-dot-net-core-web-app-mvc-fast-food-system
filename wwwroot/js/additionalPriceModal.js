const openModalButtons = document.querySelectorAll(".button-additional-price");

openModalButtons.forEach(button => {
    const modal = button
        .closest(".product-additional-price")
        .querySelector(".additional-price-and-instructions-modal");

    const closeButton = modal.querySelector(".close-modal");

    button.onclick = () => {
        modal.style.display = "flex";
    };

    closeButton.onclick = () => {
        modal.style.display = "none";
    };

    window.addEventListener("click", (event) => {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    });
});
