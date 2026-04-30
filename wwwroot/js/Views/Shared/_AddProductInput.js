const parent = document.querySelector('.product-items');
const input = document.getElementById('addProductToCartInput');

export function addProductInput() {
    const inputValue = input.value.trim();

    fetch(`/Shared/AddProductToCart?input=${inputValue}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            input.value = "";

            const existingProduct = document.querySelector(
                `.product-item[data-id="${data.id}"]`
            );

            if (existingProduct) {
                // optional: just increment quantity instead of duplicating
                const qty = existingProduct.querySelector(".product-quantity");
                qty.textContent = data.quantity;

                return;
            }

            const productDiv = document.createElement("div");
            productDiv.classList.add("product-item");
            productDiv.dataset.id = data.id;

            const itemDiv = document.createElement("div");
            itemDiv.classList.add("item");

            const productCode = document.createElement("span");
            productCode.classList.add("product-code");
            productCode.textContent = data.product.code;

            const productName = document.createElement("span");
            productName.classList.add("product-name");
            productName.textContent = data.product.name;

            const productPrice = document.createElement("span");
            productPrice.classList.add("product-price");
            productPrice.textContent = data.product.price.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

            const additionalPrice = document.createElement("span");
            additionalPrice.classList.add("additional-price");
            additionalPrice.textContent = data.additionalPrice.toLocaleString("en-CA", {
                style: "currency",
                currency: "CAD"
            });

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

            parent.appendChild(productDiv);
        }
    );
}

// This is calling addProductInput, but doesn't call updateTotalPrice; and input seems undefined
input.addEventListener("keydown", async (e) => {
    if (e.key === "Enter") {
        e.preventDefault();

        await addProductInput();
        await updateTotalPrice();

        input.value = "";
    }
});
