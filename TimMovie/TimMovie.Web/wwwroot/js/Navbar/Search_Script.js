$(function () {
    $('#input-search-navbar').keyup(function () {
        let value = $('#input-search-navbar').val();
        console.log(value);
        $.post({
            url: "/Search/SearchResults",
            data: {namePart: value},
            success: function (data) {
                $('#search-elements').html(data);
                console.log(data)
            },
            error: function () {
                console.log('поиск ajax пошёл по пизде')
            }
        })
    });
    $("#buttonClose").click(function () {
        $('#dialogContent').html();
        $('#modDialog').modal('toggle');
    });
})