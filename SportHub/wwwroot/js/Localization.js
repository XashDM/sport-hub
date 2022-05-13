$(".language-box").on("click", 'input[type="checkbox"]', function (e) {
    let status = $(this).prop('checked');
    let languageId = $(this).prop('id');
    console.log(languageId)
    console.log(status)
    $.ajax({
        headers:
        {
            'RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        async: true,
        url: '/localization',
        type: 'post',
        data: {
            'isEnabled': status,
            'id': languageId,
        }
    });
});
