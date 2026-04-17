const modal = document.getElementById("freeOrderProductModal");
function openFreeOrderProductModal() {

    if (!modal) {
        console.error("Modal not found");
        return;
    }

    if (modal.open) {

        modal.close();
    }

    modal.showModal();
}

modal.addEventListener("click", function(event) {
    const rect = this.getBoundingClientRect();

    if (
        event.clientY < rect.top ||
        event.clientY > rect.bottom ||
        event.clientX < rect.left ||
        event.clientX > rect.right
    ) {
        modal.close();
    }
})

export function handlePlaceOrder() {
    const orderFormCart = document.getElementById("orderFormCart");

    const subTotal = document.getElementById("subTotalPrice").textContent.trim().replace("$", "");

    const friedRice = document.getElementById("friedRice");
    const chickenBalls = document.getElementById("chickenBalls");
    const pepperChicken = document.getElementById("pepperChicken");



    if (subTotal >= 65) {
        openFreeOrderProductModal();
    }
    else if (subTotal >= 55 && subTotal < 65) {
        openFreeOrderProductModal();

        friedRice.checked = true;
        chickenBalls.disabled = true;
        pepperChicken.disabled = true;

    }
    else {
        orderFormCart.submit();
    }
}