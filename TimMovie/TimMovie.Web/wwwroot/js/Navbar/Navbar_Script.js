$(function () {
    $("#button-navbar-search").click(function (e) {
        e.preventDefault();
        $.get({
            url:"/Search/ModalWindow",
            success: function (data){
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
            },
            error: function () {
                console.log('модальное окно ajax пошёл по пизде')
            }
        })
    });
})