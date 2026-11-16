const isEditOrder = document.getElementById("isEditOrder");

const orderTypeRadios = document.querySelectorAll('input[name="orderType"]');
const observationsInput = document.getElementById("observations");
const customerNameInput = document.getElementById("customerName");
const customersPhoneNumberResultDiv = document.querySelector(".customers-phone-number-result");
const customerPhoneNumberInput = document.getElementById("customerPhoneNumber");
const customerAddressInput = document.getElementById("customerAddress");
const additionalChargeInput = document.getElementById("additionalCharge");

const deliveryFeeInput = document.getElementById("deliveryFee");
const discountInput = document.getElementById("discount");
const readyDateOnlyInput = document.getElementById("readyDateOnly");
const readyTimeOnlyInput = document.getElementById("readyTimeOnly");
let orderFormCart = document.getElementById("orderFormCart");

// #region Order Form Local Storage
if (isEditOrder.value === "false") {
    // Order Type
    if (localStorage.getItem("orderType")) {
        const radioToCheck = document.querySelector(
            `input[name="orderType"][value="${localStorage.getItem("orderType")}"]`
        );

        if (radioToCheck) {
            radioToCheck.checked = true;
        }
    };

    orderTypeRadios.forEach(radio => {
        radio.addEventListener("change", function () {
            localStorage.setItem("orderType", this.value);
        });
    });

    // Observation
    observationsInput.addEventListener("input", () => {
        localStorage.setItem("observations", observationsInput.value);
    });

    if (localStorage.getItem("observations")) {
        observationsInput.value = localStorage.getItem("observations");
    }

    // Customer Name
    customerNameInput.addEventListener("input", () => {
        localStorage.setItem("customerName", customerNameInput.value);
    });

    if (localStorage.getItem("customerName")) {
        customerNameInput.value = localStorage.getItem("customerName");
    }

    // Customer Phone Number
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
    customerAddressInput.addEventListener("input", () => {
        localStorage.setItem("customerAddress", customerAddressInput.value)
    });

    if (localStorage.getItem("customerAddress")) {
        customerAddressInput.value = localStorage.getItem("customerAddress");
    }

    // Additional Charge
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
    readyDateOnlyInput.addEventListener("input", function () {
        localStorage.setItem("readyDateOnly", this.value);
    })

    if (localStorage.getItem("readyDateOnly")) {
        readyDateOnlyInput.value = localStorage.getItem("readyDateOnly");
    }

    // Ready Time
    readyTimeOnlyInput.addEventListener("input", function () {
        localStorage.setItem("readyTimeOnly", this.value);
    })

    if (localStorage.getItem("readyTimeOnly")) {
        readyTimeOnlyInput.value = localStorage.getItem("readyTimeOnly");
    }

    // OrderForm
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
}
// #endregion

const subTotalPriceElement = document.getElementById("subTotalPrice");
const subTotalPrice = parseFloat(subTotalPriceElement.dataset.value) || 0;
const gstElement = document.getElementById("gst");
const pstElement = document.getElementById("pst");
const totalPriceElement = document.getElementById("totalPrice");

const radioDelivery = document.getElementById("delivery");

async function getRoundedValue(num, decimals = 2) {
    const response = await fetch(`/Shared/RoundNumber?value=${num}&decimals=${decimals}`)
    const result = await response.json();

    return result.rounded;
}

async function updateTotalPrice() {
    const additionalCharge = parseFloat(additionalChargeInput.value.replace(",", ".")) || 0;
    const deliveryFee = parseFloat(deliveryFeeInput.value.replace(",", ".")) || 0;
    const discount = parseFloat(discountInput.value.replace(",", ".")) || 0;
    const baseSubTotal = parseFloat(subTotalPriceElement.dataset.base) || 0;
    const subTotal = baseSubTotal + additionalCharge;
    const gst = await getRoundedValue(subTotal * 0.05);
    const pst = await getRoundedValue(subTotal * 0.07);
    const total = ((subTotal + gst + pst) + deliveryFee) - discount;

    gstElement.textContent = gst.toLocaleString("en-CA", {
        style: "currency",
        currency: "CAD"
    });

    pstElement.textContent = pst.toLocaleString("en-CA", {
        style: "currency",
        currency: "CAD"
    });

    subTotalPriceElement.textContent = subTotal.toLocaleString("en-CA", {
        style: "currency",
        currency: "CAD"
    });

    totalPriceElement.textContent = total.toLocaleString("en-CA", {
        style: "currency",
        currency: "CAD"
    });
}

additionalChargeInput.addEventListener("input", updateTotalPrice);
deliveryFeeInput.addEventListener("input", updateTotalPrice);
discountInput.addEventListener("input", updateTotalPrice);

document.addEventListener("DOMContentLoaded", function () {
    updateTotalPrice();
});
