let parks = [
    {
        id: 1, name: 'شهربازی ارم', rides: [
            { id: 101, name: 'ترن هوایی', price: 180000, image: '' },
            { id: 102, name: 'چرخ و فلک', price: 90000, image: '' },
        ]
    },
    {
        id: 2, name: 'لوناپارک نور', rides: [
            { id: 201, name: 'برج سقوط آزاد', price: 200000, image: '' },
        ]
    },
];
let nextParkId = 100, nextRideId = 1000;

function fmt(n) { return Math.round(n).toLocaleString('en-US'); }

function render() {
    const c = document.getElementById('parksContainer');
    c.innerHTML = parks.map(park => `
      <div class="park-card">
        <div class="park-header">
          <h2>${park.name}</h2>
          <div class="park-actions">
            <button class="icon-btn" onclick="openEditPark(${park.id})">✏️</button>
            <button class="icon-btn danger" onclick="openDelete('park', ${park.id}, '${park.name}')">🗑️</button>
            <button class="btn primary small" onclick="openAddRide(${park.id})">＋ بازی</button>
          </div>
        </div>
        ${park.rides.length === 0 ? '<div class="empty-rides">هنوز بازی‌ای ثبت نشده.</div>' : park.rides.map(r => `
          <div class="ride-row">
            <div class="ride-thumb" style="${r.image ? `background-image:url('${r.image}')` : ''}"></div>
            <div class="ride-info"><p class="name">${r.name}</p><span class="price">${fmt(r.price)} تومان</span></div>
            <div class="ride-actions">
              <button class="icon-btn" onclick='openEditRide(${park.id}, ${r.id})'>✏️</button>
              <button class="icon-btn danger" onclick="openDelete('ride', ${r.id}, '${r.name}')">🗑️</button>
            </div>
          </div>`).join('')}
      </div>`).join('');
}

function openModal(id) { document.getElementById(id).classList.add('show'); }
function closeModal(id) { document.getElementById(id).classList.remove('show'); }

function submitAddPark() {
    const name = document.getElementById('addParkName').value.trim();
    if (!name) return;
    parks.push({ id: nextParkId++, name, rides: [] });
    document.getElementById('addParkName').value = '';
    closeModal('addParkModal');
    render();
}

let editingParkId = null;
function openEditPark(id) {
    editingParkId = id;
    document.getElementById('editParkName').value = parks.find(p => p.id === id).name;
    openModal('editParkModal');
}
function submitEditPark() {
    const park = parks.find(p => p.id === editingParkId);
    park.name = document.getElementById('editParkName').value.trim() || park.name;
    closeModal('editParkModal');
    render();
}

let ridePendingParkId = null, ridePendingRideId = null;
function openAddRide(parkId) {
    ridePendingParkId = parkId; ridePendingRideId = null;
    document.getElementById('rideModalTitle').textContent = 'افزودن بازی جدید';
    document.getElementById('rideName').value = '';
    document.getElementById('ridePrice').value = '';
    document.getElementById('rideImageFile').value = '';
    document.getElementById('rideImagePreview').style.backgroundImage = '';
    openModal('rideModal');
}
function openEditRide(parkId, rideId) {
    ridePendingParkId = parkId; ridePendingRideId = rideId;
    const ride = parks.find(p => p.id === parkId).rides.find(r => r.id === rideId);
    document.getElementById('rideModalTitle').textContent = 'ویرایش بازی';
    document.getElementById('rideName').value = ride.name;
    document.getElementById('ridePrice').value = ride.price;
    document.getElementById('rideImageFile').value = '';
    document.getElementById('rideImagePreview').style.backgroundImage = ride.image ? `url('${ride.image}')` : '';
    openModal('rideModal');
}

let pendingImageDataUrl = null;
document.getElementById('rideImageFile').addEventListener('change', e => {
    const file = e.target.files[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = ev => {
        pendingImageDataUrl = ev.target.result;
        document.getElementById('rideImagePreview').style.backgroundImage = `url('${pendingImageDataUrl}')`;
    };
    reader.readAsDataURL(file);
});

function submitRide() {
    const name = document.getElementById('rideName').value.trim();
    const price = parseFloat(document.getElementById('ridePrice').value) || 0;
    const park = parks.find(p => p.id === ridePendingParkId);

    if (ridePendingRideId) {
        const ride = park.rides.find(r => r.id === ridePendingRideId);
        ride.name = name || ride.name;
        ride.price = price;
        if (pendingImageDataUrl) ride.image = pendingImageDataUrl;
    } else {
        park.rides.push({ id: nextRideId++, name, price, image: pendingImageDataUrl || '' });
    }
    pendingImageDataUrl = null;
    closeModal('rideModal');
    render();
}

let pendingDeleteType = null, pendingDeleteId = null;
function openDelete(type, id, name) {
    pendingDeleteType = type; pendingDeleteId = id;
    document.getElementById('deleteTargetName').textContent = name;
    openModal('deleteModal');
}
function confirmDelete() {
    if (pendingDeleteType === 'park') {
        parks = parks.filter(p => p.id !== pendingDeleteId);
    } else {
        parks.forEach(p => { p.rides = p.rides.filter(r => r.id !== pendingDeleteId); });
    }
    closeModal('deleteModal');
    render();
}

render();