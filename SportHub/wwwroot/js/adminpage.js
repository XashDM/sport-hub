$('#main-articles-block').on('click', '.add-new-button', () => {
    let maxArticleCount = 5;
    let currentArticleAmount = $('.configuration-body').length;

    if (currentArticleAmount === maxArticleCount + 1) {
        return;
    }

    let configurationBody = $('.configuration-body')
        .first()

    configurationBody
        .find('p.delete-button')
        .removeClass('disabled');

    configurationBody
        .next()
        .find('p.delete-button')
        .removeClass('disabled');

    let configurationBodyClone = configurationBody
        .clone()
        .attr('id', 'configuration-body' + currentArticleAmount);

    if (currentArticleAmount === (maxArticleCount)) {
        configurationBodyClone
            .find('p.add-new-button')
            .addClass('disabled');
    }

    configurationBodyClone.appendTo('#main-articles-block');
    configurationBodyClone.show();
    $('.add-new-button').eq(-2).fadeOut(400);
});

$('#main-articles-block').on('click', '.delete-button', (el) => {
    if ($('.configuration-body').length < 3) {
        return;
    }
    else {
        $('.add-new-button')
            .last()
            .removeClass('disabled');
    }

    let element = el.currentTarget.parentElement.parentElement.parentElement;
    $(element).fadeOut(400, () => {
        $(element).remove();
        if ($('.configuration-body').length === 2) {
            $('.delete-button')
                .eq(1)
                .addClass('disabled');
        }
        $('.add-new-button').eq(-1).fadeIn();
    });
});

$('#main-articles-block').on('change', 'select[name="main-a-categories"]', function () {
    const categoryId = $(this).val();

    if (!categoryId && categoryId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent();
    let subcategoriesSelect = outerBox.find('select[name="main-a-subcategories"]');
    getAllSubcategoriesByCategoryId(subcategoriesSelect, categoryId);
});

$('#main-articles-block').on('change', 'select[name="main-a-subcategories"]', function () {
    const subcategoryId = $(this).val();

    if (!subcategoryId && subcategoryId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent();
    let teamsSelect = outerBox.find('select[name="main-a-teams"]');
    getAllTeamsBySubcategoryId(teamsSelect, subcategoryId);
});

$('#main-articles-block').on('change', 'select[name="main-a-teams"]', function () {
    const teamId = $(this).val();

    if (!teamId && teamId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent().parent();
    let articlesSelect = outerBox.find('select[name="main-a-articles"]');
    getAllArticlesByTeamId(articlesSelect, teamId);
});

function getAllCategories() {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllCategories',
        type: 'get',
        success: function (response) {
            insertOptions('select[name="main-a-categories"]', response);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllSubcategoriesByCategoryId(elementToFill, categoryId) {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllSubcategoriesByCategoryId?categoryId=' + categoryId,
        type: 'get',
        success: function (response) {
            $(elementToFill).empty();
            insertOptions(elementToFill, response);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllTeamsBySubcategoryId(elementToFill, subcategoryId) {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllTeamsBySubcategoryId?subcategoryId=' + subcategoryId,
        type: 'get',
        success: function (response) {
            console.log(response);
            console.log(subcategoryId);
            $(elementToFill).empty();
            insertOptions(elementToFill, response);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllArticlesByTeamId(elementToFill, teamId) {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllArticlesByTeamId?teamId=' + teamId,
        type: 'get',
        success: function (response) {
            $(elementToFill).empty();
            insertOptions(elementToFill, response, true);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function insertOptions(selectElement, dataArrayToInsert, areArticles = false) {
    $(selectElement).append($('<option>', { value: -1, text: 'Not Chosen' }));
    if (areArticles) {
        for (var i = 0; i < dataArrayToInsert.length; i++) {
            $(selectElement).append($('<option>', { value: dataArrayToInsert[i].id, text: dataArrayToInsert[i].title }));
        }
    }
    else {
        for (var i = 0; i < dataArrayToInsert.length; i++) {
            $(selectElement).append($('<option>', { value: dataArrayToInsert[i].id, text: dataArrayToInsert[i].name }));
        }
    }
}

function displayConfigurationBlocks() {
    let configurationBody = $('.configuration-body')
        .first()

    let configurationBodyClone = configurationBody
        .clone()
        .attr('id', 'configuration-body' + 0);

    configurationBodyClone.appendTo('#main-articles-block')
        .show();;
}

$(document).ready(() => {
    getAllCategories();
    displayConfigurationBlocks();
});