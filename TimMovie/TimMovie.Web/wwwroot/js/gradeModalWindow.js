let choseButton = null;
let gradeNumber = $(".gradeNumber");
let savedFilmId = null;
let lastSettedGrade = null;

function getGrade(filmId) {
    savedFilmId = filmId;
    gradeNumber.css("background", "#1e1a2e");
    return $.post({
        url: "/Film/GetGrade",
        data: {filmId: filmId},
        success: function (data) {
            if (data != null) {
                let grade = Number(data);
                if (!isNaN(grade)) {
                    choseButton = $(`.gradeNumber:contains(${grade})`).first();
                    choseButton.css("background", "#302a45");
                    return grade;
                } else {
                    document.location.pathname = data;
                }
            }
        }
    });

}

function setGrade(filmId, e) {
    return $.post({
        url: "/Film/SetGrade",
        data: {filmId: savedFilmId, grade: e.target.innerText},
        success: function () {
            gradeNumber.css("background", "#1e1a2e");
            $(e.target).css("background", "#302a45");
            $('#modalFilmGrade').modal('hide');
            choseButton = $(e.target);
            lastSettedGrade = e.target.innerText;
        },
        error: function () {
            return null;
        }
    });
}

gradeNumber.click(function (e) {
    setGrade(savedFilmId, e);
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
});