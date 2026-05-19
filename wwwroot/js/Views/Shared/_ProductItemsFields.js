const subTotal = document.getElementById("subTotalPrice");
const totalPrice = document.getElementById("totalPrice");

export function incrementProductQuantity(productId) {
    fetch(`/Shared/IncrementProduct?id=${productId}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            const product = document.querySelector(`.product-item[data-id="${productId}"]`);

            product.querySelector(".product-quantity").textContent = data.productQuantity;
            product.querySelector(".product-total-price").textContent = data.productTotalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            const cartItemsQuantity = document.getElementById("cartItems");
            const orderItemsQuantity = document.getElementById("orderItems");
            const orderTotalItemsQuantity = document.getElementById("orderTotalItems");
            const cartTotalItemsQuantity = document.getElementById("cartTotalItems");

            if (cartItemsQuantity && cartTotalItemsQuantity) {
                cartItemsQuantity.textContent = data.items;
                cartTotalItemsQuantity.textContent = data.totalItems;
            }
            else if (orderItemsQuantity && orderTotalItemsQuantity) {
                orderItemsQuantity.textContent = data.items;
                orderTotalItemsQuantity.textContent = data.totalItems;
            }

            subTotalPriceElement.dataset.base = data.subTotalPrice;
            updateTotalPrice();
        })
};

export function decrementProductQuantity(productId) {
    fetch(`/Shared/DecrementProduct?id=${productId}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            const product = document.querySelector(`.product-item[data-id="${productId}"]`);

            if (data.removed) {

                product.remove();
            }
            else {
                console.log("decrement");
                product.querySelector(".product-quantity").textContent = data.productQuantity;

                product.querySelector(".product-total-price").textContent = data.productTotalPrice.toLocaleString("en-CA", {
                    style: "currency",
                    currency: "CAD"
                });

            }

            const cartItemsQuantity = document.getElementById("cartItems");
            const orderItemsQuantity = document.getElementById("orderItems");
            const orderTotalItemsQuantity = document.getElementById("orderTotalItems");
            const cartTotalItemsQuantity = document.getElementById("cartTotalItems");

            if (cartItemsQuantity && cartTotalItemsQuantity) {
                cartItemsQuantity.textContent = data.items;
                cartTotalItemsQuantity.textContent = data.totalItems;
            }
            else if (orderItemsQuantity && orderTotalItemsQuantity) {
                orderItemsQuantity.textContent = data.items;
                orderTotalItemsQuantity.textContent = data.totalItems;
            }

            subTotalPriceElement.dataset.base = data.subTotalPrice;
            updateTotalPrice();
        })
};
