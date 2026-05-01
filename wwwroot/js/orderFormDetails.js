const observationsInput = document.getElementById("observations");
observationsInput.addEventListener("input", () => {
    localStorage.setItem("observations", observationsInput.value);
});
observationsInput.value = localStorage.getItem("observations");

const customerNameInput = document.getElementById("customerName");
customerNameInput.addEventListener("input", () => {
    localStorage.setItem("customerName", customerNameInput.value);
});
customerNameInput.value = localStorage.getItem("customerName");

const customersPhoneNumberDiv = document.querySelector(".customers-phone-number");

const customerPhoneNumberInput = document.getElementById("customerPhoneNumber");

customerPhoneNumberInput.addEventListener("input", async () => {
    const value = customerPhoneNumberInput.value;
    localStorage.setItem("customerPhoneNumber", value);

    if (value.length < 3) {
        customersPhoneNumberDiv.hidden = true;
        customersPhoneNumberDiv.innerHTML = "";
        return;
    }

    const response = await fetch(`/Shared/SearchCustomers?phoneNumber=${value}`);
    const customers = await response.json();

    customersPhoneNumberDiv.innerHTML = "";
    customersPhoneNumberDiv.hidden = customers.length === 0;

    customers.forEach((customer) => {
        const div = document.createElement("div");
        div.style.border = "1px solid #000";
        div.style.cursor = "pointer";
        div.textContent = customer.phoneNumber;

        div.addEventListener("click", () => {
            customerNameInput.value = customer.name;
            customerPhoneNumberInput.value = customer.phoneNumber;
            customerAddressInput.value = customer.address;
            customersPhoneNumberDiv.innerHTML = "";
            customersPhoneNumberDiv.hidden = true;
        });

        customersPhoneNumberDiv.appendChild(div);
    });
});
customerPhoneNumberInput.value = localStorage.getItem("customerPhoneNumber");

const customerAddressInput = document.getElementById("customerAddress");
customerAddressInput.addEventListener("input", () => {
    localStorage.setItem("customerAddress", customerAddressInput.value)
});
customerAddressInput.value = localStorage.getItem("customerAddress");

const orderFormCart = document.getElementById("orderFormCart");

orderFormCart.addEventListener("submit", () => {
    localStorage.removeItem("observations");
    localStorage.removeItem("customerName");
    localStorage.removeItem("customerPhoneNumber");
    localStorage.removeItem("customerAddress");
})