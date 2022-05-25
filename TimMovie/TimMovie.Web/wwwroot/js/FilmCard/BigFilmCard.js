$(document).ready(function () {
    showOptimalActorCardsNumber();
});

function showOptimalActorCardsNumber() {
    let cardsToShow = getOptimalActorCardsNumber();
    for (let i = cardsToShow; i < 4; i++) {
        $(".actor").eq(i).hide();
    }
}

function getOptimalActorCardsNumber() {
    let windowWidth = $(window).width();
    if (windowWidth < 401) return 3;
    else return 4;
}

let isRemoved = false;

$(window).resize(function () {
    if ($(window).width() < 401 && !isRemoved) {
        $(".actor").last().hide();
        isRemoved = true;
    } else if ($(window).width() > 400 && isRemoved) {
        $(".actor").last().show();
        isRemoved = false;
    }
});
