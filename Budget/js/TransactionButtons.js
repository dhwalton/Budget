// enable date time picker

function voidTransaction(id) {
    var url = "../../Transactions/VoidTransaction/" + id;
    $.get(url, function (html) {
        location.reload();
    });
}

function deleteTransaction(id) {
    var url = "../../Transactions/ToggleTransaction/" + id;
    $.get(url, function (html) {
        location.reload();
    });
}

function reconcileTransaction(id) {
    var url = "../../Transactions/ToggleReconciled/" + id;
    $.get(url, function (html) {
        location.reload();
    });
}

$(document).ready(function () {
    $('.btn-edit-transaction').click(function () {
        var id = $(this).parent().attr('id');
        var url = "../../Transactions/EditModal/" + id;
        var modalId = "#edit-deposit-modal";
        $.get(url, function (html) {
            $(modalId).html(html);
            $(modalId).modal();
        });
    });

    $('.btn-reconcile-transaction').click(function () {
        var id = $(this).parent().attr('id');
        reconcileTransaction(id);
    });

    $('.btn-unreconcile-transaction').click(function () {
        var id = $(this).parent().attr('id');
        reconcileTransaction(id);
    });

    $('.btn-void-transaction').click(function () {
        var id = $(this).parent().attr('id');
        voidTransaction(id);
        
    });

    $('.btn-unvoid-transaction').click(function () {
        var id = $(this).parent().attr('id');
        voidTransaction(id);
    });

    $('.btn-delete-transaction').click(function () {
        var id = $(this).parent().attr('id');
        deleteTransaction(id);
    });
});