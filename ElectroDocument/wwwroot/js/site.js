﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Get the image element
const image = document.querySelector('.nav-link-exit');

var tokenKey = "accessToken";

// Add click event listener
image.addEventListener('click', function () {
    sessionStorage.removeItem(tokenKey);
    document.location.href = '/logout';
});

