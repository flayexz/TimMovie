$(function () {
    let filmId = document.URL.split('/').pop();
    let choosedButton = null;
    $.post({
        url: "/Film/GetGrade",
        data: {filmId: filmId},
        success: function (data) {
            if (data != null) {
                choosedButton = $(`.gradeNumber:contains(${data})`).first();
                choosedButton.css("background", "#302a45");
            }
        }
    })
    let gradeNumber = $(".gradeNumber");
    gradeNumber.click(function (e) {
        $.post({
            url: "/Film/SetGrade",
            data: {filmId: filmId, grade: e.target.innerText},
            success: function () {
                gradeNumber.css("background", "#1e1a2e");
                $(e.target).css("background", "#302a45");
                $('#modalFilmGrade').modal('hide');
                choosedButton = $(e.target);
            }
        })
    })
    gradeNumber.mouseover(function (e) {
        $(e.target).css({
            "background": "#B22222",
            "border-color": "#B22222",
            "cursor": "pointer"
        });
    });
    gradeNumber.mouseleave(function (e) {
        let background = "#1e1a2e";
        if (choosedButton != null && choosedButton[0].innerText === $(e.target)[0].innerText)
            background = "#302a45";
        $(e.target).css({
            "background": background,
            "border-color": "#1e1a2e",
            "cursor": "default"
        });
    })
})