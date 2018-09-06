jQuery(document).ready(function ($) {
    var alertMessage = $('#alertMessage').text();

    setTimeout(function () {
        if (alertMessage !== '') {
            alert(alertMessage);
        }
    }, 500);
});
