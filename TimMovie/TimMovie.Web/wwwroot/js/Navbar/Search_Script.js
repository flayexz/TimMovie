$(function () {
    $('#input-search-navbar').keyup(function () {
        let value = $('#input-search-navbar').val();
        $.post({
            url: "/Search/SearchResults",
            data: {namePart: value},
            success: function (data) {
                $('#search-elements').html(data);
            }
        })
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
        $('#modDialog').modal('toggle');
    });
})