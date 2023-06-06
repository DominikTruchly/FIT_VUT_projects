// LOGIN: xtruch01
// NAME: DOMINIK TRUCHLY

(function () {
  "use strict";

  // SELECT ELEMENTS
  const select = (element, all = false) => {
    element = element.trim()
    if (all) {
      return [...document.querySelectorAll(element)]
    } 
    else {
      return document.querySelector(element)
    }
  }

  // EVENT LISTENER
  const on = (type, element, listener, all = false) => {
    let selectedElement = select(element, all)
    if (selectedElement) {
      if (all) {
        selectedElement.forEach(e => e.addEventListener(type, listener))
      } 
      else {
        selectedElement.addEventListener(type, listener)
      }
    }
  }

  // SCROLL EVENT LISTENER
  const onscroll = (element, listener) => {
    element.addEventListener('scroll', listener)
  }

  // NAVBAR LINKS UPDATING
  let navbarlinks = select('#navbar .scrollto', true)
  const navbarlinksActive = () => {
    let position = window.scrollY + 200
    navbarlinks.forEach(navbarlink => {
      if (!navbarlink.hash) return
      let section = select(navbarlink.hash)
      if (!section) return
      if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
        navbarlink.classList.add('active')
      } 
      else {
        navbarlink.classList.remove('active')
      }
    })
  }
  window.addEventListener('load', navbarlinksActive)
  onscroll(document, navbarlinksActive)

  // SCOLL TO THE ELEMENT
  const scrollto = (element) => {
    let header = select('#header')
    let offset = header.offsetHeight

    if (!header.classList.contains('header-scrolled')) {
      offset -= 16
    }

    let elementPos = select(element).offsetTop
    window.scrollTo({
      top: elementPos - offset,
      behavior: 'smooth'
    })
  }

  // TOGGLE SCROLLED CLASS ON HEADER
  let selectHeader = select('#header')
  if (selectHeader) {
    const headerScrolled = () => {
      if (window.scrollY > 100) {
        selectHeader.classList.add('header-scrolled')
      } 
      else {
        selectHeader.classList.remove('header-scrolled')
      }
    }
    window.addEventListener('load', headerScrolled)
    onscroll(document, headerScrolled)
  }

  // BACK TO THE TOP BUTTON
  let backtotop = document.querySelector('.back-to-top');
  if (backtotop) {
    const toggleBacktotop = () => {
      if (window.scrollY > 100) {
        backtotop.classList.add('active');
      } 
      else {
        backtotop.classList.remove('active');
      }
    };
    window.addEventListener('load', toggleBacktotop);
    window.addEventListener('scroll', toggleBacktotop);

    backtotop.addEventListener('click', function (event) {
      event.preventDefault();
      const targetPosition = 0;
      const startPosition = window.pageYOffset;
      const distance = targetPosition - startPosition;
      const duration = 1000;
      let start = null;

      function step(timestamp) {
        if (!start) start = timestamp;
        const progress = timestamp - start;
        window.scrollTo(0, easeInOutCubic(progress, startPosition, distance, duration));
        if (progress < duration) window.requestAnimationFrame(step);
      }
      function easeInOutCubic(t, b, c, d) {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
      }
      window.requestAnimationFrame(step);
    });
  }

  // MOBILE NAVBAR TOGGLE
  on('click', '.scrollto', function (e) {
    if (select(this.hash)) {
      e.preventDefault()

      let navbar = select('#navbar')
      if (navbar.classList.contains('navbar-mobile')) {
        navbar.classList.remove('navbar-mobile')
        let navbarToggle = select('.mobile-nav-toggle')
        navbarToggle.classList.toggle('bi-list')
        navbarToggle.classList.toggle('bi-x')
      }
      scrollto(this.hash)
    }
  }, true)

  const menuToggle = document.querySelector('.menu-toggle');
  const navLinks = document.querySelector('#navbar ul');

  menuToggle.addEventListener('click', () => {
    menuToggle.classList.toggle('active');
    navLinks.classList.toggle('active');
  });

  navLinks.querySelectorAll('a').forEach((link) => {
    link.addEventListener('click', () => {
      menuToggle.classList.remove('active');
      navLinks.classList.remove('active');
    });
  });

})()
