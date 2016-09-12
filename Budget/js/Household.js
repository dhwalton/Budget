// modal click event
$(document).ready(function () {
    $('#view-closed').click(function () {
        $('#closed-accounts-modal').modal();
    });

    $('#btn-create-budget').click(function () {
        $('#create-budget-modal').modal();
    });
});