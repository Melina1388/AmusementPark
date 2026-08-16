
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