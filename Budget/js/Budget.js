// modal click event
$(document).ready(function () {
    $('#btn-edit-budget').click(function () {
        $('#edit-budget-modal').modal();
    });
    $('.radio-income').click(function () {
        $('#add-budget-expense').hide();
        $('#add-budget-income').show();
    });
    $('.radio-expense').click(function () {
        $('#add-budget-income').hide();
        $('#add-budget-expense').show();
    });
    $('.btn-category-edit').click(function () {
        var id = $(this).parent().find('input').val();

    });
});