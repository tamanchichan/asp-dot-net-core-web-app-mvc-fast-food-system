// Order Type
const orderTypeRadios = document.querySelectorAll('input[name="orderType"]');

if (localStorage.getItem("orderType")) {
    const radioToCheck = document.querySelector(
        `input[name="orderType"][value="${localStorage.getItem("orderType")}"]`
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

if (localStorage.getItem("observations")) {
    observationsInput.value = localStorage.getItem("observations");
}

// Customer Name
const customerNameInput = document.getElementById("customerName");
customerNameInput.addEventListener("input", () => {
    localStorage.setItem("customerName", customerNameInput.value);
});

if (localStorage.getItem("customerName")) {
    customerNameInput.value = localStorage.getItem("customerName");
}

// Customer Phone Number
const customersPhoneNumberResultDiv = document.querySelector(".customers-phone-number-result");
const customerPhoneNumberInput = document.getElementById("customerPhoneNumber");
customerPhoneNumberInput.addEventListener("input", async () => {
    const value = customerPhoneNumberInput.value;
    localStorage.setItem("customerPhoneNumber", value);

    if (value.length < 3) {
        customersPhoneNumberResultDiv.hidden = true;
        customersPhoneNumberResultDiv.innerHTML = "";
        return;
    }

    const response = await fetch(`/Shared/SearchCustomers?phoneNumber=${value}`);
    const customers = await response.json();

    customersPhoneNumberResultDiv.innerHTML = "";
    customersPhoneNumberResultDiv.hidden = customers.length === 0;

    customers.forEach((customer) => {
        const div = document.createElement("div");

        div.classList.add("customer-result")

        div.textContent = customer.phoneNumber;

        div.addEventListener("click", () => {
            if (customer.name) {
                customerNameInput.value = customer.name;
                localStorage.setItem("customerName", customer.name);
            }

            if (customer.phoneNumber) {
                customerPhoneNumberInput.value = customer.phoneNumber;
                localStorage.setItem("customerPhoneNumber", customer.phoneNumber);
            }

            if (customer.address) {
                customerAddressInput.value = customer.address;
                localStorage.setItem("customerAddress", customer.address);
            }

            customersPhoneNumberResultDiv.innerHTML = "";
            customersPhoneNumberResultDiv.hidden = true;
        });

        customersPhoneNumberResultDiv.appendChild(div);
    });
});

if (localStorage.getItem("customerPhoneNumber")) {
    customerPhoneNumberInput.value = localStorage.getItem("customerPhoneNumber");
}

// Customer Address
const customerAddressInput = document.getElementById("customerAddress");
customerAddressInput.addEventListener("input", () => {
    localStorage.setItem("customerAddress", customerAddressInput.value)
});

if (localStorage.getItem("customerAddress")) {
    customerAddressInput.value = localStorage.getItem("customerAddress");
}

// Additional Charge
//const additionalChargeInput = document.getElementById("additionalCharge"); // already define in _OrderForm.js
additionalChargeInput.addEventListener("input", function () {
    localStorage.setItem("additionalCharge", this.value);
});

if (localStorage.getItem("additionalCharge")) {
    additionalChargeInput.value = localStorage.getItem("additionalCharge");
}

// Delivery Fee
deliveryFeeInput.addEventListener("input", function () {
    localStorage.setItem("deliveryFee", this.value);
});

if (localStorage.getItem("deliveryFee")) {
    deliveryFeeInput.value = localStorage.getItem("deliveryFee");
}

if (deliveryFeeInput.value !== "" && deliveryFeeInput.value !== "0") {
    const deliveryRadio = document.getElementById("delivery");
    deliveryRadio.checked = true;
}

// Discount
discountInput.addEventListener("input", function () {
    localStorage.setItem("discount", this.value);
});

if (localStorage.getItem("discount")) {
    discountInput.value = localStorage.getItem("discount");
}

// Ready Date
const readyDateOnlyInput = document.getElementById("readyDateOnly");
readyDateOnlyInput.addEventListener("input", function () {
    localStorage.setItem("readyDateOnly", this.value);
})

if (localStorage.getItem("readyDateOnly")) {
    readyDateOnlyInput.value = localStorage.getItem("readyDateOnly");
}

// Ready Time
const readyTimeOnlyInput = document.getElementById("readyTimeOnly");
readyTimeOnlyInput.addEventListener("input", function () {
    localStorage.setItem("readyTimeOnly", this.value);
})

if (localStorage.getItem("readyTimeOnly")) {
    readyTimeOnlyInput.value = localStorage.getItem("readyTimeOnly");
}

// OrderForm
let orderFormCart = document.getElementById("orderFormCart");

if (orderFormCart == null) {
    orderFormCart = document.getElementById("orderFormOrder");
}

const tempData = document.querySelector(".temp-data");

if (tempData) {
    setTimeout(() => {
            tempData.innerText = "";
    }, 3000);
}

orderFormCart.addEventListener("submit", () => {
    localStorage.clear();
});
