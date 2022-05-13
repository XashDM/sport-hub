$('#main-articles-block').on('click', '.add-new-button', () => {
    let maxArticleCount = 5;
    let currentArticleAmount = $('.configuration-body').length;

    if (currentArticleAmount === maxArticleCount) {
        return;
    }

    let configurationBody = $('.configuration-body')
        .last()

    configurationBody
        .find('p.delete-button')
        .removeClass('disabled');

    let configurationBodyClone = configurationBody
        .clone()
        .attr('id', 'configuration-body' + currentArticleAmount)

    if (currentArticleAmount === (maxArticleCount - 1)) {
        configurationBodyClone
            .find('p.add-new-button')
            .addClass('disabled');
    }

    configurationBodyClone.appendTo('#main-articles-block');
    $('.add-new-button').eq(-2).fadeOut(400);
});

$('#main-articles-block').on('click', '.delete-button', (el) => {
    if ($('.configuration-body').length < 2) {
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
        if ($('.configuration-body').length === 1) {
            $('.delete-button')
                .first()
                .addClass('disabled');
        }
        $('.add-new-button').eq(-1).fadeIn();
    });
});