export function incrementProductQuantity(productId) {
    fetch(`/Cart/IncrementProduct?id=${productId}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            const row = document.querySelector(`.product-item[data-id="${productId}"]`);

            row.querySelector(".product-quantity").textContent = data.quantity;
        })
};

export function decrementProductQuantity(productId) {
    fetch(`/Cart/DecrementProduct?id=${productId}`, {
        method: "POST"
    })
        .then(response => response.json())
        .then(data => {
            const element = document.querySelector(`.product-item[data-id="${productId}"]`);

            element.querySelector(".product-quantity").textContent = data.quantity;
        })
};

export function test() {
    console.log("hello, world");
}

console.log('hello');