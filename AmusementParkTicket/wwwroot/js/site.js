document.addEventListener("DOMContentLoaded", function () {

    console.log("site.js loaded");
   
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

                    const result =
                        await updateBasketQuantity(
                            gameId,
                            1,
                            true
                        );


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




});





