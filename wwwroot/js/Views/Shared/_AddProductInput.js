const parent = document.querySelector('.product-items');
const input = document.getElementById('addProductInput');

export function addProductInput(model) {
    const inputValue = input.value.trim();

    let url = `/Shared/AddProductToCart?input=${inputValue}`;

    if (model === "Order") {
        url = `/Shared/AddProductToOrder?orderId=${orderId}&input=${inputValue}`;
    }

    fetch(url, { method: "POST" })
        .then(response => response.json())
        .then(data => {
            input.value = "";

            console.log(data);

            const cartItemsQuantity = document.getElementById("cartItems");
            const orderItemsQuantity = document.getElementById("orderItems");
            const orderTotalItemsQuantity = document.getElementById("orderTotalItems");
            const cartTotalItemsQuantity = document.getElementById("cartTotalItems");

            if (cartItemsQuantity && cartTotalItemsQuantity) {
                cartItemsQuantity.textContent = data.cart.cartProducts.length;
                cartTotalItemsQuantity.textContent = data.cart.quantity;
            }
            else if (orderItemsQuantity && orderTotalItemsQuantity) {
                orderItemsQuantity.textContent = data.order.orderProducts.length;
                orderTotalItemsQuantity.textContent = data.order.quantity;
            }

            const existingProduct = document.querySelector(
                `.product-item[data-id="${data.id}"]`
            );

            if (existingProduct) {
                // optional: just increment quantity instead of duplicating
                const productQuantity = existingProduct.querySelector(".product-quantity");
                productQuantity.textContent = data.quantity;

                const productTotalPrice = existingProduct.querySelector(".product-total-price");
                productTotalPrice.textContent = data.totalPrice.toLocaleString("en-CA", {
                    style: "currency",
                    currency: "CAD"
                });;

                subTotalPriceElement.dataset.base = data.cart.subTotalPrice;
                updateTotalPrice();

                return;
            }

            const productDiv = document.createElement("div");
            productDiv.classList.add("product-item");
            productDiv.dataset.id = data.id;

            const itemDiv = document.createElement("div");
            itemDiv.classList.add("item");

            const productCode = document.createElement("span");
            productCode.classList.add("product-code");
            productCode.textContent = data.code;

            const productName = document.createElement("span");
            productName.classList.add("product-name");
            productName.textContent = data.name;

            if (data.hasOptions) {
                productCode.textContent += data.foodOption.substring(0, 1);
                productName.textContent += ` ${data.foodOption}`;
            }

            const productPrice = document.createElement("span");
            productPrice.classList.add("product-price");
            productPrice.textContent = data.price.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            const additionalPrice = document.createElement("span");
            additionalPrice.classList.add("product-additional-price");
            additionalPrice.textContent = data.additionalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            const editProduct = document.createElement("button");
            editProduct.classList.add("button", "button-additional-price");
            additionalPrice.appendChild(editProduct);

            const editProductIcon = document.createElement("i");
            editProductIcon.classList.add("fa-solid", "fa-pen-to-square");
            editProduct.appendChild(editProductIcon);

            const productQuantity = document.createElement("span");
            productQuantity.classList.add("product-quantity");
            productQuantity.textContent = data.quantity;

            const productTotalPrice = document.createElement("span");
            productTotalPrice.classList.add("product-total-price");
            productTotalPrice.textContent = data.totalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            const incrementButton = document.createElement("button");
            incrementButton.classList.add("button");
            incrementButton.classList.add("product-increment");
            incrementButton.textContent = "+";
            incrementButton.addEventListener("click", () => {
                incrementProductQuantity(data.id)
            });

            const decrementButton = document.createElement("button");
            decrementButton.classList.add("button");
            decrementButton.classList.add("product-decrement");
            decrementButton.textContent = "-";
            decrementButton.addEventListener("click", () => {
                decrementProductQuantity(data.id)
            });

            itemDiv.appendChild(productCode);
            itemDiv.appendChild(productName);
            itemDiv.appendChild(productPrice);
            itemDiv.appendChild(additionalPrice);
            itemDiv.appendChild(productQuantity);
            itemDiv.appendChild(productTotalPrice);
            itemDiv.appendChild(incrementButton);
            itemDiv.appendChild(decrementButton);

            productDiv.appendChild(itemDiv);

            const firstProduct = parent.querySelector(".product-item");

            if (firstProduct) {
                parent.insertBefore(productDiv, firstProduct);
            }
            else {
                parent.appendChild(productDiv);
            }

            if (data.cart?.subTotalPrice != null) {
                subTotalPriceElement.dataset.base = data.cart.subTotalPrice;
            }
            else {
                subTotalPriceElement.dataset.base = data.order.subTotalPrice;
            }

            updateTotalPrice();
        }
    );
}

input.addEventListener("keydown", async (e) => {
    if (e.key === "Enter") {
        e.preventDefault();

        window.addProductInput();
    }
});
