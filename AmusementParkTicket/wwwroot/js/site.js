document.addEventListener("DOMContentLoaded", function () {

    console.log("site.js loaded");
    // =====================================================
    // LIVE SEARCH
    // =====================================================

    const searchInput =
        document.getElementById("searchInput");

    const searchSuggestions =
        document.getElementById("searchSuggestions");

    const searchForm =
        document.getElementById("searchForm");


    let searchTimer = null;


    function hideSearchSuggestions() {

        if (!searchSuggestions) {
            return;
        }

        searchSuggestions.innerHTML = "";

        searchSuggestions.classList.remove("show");
    }


    function showSearchSuggestions(items) {

        if (!searchSuggestions) {
            return;
        }

        searchSuggestions.innerHTML = "";


        if (!items || items.length === 0) {

            hideSearchSuggestions();

            return;
        }


        items.forEach(function (item) {

            const button =
                document.createElement("button");

            button.type = "button";

            button.className =
                "search-suggestion-item";


            const gameName =
                document.createElement("span");

            gameName.className =
                "search-suggestion-name";

            gameName.textContent =
                item.gameName || "بازی";


            const amusementName =
                document.createElement("span");

            amusementName.className =
                "search-suggestion-amusement";

            amusementName.textContent =
                item.amusementName || "";


            button.appendChild(gameName);


            if (item.amusementName) {

                button.appendChild(
                    amusementName
                );
            }


            button.addEventListener(
                "click",
                function () {

                    searchInput.value =
                        item.gameName || "";

                    searchForm.submit();

                });


            searchSuggestions.appendChild(
                button
            );

        });


        searchSuggestions.classList.add(
            "show"
        );
    }


    async function searchLive(text) {

        const value =
            text.trim();


        if (value.length < 1) {

            hideSearchSuggestions();

            return;
        }


        try {

            const response =
                await fetch(
                    `/Home/SearchSuggestions?search=${encodeURIComponent(value)}`
                );


            if (!response.ok) {

                hideSearchSuggestions();

                return;
            }


            const results =
                await response.json();


            showSearchSuggestions(
                results
            );

        }
        catch (error) {

            console.error(
                "Live search error:",
                error
            );

            hideSearchSuggestions();
        }
    }


    if (searchInput) {

        searchInput.addEventListener(
            "input",
            function () {

                clearTimeout(
                    searchTimer
                );


                searchTimer =
                    setTimeout(
                        function () {

                            searchLive(
                                searchInput.value
                            );

                        },
                        250
                    );

            });


        searchInput.addEventListener(
            "keydown",
            function (event) {

                if (event.key === "Escape") {

                    hideSearchSuggestions();

                    return;
                }


                if (event.key === "Enter") {

                    hideSearchSuggestions();

                }

            });


        document.addEventListener(
            "click",
            function (event) {

                if (
                    !searchSuggestions ||
                    !searchInput
                ) {
                    return;
                }


                if (
                    !searchSuggestions.contains(
                        event.target
                    ) &&
                    event.target !== searchInput
                ) {

                    hideSearchSuggestions();

                }

            });
    }
    // =====================================================
    // MENU
    // =====================================================

    const menuBtn = document.getElementById("menuBtn");
    const sidebar = document.getElementById("sidebar");
    const overlay = document.getElementById("overlay");

    function toggleSidebar(open) {

        if (!menuBtn || !sidebar || !overlay) {
            console.error("Menu elements not found.");
            return;
        }

        menuBtn.classList.toggle("open", open);
        sidebar.classList.toggle("show", open);
        overlay.classList.toggle("show", open);

        menuBtn.setAttribute(
            "aria-expanded",
            open.toString()
        );
    }

    if (menuBtn) {

        menuBtn.addEventListener("click", function () {

            const isOpen =
                sidebar.classList.contains("show");

            toggleSidebar(!isOpen);
        });
    }

    if (overlay) {

        overlay.addEventListener("click", function () {

            toggleSidebar(false);
        });
    }


    // =====================================================
    // CLOSE MENU AFTER CLICKING LINK
    // =====================================================

    if (sidebar) {

        const links = sidebar.querySelectorAll("a");

        links.forEach(function (link) {

            link.addEventListener("click", function () {

                toggleSidebar(false);
            });
        });
    }


    // =====================================================
    // ALL PARKS
    // =====================================================

    const allParksLink =
        document.getElementById("allParksLink");

    const parksContainer =
        document.getElementById("parksContainer");

    if (allParksLink && parksContainer) {

        allParksLink.addEventListener(
            "click",
            function (event) {

                event.preventDefault();

                parksContainer.scrollIntoView({
                    behavior: "smooth",
                    block: "start"
                });

                toggleSidebar(false);
            }
        );
    }


    // =====================================================
    // BACK TO TOP
    // =====================================================

    const backToTop =
        document.getElementById("backToTop");

    function updateBackToTop() {

        if (!backToTop) {
            return;
        }

        if (window.scrollY > 300) {

            backToTop.classList.add("visible");

        } else {

            backToTop.classList.remove("visible");
        }
    }

    if (backToTop) {

        backToTop.addEventListener(
            "click",
            function () {

                window.scrollTo({
                    top: 0,
                    behavior: "smooth"
                });
            }
        );

        window.addEventListener(
            "scroll",
            updateBackToTop
        );

        updateBackToTop();
    }


    // =====================================================
    // PARK REVEAL
    // =====================================================

    const sections =
        document.querySelectorAll(".park-section");

    if (
        sections.length > 0 &&
        "IntersectionObserver" in window
    ) {

        const revealObserver =
            new IntersectionObserver(
                function (entries) {

                    entries.forEach(function (entry) {

                        if (entry.isIntersecting) {

                            entry.target.classList.add(
                                "revealed"
                            );

                            revealObserver.unobserve(
                                entry.target
                            );
                        }
                    });

                },
                {
                    threshold: 0.05
                }
            );

        sections.forEach(function (section) {

            revealObserver.observe(section);
        });

    } else {

        sections.forEach(function (section) {

            section.classList.add("revealed");
        });
    }


    // =====================================================
    // ESCAPE
    // =====================================================

    document.addEventListener(
        "keydown",
        function (event) {

            if (event.key === "Escape") {

                toggleSidebar(false);
            }
        }
    );


    // =====================================================
    // CONFETTI
    // =====================================================

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


    // =====================================================
    // BASKET
    // =====================================================

    function formatNumber(v) {
        return Math.round(v).toLocaleString('en-US');
    }

    function recalcGrandTotal() {

        const rows =
            document.querySelectorAll('[data-cart-row]');

        const grandTotal =
            document.querySelector('[data-grand-total]');

        const totalBar =
            document.getElementById('totalBar');

        // صفحه Home است و سبد خرید وجود ندارد
        if (!grandTotal) {
            return;
        }

        let total = 0;

        rows.forEach(function (row) {

            const unitPrice =
                parseFloat(
                    row.getAttribute('data-unit-price')
                );

            const quantity =
                parseInt(
                    row.getAttribute('data-quantity'),
                    10
                );

            if (!isNaN(unitPrice) && !isNaN(quantity)) {

                total += unitPrice * quantity;

            }
        });

        grandTotal.textContent =
            formatNumber(total);

        if (totalBar) {

            totalBar.style.display =
                rows.length === 0 ? 'none' : '';

        }
    }

    function removeRow(row) {

        if (!row) {
            return;
        }

        row.classList.add('row-removing');

        row.addEventListener(
            'transitionend',
            function handler() {

                row.removeEventListener(
                    'transitionend',
                    handler
                );

                const group =
                    row.closest('[data-park-group]');

                row.remove();

                if (
                    group &&
                    group.querySelectorAll(
                        '[data-cart-row]'
                    ).length === 0
                ) {
                    group.remove();
                }

                recalcGrandTotal();

                if (
                    document.querySelectorAll(
                        '[data-cart-row]'
                    ).length === 0
                ) {

                    const wrap =
                        document.getElementById('cartWrap');

                    if (wrap) {

                        const empty =
                            document.createElement('div');

                        empty.className =
                            'cart-empty';

                        empty.innerHTML =
                            '<span class="big-emoji">🎈</span> سبد خریدت فعلاً خالیه!';

                        wrap.appendChild(empty);
                    }
                }

            },
            { once: true }
        );
    }


    document
        .querySelectorAll('[data-cart-row]')
        .forEach(function (row) {

            const qtyEl =
                row.querySelector('[data-qty-value]');

            const lineTotalEl =
                row.querySelector('[data-line-total]');

            const increaseBtn =
                row.querySelector('[data-qty-increase]');

            const decreaseBtn =
                row.querySelector('[data-qty-decrease]');

            const removeBtn =
                row.querySelector('[data-remove-row]');

            const unitPrice =
                parseFloat(
                    row.getAttribute('data-unit-price')
                );


            if (
                !qtyEl ||
                !lineTotalEl ||
                !increaseBtn ||
                !decreaseBtn
            ) {
                return;
            }


            function updateRow(newQty) {

                row.setAttribute(
                    'data-quantity',
                    newQty
                );

                qtyEl.textContent =
                    newQty;

                lineTotalEl.textContent =
                    formatNumber(
                        unitPrice * newQty
                    );

                qtyEl.classList.remove('bump');

                void qtyEl.offsetWidth;

                qtyEl.classList.add('bump');

                row.classList.remove('row-pulse');

                void row.offsetWidth;

                row.classList.add('row-pulse');
            }


            increaseBtn.addEventListener(
                'click',
                function () {

                    const current =
                        parseInt(
                            row.getAttribute(
                                'data-quantity'
                            ),
                            10
                        );

                    updateRow(current + 1);

                    recalcGrandTotal();
                }
            );


            decreaseBtn.addEventListener(
                'click',
                function () {

                    const current =
                        parseInt(
                            row.getAttribute(
                                'data-quantity'
                            ),
                            10
                        );

                    const newQty =
                        current - 1;

                    if (newQty <= 0) {

                        removeRow(row);

                        return;
                    }

                    updateRow(newQty);

                    recalcGrandTotal();
                }
            );


            if (removeBtn) {

                removeBtn.addEventListener(
                    'click',
                    function () {

                        removeRow(row);

                    }
                );
            }

        });


    recalcGrandTotal();

//////////////////////////////////////
///////////////////Payment///////////////////
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


// =====================================================
// PAYMENT BUTTON
// =====================================================

    const paymentForm =
        document.getElementById('paymentForm');

    if (
        paymentForm &&
        cardNumberInput &&
        cardHolderInput &&
        expiryInput &&
        cvvInput
    ) {
    paymentForm.addEventListener('submit', function (e) {

        e.preventDefault();

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
            return;
        }


        const payBtn =
            document.getElementById('payBtn');

        if (payBtn) {
            payBtn.classList.add('loading');
        }


        setTimeout(() => {

            const digits =
                cardNumberInput.value.replace(/\D/g, '');

            const receiptHolder =
                document.getElementById('receiptHolder');

            const receiptCard =
                document.getElementById('receiptCard');

            const receiptOrderId =
                document.getElementById('receiptOrderId');

            const trackingCodeText =
                document.getElementById('trackingCodeText');

            const formSection =
                document.getElementById('formSection');

            const successSection =
                document.getElementById('successSection');


            if (receiptHolder) {
                receiptHolder.textContent =
                    cardHolderInput.value.toUpperCase();
            }

            if (receiptCard) {
                receiptCard.textContent =
                    '•••• •••• •••• ' + digits.slice(-4);
            }

            if (receiptOrderId) {
                receiptOrderId.textContent =
                    '#' + Math.floor(
                        100000 + Math.random() * 900000
                    );
            }

            if (trackingCodeText) {
                trackingCodeText.textContent =
                    generateTrackingCode();
            }

            if (formSection) {
                formSection.style.display = 'none';
            }

            if (successSection) {
                successSection.classList.add('show');
            }

            burstConfetti();

        }, 1200);

    });
}
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

    // =====================================================
    // TICKET QUANTITY PER GAME CARD
    // =====================================================

    const ticketCounters =
        document.querySelectorAll(".ticket-counter");

    console.log(
        "Ticket counters found:",
        ticketCounters.length
    );
    async function updateBasketQuantity(
        gameId,
        quantity,
        showMessage = false
    ) {
        try {

            const tokenElement =
                document.querySelector(
                    'input[name="__RequestVerificationToken"]'
                );

            const token =
                tokenElement
                    ? tokenElement.value
                    : "";

            const response =
                await fetch(
                    "/ShopBasket/SelectTicket",
                    {
                        method: "POST",

                        headers: {
                            "Content-Type":
                                "application/json",

                            "RequestVerificationToken":
                                token
                        },

                        body: JSON.stringify({
                            gameId: gameId,
                            quantity: quantity
                        })
                    }
                );

            if (!response.ok) {

                console.error(
                    "خطا در بروزرسانی سبد خرید"
                );

                return null;
            }

            const result =
                await response.json();

            if (
                showMessage &&
                result.isNewItem
            ) {
                showTicketAddedMessage();
            }

            return result;

        }
        catch (error) {

            console.error(
                "Basket update error:",
                error
            );

            return null;
        }
    }

    ticketCounters.forEach(function (counter) {

        const toggle =
            counter.querySelector(".ticket-toggle");

        const controls =
            counter.querySelector(".ticket-controls");

        const minus =
            counter.querySelector(".ticket-minus");

        const plus =
            counter.querySelector(".ticket-plus");

        const countElement =
            counter.querySelector(".ticket-count");


        if (
            !toggle ||
            !controls ||
            !minus ||
            !plus ||
            !countElement
        ) {

            console.error(
                "Ticket counter structure is incomplete:",
                counter
            );

            return;
        }


        let count = 0;


        function render() {

            countElement.textContent =
                count.toString();


            if (count === 0) {

                toggle.hidden = false;
                controls.hidden = true;

            }
            else {

                toggle.hidden = true;
                controls.hidden = false;

            }
        }


        // انتخاب بلیت

        toggle.addEventListener(
            "click",
            function (event) {

                event.preventDefault();

                count = 1;

                render();

            }
        );


        // +

        plus.addEventListener(
            "click",
            async function (event) {

                event.preventDefault();

                count++;

                render();

                const gameId =
                    Number(
                        toggle.dataset.gameId
                    );

                await updateBasketQuantity(
                    gameId,
                    count
                );
            }
        );


        // -

        minus.addEventListener(
            "click",
            async function (event) {

                event.preventDefault();

                if (count <= 0) {
                    return;
                }

                count--;

                render();

                const gameId =
                    Number(
                        toggle.dataset.gameId
                    );

                if (count === 0) {

                    // حذف کامل از سبد
                    await updateBasketQuantity(
                        gameId,
                        0
                    );

                }
                else {

                    await updateBasketQuantity(
                        gameId,
                        count
                    );
                }
            }
        );


        render();

    });




});




///////////////////contactus//////////////////////
document.addEventListener("DOMContentLoaded", function () {

    console.log("contact.js loaded");

    const faDigits = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];

    function toPersianDigits(value) {
        return String(value).replace(/[0-9]/g, function (d) {
            return faDigits[d];
        });
    }

    const confetti = document.getElementById("confetti");

    function burstConfetti(count) {

        if (!confetti) {
            return;
        }

        const colors = ["#ff3d7f", "#00d9c0", "#ffc94d", "#9d7bff"];

        for (let i = 0; i < count; i++) {

            const piece = document.createElement("span");

            piece.style.left = Math.random() * 100 + "%";
            piece.style.background = colors[i % colors.length];
            piece.style.animationDuration = (2.4 + Math.random() * 2.2) + "s";
            piece.style.animationDelay = (Math.random() * 0.35) + "s";

            confetti.appendChild(piece);

            setTimeout(function () {
                piece.remove();
            }, 5200);
        }
    }

    // =====================================================
    // بازی «باز کردن جعبه‌های شگفتی» تیم
    // =====================================================

    const cards = document.querySelectorAll(".mystery-card");
    const progressFill = document.getElementById("progressFill");
    const progressPercent = document.getElementById("progressPercent");
    const progressHint = document.getElementById("progressHint");

    const total = cards.length;
    let opened = 0;

    function updateProgress() {

        const percent = total > 0 ? Math.round((opened / total) * 100) : 0;

        if (progressFill) {
            progressFill.style.width = percent + "%";
        }

        if (progressPercent) {
            progressPercent.textContent = toPersianDigits(percent) + "٪";
        }

        if (progressHint) {
            progressHint.textContent = opened >= total && total > 0
                ? "🎉 آفرین! کل تیم رو شناختی!"
                : toPersianDigits(opened) + " از " + toPersianDigits(total) + " جعبه باز شد";
        }
    }

    cards.forEach(function (card) {

        function openCard() {

            if (card.classList.contains("flipped")) {
                return;
            }

            card.classList.add("flipped");
            opened++;
            updateProgress();
            burstConfetti(16);
        }

        card.addEventListener("click", openCard);

        card.addEventListener("keydown", function (event) {

            if (event.key === "Enter" || event.key === " ") {
                event.preventDefault();
                openCard();
            }
        });
    });

    updateProgress();

    // =====================================================
    // امتیازدهی با ستاره
    // =====================================================

    const starButtons = document.querySelectorAll(".star-btn");
    const submitBtn = document.getElementById("submitRating");
    const toast = document.getElementById("thankYouToast");

    let selectedRating = Number(localStorage.getItem("parkRating")) || 0;

    function paintStars(value) {

        starButtons.forEach(function (btn) {

            const starValue = Number(btn.dataset.value);

            btn.classList.toggle("active", starValue <= value);
        });
    }

    if (selectedRating > 0) {

        paintStars(selectedRating);

        if (submitBtn) {
            submitBtn.classList.add("ready");
        }
    }

    starButtons.forEach(function (btn) {

        btn.addEventListener("mouseenter", function () {
            paintStars(Number(btn.dataset.value));
        });

        btn.addEventListener("mouseleave", function () {
            paintStars(selectedRating);
        });

        btn.addEventListener("click", function () {

            selectedRating = Number(btn.dataset.value);

            paintStars(selectedRating);

            localStorage.setItem("parkRating", String(selectedRating));

            if (submitBtn) {
                submitBtn.classList.add("ready");
            }
        });
    });

    if (submitBtn) {

        submitBtn.addEventListener("click", function () {

            if (!selectedRating) {

                submitBtn.classList.add("shake");

                setTimeout(function () {
                    submitBtn.classList.remove("shake");
                }, 500);

                return;
            }

            burstConfetti(26);

            if (toast) {

                toast.classList.add("show");

                setTimeout(function () {
                    toast.classList.remove("show");
                }, 3200);
            }
        });
    }

});
document.addEventListener("click", async function (event) {

    const button =
        event.target.closest(".ticket-toggle");

    if (!button) {
        return;
    }

    const counter =
        button.closest(".ticket-counter");

    if (!counter) {
        return;
    }

    const gameId =
        Number(button.dataset.gameId);

    const countElement =
        counter.querySelector(".ticket-count");

    if (!gameId || !countElement) {
        return;
    }

    const quantity =
        Number(countElement.textContent);

    if (quantity <= 0) {
        return;
    }

    await updateBasketQuantity(
        gameId,
        quantity,
        true
    );
});
function showTicketAddedMessage() {
    const modal =
        document.getElementById(
            "ticket-success-modal");

    if (!modal) {
        return;
    }

    modal.classList.remove("hidden");
}
document.addEventListener("click", function (event) {
    if (event.target.id === "close-ticket-message") {
        const modal =
            document.getElementById(
                "ticket-success-modal");

        if (modal) {
            modal.classList.add("hidden");
        }
    }
})