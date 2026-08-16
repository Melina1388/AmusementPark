// ---- ترکاندن بادکنک‌ها با کلیک ----
const allBalloons = document.querySelectorAll('.balloon-group');
const gondola = document.getElementById('gondola');
allBalloons.forEach(group => {
    group.addEventListener('click', () => {
        if (group.classList.contains('popped')) return;
        group.classList.add('popped');
        const poppedCount = document.querySelectorAll('.balloon-group.popped').length;
        if (poppedCount === allBalloons.length) {
            setTimeout(() => {
                gondola.classList.add('falling');
                gondola.addEventListener('transitionend', function onFallEnd(ev) {
                    if (ev.propertyName === 'transform') {
                        gondola.style.visibility = 'hidden';
                        gondola.removeEventListener('transitionend', onFallEnd);
                    }
                });
            }, 350);
        }
    });
});

// ---- نمایش اطلاعات هر کاراکتر با هاور/کلیک (برای موبایل) ----
const cardSara = document.getElementById('cardSara');
const cardNegar = document.getElementById('cardNegar');
const charSara = document.querySelector('.character[data-member="sara"]');
const charNegar = document.querySelector('.character[data-member="negar"]');

function showCard(card, character) {
    card.classList.add('show');
    character.classList.add('active');
}
function hideCard(card, character) {
    card.classList.remove('show');
    character.classList.remove('active');
}

charSara.addEventListener('mouseenter', () => showCard(cardSara, charSara));
charSara.addEventListener('mouseleave', () => hideCard(cardSara, charSara));
charSara.addEventListener('click', () => cardSara.classList.contains('show') ? hideCard(cardSara, charSara) : showCard(cardSara, charSara));

charNegar.addEventListener('mouseenter', () => showCard(cardNegar, charNegar));
charNegar.addEventListener('mouseleave', () => hideCard(cardNegar, charNegar));
charNegar.addEventListener('click', () => cardNegar.classList.contains('show') ? hideCard(cardNegar, charNegar) : showCard(cardNegar, charNegar));

// ---- بازی بله/خیر: دکمه‌ی «خیر» هرگز کلیک نمی‌شه، همیشه فرار می‌کنه ----
const noBtn = document.getElementById('noBtn');
const yesBtn = document.getElementById('yesBtn');
const feedbackThanks = document.getElementById('feedbackThanks');
let noBtnPlaced = false;

function moveNoButtonAway() {
    const btnWidth = noBtn.offsetWidth || 110;
    const btnHeight = noBtn.offsetHeight || 46;
    const margin = 14;
    const maxLeft = Math.max(margin, window.innerWidth - btnWidth - margin);
    const maxTop = Math.max(margin, window.innerHeight - btnHeight - margin);
    const randomLeft = margin + Math.random() * (maxLeft - margin);
    const randomTop = margin + Math.random() * (maxTop - margin);
    noBtn.style.position = 'fixed';
    noBtn.style.left = randomLeft + 'px';
    noBtn.style.top = randomTop + 'px';
    noBtn.classList.remove('fleeing');
    void noBtn.offsetWidth;
    noBtn.classList.add('fleeing');
}

// با نزدیک شدن ماوس (حتی قبل از رسیدن به لبه‌ی دکمه، به‌خاطر ناحیه‌ی نامرئی اطراف آن) فرار می‌کنه
noBtn.addEventListener('mouseenter', moveNoButtonAway);
// حالت لمسی (موبایل): با اولین لمس فرار کنه
noBtn.addEventListener('touchstart', function (e) { e.preventDefault(); moveNoButtonAway(); }, { passive: false });
// اگه به هر شکلی (مثلاً کیبورد Tab) فوکوس گرفت، فرار کنه و فوکوس رو رها کنه
noBtn.addEventListener('focus', function () { moveNoButtonAway(); noBtn.blur(); });
// یک لایه‌ی احتیاطی: اگه هنوزم کلیک بشه، فقط فرار کنه و کاری انجام نده
noBtn.addEventListener('click', function (e) { e.preventDefault(); moveNoButtonAway(); });

function placeNoButtonAtStart() {
    const yesRect = yesBtn.getBoundingClientRect();
    if (!yesRect.width) { return; } // هنوز چیدمان آماده نیست، بعداً دوباره تلاش می‌کنیم
    const btnWidth = noBtn.offsetWidth || 110;
    const margin = 10;
    let left = yesRect.left - btnWidth - 16;
    if (left < margin) left = yesRect.right + 16; // اگر جا نبود، کنار راست بله
    noBtn.style.position = 'fixed';
    noBtn.style.top = yesRect.top + 'px';
    noBtn.style.left = left + 'px';
    noBtnPlaced = true;
}

placeNoButtonAtStart();
window.addEventListener('load', placeNoButtonAtStart);
window.addEventListener('resize', placeNoButtonAtStart);
// اطمینان از دیده‌شدن دکمه حتی اگر فونت/چیدمان دیر آماده بشه
setTimeout(placeNoButtonAtStart, 150);
setTimeout(placeNoButtonAtStart, 500);

function rainConfetti() {
    const colors = ['#ff3d7f', '#00d9c0', '#ffc94d', '#9d7bff'];
    const wrap = document.createElement('div');
    wrap.style.position = 'fixed';
    wrap.style.inset = '0';
    wrap.style.zIndex = '90';
    wrap.style.pointerEvents = 'none';
    wrap.style.overflow = 'hidden';
    document.body.appendChild(wrap);

    for (let i = 0; i < 90; i++) {
        const piece = document.createElement('span');
        piece.style.position = 'absolute';
        piece.style.top = '-20px';
        piece.style.left = Math.random() * 100 + '%';
        piece.style.width = (6 + Math.random() * 6) + 'px';
        piece.style.height = (10 + Math.random() * 8) + 'px';
        piece.style.background = colors[i % colors.length];
        piece.style.borderRadius = Math.random() > 0.5 ? '50%' : '2px';
        piece.style.opacity = '.9';
        piece.style.animation = `confetti-fall ${2.2 + Math.random() * 2.2}s linear ${Math.random() * 0.6}s forwards`;
        wrap.appendChild(piece);
    }
    setTimeout(() => wrap.remove(), 5000);
}

yesBtn.addEventListener('click', function () {
    rainConfetti();
    feedbackThanks.textContent = 'آفرین، منم با تو حال کردم.';
    feedbackThanks.classList.add('show');
});