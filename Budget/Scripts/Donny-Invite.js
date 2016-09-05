$(document).ready(function () {
    $('#invite-submit').click(function () {
        var email = $('#invite-email').val();
        var householdId = $('#invite-household').val();
        var url = "../InviteUser";
        $.post(url, {
            HouseholdId: householdId,
            EmailAddress: email,
            Id: 0
        },
        function (data) {
            if (data == 0) {
                alert("You do not have the appropriate permission to invite a user!")
            } else if (data == -1) {
                alert("Model State Error");
            } else if (data == -2) {
                alert("User is already in the household!");
            } else if (data == -3) {
                alert("User has already been invited to the household!");            
            } else {
                alert("Success!");
            }
        });
    });
});