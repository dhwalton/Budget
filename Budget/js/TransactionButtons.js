// enable date time picker
$(document).ready(function () {
    $('.btn-edit-transaction').click(function () {
        var id = $(this).attr('id');
        var url = "../../Transactions/Edit/" + id;
        var modalId = "#edit-deposit-modal";
        //$.ajax({
        //    type: get,
        //    async: false,
        //    url: url,
        //    success: function () {
        //        $('#edit-deposit-modal').html(html);
        //        $('#edit-deposit-modal').modal();
        //    }
        //});

        $.get(url, function (html) {
            $(modalId).html(html);
            $(modalId).modal();
        });
    })
});