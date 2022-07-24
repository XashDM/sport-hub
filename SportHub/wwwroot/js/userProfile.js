$("#dropdown").on({
    mouseenter: function () {
        $('.dropdown-content').show();
        $('main').css('z-index', '-1');
    },
    mouseleave: function () {
        if ($('#search-field-tag').css('display') != 'block') {
            $('main').css('z-index', '');
        }
    }
});