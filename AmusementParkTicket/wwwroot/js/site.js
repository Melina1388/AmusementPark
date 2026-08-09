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

});

/////////////////////////////Basket///////////////////////
function formatNumber(v) { return Math.round(v).toLocaleString('en-US'); }

function recalcGrandTotal() {
    var rows = document.querySelectorAll('[data-cart-row]');
    var total = 0;
    rows.forEach(r => total += parseFloat(r.getAttribute('data-unit-price')) * parseInt(r.getAttribute('data-quantity'), 10));
    document.querySelector('[data-grand-total]').textContent = formatNumber(total);
    if (rows.length === 0) document.getElementById('totalBar').style.display = 'none';
}

function removeRow(row) {
    row.classList.add('row-removing');
    row.addEventListener('transitionend', function handler() {
        row.removeEventListener('transitionend', handler);
        var group = row.closest('[data-park-group]');
        row.remove();
        if (group && group.querySelectorAll('[data-cart-row]').length === 0) group.remove();
        recalcGrandTotal();
        if (document.querySelectorAll('[data-cart-row]').length === 0) {
            var wrap = document.getElementById('cartWrap');
            var empty = document.createElement('div');
            empty.className = 'cart-empty';
            empty.innerHTML = '<span class="big-emoji">🎈</span> سبد خریدت فعلاً خالیه!';
            wrap.appendChild(empty);
        }
    }, { once: true });
}

document.querySelectorAll('[data-cart-row]').forEach(function (row) {
    var qtyEl = row.querySelector('[data-qty-value]');
    var lineTotalEl = row.querySelector('[data-line-total]');
    var unitPrice = parseFloat(row.getAttribute('data-unit-price'));

    function updateRow(newQty) {
        row.setAttribute('data-quantity', newQty);
        qtyEl.textContent = newQty;
        lineTotalEl.textContent = formatNumber(unitPrice * newQty);
        qtyEl.classList.remove('bump'); void qtyEl.offsetWidth; qtyEl.classList.add('bump');
        row.classList.remove('row-pulse'); void row.offsetWidth; row.classList.add('row-pulse');
    }

    row.querySelector('[data-qty-increase]').addEventListener('click', function () {
        updateRow(parseInt(row.getAttribute('data-quantity'), 10) + 1);
        recalcGrandTotal();
    });
    row.querySelector('[data-qty-decrease]').addEventListener('click', function () {
        var newQty = parseInt(row.getAttribute('data-quantity'), 10) - 1;
        if (newQty <= 0) { removeRow(row); return; }
        updateRow(newQty);
        recalcGrandTotal();
    });
});

recalcGrandTotal();