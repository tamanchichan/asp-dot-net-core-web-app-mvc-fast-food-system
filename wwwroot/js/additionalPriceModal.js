const openModalButtons = document.querySelectorAll(".button-additional-price");
const modal = document.querySelector(".additional-price-and-instructions-modal");

openModalButtons.forEach(button => {
    const closeButton = modal.querySelector(".close-modal");

    button.onclick = () => {
        const productItem = button.closest(".product-item");
        console.log("data-options:", productItem.dataset.options);

        openEditProductModal({
            id: productItem.dataset.id,
            additionalPrice: productItem.dataset.additionalPrice,
            instructions: productItem.dataset.instructions,
            options: JSON.parse(productItem.dataset.options || "[]")
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

    // Reset and update checkboxes
    editForm.querySelectorAll('input[type="checkbox"]').forEach(checkbox => {
        const option = checkbox.value.toUpperCase();
        const name = checkbox.name;

        let formattedOption = "";

        switch (name) {
            case "onSideOptions":
                formattedOption = `${option} ON SIDE`;
                break;

            case "onTopOptions":
                formattedOption = `${option} ON TOP`;
                break;

            case "extraOptions":
                formattedOption = `EXTRA ${option}`;
                break;

            case "lessOptions":
                formattedOption = `LESS ${option}`;
                break;

            case "onlyOptions":
                formattedOption = `ONLY ${option}`;
                break;

            case "noOptions":
                formattedOption = `NO ${option}`;
                break;
        }

        checkbox.checked = product.options.some(
            o => o.toUpperCase() === formattedOption
        );
    });

    modal.style.display = "flex";
}
