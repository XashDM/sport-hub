$('#main-articles-block').on('click', '.add-new-button', () => {
    let maxArticleCount = 5;
    let currentArticleAmount = $('.configuration-body').length;

    if (currentArticleAmount === maxArticleCount) {
        return;
    }

    let confBody = $('.configuration-body')
        .last()

    confBody
        .find('p.delete-button')
        .removeClass('disabled');

    let confBodyClone = confBody
        .clone()
        .attr('id', 'configuration-body' + currentArticleAmount)

    if (currentArticleAmount === (maxArticleCount - 1)) {
        confBodyClone
            .find('p.add-new-button')
            .addClass('disabled');
    }

    confBodyClone.appendTo('#main-articles-block');
    $('.add-new-button').eq(-2).fadeOut(400);
});

$('#main-articles-block').on('click', '.delete-button', (e) => {
    if ($('.configuration-body').length < 2) {
        return;
    }
    else {
        $('.add-new-button')
            .last()
            .removeClass('disabled');
    }

    let el = e.currentTarget.parentElement.parentElement.parentElement;
    $(el).fadeOut(400, () => {
        $(el).remove();
        if ($('.configuration-body').length === 1) {
            $('.delete-button')
                .first()
                .addClass('disabled');
        }
        $('.add-new-button').eq(-1).fadeIn();
    });
});