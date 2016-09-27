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

    // show category edit modal
    $('#btn-edit-categories').click(function () {
        $('#this-category').css('display', 'none');
        $('#category-edit-modal').modal();
    });

    // functionality for category edit button
    $('.btn-category-edit').click(function () {
        // get id from hidden field in table cell
        var id = $(this).parent().find('input').val();

        // get the category name table cell element
        var categoryName = $(this).parent().parent().find('.category-name');

        // get the category type table cell element
        var categoryType = $(this).parent().parent().find('.category-type');

        // populate the edit form
        // hidden
        $('#categoryId').val(id);
        // name input
        $('#categoryName').val(categoryName.text());
        // uncheck the radio buttons
        $('.radio-category').prop('checked', false);
        // removed the checked class
        $('.radio-category').removeClass('checked');
        // check the appropriate radio button
        $('#' + categoryType.text().trim()).addClass('checked');
        // add the checked class to the appropriate radio button
        $('#' + categoryType.text().trim()).prop('checked', true);
        // display the form
        $('#this-category').css('display', 'block');
    });

    // radio button click event, helps with ui update
    $('.radio-category').change(function () {
        // remove the checked class from all radio-category
        $('.radio-category').removeClass('checked');
        // add the checked class to the clicked button
        $(this).addClass('checked');
    });

    // functionality for category edit submit button
    $('#category-edit-submit').click(function () {
        // get category id
        var id = $('#categoryId').val();
        // get category name
        var categoryName = $('#categoryName').val().trim();
        
        // get category type (based on checked class)
        var categoryType = "Withdrawal";
        if ($('#Deposit').hasClass('checked')) {
            categoryType = "Deposit";
        }

        // run controller action to update db
        $.post('../../Budgets/EditCategory', {categoryName: categoryName, categoryType: categoryType, categoryId: id }).then(function (response) {
            // update ui (needs to be after a successful update)
            $('tr#' + id + " td.category-name").text(categoryName);
            $('tr#' + id + " td.category-type").text(categoryType);
            // hide the form
            $('#this-category').css('display', 'none');
        });
        
    });

    // functionality for category delete button
    $('.btn-category-delete').click(function () {
        // get id of the category
        var id = $(this).parent().parent().attr('id');
        var budgetId = $('#budget-id').val();
        // run controller action to update db
        $.post('../../Budgets/DeleteCategory', { id: id, budgetId: budgetId  }).then(function (response) {
            // update ui (needs to be run after a successful update)
            $('#' + id).remove();
        });
    });

    // show add category modal
    $('#btn-add-category').click(function () {
        $('#add-category-modal').modal();
    });

    // edit budget item modal
    $('.btn-edit-budget-item').click(function () {
        var id = $(this).parent().attr('id');
        var url = "../../BudgetItems/EditModal/" + id;
        var modalId = "#edit-budget-item-modal";
        $.get(url, function (html) {
            $(modalId).html(html);
            $(modalId).modal();
        });
    });

    $('.btn-delete-budget-item').click(function () {
        var id = $(this).parent().attr('id');
        var row = $(this).parent().parent();
        var url = "../../BudgetItems/DeleteBudgetItem/" + id;
        $.post(url, function (html) {
            row.remove();
        });

        

    });

});

