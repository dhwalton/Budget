// modal click event
$(document).ready(function () {
    $('#view-closed').click(function () {
        $('#closed-accounts-modal').modal();
    });

    $('#btn-create-budget').click(function () {
        $('#create-budget-modal').modal();
    });

    $('#chart1').click(function () {
        //$('#chart-modal').html($('#chart1').html());
        //$('#chart-modal').modal();
    });
});