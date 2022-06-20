let allowedExtensions = ["jpg", "jpeg", "png", "gif"];

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
    const subcategoriesSelect = outerBox.find('select[name="main-a-subcategories"]');
    const teamsSelect = outerBox.find('select[name="main-a-teams"]');
    const articlesSelect = outerBox.find('.custom-selector-body');
    getAllSubcategoriesByCategoryId(subcategoriesSelect, categoryId);
    getAllTeamsByParentId(teamsSelect, categoryId);
    getAllArticlesByParentId(articlesSelect, generatePageArguments(1, 20), categoryId);
});

$('#main-articles-block').on('change', 'select[name="main-a-subcategories"]', function () {
    const subcategoryId = $(this).val();

    if (!subcategoryId && subcategoryId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent().parent();
    resetNextSelectElementsByContainer(outerBox, 1);
    const teamsSelect = outerBox.find('select[name="main-a-teams"]');
    const articlesSelect = outerBox.find('.custom-selector-body');
    getAllTeamsByParentId(teamsSelect, subcategoryId);
    getAllArticlesByParentId(articlesSelect, generatePageArguments(1, 15), subcategoryId);
});

$('#main-articles-block').on('change', 'select[name="main-a-teams"]', function () {
    const teamId = $(this).val();

    if (!teamId && teamId != -1) {
        return;
    }

    let outerBox = $(this).parent().parent().parent();
    resetNextSelectElementsByContainer(outerBox, 2);
    const articlesSelect = outerBox.find('.custom-selector-body');

    getAllArticlesByParentId(articlesSelect, generatePageArguments(1, 15), teamId);
});

$('#main-articles-block').on('click', '.main-a-articles-selector', function () {
    const outerBox = $(this).parent();;
    const selectorBody = outerBox.find('.custom-selector-body');
    selectorBody.toggle();
});

$('#main-articles-block').on('click', '.custom-selector-body option', function () {
    const selectedOption = $(this);
    const outerBox = $(this).parent().parent();
    const selectorBody = outerBox.find('.custom-selector-body');
    const selector = outerBox.find('.main-a-articles-selector option');
    selectorBody.hide();
    selector.val(selectedOption.val());
    selector.text(selectedOption.text());
});

var scrollLimit = 100;

document.addEventListener('scroll', function (event) {
    if ($(event.target).attr('class') === 'custom-selector-body') {
        if ($(event.target).scrollTop() > scrollLimit) {
            let currentScrollLimitValue = $(event.target).attr('value');
            if (!currentScrollLimitValue) {
                currentScrollLimitValue = scrollLimit;
                $(event.target).attr('value', currentScrollLimitValue);
            }
            else {
                currentScrollLimitValue = parseInt(currentScrollLimitValue) + 100;
                $(event.target).attr('value', currentScrollLimitValue);
            }

            const page = (currentScrollLimitValue / 100) + 2;
            const lastArticleLoadedReferenceId = $(event.target).parent().find('.custom-selector-body option').last().val().split(',')[1];
            getAllArticlesByParentId($(event.target), generatePageArguments(page, 10), lastArticleLoadedReferenceId, -1, false, false);
        }
    }
}, true);

$('#save-changes-button').click(function () {
    applyMainArticlesConfigurationChanges();
    uploadPhotoOfTheDay();
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
        url: '/api/Articles/SaveMainArticles',
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
            if (response.length == 0) {
                selectedItem = -1;
            }
            selectItemBySelectorAndSelectedItem(elementToFill, selectedItem);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllTeamsByParentId(elementToFill, parentId, selectedItem = -1) {
    $.ajax({
        async: true,
        url: '/api/Articles/GetAllTeamsByParentId?parentId=' + parentId,
        type: 'get',
        success: function (response) {
            $(elementToFill).empty();
            insertOptions(elementToFill, response);
            if (response.length == 0) {
                selectedItem = -1;
            }
            selectItemBySelectorAndSelectedItem(elementToFill, selectedItem);
        },
        error: function (response) {
            console.error(response);
        }
    });
}

function getAllArticlesByParentId(elementToFill, pageArguments, articleParentId, selectedItem = -1, allowErase = true, changeSelected = true) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true,
        url: '/api/Articles/GetAllArticlesByParentIdPaginated',
        type: 'post',
        data: JSON.stringify({ 'PageArgs': pageArguments, 'ArticleParentId': articleParentId}),
        success: function (response) {
            selectorElementToFill = elementToFill.parent().find('.main-a-articles-selector option');
            if (allowErase) {
                $(elementToFill).empty();
            }
            insertOptions(elementToFill, response, true, false);
            if (changeSelected) {
                if (selectedItem != -1 && response.length != 0) {
                    selectorElementToFill.val([selectedItem.id, selectedItem.referenceItemId]);
                    selectorElementToFill.text(selectedItem.title);
                }
                else {
                    selectorElementToFill.val(-1);
                    selectorElementToFill.text('Not Chosen');
                }
            }  
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
    const customArticleSelector = container.find('.main-a-articles-selector option');
    customArticleSelector.empty();
}

function insertOptions(selectElement, dataArrayToInsert, areArticles = false, allowInsertDefault = true) {
    if (allowInsertDefault) {
        $(selectElement).append($('<option>', { value: -1, text: 'Not Chosen' }));
    }
    if (areArticles) {
        for (var i = 0; i < dataArrayToInsert.length; i++) {
            $(selectElement).append($('<option>', { value: [dataArrayToInsert[i].id, dataArrayToInsert[i].referenceItemId], text: dataArrayToInsert[i].title }));
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
            const articleId = $(this).find('.main-a-articles-selector option').val().split(',')[0];
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

        let parentsArray = []
        const currentArticle = $(this)[0].article;
        let currentParentElement = currentArticle.referenceItem;
        parentsArray.push(currentParentElement);
        while (currentParentElement.parentsItem != null) {
            parentsArray.push(currentParentElement.parentsItem);
            currentParentElement = currentParentElement.parentsItem;
        }

        const currentIsDisplayedState = $(this)[0].isDisplayed;
        const articleSelector = configurationBodyClone.find('.custom-selector-body');
        const isDisplayedCheckbox = configurationBodyClone.find('input[type="checkbox"]');
        const teamSelector = configurationBodyClone.find('select[name="main-a-teams"]');
        const subcategorySelector = configurationBodyClone.find('select[name="main-a-subcategories"]');
        const categorySelector = configurationBodyClone.find('select[name="main-a-categories"]');
        selectItemBySelectorAndSelectedItem(categorySelector, parentsArray[parentsArray.length - 1].id);
        if (parentsArray.length >= 2) {
            if (parentsArray[parentsArray.length - 2].type === 'Team') {
                getAllSubcategoriesByCategoryId(subcategorySelector, parentsArray[parentsArray.length - 1].id);
                getAllTeamsByParentId(teamSelector, parentsArray[parentsArray.length - 1].id, parentsArray[0].id);
            }
            else {
                getAllSubcategoriesByCategoryId(subcategorySelector, parentsArray[parentsArray.length - 1].id, parentsArray[parentsArray.length - 2].id);
                getAllTeamsByParentId(teamSelector, parentsArray[parentsArray.length - 2].id, parentsArray[0].id);
            }
        }
        else {
            getAllSubcategoriesByCategoryId(subcategorySelector, parentsArray[parentsArray.length - 1].id);
            getAllTeamsByParentId(teamSelector, parentsArray[parentsArray.length - 1].id, parentsArray[0].id);
        }
        getAllArticlesByParentId(articleSelector, generatePageArguments(1, 15), parentsArray[0].id, currentArticle);

        isDisplayedCheckbox.prop('checked', currentIsDisplayedState);

        configurationBodyClone.appendTo('#main-articles-block')
            .show();
    });
}

function GetPhotoOfTheDay() {
    $.ajax({
        async: true,
        url: "/api/Articles/GetPhotoOfTheDay",
        type: "GET",
        success: function (response) {
            console.log(response);
            $('#day-photo-background-img').attr('src', response.imageItem.imageLink);
            $('#alt-input').val(response.imageItem.alt);
            $('#title-input').val(response.imageItem.photoTitle);
            $('#description-input').val(response.imageItem.shortDescription);
            $('#author-input').val(response.imageItem.author);
            $('#photo-of-day-isDisplayed').prop('checked', response.isDisplayed);
            $('input[name="imageFile"]').val('');
            console.log($('input[name="imageFile"]').prop('files'));
        }
    });
}

$('#photo-upload-input').on('change', function () {
    const file = $('input[name="imageFile"]').prop('files')[0];
    if (file) {
        $('#day-photo-background-img').attr('src', URL.createObjectURL(file));
    }
});

$('.image-upload-box').on('drag dragover dragenter dragleave drop', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();
});

$('.image-upload-box').on('dragenter', function (evt) {
    $('.hide-on-draggedover').hide();
    $('.show-on-draggedover').show();
    $('.image-upload-box').addClass('isDragover');
});

$('.image-upload-box').on('dragleave', function (evt) {
    $('.image-upload-box').removeClass('isDragover');
    $('.hide-on-draggedover').show();
    $('.show-on-draggedover').hide();
});

$('.image-upload-box').on('drop', function (evt) {
    $('.image-upload-box').removeClass('isDragover');
    $('.hide-on-draggedover').show();
    $('.show-on-draggedover').hide();

    let dt = evt.originalEvent.dataTransfer;
    let files = dt.files;
    const file = files[0];

    if (file) {
        let fileExtension = file.name.split(".").pop();
        if (allowedExtensions.includes(fileExtension)) {
            $('#day-photo-background-img').attr('src', URL.createObjectURL(file));
            $('input[name="imageFile"]').prop('files', files);
        }
        else {
            alert("Wrong file extension!");
        }
    }
});

function uploadPhotoOfTheDay() {
    infoForm = $('#photo-info-form')[0];
    var fd = new FormData(infoForm);
    fd.append('imageFile', $('input[name="imageFile"]').prop('files')[0]);
    isDisplayed = $('#photo-of-day-isDisplayed').prop('checked')
    fd.append('isDisplayed', isDisplayed);
    $.ajax({
        async: true,
        url: "/api/Articles/UploadPhotoOfTheDay",
        type: "PUT",
        processData: false,
        contentType: false,
        data: fd,

        success: function (response) {
            $('#success-banner').css('display', 'flex');
            $('#success-banner').delay(5000).fadeOut();
        },
        
        error: function () {
            $('#error-banner').css('display', 'flex');
            $('#error-banner').delay(5000).fadeOut();
        }
    });
}

$('.close-button').on('click', function () {
    $(this).parent().hide();
})


$(document).ready(() => {
    getAllCategories();
    getMainArticles();
    GetPhotoOfTheDay();
});