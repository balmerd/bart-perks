jQuery(document).ready(function ($) {
    function hideModal() {
        $('#modal').hide();
    }

    function showModal(errorMessage, errorDetails) {
        setTimeout(function () {
            $('#modal-message').html(errorMessage + (errorDetails ? '<p class="error-details">(see console for details)</p>' : ''));
            $('#modal').show();
        }, 500);
        $(document).on('keyup', function (e) {
            hideModal();
            $(document).off('keyup');
        });
    }

    $('#modal').on('click', hideModal);

    var errorMessage = $('#errorMessage').text();
    var errorDetails = $('#errorDetails').text();

    if (errorMessage !== '') {
        showModal(errorMessage, errorDetails);
    }

    if (errorDetails !== '') {
        console.log(errorDetails);
    }
});
