// =========================
// Minimal JS (hamburger only)
// =========================
const btn = document.querySelector('[data-menu-btn]');
const menu = document.querySelector('[data-menu]');
const links = menu?.querySelectorAll('[data-close]');

function openMenu() {
    btn.setAttribute('aria-expanded', 'true');
    menu.hidden = false;
    requestAnimationFrame(() => menu.classList.add('open'));
    document.body.style.overflow = 'hidden'; // prevent background scroll
}
function closeMenu() {
    btn.setAttribute('aria-expanded', 'false');
    menu.classList.remove('open');
    setTimeout(() => menu.hidden = true, 180);
    document.body.style.overflow = '';
}

btn?.addEventListener('click', () => {
    const open = btn.getAttribute('aria-expanded') === 'true';
    open ? closeMenu() : openMenu();
});
links?.forEach(a => a.addEventListener('click', closeMenu));
window.addEventListener('keydown', e => {
    if (e.key === 'Escape' && btn?.getAttribute('aria-expanded') === 'true') closeMenu();
});

// Optional: mark current hash link active on load (desktop nav)
const deskNav = document.querySelector('nav.primary');
function setActive() {
    const hash = location.hash || '#';
    deskNav?.querySelectorAll('a').forEach(a => {
        a.setAttribute('aria-current', a.getAttribute('href') === hash ? 'page' : null);
    });
}
window.addEventListener('hashchange', setActive);
setActive();
