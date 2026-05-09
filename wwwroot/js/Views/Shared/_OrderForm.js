const additionalChargeInput = document.getElementById("additionalCharge");
const gstElement = document.getElementById("gst");
const pstElement = document.getElementById("pst");
const subTotalPriceElement = document.getElementById("subTotalPrice");
const subTotalPrice = parseFloat(subTotalPriceElement.dataset.value) || 0;
const totalPriceElement = document.getElementById("totalPrice");
const deliveryFeeInput = document.getElementById("deliveryFee");
const discountInput = document.getElementById("discount");

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

document.addEventListener("DOMContentLoaded", function () {
    updateTotalPrice();
})