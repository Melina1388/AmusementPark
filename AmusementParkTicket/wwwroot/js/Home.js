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
// TICKET QUANTITY / HOME BASKET SYNC
// =====================================================
//
// این بخش فقط مربوط به کنترل تعداد بلیت بازی‌هاست.
//
// رفتار:
//
// 0 → 1
//   بازی جدید وارد سبد می‌شود
//   و پیام «بلیت اضافه شد» نمایش داده می‌شود.
//
// 1 → 2
// 2 → 3
// ...
//   فقط تعداد سبد بروزرسانی می‌شود.
//   پیام نمایش داده نمی‌شود.
//
// 1 → 0
//   بازی از سبد حذف می‌شود.
//
// همچنین تعداد اولیه از data-initial-quantity
// گرفته می‌شود تا وقتی کاربر از Basket به Home
// برمی‌گردد، تعداد قبلی همان بازی نمایش داده شود.
// =====================================================


const ticketCounters =
    document.querySelectorAll(".ticket-counter");


console.log(
    "Ticket counters found:",
    ticketCounters.length
);


// =====================================================
// نمایش پیام اضافه شدن اولین بلیت
// =====================================================

function showTicketAddedMessage() {

    const modal =
        document.getElementById(
            "ticket-success-modal"
        );


    if (!modal) {
        return;
    }


    modal.classList.remove("hidden");
}


// =====================================================
// بستن پیام اضافه شدن بلیت
// =====================================================

const closeTicketMessage =
    document.getElementById(
        "close-ticket-message"
    );


if (closeTicketMessage) {

    closeTicketMessage.addEventListener(
        "click",
        function () {

            const modal =
                document.getElementById(
                    "ticket-success-modal"
                );


            if (modal) {

                modal.classList.add(
                    "hidden"
                );
            }

        }
    );
}


// =====================================================
// ارسال تعداد بلیت به سبد خرید
// =====================================================

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

                        gameId:
                            gameId,

                        quantity:
                            quantity
                    })
                }
            );


        if (!response.ok) {

            console.error(
                "خطا در بروزرسانی سبد خرید:",
                response.status
            );

            return null;
        }


        const result =
            await response.json();


        // ---------------------------------------------
        // پیام فقط زمانی نمایش داده شود که:
        //
        // 1. درخواست موفق باشد
        // 2. بازی برای اولین بار وارد سبد شده باشد
        // ---------------------------------------------

        if (
            showMessage &&
            result &&
            result.success &&
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



// =====================================================
// کنترل هر بازی
// =====================================================

ticketCounters.forEach(
    function (counter) {


        // ---------------------------------------------
        // عناصر مربوط به همین بازی
        // ---------------------------------------------

        const toggle =
            counter.querySelector(
                ".ticket-toggle"
            );


        const controls =
            counter.querySelector(
                ".ticket-controls"
            );


        const minus =
            counter.querySelector(
                ".ticket-minus"
            );


        const plus =
            counter.querySelector(
                ".ticket-plus"
            );


        const countElement =
            counter.querySelector(
                ".ticket-count"
            );


        // ---------------------------------------------
        // اگر ساختار HTML این بازی ناقص بود،
        // فقط همین بازی را رد کن.
        //
        // روی صفحات دیگر هیچ اثری ندارد.
        // ---------------------------------------------

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


        // =================================================
        // شناسه بازی
        // =================================================

        const gameId =
            Number(
                counter.dataset.gameId ||
                toggle.dataset.gameId
            );


        if (!gameId) {

            console.error(
                "Invalid gameId:",
                counter
            );

            return;
        }


        // =================================================
        // تعداد اولیه
        // =================================================
        //
        // این مقدار از Home.cshtml می‌آید.
        //
        // مثال:
        //
        // data-initial-quantity="3"
        //
        // یعنی این بازی از قبل 3 بلیت در سبد دارد.
        // =================================================

        let count =
            Number(
                counter.dataset.initialQuantity
            ) || 0;


        // =================================================
        // بروزرسانی ظاهر
        // =================================================

        function render() {


            countElement.textContent =
                count.toString();


            // ---------------------------------------------
            // اگر تعداد صفر است:
            // دکمه «انتخاب بلیت» نمایش داده شود.
            // ---------------------------------------------

            if (count === 0) {

                toggle.hidden =
                    false;

                controls.hidden =
                    true;

                toggle.setAttribute(
                    "aria-expanded",
                    "false"
                );

            }


            // ---------------------------------------------
            // اگر تعداد بیشتر از صفر است:
            // کنترل + و - نمایش داده شود.
            // ---------------------------------------------

            else {

                toggle.hidden =
                    true;

                controls.hidden =
                    false;

                toggle.setAttribute(
                    "aria-expanded",
                    "true"
                );
            }
        }


        // =================================================
        // انتخاب اولین بلیت
        // =================================================

        toggle.addEventListener(
            "click",
            async function (event) {

                event.preventDefault();


                // اگر قبلاً تعداد بیشتر از صفر است،
                // نباید دوباره بازی را اضافه کنیم.
                if (count > 0) {
                    return;
                }


                const oldCount =
                    count;


                // -----------------------------------------
                // تغییر فوری UI
                // -----------------------------------------

                count =
                    1;

                render();


                // -----------------------------------------
                // ارسال واقعی به Session / Basket
                // -----------------------------------------

                count = 1;
                render();

                showTicketAddedMessage();

                const result =
                    await updateBasketQuantity(
                        gameId,
                        1,
                        false
                    );

                if (!result || !result.success) {
                    count = oldCount;
                    render();

                    const modal =
                        document.getElementById("ticket-success-modal");

                    if (modal) {
                        modal.classList.add("hidden");
                    }

                    return;
                }


                // -----------------------------------------
                // اگر درخواست شکست خورد،
                // ظاهر را به حالت قبلی برگردان.
                // -----------------------------------------

                if (
                    !result ||
                    !result.success
                ) {

                    count =
                        oldCount;

                    render();

                    return;
                }

            }
        );


        // =================================================
        // افزایش تعداد +
        // =================================================

        plus.addEventListener(
            "click",
            async function (event) {

                event.preventDefault();


                const oldCount =
                    count;


                count++;


                render();


                // -----------------------------------------
                // این بار پیام نمایش داده نمی‌شود.
                //
                // چون بازی قبلاً در سبد وجود دارد.
                // -----------------------------------------

                const result =
                    await updateBasketQuantity(
                        gameId,
                        count,
                        false
                    );


                // -----------------------------------------
                // اگر سرور خطا داد،
                // مقدار قبلی را برگردان.
                // -----------------------------------------

                if (
                    !result ||
                    !result.success
                ) {

                    count =
                        oldCount;

                    render();

                    return;
                }

            }
        );


        // =================================================
        // کاهش تعداد -
        // =================================================

        minus.addEventListener(
            "click",
            async function (event) {

                event.preventDefault();


                if (count <= 0) {
                    return;
                }


                const oldCount =
                    count;


                count--;


                render();


                // -----------------------------------------
                // اگر تعداد به صفر رسید،
                // بازی کامل از سبد حذف می‌شود.
                // -----------------------------------------

                const result =
                    await updateBasketQuantity(
                        gameId,
                        count,
                        false
                    );


                // -----------------------------------------
                // اگر سرور خطا داد،
                // مقدار قبلی را برگردان.
                // -----------------------------------------

                if (
                    !result ||
                    !result.success
                ) {

                    count =
                        oldCount;

                    render();

                    return;
                }

            }
        );


        // =================================================
        // اجرای وضعیت اولیه
        // =================================================

        render();

    }
);


