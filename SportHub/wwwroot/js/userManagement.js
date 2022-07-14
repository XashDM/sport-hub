$('#users-tab').click(function () {
    $('.tab-name').removeClass('red-accent');
    $(this).addClass('red-accent');
    const isUserTableVisible = $('.user-table').is(":visible");
    if (isUserTableVisible == false) {
        $('#user-0').nextAll('.table-row').remove();
        getAllUsers();
        $('.user-table').slideDown();
        $('.admin-table').slideUp();
    }
});

$('#admins-tab').click(function () {
    $('.tab-name').removeClass('red-accent');
    $(this).addClass('red-accent');
    const isAdminTableVisible = $('.admin-table').is(":visible");
    if (isAdminTableVisible == false) {
        $('#admin-0').nextAll('.table-row').remove();
        getAllAdmins();
        $('.user-table').slideUp();
        $('.admin-table').slideDown();
    }
});

$('.users-management-box').on('click', '.arrow-button', function () {
    console.log($(this));
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

$(document).on('click', function (event) {
    var $trigger = $('.custom-dropdown-body');
    if ($trigger !== event.target && !$trigger.has(event.target).length) {
        $('.custom-dropdown-options').slideUp();
    }
});

$('.users-management-box').on('click', '.custom-option', function () {
    selectAnOption($(this));
});

//apply selected action
$('.users-management-box').on('click', '.selected-value', function () {
    let selectedValue = $(this).parent();
    let action = selectedValue.attr('actiontype');
    let rowElement = $(this).parents('li');

    //isadminhere, need later
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
            url: '/api/Users/Block',
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
            url: '/api/Users/Activate',
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
            url: '/api/Users/Delete',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                console.log('Delete!');

                if (isAdminUser == "true") {
                    decreaseCounters(true);
                }
                else {
                    decreaseCounters(false);
                }

                rowElement.slideUp(function () {
                    $(this).remove();
                });
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
            url: '/api/Users/RemoveAdminRole',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                console.log('Remove admin permissions!');
                rowElement.slideUp(function () {
                    $(this).remove();
                });
                decreaseAdminCounter();
                increaseUserCounter();
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
            url: '/api/Users/GrantAdminRole',
            type: 'post',
            data: JSON.stringify({ 'userid': userId }),
            success: function () {
                rowElement.attr('isadmin', 'true');
                selectAnOption(rowElement.find('#block'));
                increaseAdminCounter();
                decreaseUserCounter();
                console.log('Grant admin permissions!');
                rowElement.slideUp(function () {
                    $(this).remove();
                });
            },
            error: function (xhr, status, error) {
                console.error(xhr);
                console.error(error);
            }
        });
    }
});

//decreases number in user tab and, if admin deleted, in admin tab 
function decreaseCounters(isAdmin) {
    let adminNumber = Number($('#admins-tab').attr('counter')) - 1;
    let usersNumber = Number($('#users-tab').attr('counter')) - 1;
    if (isAdmin == true) {
        $('#admins-tab').text(`Admins (${adminNumber})`);
        $('#admins-tab').attr('counter', adminNumber);
    }
    else {
        $('#users-tab').text(`Users (${usersNumber})`);
        $('#users-tab').attr('counter', usersNumber);
    }
}

//increase number in admin tab, if admin got his permissions
function increaseAdminCounter() {
    let adminNumber = Number($('#admins-tab').attr('counter'));
    $('#admins-tab').text(`Admins (${adminNumber + 1})`);
    $('#admins-tab').attr('counter', adminNumber + 1);
}

//decrease number in admin tab, if admin lost his permissions
function decreaseAdminCounter() {
    let adminNumber = Number($('#admins-tab').attr('counter'));
    $('#admins-tab').text(`Admins (${adminNumber - 1})`);
    $('#admins-tab').attr('counter', adminNumber - 1);
}


//decrease number in user tab, if user receive his permissions
function decreaseUserCounter() {
    let userNumber = Number($('#users-tab').attr('counter'));
    $('#users-tab').text(`Users (${userNumber - 1})`);
    $('#users-tab').attr('counter', userNumber - 1);
}

//increase number in user tab
function increaseUserCounter() {
    let userNumber = Number($('#users-tab').attr('counter'));
    $('#users-tab').text(`Users (${userNumber + 1})`);
    $('#users-tab').attr('counter', userNumber + 1);
}

function drawUsers(userList) {
    $('#user-0').show();
    $('#users-tab').text(`Users (${userList.length})`);
    $('#users-tab').attr('counter', userList.length);
    for (var i = 0; i < userList.length; i++) {
        let user = userList[i];

        var adminRole = user.roles.find(function (element) {
            return element.roleName == 'Admin';
        });

        let newRow = $('#user-0').clone();
        newRow.attr('id', user.id);

        let statusLabel = newRow.find('.status-label');

        newRow.find('.custom-dropdown-options').attr('id', `dropdown-options-${user.id}`);
        newRow.find('#name-label').text(user.firstName + " " + user.lastName);

        if (typeof adminRole == "undefined") {
            newRow.attr('isadmin', 'false');
        }
        else {
            newRow.attr('isadmin', 'true');
        }

        if (user.isActive == true) {
            let blockOption = newRow.find('#block');
            statusLabel.text('Active');
            statusLabel.toggleClass('green-accent');
            newRow.attr('isactive', 'true');
            selectAnOption(blockOption);
        }
        else {
            let activateOption = newRow.find('#activate');
            statusLabel.text('Blocked');
            newRow.attr('isactive', 'false');
            selectAnOption(activateOption);
        }

        newRow.appendTo('#table-row-keeper');
    }
    $('#user-0').hide();
}

function drawAdmins(userList) {
    $('#admin-0').show();
    $('#admins-tab').text(`Admins (${userList.length})`);
    $('#admins-tab').attr('counter', userList.length);
    for (var i = 0; i < userList.length; i++) {
        let user = userList[i];
        let newRow = $('#admin-0').clone();
        newRow.attr('id', user.id);

        let statusLabel = newRow.find('.status-label');

        newRow.find('.custom-dropdown-options').attr('id', `dropdown-options-${user.id}`);
        newRow.find('#name-label').text(user.firstName + " " + user.lastName);
        newRow.attr('isadmin', 'true');

        let removeOption = newRow.find('#remove-admin');
        selectAnOption(removeOption);

        if (user.isActive == true) {
            statusLabel.text('Active');
            statusLabel.toggleClass('green-accent');
            newRow.attr('isactive', 'true');
        }
        else {
            statusLabel.text('Blocked');
            newRow.attr('isactive', 'false');
        }

        newRow.appendTo('#admin-table-row-keeper');
    }
    $('#admin-0').hide();
}

function getAllUsers() {
    $.ajax({
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true,
        url: '/api/Users/AllUsersList',
        type: 'get',
        success: function (result) {
            drawUsers(result);
        },
        error: function (xhr, status, error) {
            console.error(xhr);
            console.error(error);
        }
    });
}

function getAllAdmins() {
    $.ajax({
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true,
        url: '/api/Users/AllAdminsList',
        type: 'get',
        success: function (result) {
            drawAdmins(result);
        },
        error: function (xhr, status, error) {
            console.error(xhr);
            console.error(error);
        }
    });
}

$(document).ready(function () {
    $('#user-management-button').toggleClass('red-accent-img');
    getAllUsers();
    getAllAdmins();
});