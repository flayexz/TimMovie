$(function () {
    $("#button-navbar-search").click(function (e) {
        e.preventDefault();
        $.get({
            url:"/Search/ModalWindow",
            success: function (data){
                $('.container-xl').append(data);
                $('#modDialog').modal('show');
            }
        })
    });
})