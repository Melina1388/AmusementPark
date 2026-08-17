
let tickets = window.tickets || [];


function formatNumber(v) {
    return Math.round(v).toLocaleString('en-US');
}


function getTicketStatus(value) {

    const status = (value || '')
        .toString()
        .trim()
        .toLowerCase();

    if (
        status === 'unused' ||
        status === 'notused'
    ) {
        return 'unused';
    }

    if (status === 'used') {
        return 'used';
    }

    if (
        status === 'cancelled' ||
        status === 'canceled'
    ) {
        return 'cancelled';
    }

    return 'unused';
}


function ticketRowHtml(t) {

    let actionsHtml = '';

    if (t.status === 'unused') {

        actionsHtml = `
            <button
                class="ticket-btn view-btn"
                data-action="view"
                data-id="${t.ticketID}">
                نمایش QR
            </button>

            <button
                class="ticket-btn cancel-btn"
                data-action="cancel"
                data-id="${t.ticketID}">
                لغو بلیط
            </button>
        `;

    }
    else if (t.status === 'used') {

        actionsHtml = `
            <span class="status-badge used">
                ✔ استفاده‌شده
            </span>
        `;

    }
    else {

        actionsHtml = `
            <span class="status-badge cancelled">
                ✖ لغوشده
            </span>
        `;
    }


    return `
    <div
        class="ticket-row"
        data-id="${t.ticketID}"
        data-status="${t.status}">

        <div class="ticket-icon">
            🎫
        </div>

        <div class="ticket-info">

            <p class="park-name">
                ${t.amusementName}
            </p>

            <p class="ride-name">
                ${t.gameName}
            </p>

            <div class="meta-row">

                <span class="price">
                    ${formatNumber(t.gamePrice)} تومان
                </span>

                <span class="tracking">
                    #${t.trackingNum}
                </span>

            </div>

        </div>

        <div class="ticket-actions">
            ${actionsHtml}
        </div>

    </div>
`;
}


function renderAll() {

    const panels = {
        unused: document.getElementById('panelUnused'),
        used: document.getElementById('panelUsed'),
        cancelled: document.getElementById('panelCancelled')
    };


    const groups = {
        unused: [],
        used: [],
        cancelled: []
    };


    tickets.forEach(t => {

        t.status = getTicketStatus(t.isUsed);

        if (groups[t.status]) {
            groups[t.status].push(t);
        }

    });


    Object.keys(panels).forEach(key => {

        const list = groups[key];

        panels[key].innerHTML = list.length

            ? list
                .map(ticketRowHtml)
                .join('')

            : `
                <div class="empty-tab">

                    <span class="big-emoji">
                        ${key === 'unused'
                ? '🎈'
                : key === 'used'
                    ? '✅'
                    : '🗂️'
            }
                    </span>

                    ${key === 'unused'
                ? 'بلیط استفاده‌نشده‌ای نداری.'
                : key === 'used'
                    ? 'هنوز بلیطی استفاده نکردی.'
                    : 'بلیط لغوشده‌ای نیست.'
            }

                </div>
            `;
    });


    document.getElementById('countUnused').textContent =
        groups.unused.length;

    document.getElementById('countUsed').textContent =
        groups.used.length;

    document.getElementById('countCancelled').textContent =
        groups.cancelled.length;


    attachRowActions();

    applySearchFilter();
}


document
    .querySelectorAll('.tab-btn')
    .forEach(btn => {

        btn.addEventListener('click', () => {

            document
                .querySelectorAll('.tab-btn')
                .forEach(b =>
                    b.classList.remove('active')
                );

            document
                .querySelectorAll('.tab-panel')
                .forEach(p =>
                    p.classList.remove('active')
                );

            btn.classList.add('active');

            document
                .querySelector(
                    `[data-panel="${btn.getAttribute('data-tab')}"]`
                )
                .classList.add('active');

        });

    });


function applySearchFilter() {

    const q =
        document
            .getElementById('searchInput')
            .value
            .trim()
            .toUpperCase();


    document
        .querySelectorAll('.ticket-row')
        .forEach(row => {

            const ticketId =
                row.getAttribute('data-id');


            const ticket =
                tickets.find(
                    x =>
                        x.ticketID.toString() ===
                        ticketId
                );


            if (!ticket) {
                return;
            }


            const code =
                (ticket.trackingNum || '')
                    .toUpperCase();


            row.classList.toggle(
                'hidden-by-search',
                q.length > 0 &&
                !code.includes(q)
            );

        });
}


document
    .getElementById('searchInput')
    .addEventListener(
        'input',
        applySearchFilter
    );


//function openModal(id) {

//    document
//        .getElementById(id)
//        .classList.add('show');
//}


//function closeModal(id) {

//    document
//        .getElementById(id)
//        .classList.remove('show');
//}


//function buildQrPattern() {

//    const box =
//        document.getElementById('qrBox');

//    box.innerHTML = '';


//    for (let i = 0; i < 121; i++) {

//        const cell =
//            document.createElement('div');

//        const row =
//            Math.floor(i / 11);

//        const col =
//            i % 11;


//        const inFinder =
//            (row < 3 && col < 3) ||
//            (row < 3 && col > 7) ||
//            (row > 7 && col < 3);


//        cell.className =
//            'cell' +
//            (
//                inFinder ||
//                    Math.random() > 0.55
//                    ? ' on'
//                    : ''
//            );


//        box.appendChild(cell);
//    }
//}




//async function updateTicketStatus(ticketId, status) {

//    try {

//        const response = await fetch('/Ticket/UpdateStatus', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/x-www-form-urlencoded'
//            },
//            body:
//                `ticketId=${encodeURIComponent(ticketId)}` +
//                `&status=${encodeURIComponent(status)}`
//        });

//        if (!response.ok) {
//            throw new Error('خطا در تغییر وضعیت بلیط');
//        }

//        const result = await response.json();

//        return result.success;

//    }
//    catch (error) {

//        console.error(error);

//        alert('تغییر وضعیت بلیط انجام نشد.');

//        return false;
//    }
//}


//async function updateTicketStatus(ticketId, status) {

//    try {

//        const response = await fetch('/Ticket/UpdateStatus', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/x-www-form-urlencoded'
//            },
//            body:
//                `ticketId=${encodeURIComponent(ticketId)}` +
//                `&status=${encodeURIComponent(status)}`
//        });

//        if (!response.ok) {
//            throw new Error('خطا در تغییر وضعیت بلیط');
//        }

//        const result = await response.json();

//        return result.success;

//    }
//    catch (error) {

//        console.error(error);

//        alert('تغییر وضعیت بلیط انجام نشد.');

//        return false;
//    }
//}


///*function attachRowActions() {

//    document
//        .querySelectorAll('[data-action="view"]')
//        .forEach(btn => {

//            btn.addEventListener('click', () => {

//                activeTicketId =
//                    btn.getAttribute('data-id');

//                openModal('qrConfirmModal');

//            });

//        });


//    document
//        .querySelectorAll('[data-action="cancel"]')
//        .forEach(btn => {

//            btn.addEventListener('click', () => {

//                activeTicketId =
//                    btn.getAttribute('data-id');

//                const ticket =
//                    tickets.find(
//                        x =>
//                            x.ticketID.toString() ===
//                            activeTicketId
//                    );


//                if (!ticket) {
//                    return;
//                }


//                openModal('cancelConfirmModal');

//            });

//        });
//}*/
//let activeTicketId = null;
//function attachRowActions() {

//    document
//        .querySelectorAll('[data-action="view"]')
//        .forEach(btn => {

//            btn.addEventListener('click', () => {

//                activeTicketId =
//                    btn.getAttribute('data-id');

//                openModal('qrConfirmModal');
//            });
//        });


//    document
//        .querySelectorAll('[data-action="cancel"]')
//        .forEach(btn => {

//            btn.addEventListener('click', () => {

//                activeTicketId =
//                    btn.getAttribute('data-id');

//                const t =
//                    tickets.find(
//                        x =>
//                            x.ticketID.toString() ===
//                            activeTicketId
//                    );

//                if (!t) {
//                    return;
//                }

//                openModal('cancelConfirmModal');
//            });
//        });
//}

//document
//    .getElementById('qrCancelBtn')
//    .addEventListener(
//        'click',
//        () => closeModal('qrConfirmModal')
//    );


//document
//    .getElementById('qrConfirmBtn')
//    .addEventListener(
//        'click',
//        () => {

//            closeModal('qrConfirmModal');

//            buildQrPattern();

//            openModal('qrShowModal');

//        }
//    );


//document.getElementById('qrCloseBtn').addEventListener('click', async () => {

//    const ticketId = activeCode;

//    const success =
//        await updateTicketStatus(ticketId, 'Used');

//    if (!success) {
//        return;
//    }

//    closeModal('qrShowModal');

//    const t =
//        tickets.find(
//            x => x.ticketID.toString() === ticketId
//        );

//    if (t) {
//        t.isUsed = 'Used';
//        t.status = 'used';
//    }

//    renderAll();
//});
let activeTicketId = null;


// ===============================
// باز کردن و بستن Modal
// ===============================

function openModal(id) {
    const modal = document.getElementById(id);

    if (modal) {
        modal.classList.add('show');
    }
}

function closeModal(id) {
    const modal = document.getElementById(id);

    if (modal) {
        modal.classList.remove('show');
    }
}


// ===============================
// ساخت QR نمایشی
// ===============================

function buildQrPattern() {

    const box = document.getElementById('qrBox');

    if (!box) {
        console.error('qrBox پیدا نشد.');
        return;
    }

    box.innerHTML = '';

    for (let i = 0; i < 121; i++) {

        const cell = document.createElement('div');

        const row = Math.floor(i / 11);
        const col = i % 11;

        const inFinder =
            (row < 3 && col < 3) ||
            (row < 3 && col > 7) ||
            (row > 7 && col < 3);

        cell.className =
            'cell' +
            ((inFinder || Math.random() > 0.55)
                ? ' on'
                : '');

        box.appendChild(cell);
    }
}


// ===============================
// اتصال دکمه‌های هر بلیط
// ===============================

function attachRowActions() {

    // دکمه نمایش QR
    document.querySelectorAll('.view-btn').forEach(function (button) {

        button.addEventListener('click', function () {

            activeTicketId =
                this.getAttribute('data-id');

            console.log(
                'QR button clicked. TicketID =',
                activeTicketId
            );

            if (!activeTicketId) {

                console.error(
                    'TicketID برای این بلیط پیدا نشد.'
                );

                return;
            }

            openModal('qrConfirmModal');
        });
    });


    // دکمه لغو
    document.querySelectorAll('.cancel-btn').forEach(function (button) {

        button.addEventListener('click', function () {

            activeTicketId =
                this.getAttribute('data-id');

            console.log(
                'Cancel button clicked. TicketID =',
                activeTicketId
            );

            if (!activeTicketId) {
                return;
            }

            openModal('cancelConfirmModal');
        });
    });
}


// ===============================
// تغییر وضعیت در Database
// ===============================

async function updateTicketStatus(ticketId, status) {

    try {

        const response = await fetch(
            '/Ticket/UpdateStatus',
            {
                method: 'POST',

                headers: {
                    'Content-Type':
                        'application/x-www-form-urlencoded; charset=UTF-8'
                },

                body:
                    'ticketId=' +
                    encodeURIComponent(ticketId) +
                    '&status=' +
                    encodeURIComponent(status)
            }
        );

        console.log(
            'Update response:',
            response.status
        );

        if (!response.ok) {

            const text =
                await response.text();

            console.error(
                'Server error:',
                text
            );

            return false;
        }

        const result =
            await response.json();

        console.log(
            'Server result:',
            result
        );

        return result.success === true;

    }
    catch (error) {

        console.error(
            'UpdateTicketStatus Error:',
            error
        );

        return false;
    }
}


// ===============================
// دکمه انصراف QR
// ===============================

document
    .getElementById('qrCancelBtn')
    .addEventListener('click', function () {

        closeModal('qrConfirmModal');

    });


// ===============================
// تأیید نمایش QR
// ===============================

document
    .getElementById('qrConfirmBtn')
    .addEventListener('click', function () {

        console.log(
            'QR Confirm clicked. TicketID =',
            activeTicketId
        );

        closeModal('qrConfirmModal');

        buildQrPattern();

        openModal('qrShowModal');

    });


// ===============================
// بستن QR و تبدیل بلیط به Used
// ===============================

document
    .getElementById('qrCloseBtn')
    .addEventListener('click', async function () {

        console.log(
            'QR Close clicked. TicketID =',
            activeTicketId
        );

        if (!activeTicketId) {

            console.error(
                'activeTicketId خالی است.'
            );

            return;
        }

        const success =
            await updateTicketStatus(
                activeTicketId,
                'Used'
            );

        if (!success) {

            alert(
                'تغییر وضعیت بلیط انجام نشد.'
            );

            return;
        }

        const ticket =
            tickets.find(function (x) {

                return x.ticketID.toString() ===
                    activeTicketId;

            });

        if (ticket) {

            ticket.isUsed = 'Used';
            ticket.status = 'used';

        }

        closeModal('qrShowModal');

        renderAll();

    });


// ===============================
// انصراف از لغو
// ===============================

document
    .getElementById('cancelDismissBtn')
    .addEventListener('click', function () {

        closeModal('cancelConfirmModal');

    });


// ===============================
// تأیید لغو بلیط
// ===============================

document
    .getElementById('cancelConfirmBtn')
    .addEventListener('click', async function () {

        if (!activeTicketId) {

            console.error(
                'activeTicketId خالی است.'
            );

            return;
        }

        const success =
            await updateTicketStatus(
                activeTicketId,
                'Cancelled'
            );

        if (!success) {

            alert(
                'لغو بلیط انجام نشد.'
            );

            return;
        }

        const ticket =
            tickets.find(function (x) {

                return x.ticketID.toString() ===
                    activeTicketId;

            });

        if (ticket) {

            ticket.isUsed = 'Cancelled';
            ticket.status = 'cancelled';

        }

        closeModal('cancelConfirmModal');

        renderAll();

    });

document
    .getElementById('cancelDismissBtn')
    .addEventListener('click', () => {

        closeModal('cancelConfirmModal');

    });


document
    .getElementById('cancelConfirmBtn')
    .addEventListener('click', async () => {

        if (!activeTicketId) {

            console.error(
                'Ticket ID پیدا نشد.'
            );

            return;
        }

        const success =
            await updateTicketStatus(
                activeTicketId,
                'Cancelled'
            );

        if (!success) {

            alert(
                'لغو بلیط انجام نشد.'
            );

            return;
        }

        const ticket =
            tickets.find(
                x =>
                    x.ticketID.toString() ===
                    activeTicketId
            );

        if (ticket) {

            ticket.isUsed = 'Cancelled';
            ticket.status = 'cancelled';

        }

        closeModal('cancelConfirmModal');

        renderAll();

    });


renderAll();