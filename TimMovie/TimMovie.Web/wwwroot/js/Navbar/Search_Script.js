$(function () {
    let element = $('#input-search-entities-navbar')
    let previousRequest;
    let currentRequest;
    element.keyup(function () {
        let value = element.val();
        if (value == null || value.trim() === '')
            return;
        previousRequest = currentRequest;
        currentRequest = $.post({
            url: "/Search/SearchEntityResults",
            data: {namePart: value},
            success: function (data) {
                $('.search-elements').html(data);
                previousRequest = null;
            }
        })
        if (previousRequest !== null)
            currentRequest.abort();
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
    });
})