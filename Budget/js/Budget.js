// modal click event
$(document).ready(function () {
    $('#btn-edit-budget').click(function () {
        $('#edit-budget-modal').modal();
    });
    $('.radio-income').click(function () {
        $('#add-budget-expense').fadeOut(100);
        $('#add-budget-income').fadeIn(200);
    });
    $('.radio-expense').click(function () {
        $('#add-budget-income').fadeOut(100);
        $('#add-budget-expense').fadeIn(200);
    });
});