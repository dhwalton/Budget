// enable date time picker
$(document).ready(function () {
    $('.data-table').DataTable();
    $('.data-table-bare').DataTable({ "paging": false, "searching": false, "info": false });
    //$('.no-search').DataTable({ "search": false });
});