
// Minimal JS 


// Grab important elements from the page using data-* attributes.
// We use optional chaining so the code doesn't crash if something isn't found.
const btn = document.querySelector('[data-menu-btn]');   // the hamburger button you tap/click
const menu = document.querySelector('[data-menu]');      // the full-screen mobile menu panel
const links = menu?.querySelectorAll('[data-close]');    // any link inside menu that should close it

// Open the mobile menu (called when the button is toggled on)
function openMenu() {
    // tell screen readers the menu is expanded
    btn.setAttribute('aria-expanded', 'true');

    // Show the element immediately so it can animate in
    // Using the built-in 'hidden' attribute means display: none until false
    menu.hidden = false;

    // Wait one animation frame before adding the .open class.
    // Why? So the browser can apply "start" styles first, then animate to "open".
    requestAnimationFrame(() => menu.classList.add('open'));

    // Stop the background page from scrolling while the overlay is open
    document.body.style.overflow = 'hidden';
}

// Close the mobile menu called when the button is toggled off or user hits Escape
function closeMenu() {
    // let assistive tech know the menu is collapsed
    btn.setAttribute('aria-expanded', 'false');

    // Remove the .open class so CSS can play the closing transition
    menu.classList.remove('open');

    // Wait for the CSS transition to finish before hiding the element.
    // 180ms matches the CSS: transition
    setTimeout(() => menu.hidden = true, 180);

    // Re-enable page scrolling
    document.body.style.overflow = '';
}

// When the hamburger is clicked toggle between open and close states
btn?.addEventListener('click', () => {
    // Read the current ARIA state to decide what to do
    const open = btn.getAttribute('aria-expanded') === 'true';
    open ? closeMenu() : openMenu(); // if open close it if closed open it
});

// If any menu link has data-close clicking it will close the menu nice UI
links?.forEach(a => a.addEventListener('click', closeMenu));

// Add a global key listener so pressing Escape closes the menu accessibility + convenience
window.addEventListener('keydown', e => {
    if (e.key === 'Escape' && btn?.getAttribute('aria-expanded') === 'true') closeMenu();
});


// Optional: "active" state for desktop nav based on URL hash


// Find the desktop nav (does nothing on mobile overlay)
const deskNav = document.querySelector('nav.primary');

// This function marks the current section link as active using aria-current.
// It looks at window.location.hash and compares to link hrefs.
function setActive() {
    // If there's no hash in the URL, default to "#"
    const hash = location.hash || '#';

    // For every nav link if it matches the current hash otherwise remove it.
    deskNav?.querySelectorAll('a').forEach(a => {
        a.setAttribute('aria-current', a.getAttribute('href') === hash ? 'page' : null);
    });
}

// If the hash changes like clicking in-page links, update the active link
window.addEventListener('hashchange', setActive);

// Run once on page load so the correct link is highlighted immediately
setActive();
