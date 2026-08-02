const menuBtn = document.getElementById('menuBtn');
const sidebar = document.getElementById('sidebar');
const overlay = document.getElementById('overlay');
function toggleSidebar(open) {
    menuBtn.classList.toggle('open', open);
    sidebar.classList.toggle('show', open);
    overlay.classList.toggle('show', open);
}
menuBtn.addEventListener('click', () => toggleSidebar(!sidebar.classList.contains('show')));
overlay.addEventListener('click', () => toggleSidebar(false));

document.getElementById('backToTop').addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
});

const searchInput = document.getElementById('searchInput');
const sections = document.querySelectorAll('.park-section');
searchInput.addEventListener('input', (e) => {
    const q = e.target.value.trim();
    sections.forEach(sec => {
        const name = sec.getAttribute('data-park-name');
        sec.style.display = name.includes(q) ? '' : 'none';
    });
});

// نمایان‌سازی تدریجی هر بخش شهربازی هنگام اسکرول
const revealObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('revealed');
            revealObserver.unobserve(entry.target);
        }
    });
}, { threshold: .15 });
sections.forEach(sec => revealObserver.observe(sec));

// تولید چند تکه کنفتی پس‌زمینه (سبک و کم‌تعداد برای حفظ کارایی)
const confettiColors = ['#ff3d7f', '#00d9c0', '#ffc94d', '#9d7bff'];
const confettiWrap = document.getElementById('confetti');
for (let i = 0; i < 14; i++) {
    const piece = document.createElement('span');
    piece.style.left = Math.random() * 100 + '%';
    piece.style.background = confettiColors[i % confettiColors.length];
    piece.style.animationDuration = (7 + Math.random() * 6) + 's';
    piece.style.animationDelay = (Math.random() * 8) + 's';
    confettiWrap.appendChild(piece);
}