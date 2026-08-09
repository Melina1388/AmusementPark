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