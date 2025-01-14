// Initialize Google Places Autocomplete for Payment Address
function initializeAddressAutocomplete() {
    const addressInput = document.getElementById("PaymentAddress");
    if (addressInput) {
        const autocomplete = new google.maps.places.Autocomplete(addressInput);
        autocomplete.setFields(["formatted_address"]);
        autocomplete.addListener("place_changed", function () {
            const place = autocomplete.getPlace();
            console.log("Selected address:", place.formatted_address);
        });
    }
}

google.maps.event.addDomListener(window, "load", initializeAddressAutocomplete);

// Initialize intl-tel-input for Phone Number
function setupIntlTelInput() {
    const phoneInput = document.getElementById("phoneNumber");
    if (phoneInput) {
        const intlInput = window.intlTelInput(phoneInput, {
            initialCountry: "auto",
            geoIpLookup: (callback) => {
                fetch("https://ipinfo.io/json?token=YOUR_IPINFO_TOKEN")
                    .then((response) => response.json())
                    .then((data) => callback(data.country))
                    .catch(() => callback("us"));
            },
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        });

        document.querySelector("form").addEventListener("submit", function (e) {
            if (!intlInput.isValidNumber()) {
                e.preventDefault();
                alert("Invalid phone number.");
            } else {
                const hiddenInput = document.createElement("input");
                hiddenInput.type = "hidden";
                hiddenInput.name = "newValue";
                hiddenInput.value = intlInput.getNumber();
                this.appendChild(hiddenInput);
            }
        });
    }
}

document.addEventListener("DOMContentLoaded", setupIntlTelInput);

// Password Validation
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const passwordInput = document.getElementById("Password");
    const passwordError = document.getElementById("PasswordError");

    if (passwordInput) {
        form.addEventListener("submit", function (e) {
            const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

            if (!passwordRegex.test(passwordInput.value)) {
                e.preventDefault();
                passwordError.style.display = "block";
                passwordError.textContent =
                    "Password must include uppercase, lowercase, number, and special character.";
            }
        });
    }
});

// Toggle Password Visibility
document.addEventListener("DOMContentLoaded", function () {
    const togglePasswordButton = document.getElementById("togglePassword");
    if (togglePasswordButton) {
        togglePasswordButton.addEventListener("click", function () {
            const passwordInput = document.getElementById("Password");
            const type = passwordInput.type === "password" ? "text" : "password";
            passwordInput.type = type;
        });
    }
});

// Email Validation
document.addEventListener("DOMContentLoaded", function () {
    const emailInput = document.getElementById("email");
    const emailError = document.getElementById("emailError");

    if (emailInput) {
        emailInput.addEventListener("blur", function () {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            if (!emailRegex.test(emailInput.value)) {
                emailError.style.display = "block";
                emailError.textContent = "Invalid email address.";
            } else {
                emailError.style.display = "none";
            }
        });
    }
});

// Form Submission Confirmation
document.getElementById("editForm").onsubmit = function (event) {
    if (!confirm("Are you sure you want to save the changes?")) {
        event.preventDefault();
    }
};
