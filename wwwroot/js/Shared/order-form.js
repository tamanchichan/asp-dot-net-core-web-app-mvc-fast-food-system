const customerNameInput = document.getElementById("customerName");
const customerPhoneNumberInput = document.getElementById("customerPhoneNumber");
const customerAddressInput = document.getElementById("customerAddress");

const customersPhoneNumberDiv = document.querySelector(".customers-phone-number");

const radioDelivery = document.getElementById("delivery");

customerPhoneNumberInput.addEventListener("input", async () => {

    const value = customerPhoneNumberInput.value;

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

const additionalChargeInput = document.getElementById("additionalCharge");
const gstElement = document.getElementById("gst");
const pstElement = document.getElementById("pst");
const subTotalPriceElement = document.getElementById("subTotalPrice");
const subTotalPrice = parseFloat(subTotalPriceElement.dataset.value) || 0;
const totalPriceElement = document.getElementById("totalPrice");
const deliveryFeeInput = document.getElementById("deliveryFee");
const discountInput = document.getElementById("discount");

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
    console.log(`(${subTotal} + ${gst} + ${pst}) + ${deliveryFee} - ${discount} = ${total} `)

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
