let parks = [];

const API_BASE_URL =
    'https://localhost:7068';

let editingParkName = null;

let ridePendingParkName = null;
let ridePendingRideId = null;

let pendingImageDataUrl = null;

let pendingDeleteType = null;
let pendingDeleteId = null;
let pendingDeleteParkName = null;


// =====================================================
// Helpers
// =====================================================

function fmt(n) {
    return Math.round(Number(n) || 0)
        .toLocaleString('en-US');
}


function escapeHtml(value) {
    return String(value ?? '')
        .replaceAll('&', '&amp;')
        .replaceAll('<', '&lt;')
        .replaceAll('>', '&gt;')
        .replaceAll('"', '&quot;')
        .replaceAll("'", '&#039;');
}


function escapeJs(value) {
    return String(value ?? '')
        .replaceAll('\\', '\\\\')
        .replaceAll("'", "\\'");
}


// =====================================================
// API
// =====================================================

async function apiRequest(url, options = {}) {

    const response = await fetch(
        `${API_BASE_URL}${url}`,
        {
            headers: {
                'Content-Type': 'application/json'
            },
            ...options
        }
    );

    if (!response.ok) {

        let message =
            'خطایی در ارتباط با سرور رخ داد.';

        try {
            const data = await response.json();

            if (data.message) {
                message = data.message;
            }
        }
        catch {
            // response may not contain JSON
        }

        throw new Error(message);
    }

    if (response.status === 204) {
        return null;
    }

    try {
        return await response.json();
    }
    catch {
        return null;
    }
}


// =====================================================
// Load
// =====================================================

async function loadAdminData() {

    try {

        

        const response =
            await fetch(
                `${API_BASE_URL}/api/GameApi`
            );

        if (!response.ok) {
            throw new Error(
                'دریافت بازی‌ها انجام نشد.'
            );
        }

        const games =
            await response.json();


        /*
         * از Game.AmusementName
         * دسته‌بندی شهربازی‌ها را می‌سازیم.
         */

        const parkNames = [
            ...new Set(
                games
                    .map(game => game.amusementName)
                    .filter(name =>
                        name &&
                        name.trim() !== ''
                    )
            )
        ];


        parks = parkNames.map(name => ({
            name: name,
            rides: games.filter(
                game =>
                    game.amusementName === name
            )
        }));


        render();

    }
    catch (error) {

        console.error(error);

        alert(
            error.message ||
            'دریافت اطلاعات انجام نشد.'
        );
    }
}


// =====================================================
// Render
// =====================================================

function render() {

    const container =
        document.getElementById(
            'parksContainer'
        );

    if (!container)
        return;


    if (parks.length === 0) {

        container.innerHTML = `
            <div class="empty-rides">
                هنوز هیچ شهربازی ثبت نشده است.
            </div>
        `;

        return;
    }


    container.innerHTML =
        parks.map(park => {

            return `
                <div class="park-card">

                    <div class="park-header">

                        <h2>
                            ${escapeHtml(park.name)}
                        </h2>

                        <div class="park-actions">

                            <button
                                class="icon-btn"
                                onclick="openEditPark('${escapeJs(park.name)}')">
                                ✏️
                            </button>

                            <button
                                class="icon-btn danger"
                                onclick="openDeletePark('${escapeJs(park.name)}')">
                                🗑️
                            </button>

                            <button
                                class="btn primary small"
                                onclick="openAddRide('${escapeJs(park.name)}')">
                                ＋ بازی
                            </button>

                        </div>

                    </div>


                    ${
                        park.rides.length === 0

                        ?

                        `
                        <div class="empty-rides">
                            هنوز بازی‌ای ثبت نشده.
                        </div>
                        `

                        :

                        park.rides.map(ride => {

                            const imageStyle =
                                ride.gamePic
                                    ? `background-image:url('${escapeJs(ride.gamePic)}')`
                                    : '';

                            return `
                                <div class="ride-row">

                                    <div
                                        class="ride-thumb"
                                        style="${imageStyle}">
                                    </div>

                                    <div class="ride-info">

                                        <p class="name">
                                            ${escapeHtml(
                                                ride.gameName
                                            )}
                                        </p>

                                        <span class="price">
                                            ${fmt(
                                                ride.gamePrice
                                            )} تومان
                                        </span>

                                    </div>

                                    <div class="ride-actions">

                                        <button
                                            class="icon-btn"
                                            onclick="openEditRide(
                                                '${escapeJs(park.name)}',
                                                ${ride.gameID}
                                            )">
                                            ✏️
                                        </button>

                                        <button
                                            class="icon-btn danger"
                                            onclick="openDeleteRide(
                                                ${ride.gameID},
                                                '${escapeJs(ride.gameName)}'
                                            )">
                                            🗑️
                                        </button>

                                    </div>

                                </div>
                            `;
                        }).join('')
                    }

                </div>
            `;

        }).join('');
}


// =====================================================
// Modal
// =====================================================

function openModal(id) {

    const modal =
        document.getElementById(id);

    if (modal) {
        modal.classList.add('show');
    }
}


function closeModal(id) {

    const modal =
        document.getElementById(id);

    if (modal) {
        modal.classList.remove('show');
    }
}


// =====================================================
// Add Park
// =====================================================

function submitAddPark() {

    const name =
        document
            .getElementById('addParkName')
            .value
            .trim();


    if (!name) {

        alert(
            'نام شهربازی را وارد کنید.'
        );

        return;
    }


    /*
     * نکته:
     *
     * چون در طراحی فعلی فقط جدول Game داریم،
     * شهربازی بدون بازی قابل ذخیره نیست.
     *
     * بنابراین بعد از وارد کردن نام شهربازی،
     * مستقیماً فرم اولین بازی باز می‌شود.
     */

    const alreadyExists =
        parks.some(
            park =>
                park.name.toLowerCase() ===
                name.toLowerCase()
        );


    if (alreadyExists) {

        alert(
            'این شهربازی قبلاً وجود دارد.'
        );

        return;
    }


    document
        .getElementById('addParkName')
        .value = '';


    closeModal(
        'addParkModal'
    );


    openAddRide(name);
}


// =====================================================
// Edit Park
// =====================================================

function openEditPark(name) {

    editingParkName = name;

    document
        .getElementById('editParkName')
        .value = name;

    openModal(
        'editParkModal'
    );
}


async function submitEditPark() {

    const newName =
        document
            .getElementById('editParkName')
            .value
            .trim();


    if (!newName) {

        alert(
            'نام جدید را وارد کنید.'
        );

        return;
    }


    if (
        newName.toLowerCase() ===
        editingParkName.toLowerCase()
    ) {

        closeModal(
            'editParkModal'
        );

        return;
    }


    try {

        await apiRequest(
            '/api/GameApi/parks',
            {
                method: 'PUT',

                body: JSON.stringify({
                    oldName:
                        editingParkName,

                    newName:
                        newName
                })
            }
        );


        closeModal(
            'editParkModal'
        );


        await loadAdminData();

    }
    catch (error) {

        alert(error.message);
    }
}


// =====================================================
// Add Game
// =====================================================

function openAddRide(
    parkName) {

    ridePendingParkName =
        parkName;

    ridePendingRideId =
        null;

    pendingImageDataUrl =
        null;


    document
        .getElementById(
            'rideModalTitle'
        )
        .textContent =
        'افزودن بازی جدید';


    document
        .getElementById(
            'rideName'
        )
        .value = '';


    document
        .getElementById(
            'ridePrice'
        )
        .value = '';


    document
        .getElementById(
            'rideImageFile'
        )
        .value = '';


    document
        .getElementById(
            'rideImagePreview'
        )
        .style
        .backgroundImage = '';


    openModal(
        'rideModal'
    );
}


// =====================================================
// Edit Game
// =====================================================

function openEditRide(
    parkName,
    rideId) {

    const park =
        parks.find(
            p =>
                p.name === parkName
        );


    if (!park)
        return;


    const ride =
        park.rides.find(
            r =>
                r.gameID === rideId
        );


    if (!ride)
        return;


    ridePendingParkName =
        parkName;

    ridePendingRideId =
        rideId;

    pendingImageDataUrl =
        null;


    document
        .getElementById(
            'rideModalTitle'
        )
        .textContent =
        'ویرایش بازی';


    document
        .getElementById(
            'rideName'
        )
        .value =
        ride.gameName || '';


    document
        .getElementById(
            'ridePrice'
        )
        .value =
        ride.gamePrice || '';


    document
        .getElementById(
            'rideImageFile'
        )
        .value = '';


    document
        .getElementById(
            'rideImagePreview'
        )
        .style
        .backgroundImage =
        ride.gamePic
            ? `url('${escapeJs(ride.gamePic)}')`
            : '';


    openModal(
        'rideModal'
    );
}


// =====================================================
// Image Preview
// =====================================================

const imageInput =
    document.getElementById(
        'rideImageFile'
    );


if (imageInput) {

    imageInput.addEventListener(
        'change',
        function (event) {

            const file =
                event.target.files[0];


            if (!file)
                return;


            const reader =
                new FileReader();


            reader.onload =
                function (event) {

                    pendingImageDataUrl =
                        event.target.result;


                    document
                        .getElementById(
                            'rideImagePreview'
                        )
                        .style
                        .backgroundImage =
                        `url('${pendingImageDataUrl}')`;
                };


            reader.readAsDataURL(file);
        }
    );
}


// =====================================================
// Save Game
// =====================================================

async function submitRide() {

    const name =
        document
            .getElementById('rideName')
            .value
            .trim();


    const price =
        Number(
            document
                .getElementById('ridePrice')
                .value
        );


    if (!name) {

        alert(
            'نام بازی را وارد کنید.'
        );

        return;
    }


    if (!price || price <= 0) {

        alert(
            'قیمت بازی را وارد کنید.'
        );

        return;
    }


    try {

        // ==========================================
        // UPDATE
        // ==========================================

        if (
            ridePendingRideId !== null
        ) {

            const park =
                parks.find(
                    p =>
                        p.name ===
                        ridePendingParkName
                );


            if (!park)
                throw new Error(
                    'شهربازی پیدا نشد.'
                );


            const ride =
                park.rides.find(
                    r =>
                        r.gameID ===
                        ridePendingRideId
                );


            if (!ride)
                throw new Error(
                    'بازی پیدا نشد.'
                );


            const game = {

                gameID:
                    ride.gameID,

                gameName:
                    name,

                amusementName:
                    ridePendingParkName,

                gamePrice:
                    price,

                gameComment:
                    ride.gameComment || '',

                gamePic:
                    pendingImageDataUrl ??
                    ride.gamePic ??
                    ''
            };


            await apiRequest(
                `/api/GameApi/${ride.gameID}`,
                {
                    method: 'PUT',

                    body:
                        JSON.stringify(game)
                }
            );
        }


        // ==========================================
        // INSERT
        // ==========================================

        else {

            const game = {

                gameName:
                    name,

                amusementName:
                    ridePendingParkName,

                gamePrice:
                    price,

                gameComment:
                    '',

                gamePic:
                    pendingImageDataUrl || ''
            };


            await apiRequest(
                '/api/GameApi',
                {
                    method: 'POST',

                    body:
                        JSON.stringify(game)
                }
            );
        }


        pendingImageDataUrl =
            null;


        closeModal(
            'rideModal'
        );


        await loadAdminData();

    }
    catch (error) {

        alert(error.message);
    }
}


// =====================================================
// Delete Park
// =====================================================

function openDeletePark(
    parkName) {

    pendingDeleteType =
        'park';

    pendingDeleteParkName =
        parkName;

    pendingDeleteId =
        null;


    document
        .getElementById(
            'deleteTargetName'
        )
        .textContent =
        parkName;


    openModal(
        'deleteModal'
    );
}


// =====================================================
// Delete Game
// =====================================================

function openDeleteRide(
    rideId,
    rideName) {

    pendingDeleteType =
        'ride';

    pendingDeleteId =
        rideId;

    pendingDeleteParkName =
        null;


    document
        .getElementById(
            'deleteTargetName'
        )
        .textContent =
        rideName;


    openModal(
        'deleteModal'
    );
}


// =====================================================
// Confirm Delete
// =====================================================

async function confirmDelete() {

    try {

        // ==========================================
        // DELETE PARK
        // ==========================================

        if (
            pendingDeleteType ===
            'park'
        ) {

            const encodedName =
                encodeURIComponent(
                    pendingDeleteParkName
                );


            await apiRequest(
                `/api/GameApi/parks?name=${encodedName}`,
                {
                    method: 'DELETE'
                }
            );
        }


        // ==========================================
        // DELETE GAME
        // ==========================================

        else {

            await apiRequest(
                `/api/GameApi/${pendingDeleteId}`,
                {
                    method: 'DELETE'
                }
            );
        }


        closeModal(
            'deleteModal'
        );


        await loadAdminData();

    }
    catch (error) {

        alert(error.message);
    }
}


// =====================================================
// Start
// =====================================================

loadAdminData();