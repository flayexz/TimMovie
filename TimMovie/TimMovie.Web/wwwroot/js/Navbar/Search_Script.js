$(function () {
    $('#input-search-navbar').keyup(function () {
        let value = $('#input-search-navbar').val();
        $.post({
            url: "/Search/SearchEntityResults",
            data: {namePart: value},
            success: function (data) {
                $('#search-elements').html(data);
            }
        })
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
    });
})