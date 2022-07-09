let IsAdminTable = false;
if ($('.admin-table').length) {
    IsAdminTable = true;
}

$('.arrow-button').on('click', function () {
    let id = $(this).parents('li').attr('id');
    let parentItem = $(this).parents('li');
    console.log('Click on arrow!')
    console.log('id: ' + id);
    $('.custom-dropdown-options').slideUp();
    const isDropped = parentItem.find(`#dropdown-options-${id}`).is(":visible");
    if (!isDropped) {
        parentItem.find(`#dropdown-options-${id}`).slideToggle();
    }
});

//requires '.custom-option' object
function selectAnOption(customOption) {
    //id of row
    let id = customOption.parents('li').attr('id');
    //bool for user status
    let isActiveUser = customOption.parents('li').attr('isactive');
    //bool for admin role in user table
    let isAdminUser = customOption.parents('li').attr('isadmin');
    //style class of option (stored in attr)
    let elemStyleClass = customOption.attr('optionstyle');
    //element with selected value
    let selectedValue = customOption.parent().siblings('.custom-dropdown-selected');
    //text inside the selected option
    let textVal = customOption.find('.button-text').text();
    //get action type (stored as id)
    let action = customOption.attr('id');
    //element that keeps all custom options inside
    let optionKeeper = customOption.parent();


    selectedValue.removeClass('red-option grey-option green-option blue-option');
    selectedValue.addClass(elemStyleClass);
    selectedValue.attr('actiontype', action);

    selectedValue.find('.selected-value').text(textVal);
    
    //hide options box
    customOption.parent().slideUp('slow', function () {
        optionKeeper.find('.custom-option').show();
        if (isActiveUser == "true") {
            optionKeeper.find('#activate').hide();
        }
        else if (isActiveUser == "false") {
            optionKeeper.find('#block').hide();
            optionKeeper.find('#grant-admin').hide();
        }
        if (isAdminUser == "true") {
            optionKeeper.find('#grant-admin').hide();
        }
        customOption.hide();
    });

}

$('.custom-option').on('click', function () {
    selectAnOption($(this));
});

//apply selected action
$('.selected-value').on('click', function () {
    let selectedValue = $(this).parent();
    let action = selectedValue.attr('actiontype');
    let rowElement = $(this).parents('li');
    let isAdminUser = rowElement.attr('isadmin');
    let userId = rowElement.attr('id');
    let statusLabel = rowElement.find('.status-label');
    console.log(userId);
    if (action == 'block') {
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: true,
            url: '/api/Users/BlockUserById',
            type: 'post',
            data: JSON.stringify({'userid': userId}),
            success: function () {
                console.log('Block!');
                rowElement.attr('isactive', "false");
                selectedValue.attr('actiontype', 'activate');
                selectedValue.removeClass('grey-option');
                selectedValue.addClass('green-option');
                selectedValue.find('.selected-value').text('Activate');
                rowElement.find('#grant-admin').hide();

                statusLabel.text('Blocked');
                statusLabel.removeClass('green-accent');
                statusLabel.addClass('grey-accent');
                return;
            },
            error: function (xhr, status, error) {
                console.error(xhr);
                console.error(error);
            }
        });
    }
    else if (action == 'activate') {
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: true,
            url: '/api/Users/ActivateUserById',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                console.log('Activate!');
                rowElement.attr('isactive', "true");
                selectedValue.attr('actiontype', 'block');
                selectedValue.removeClass('green-option');
                selectedValue.addClass('grey-option');
                selectedValue.find('.selected-value').text('Block');
                if (isAdminUser == "false") {
                    rowElement.find('#grant-admin').show();
                }

                statusLabel.text('Active');
                statusLabel.removeClass('grey-accent');
                statusLabel.addClass('green-accent');
            },
            error: function (xhr, status, error) {
                console.error(xhr);
                console.error(error);
            }
        });
    }
    else if (action == 'delete') {
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: true,
            url: '/api/Users/DeleteUserById',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                console.log('Delete!');
                rowElement.slideUp();
            },
            error: function (xhr, status, error) {
                console.error(xhr);
                console.error(error);
            }
        });
    }
    else if (action == 'remove-admin') {
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: true,
            url: '/api/Users/RemoveAdminRoleById',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                console.log('Remove admin permissions!');
                rowElement.slideUp();
            },
            error: function (xhr, status, error) {
                console.error(xhr);
                console.error(error);
            }
        });
    }
    else if (action == 'grant-admin') {
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            async: true,
            url: '/api/Users/GrantAdminRoleById',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                rowElement.attr('isadmin', 'true');
                selectAnOption(rowElement.find('#block'));
                console.log('Grant admin permissions!');
            },
            error: function (xhr, status, error) {
                console.error(xhr);
                console.error(error);
            }
        });
    }
});