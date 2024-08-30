// JavaScript to hide the spinner after the page has fully loaded
document.addEventListener("DOMContentLoaded", function () {
    const spinner = document.getElementById('spinner');
    spinner.classList.add('hide');

    // Optionally remove the spinner element from the DOM after the transition
    spinner.addEventListener('transitionend', function () {
        spinner.remove();
    });
});

