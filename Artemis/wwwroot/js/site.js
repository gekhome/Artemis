// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Initialize elements for popper-styled tooltips
(function () {
    'use strict'
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl)
    })
})()

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    let mybutton = document.getElementsByClassName("topBtn")[0];
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}


function toggleNav() {
    MenuStatus = document.getElementById('sidebar').style.display;
    if (MenuStatus == 'none') {
        document.getElementById('main').style.marginLeft = '15%';
        document.getElementById('sidebar').style.display = 'block';
        document.getElementById('footer').style.marginLeft = "15%";
        document.getElementById('footer').style.display = 'block';
    } else {
        document.getElementById('sidebar').style.display = 'none';
        document.getElementById('main').style.marginLeft = '0%';
        document.getElementById('footer').style.marginLeft = '0%';
    }
}

function toggleSidebar() {
    var backdropHeight = $(document).height();
    $('#backdrop').css('height', backdropHeight);

    MenuStatus = document.getElementById('sidebar').style.display;
    if (MenuStatus == 'none') {
        sidebarOpen();
        renderCloseIcon();
    } else {
        sidebarClose();
        renderToggleIcon();
    }
}


function sidebarOpen() {
    document.getElementById('sidebar').style.display = 'block';
    $('#backdrop').fadeIn(100);
}

function sidebarClose() {
    document.getElementById('sidebar').style.display = 'none';
    $('#backdrop').fadeOut(100);
    renderToggleIcon();
}

function renderToggleIcon() {
    var topBar = document.getElementById('top-bar');
    var midBar = document.getElementById('mid-bar');
    var botBar = document.getElementById('bot-bar');

    topBar.style.top = '0';
    topBar.style.position = 'absolute';
    topBar.style.transform = "rotate(0deg)";

    midBar.style.top = '8px';
    midBar.style.position = 'absolute';
    midBar.style.opacity = '1';

    botBar.style.top = '16px';
    botBar.style.position = 'absolute';
    botBar.style.transform = "rotate(0deg)";
}

function renderCloseIcon() {
    var topBar = document.getElementById('top-bar');
    var midBar = document.getElementById('mid-bar');
    var botBar = document.getElementById('bot-bar');

    topBar.style.top = 'inherit';
    topBar.style.transform = "rotate(135deg)";

    midBar.style.top = 'inherit';
    midBar.style.opacity = '0';

    botBar.style.top = 'inherit';
    botBar.style.transform = "rotate(-135deg)";
}