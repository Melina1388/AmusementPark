
const otpBoxes =
    document.querySelectorAll(".otp-box");

const otpInput =
    document.getElementById("otp");


otpBoxes.forEach((box, index) => {

    box.addEventListener("input", function () {

        // فقط عدد
        this.value =
            this.value
                .replace(/\D/g, "")
                .slice(0, 1);


        // رفتن به خانه بعدی
        if (
            this.value &&
            index < otpBoxes.length - 1
        ) {
            otpBoxes[index + 1].focus();
        }


        updateOtp();

    });


    box.addEventListener(
        "keydown",
        function (event) {

            if (
                event.key === "Backspace" &&
                !this.value &&
                index > 0
            ) {
                otpBoxes[index - 1].focus();
            }

        });
});


function updateOtp() {

    let code = "";

    otpBoxes.forEach(box => {

        code += box.value;

    });

    otpInput.value = code;
}