// Order Type
const orderTypeRadios = document.querySelectorAll('input[name="orderType"]');

const savedOrderType = localStorage.getItem("orderType");

if (savedOrderType) {
    const radioToCheck = document.querySelector(
        `input[name="orderType"][value="${savedOrderType}"]`
    );

    if (radioToCheck) {
        radioToCheck.checked = true;
    }
};

orderTypeRadios.forEach(radio => {
    radio.addEventListener("change", function() {
        localStorage.setItem("orderType", this.value);
    });
});

// Observation
const observationsInput = document.getElementById("observations");
observationsInput.addEventListener("input", () => {
    localStorage.setItem("observations", observationsInput.value);
});
observationsInput.value = localStorage.getItem("observations");

// Customer Name
const customerNameInput = document.getElementById("customerName");
customerNameInput.addEventListener("input", () => {
    localStorage.setItem("customerName", customerNameInput.value);
});
customerNameInput.value = localStorage.getItem("customerName");

// Customer Phone Number
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

// Customer Address
const customerAddressInput = document.getElementById("customerAddress");
customerAddressInput.addEventListener("input", () => {
    localStorage.setItem("customerAddress", customerAddressInput.value)
});
customerAddressInput.value = localStorage.getItem("customerAddress");

// Additional Charge
//const additionalChargeInput = document.getElementById("additionalCharge"); // already define in _OrderForm.js
additionalChargeInput.addEventListener("input", function () {
    localStorage.setItem("additionalCharge", this.value);
});
additionalChargeInput.value = localStorage.getItem("additionalCharge");

// Delivery Fee
deliveryFeeInput.addEventListener("input", function () {
    localStorage.setItem("deliveryFee", this.value);
});
deliveryFeeInput.value = localStorage.getItem("deliveryFee");

if (deliveryFeeInput.value !== "" && deliveryFeeInput.value !== "0") {
    const deliveryRadio = document.getElementById("delivery");
    deliveryRadio.checked = true;
}

// Discount
discountInput.addEventListener("input", function () {
    localStorage.setItem("discount", this.value);
});
discountInput.value = localStorage.getItem("discount");

// Ready Date
const readyDateOnlyInput = document.getElementById("readyDateOnly");
readyDateOnlyInput.addEventListener("input", function () {
    localStorage.setItem("readyDateOnly", this.value);
})

const readyDateOnly = localStorage.getItem("readyDateOnly");

if (readyDateOnly) {
    readyDateOnlyInput.value = localStorage.getItem("readyDateOnly");
}

// Ready Time
const readyTimeOnlyInput = document.getElementById("readyTimeOnly");
readyTimeOnlyInput.addEventListener("input", function () {
    localStorage.setItem("readyTimeOnly", this.value);
})

const readyTimeOnly = localStorage.getItem("readyTimeOnly");

if (readyTimeOnly) {
    readyTimeOnlyInput.value = localStorage.getItem("readyTimeOnly");
}

const orderFormCart = document.getElementById("orderFormCart");

orderFormCart.addEventListener("submit", () => {
    localStorage.removeItem("orderType");
    localStorage.removeItem("observations");
    localStorage.removeItem("customerName");
    localStorage.removeItem("customerPhoneNumber");
    localStorage.removeItem("customerAddress");
    localStorage.removeItem("additionalCharge");
    localStorage.removeItem("deliveryFee");
    localStorage.removeItem("discount");
    localStorage.removeItem("readyDateOnly");
    localStorage.removeItem("readyTimeOnly");
    //localStorage.clear();
})