let choseButton = null;
let gradeNumber = $(".gradeNumber");
let savedFilmId = null;
function getGrade(filmId){
    savedFilmId = filmId;
    gradeNumber.css("background", "#1e1a2e");
    $.post({
        url: "/Film/GetGrade",
        data: {filmId: filmId},
        success: function (data) {
            if (data != null) {
                choseButton = $(`.gradeNumber:contains(${data})`).first();
                choseButton.css("background", "#302a45");
                return data;
            }
        }
    })
}
gradeNumber.click(function (e) {
    $.post({
        url: "/Film/SetGrade",
        data: {filmId: savedFilmId, grade: e.target.innerText},
        success: function () {
            gradeNumber.css("background", "#1e1a2e");
            $(e.target).css("background", "#302a45");
            $('#modalFilmGrade').modal('hide');
            choseButton = $(e.target);
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
    if (choseButton != null && choseButton[0].innerText === $(e.target)[0].innerText)
        background = "#302a45";
    $(e.target).css({
        "background": background,
        "border-color": "#1e1a2e",
        "cursor": "default"
    });
})