const durationForShow = 65;
const nameAdditionalInformationClassName = ".film-card__more-info-container";

function displayMoreInfo() {
    $(this)
        .find(nameAdditionalInformationClassName)
        .show(durationForShow, "swing");
}

function hideMoreInfo() {
    $(this)
        .find(nameAdditionalInformationClassName)
        .hide(durationForShow, "swing");
}

function hideAllMoreInfo() {
    let moreInfo = $(nameAdditionalInformationClassName);
    $.each(moreInfo, function () {
        let isHidden = $(this).is(":hidden");
        if (!isHidden) {
            $(this).hide();
        }
    });
}

function prepareFilms() {
    hideAllMoreInfo();

    let cards = $(".film-card");
    $.each(cards, function () {
        $(this).hover(displayMoreInfo, hideMoreInfo);
    });
}

$(document).ready(function () {
    prepareFilms();
});

$("body").on('click', '.setGradeButton', () => {
    event.preventDefault();
    let filmId = $('.film-card__more-info-container').filter(function () {
        return $(this).css('display') !== 'none';
    }).attr("href").split('/').pop();
    getGrade(filmId);
});

$('.addToWatchLaterButton').on('click', () => {
    event.preventDefault();
    let filmId = $('.film-card__more-info-container').filter(function () {
        return $(this).css('display') !== 'none';
    }).attr("href").split('/').pop();
    isWatched(filmId);
    }
)
