$(".language-block .language-box").on("click", 'input[type="checkbox"]', function (e) {
    e.stopPropagation();
    let status = $(this).prop('checked');
    let languageId = $(this).prop('id');
    $.ajax({
        headers:
        {
            'RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        async: true,
        url: '/localization',
        type: 'post',
        data: {
            'id': languageId,
            'isEnable': status,
        }
      
    });
});
