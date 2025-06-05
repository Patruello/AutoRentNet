//--------------------------------------------------------------
//  Helpers
//--------------------------------------------------------------

// pobieranie JSON z obsługą błędów
async function fetchJSON(url, options = {}) {
    const res = await fetch(url, {
        headers: { 'Content-Type': 'application/json' },
        ...options
    });
    if (!res.ok) {
        const msg = await res.text().catch(() => res.statusText);
        throw msg || res.statusText;
    }
    return res.status === 204 ? null : res.json();
}

// "dd.MM.yyyy"  lub "yyyy-MM-dd" (+ " HH:mm")  -> Date
function toDateTime(raw) {
    if (raw instanceof Date) return raw;
    if (!raw) throw 'Brak daty';

    const [datePart, timePart = '00:00'] = String(raw).trim().split(/[ T]/);

    let isoDate;
    if (datePart.includes('-')) {
        isoDate = datePart;                 // yyyy-MM-dd
    } else {
        const [d, m, y] = datePart.split('.');
        isoDate = `${y}-${m.padStart(2, '0')}-${d.padStart(2, '0')}`; // dd.MM.yyyy
    }
    return new Date(`${isoDate}T${timePart}:00`);
}

// sprawdza, czy rezerwacje nie kolidują
function isVehicleAvailable(vehicleId, startRaw, endRaw, reservations) {
    const s = toDateTime(startRaw);
    const e = toDateTime(endRaw);

    return !reservations.some(r =>
        r.vehicleId === vehicleId &&
        new Date(r.pickupDateTime) < e &&
        s < new Date(r.dropoffDateTime)
    );
}

//--------------------------------------------------------------
//  INDEX.html  – wyszukiwanie
//--------------------------------------------------------------
async function searchCars() {
    const pickupLoc  = document.getElementById('pickupLocation')?.value;
    const dropoffLoc = document.getElementById('dropoffLocation')?.value;
    const pickupDate = document.getElementById('pickupDate')?.value;
    const pickupTime = document.getElementById('pickupTime')?.value || '00:00';
    const dropDate   = document.getElementById('dropoffDate')?.value;
    const dropTime   = document.getElementById('dropoffTime')?.value || '00:00';

    if (!pickupLoc || !dropoffLoc || !pickupDate || !dropDate) {
        alert('Uzupełnij wszystkie pola miejsca i dat.');
        return;
    }

    const pickupDT = `${pickupDate} ${pickupTime}`;
    const dropDT   = `${dropDate} ${dropTime}`;

    if (toDateTime(pickupDT) >= toDateTime(dropDT)) {
        alert('Data zwrotu musi być późniejsza od daty odbioru.');
        return;
    }

    try {
        const [vehicles, reservations] = await Promise.all([
            fetchJSON('/api/vehicles'),
            fetchJSON('/api/reservations')
        ]);

        const results = vehicles.filter(v =>
            isVehicleAvailable(v.id, pickupDT, dropDT, reservations)
        );

        sessionStorage.setItem('searchResults',        JSON.stringify(results));
        sessionStorage.setItem('pickupLocation',       pickupLoc);
        sessionStorage.setItem('dropoffLocation',      dropoffLoc);
        sessionStorage.setItem('pickupDateTime',       pickupDT);
        sessionStorage.setItem('dropoffDateTime',      dropDT);

        window.location.href = 'reservation.html';
    } catch (err) {
        console.error(err);
        alert('Błąd pobierania danych: ' + err);
    }
}

//--------------------------------------------------------------
//  RESERVATION.html – lista i modal
//--------------------------------------------------------------
async function loadSearchResults() {
    const container = document.getElementById('reservationContainer');
    if (!container) return;

    const data = JSON.parse(sessionStorage.getItem('searchResults') || '[]');

    container.innerHTML = data.length
        ? ''
        : '<p>Brak dostępnych pojazdów dla wybranego terminu.</p>';

    data.forEach(v => {
        container.insertAdjacentHTML(
            'beforeend',
            `<div class="col-md-4 mb-3">
         <div class="card">
           <img src="${v.image}" class="card-img-top" alt="${v.name}">
           <div class="card-body">
             <h5 class="card-title">${v.name}</h5>
             <div class="d-flex justify-content-between">
                 <div class="icon" data-tooltip="Rodzaj paliwa"><i class="material-symbols-outlined">local_gas_station</i> ${v.fuel}</div>
                 <div class="icon" data-tooltip="Rodzaj skrzyni biegów"><i class="material-symbols-outlined">auto_transmission</i> ${v.transmission}</div>
                 <div class="icon" data-tooltip="Średnie spalanie"><img src="/img/gas.png" class="icon-img"> ${v.consumption}</div>
                 <div class="icon" data-tooltip="Pojemność bagażnika"><i class="material-symbols-outlined">luggage</i> ${v.trunk}</div>
                 <div class="icon" data-tooltip="Liczba drzwi"><img src="/img/door.png" class="icon-img"> ${v.doors}</div>
                 <div class="icon" data-tooltip="Liczba siedzeń"><i class="material-symbols-outlined">airline_seat_recline_normal</i> ${v.seats}</div>
               </div>
             <button class="btn btn-primary" onclick="openForm(${v.id}, '${v.name.replace(/'/g, '&#39;')}')">
               Rezerwuj
             </button>
           </div>
         </div>
       </div>`
        );
    });
}

function openForm(id, name) {
    const pickupLoc  = sessionStorage.getItem('pickupLocation');
    const dropoffLoc = sessionStorage.getItem('dropoffLocation');
    const pickupDT   = sessionStorage.getItem('pickupDateTime');
    const dropDT     = sessionStorage.getItem('dropoffDateTime');

    // podgląd
    document.getElementById('summaryVehicle')   .textContent = name;
    document.getElementById('summaryPickupLoc') .textContent = pickupLoc;
    document.getElementById('summaryDropoffLoc').textContent = dropoffLoc;
    document.getElementById('summaryPickupDt')  .textContent = pickupDT;
    document.getElementById('summaryDropoffDt') .textContent = dropDT;

    // hidden fields (ISO do API)
    document.getElementById('vehicleId')       .value = id;
    document.getElementById('pickupLocation')  .value = pickupLoc;
    document.getElementById('dropoffLocation') .value = dropoffLoc;
    document.getElementById('pickupDateTime')  .value = new Date(pickupDT).toISOString();
    document.getElementById('dropoffDateTime') .value = new Date(dropDT).toISOString();

    bootstrap.Modal.getOrCreateInstance('#reservationModal').show();
}

async function submitReservation(e) {
    e.preventDefault();
    const dto = Object.fromEntries(new FormData(e.target));

    try {
        await fetchJSON('/api/reservations', {
            method: 'POST',
            body: JSON.stringify(dto)
        });
        alert('Rezerwacja zapisana ✔');
        bootstrap.Modal.getInstance('#reservationModal').hide();
    } catch (err) {
        alert('Błąd: ' + err);
    }
}

//--------------------------------------------------------------
//  FLEET.html  – wyszukiwanie
//--------------------------------------------------------------

async function loadFleet() {
    const box = document.getElementById('fleetContainer');
    if (!box) return;                                   

    box.innerHTML = '<p class="text-center w-100">Ładuję flotę…</p>';

    try {
        const vehicles = await fetchJSON('/api/vehicles');

        box.innerHTML = '';
        vehicles.forEach(v => {
            box.insertAdjacentHTML(
                'beforeend',
                `<div class="col-lg-4 col-sm-6 col-xs-12">
           <div class="card">
             <img src="${v.image}" class="card-img-top" alt="${v.name}">
             <div class="card-body">
               <h5 class="card-title">${v.name}</h5>
               <div class="d-flex justify-content-between">
                 <div class="icon" data-tooltip="Rodzaj paliwa"><i class="material-symbols-outlined">local_gas_station</i> ${v.fuel}</div>
                 <div class="icon" data-tooltip="Rodzaj skrzyni biegów"><i class="material-symbols-outlined">auto_transmission</i> ${v.transmission}</div>
                 <div class="icon" data-tooltip="Średnie spalanie"><img src="/img/gas.png" class="icon-img"> ${v.consumption}</div>
                 <div class="icon" data-tooltip="Pojemność bagażnika"><i class="material-symbols-outlined">luggage</i> ${v.trunk}</div>
                 <div class="icon" data-tooltip="Liczba drzwi"><img src="/img/door.png" class="icon-img"> ${v.doors}</div>
                 <div class="icon" data-tooltip="Liczba siedzeń"><i class="material-symbols-outlined">airline_seat_recline_normal</i> ${v.seats}</div>
               </div>
             </div>
           </div>
         </div>`
            );
        });
    } catch (err) {
        box.innerHTML = `<p class="text-danger">Błąd ładowania floty: ${err}</p>`;
    }
}

//--------------------------------------------------------------
//  CONTACT.html – przełączanie sekcji
//--------------------------------------------------------------
function showSection(id) {
    const section = document.getElementById(id);
    if (!section) return;                    // 🔒 ochrona – brak sekcji

    document.querySelectorAll('.contact-section')
        .forEach(sec => (sec.style.display = 'none'));
    section.style.display = 'block';
}

//--------------------------------------------------------------
//  Init
//--------------------------------------------------------------
document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('reservationContainer')) loadSearchResults();
    if (document.getElementById('fleetContainer'))       loadFleet();
});