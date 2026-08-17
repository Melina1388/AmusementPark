let tickets = [
    { code: 'K7M2XQ9P', park: 'شهربازی ارم', ride: 'ترن هوایی', price: 180000, status: 'unused' },
    { code: 'B3T8LR4W', park: 'شهربازی ارم', ride: 'ترن هوایی', price: 180000, status: 'unused' },
    { code: 'H9D5NF2K', park: 'شهربازی ارم', ride: 'چرخ و فلک', price: 90000, status: 'unused' },
    { code: 'Q2W7YT6M', park: 'شهربازی بسیج', ride: 'قطار وحشت', price: 140000, status: 'unused' },
    { code: 'Z4X9CV3B', park: 'شهربازی بسیج', ride: 'قطار وحشت', price: 140000, status: 'used' },
    { code: 'L6P2ND8R', park: 'شهربازی ارم', ride: 'خانه وحشت', price: 120000, status: 'used' },
    { code: 'M8K3JH5T', park: 'لوناپارک نور', ride: 'برج سقوط آزاد', price: 200000, status: 'cancelled' },
];

const buyerName = 'رضا احمدی';
const buyerCard = '•••• •••• •••• 4521';

function formatNumber(v) { return Math.round(v).toLocaleString('en-US'); }

function ticketRowHtml(t) {
    let actionsHtml = '';
    if (t.status === 'unused') {
        actionsHtml = `
        <button class="ticket-btn view-btn" data-action="view" data-code="${t.code}">نمایش QR</button>
        <button class="ticket-btn cancel-btn" data-action="cancel" data-code="${t.code}">لغو بلیط</button>`;
    } else if (t.status === 'used') {
        actionsHtml = `<span class="status-badge used">✔ استفاده‌شده</span>`;
    } else {
        actionsHtml = `<span class="status-badge cancelled">✖ لغو‌شده</span>`;
    }

    return `
      <div class="ticket-row" data-code="${t.code}" data-status="${t.status}">
        <div class="ticket-icon">🎫</div>
        <div class="ticket-info">
          <p class="park-name">${t.park}</p>
          <p class="ride-name">${t.ride}</p>
          <div class="meta-row">
            <span class="price">${formatNumber(t.price)} تومان</span>
            <span class="tracking">#${t.code}</span>
          </div>
        </div>
        <div class="ticket-actions">${actionsHtml}</div>
      </div>`;
}

function renderAll() {
    const panels = { unused: document.getElementById('panelUnused'), used: document.getElementById('panelUsed'), cancelled: document.getElementById('panelCancelled') };
    const groups = { unused: [], used: [], cancelled: [] };
    tickets.forEach(t => groups[t.status].push(t));

    Object.keys(panels).forEach(key => {
        const list = groups[key];
        panels[key].innerHTML = list.length
            ? list.map(ticketRowHtml).join('')
            : `<div class="empty-tab"><span class="big-emoji">${key === 'unused' ? '🎈' : key === 'used' ? '✅' : '🗂️'}</span>${key === 'unused' ? 'بلیط استفاده‌نشده‌ای نداری.' : key === 'used' ? 'هنوز بلیطی استفاده نکردی.' : 'بلیط لغو‌شده‌ای نیست.'
            }</div>`;
    });

    document.getElementById('countUnused').textContent = groups.unused.length;
    document.getElementById('countUsed').textContent = groups.used.length;
    document.getElementById('countCancelled').textContent = groups.cancelled.length;

    attachRowActions();
    applySearchFilter();
}

document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        document.querySelectorAll('.tab-panel').forEach(p => p.classList.remove('active'));
        btn.classList.add('active');
        document.querySelector(`[data-panel="${btn.getAttribute('data-tab')}"]`).classList.add('active');
    });
});

function applySearchFilter() {
    const q = document.getElementById('searchInput').value.trim().toUpperCase();
    document.querySelectorAll('.ticket-row').forEach(row => {
        const code = row.getAttribute('data-code');
        row.classList.toggle('hidden-by-search', q.length > 0 && !code.includes(q));
    });
}
document.getElementById('searchInput').addEventListener('input', applySearchFilter);

function openModal(id) { document.getElementById(id).classList.add('show'); }
function closeModal(id) { document.getElementById(id).classList.remove('show'); }

function buildQrPattern() {
    const box = document.getElementById('qrBox');
    box.innerHTML = '';
    for (let i = 0; i < 121; i++) {
        const cell = document.createElement('div');
        const row = Math.floor(i / 11), col = i % 11;
        const inFinder = (row < 3 && col < 3) || (row < 3 && col > 7) || (row > 7 && col < 3);
        cell.className = 'cell' + ((inFinder || Math.random() > 0.55) ? ' on' : '');
        box.appendChild(cell);
    }
}

let activeCode = null;

function attachRowActions() {
    document.querySelectorAll('[data-action="view"]').forEach(btn => {
        btn.addEventListener('click', () => { activeCode = btn.getAttribute('data-code'); openModal('qrConfirmModal'); });
    });
    document.querySelectorAll('[data-action="cancel"]').forEach(btn => {
        btn.addEventListener('click', () => {
            activeCode = btn.getAttribute('data-code');
            const t = tickets.find(x => x.code === activeCode);
            document.getElementById('refundName').textContent = buyerName;
            document.getElementById('refundCard').textContent = buyerCard;
            document.getElementById('refundAmount').textContent = formatNumber(t.price) + ' تومان';
            openModal('cancelConfirmModal');
        });
    });
}

document.getElementById('qrCancelBtn').addEventListener('click', () => closeModal('qrConfirmModal'));
document.getElementById('qrConfirmBtn').addEventListener('click', () => {
    closeModal('qrConfirmModal');
    buildQrPattern();
    openModal('qrShowModal');
});
document.getElementById('qrCloseBtn').addEventListener('click', () => {
    closeModal('qrShowModal');
    const t = tickets.find(x => x.code === activeCode);
    if (t) t.status = 'used';
    renderAll();
});

document.getElementById('cancelDismissBtn').addEventListener('click', () => closeModal('cancelConfirmModal'));
document.getElementById('cancelConfirmBtn').addEventListener('click', () => {
    closeModal('cancelConfirmModal');
    const t = tickets.find(x => x.code === activeCode);
    if (t) t.status = 'cancelled';
    renderAll();
});

renderAll();