$(document).ready(function() {
    moveDescription();
    cropDescriptrion();
    showOprimalActorCardsNumber();
    hideActorsAndProducers();
});

let isRemoved = false;
const showMoreActorsAndProducersBtn = $("#actorsProducersMoreBtn");
const hideActorsAndProducersBtn = $("#actorsProducersHideBtn");
const actorsAndProducersContainer = $(".actors-producers__container");
const durationForShow = 400;

function hideActorsAndProducers() {
    let e = $(".actors-producers__item__container_show");
    $(".actors-producers__item__container_show").each(function() {
        $(this).hide();
    })
    hideActorsAndProducersBtn.hide();
}

function showMoreActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function() {
        $(this).show(durationForShow);
    })
    showMoreActorsAndProducersBtn.hide();
    hideActorsAndProducersBtn.show(durationForShow);
}

function hideActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function() {
        $(this).hide();
    })
    showMoreActorsAndProducersBtn.show(durationForShow);
    hideActorsAndProducersBtn.hide();
}

function getOptimalActorCardsNumber() {
    let windowWidth = $(window).width();
    if (windowWidth < 501 && windowWidth > 340) return 5;
    else if (windowWidth < 340 && windowWidth > 265) return 4;
    else if (windowWidth < 265) return 3;
    else return 6;
}

function showOprimalActorCardsNumber() {
    let cardsToShow = getOptimalActorCardsNumber();
    for (let i = cardsToShow; i < 7; i++) {
        $(".actor").eq(i).hide();
    }
}

$(window).resize(function() {
    moveDescription();
    if ($(window).width() < 501 && !isRemoved) {
        $(".actor").last().hide();
        if ($(window).width() < 340) {
            $(".actor").eq(4).hide();
            if ($(window).width() < 265) {
                $(".actor").eq(3).hide();
                isRemoved = true;
            }
        }
    } else if ($(window).width() > 265) {
        $(".actor").eq(3).show();
        if ($(window).width() > 340) {
            $(".actor").eq(4).show();
        }
        if ($(window).width() > 500) {
            $(".actor").last().show();
            isRemoved = false;
        }
    }
});

function moveDescription() {
    let filmDescription = $("#film_description");
    let windowWidth = $(window).width();
    if (windowWidth > 1199) filmDescription.appendTo("#film_info_container");
    else filmDescription.appendTo("#film-row");
}

function cropDescriptrion() {
    let description = $("#description");
    paragraphs = description.text().trim().split("\n\n");
    let descLess = $("<p>" + paragraphs[0] + "</p>").prop("id", "descLess");
    let descMore = $("<p>" + paragraphs[1] + "</p>").prop("id", "descMore");
    description.empty();
    descLess.appendTo(description);
    descMore.appendTo(description);
    $("#descMore").hide();
}

function showMore() {
    $("#descMore").show();
}

function showLess() {
    $("#descMore").hide();
}

$("#more").on("click", function() {
    if ($(this).text() === "Читать дальше...") {
        $(this).text("Свернуть");
        showMore();
    } else {
        $(this).text("Читать дальше...");
        showLess();
    }
});

showMoreActorsAndProducersBtn.on("click", showMoreActorsAndProducers);
hideActorsAndProducersBtn.on("click", hideActorsAndProducers)

// console.log($("#rate_movie").text());