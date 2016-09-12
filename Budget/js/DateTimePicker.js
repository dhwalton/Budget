// enable date time picker
$(document).ready(function () {
    $.datetimepicker.setLocale('en');
    $('.datepicker').datetimepicker({
        yearOffset: 0,
        timepicker: false,
        format: 'm/d/Y'
    });
});