$(function () {
    let element = $('#input-search-entities-navbar')
    let isRequestFinished = true;
    element.keyup(function () {
        let value = element.val();
        if (!isRequestFinished || value == null || value.trim() === '')
            return;
        $.post({
            url: "/Search/SearchEntityResults",
            data: {namePart: value},
            success: function (data) {
                $('.search-elements').html(data);
                isRequestFinished = true;
            },
            error: function (){
                isRequestFinished = true;
            }
        })
        isRequestFinished = false;
        
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
    });
})