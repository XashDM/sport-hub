$('#main-articles-block').on('click', '.add-new-button', () => {
    let maxArticleCount = 4;
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

    let outerBox = $(this).parent().parent().parent();
    resetNextSelectElementsByContainer(outerBox, 0);
    let subcategoriesSelect = outerBox.find('select[name="main-a-subcategories"]');
    getAllSubcategoriesByCategoryId(subcategoriesSelect, categoryId);
});

$('#main-articles-block').on('change', 'select[name="main-a-subcategories"]', function () {
    const subcategoryId = $(this).val();

    if (!subcategoryId && subcategoryId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent().parent();
    resetNextSelectElementsByContainer(outerBox, 1);
    let teamsSelect = outerBox.find('select[name="main-a-teams"]');
    getAllTeamsBySubcategoryId(teamsSelect, subcategoryId);
});

$('#main-articles-block').on('change', 'select[name="main-a-teams"]', function () {
    const teamId = $(this).val();

    if (!teamId && teamId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent().parent();
    resetNextSelectElementsByContainer(outerBox, 2);
    let articlesSelect = outerBox.find('select[name="main-a-articles"]');

    getAllArticlesByTeamId(articlesSelect, generatePageArguments(1, 20), teamId);
});

$('#save-changes-button').click(function () {
    applyMainArticlesConfigurationChanges();
});

function generatePageArguments(pageNumber, pageSize) {
    const pageArguments = { 'PageNumber': pageNumber, 'PageSize': pageSize }

    return pageArguments;
}

function applyMainArticlesConfigurationChanges() {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true,
        url: '/api/Articles/ApplyMainArticlesDisplayChanges',
        type: 'post',
        data: JSON.stringify(gatherMainArticlesInput()),
        success: function () {
            return;
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getMainArticles() {
    $.ajax({
        async: true,
        url: '/api/Articles/GetMainArticles',
        type: 'get',
        success: function (data) {
            displayConfigurationBlocks(data);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

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

function getAllSubcategoriesByCategoryId(elementToFill, categoryId, selectedItem = -1) {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllSubcategoriesByCategoryId?categoryId=' + categoryId,
        type: 'get',
        success: function (response) {
            $(elementToFill).empty();
            insertOptions(elementToFill, response);
            selectItemBySelectorAndSelectedItem(elementToFill, selectedItem);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllTeamsBySubcategoryId(elementToFill, subcategoryId, selectedItem = -1) {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllTeamsBySubcategoryId?subcategoryId=' + subcategoryId,
        type: 'get',
        success: function (response) {
            $(elementToFill).empty();
            insertOptions(elementToFill, response);
            selectItemBySelectorAndSelectedItem(elementToFill, selectedItem);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllArticlesByTeamId(elementToFill, pageArguments, articleParentId, selectedItem = -1) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true,
        url: '/api/Articles/GetAllArticlesByTeamIdPaginated',
        type: 'post',
        data: JSON.stringify({ 'PageArgs': pageArguments, 'ArticleParentId': articleParentId}),
        success: function (response) {
            $(elementToFill).empty();
            insertOptions(elementToFill, response, true);
            selectItemBySelectorAndSelectedItem(elementToFill, selectedItem);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function resetNextSelectElementsByContainer(container, index) {
    let selects = $(container).find('select');
    $(selects).each(function (idx) {
        if (idx > index) {
            $(this).empty();
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

function gatherMainArticlesInput() {
    let mainArticlesInput = {}
    $('.configuration-body').each(function (idx) {
        if (idx != 0) {
            const articleId = $(this).find('select[name="main-a-articles"]').val()
            const isDisplayed = $(this).find('input[type="checkbox"]').prop('checked');

            mainArticlesInput[articleId] = isDisplayed;
        }
    });

    return mainArticlesInput;
}

function selectItemBySelectorAndSelectedItem(selector, selectedItem) {
    selector.val(selectedItem);
}

function displayConfigurationBlocks(mainArticles) {
    let configurationBody = $('.configuration-body')
        .first()

    if (mainArticles.length == 0) {
        let configurationBodyClone = configurationBody
            .clone()
        configurationBodyClone.appendTo('#main-articles-block')
            .show();;
    }

    $(mainArticles).each(function (idx) {
        if (idx != 0) {
            $('.add-new-button').eq(-1).hide();
        }

        if (idx === 3) {
            $('p.add-new-button').eq(0).addClass('disabled');
        }

        if (mainArticles.length > 1) {
            $('p.delete-button').eq(0).removeClass('disabled');
        }

        let configurationBodyClone = configurationBody
            .clone()

        const currentArticle = $(this)[0].article
        const currentTeam = currentArticle.referenceItem;
        const currentSubcategory = currentTeam.parentsItem;
        const currentCategory = currentSubcategory.parentsItem;
        const currentIsDisplayedState = $(this)[0].isDisplayed;

        const articleSelector = configurationBodyClone.find('select[name="main-a-articles"]');
        const isDisplayedCheckbox = configurationBodyClone.find('input[type="checkbox"]');
        const teamSelector = configurationBodyClone.find('select[name="main-a-teams"]');
        const subcategorySelector = configurationBodyClone.find('select[name="main-a-subcategories"]');
        const categorySelector = configurationBodyClone.find('select[name="main-a-categories"]');

        selectItemBySelectorAndSelectedItem(categorySelector, currentCategory.id);
        getAllSubcategoriesByCategoryId(subcategorySelector, currentCategory.id, currentSubcategory.id);
        getAllTeamsBySubcategoryId(teamSelector, currentSubcategory.id, currentTeam.id);
        getAllArticlesByTeamId(articleSelector, generatePageArguments(1, 20), currentTeam.id, currentArticle.id);

        isDisplayedCheckbox.prop('checked', currentIsDisplayedState);

        configurationBodyClone.appendTo('#main-articles-block')
            .show();;
    });
}

$(document).ready(() => {
    getAllCategories();
    getMainArticles();
});