const openModalButtons = document.querySelectorAll(".button-additional-price");
const modal = document.querySelector(".additional-price-and-instructions-modal");

openModalButtons.forEach(button => {
    const closeButton = modal.querySelector(".close-modal");

    button.onclick = () => {
        const productItem = button.closest(".product-item");

        openEditProductModal({
            id: productItem.dataset.id,
            additionalPrice: productItem.dataset.additionalPrice,
            instructions: productItem.dataset.instructions
        });

    };

    closeButton.onclick = () => {
        modal.style.display = "none";
    }

    window.addEventListener("click", (event) => {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    });
});

function openEditProductModal(product) {

    const editForm = document.getElementById("editProductForm");
    editForm.action = `/Shared/EditProductAdditionalPriceAndInstructions/${product.id}`;

    const freeItemForm = document.getElementById("setFreeItemForm");
    freeItemForm.action = `/Shared/SetFreeItem/${product.id}`;

    document.getElementById("additionalPrice").value = product.additionalPrice ?? "";

    document.getElementById("instructions").value = product.instructions ?? "";

    modal.style.display = "flex";
}
