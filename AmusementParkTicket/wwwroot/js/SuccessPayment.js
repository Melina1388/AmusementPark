//////////////////successpayment/////////////
// کپی کردن کد پیگیری با یک کلیک، همراه با یک تولتیپ کوتاه «کپی شد ✔»
const copyTrackingBtn =
    document.getElementById('copyTrackingBtn');

if (copyTrackingBtn) {

    copyTrackingBtn.addEventListener('click', function () {

        const trackingCode =
            document.getElementById('trackingCodeText');

        if (!trackingCode) {
            return;
        }

        const code = trackingCode.textContent;

        navigator.clipboard.writeText(code).then(() => {

            this.classList.add('copied');

            setTimeout(() => {
                this.classList.remove('copied');
            }, 1600);

        });

    });

}