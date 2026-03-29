const subTotal = document.getElementById("subTotalPrice");
const totalPrice = document.getElementById("totalPrice");

export function incrementProductQuantity(productId) {
    fetch(`/Cart/IncrementProduct?id=${productId}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            const product= document.querySelector(`.product-item[data-id="${productId}"]`);

            product.querySelector(".product-quantity").textContent = data.quantity;
            product.querySelector(".product-total-price").textContent = data.productTotalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            subTotal.textContent = data.subTotalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            totalPrice.textContent = data.totalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });
        })
};

export function decrementProductQuantity(productId) {
    fetch(`/Cart/DecrementProduct?id=${productId}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            const product = document.querySelector(`.product-item[data-id="${productId}"]`);

            if (data.empty) {
                window.location.href = "/Cart/Index";
            }
            else if (data.removed) {
                product.remove();
            }
            else {
                product.querySelector(".product-quantity").textContent = data.quantity;

                product.querySelector(".product-total-price").textContent = data.productTotalPrice.toLocaleString("en-CA", {
                    style: "currency",
                    currency: "CAD"
                });

            }

            subTotal.textContent = data.subTotalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            totalPrice.textContent = data.totalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });
        })
};
