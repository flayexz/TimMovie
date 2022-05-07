$(function () {
    $('#input-search-entities-navbar').keyup(function () {
        $.post({
            url: "/Search/SearchEntityResults",
            data: {namePart: $('#input-search-entities-navbar').val()},
            success: function (data) {
                $('.subscribe-body').html(data);
            }
        })
        
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
    });
})