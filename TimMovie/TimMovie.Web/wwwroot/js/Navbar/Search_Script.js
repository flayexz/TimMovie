$(function () {
    let element = $('#input-search-entities-navbar')
    element.keyup(function (e) {
        let value = element.val();
        if (value == null || value.trim() === '')
            return;
        $.post({
            url: "/Search/SearchEntityResults",
            data: {namePart: value},
            success: function (data) {
                $('.search-elements').html(data);
                element.focus();
            },
            error: function (){
                element.focus();
            }
        })
        element.blur();
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
    });
})