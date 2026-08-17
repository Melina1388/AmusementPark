///////////////////confetti
const confetti =
    document.getElementById("confetti");

if (confetti) {

    const colors = [
        "#ff3d7f",
        "#00d9c0",
        "#ffc94d",
        "#9d7bff"
    ];

    for (let i = 0; i < 14; i++) {

        const piece =
            document.createElement("span");

        piece.style.left =
            Math.random() * 100 + "%";

        piece.style.background =
            colors[i % colors.length];

        piece.style.animationDuration =
            (7 + Math.random() * 6) + "s";

        piece.style.animationDelay =
            (Math.random() * 8) + "s";

        confetti.appendChild(piece);
    }
}
//////////////////////////////////
const cardNumberInput = document.getElementById('cardNumberInput');
const cardHolderInput = document.getElementById('cardHolderInput');
const expiryInput = document.getElementById('expiryInput');
const cvvInput = document.getElementById('cvvInput');

const previewCardNumber = document.getElementById('previewCardNumber');
const previewCardHolder = document.getElementById('previewCardHolder');
const previewExpiry = document.getElementById('previewExpiry');
const previewCvv = document.getElementById('previewCvv');

const card3d = document.getElementById('card3d');


// فقط در صفحه Payment اجرا شود
if (
    cardNumberInput &&
    cardHolderInput &&
    expiryInput &&
    cvvInput &&
    previewCardNumber &&
    previewCardHolder &&
    previewExpiry &&
    previewCvv &&
    card3d
) {

    cardNumberInput.addEventListener('input', () => {

        const digits =
            cardNumberInput.value
                .replace(/\D/g, '')
                .slice(0, 16);

        cardNumberInput.value =
            digits.replace(/(.{4})/g, '$1 ').trim();

        previewCardNumber.textContent =
            digits
                .padEnd(16, '•')
                .replace(/(.{4})/g, '$1 ')
                .trim();
    });


    cardHolderInput.addEventListener('input', () => {

        previewCardHolder.textContent =
            cardHolderInput.value.trim()
                ? cardHolderInput.value.toUpperCase()
                : 'CARD HOLDER';

    });


    expiryInput.addEventListener('input', () => {

        const digits =
            expiryInput.value
                .replace(/\D/g, '')
                .slice(0, 4);

        expiryInput.value =
            digits.length >= 3
                ? digits.slice(0, 2) + '/' + digits.slice(2)
                : digits;

        previewExpiry.textContent =
            expiryInput.value || 'MM/YY';

    });


    cvvInput.addEventListener('input', () => {

        const digits =
            cvvInput.value
                .replace(/\D/g, '')
                .slice(0, 4);

        cvvInput.value = digits;

        previewCvv.textContent =
            digits.padEnd(3, '•');

    });


    cvvInput.addEventListener(
        'focus',
        () => card3d.classList.add('flipped')
    );

    cvvInput.addEventListener(
        'blur',
        () => card3d.classList.remove('flipped')
    );
}
//////////////payment
paymentForm.addEventListener('submit', function (e) {

    document
        .querySelectorAll('.field-error')
        .forEach(el => el.textContent = '');

    let hasError = false;

    if (
        cardNumberInput.value
            .replace(/\D/g, '')
            .length !== 16
    ) {
        document.getElementById('cardNumberError')
            .textContent =
            'شماره کارت باید ۱۶ رقم باشد.';

        hasError = true;
    }

    if (!cardHolderInput.value.trim()) {
        document.getElementById('cardHolderError')
            .textContent =
            'نام دارنده‌ی کارت را وارد کنید.';

        hasError = true;
    }

    if (!/^\d{2}\/\d{2}$/.test(expiryInput.value)) {
        document.getElementById('expiryError')
            .textContent =
            'قالب تاریخ باید MM/YY باشد.';

        hasError = true;
    }

    if (!/^\d{3,4}$/.test(cvvInput.value)) {
        document.getElementById('cvvError')
            .textContent =
            'CVV2 باید ۳ یا ۴ رقم باشد.';

        hasError = true;
    }

    if (hasError) {
        e.preventDefault();
        return;
    }

    const payBtn =
        document.getElementById('payBtn');

    if (payBtn) {
        payBtn.classList.add('loading');
    }
});



















