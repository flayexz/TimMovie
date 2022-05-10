$(document).ready(function () {
    moveDescription();
    cropDescriptrion();
    hideAllActorsAndProducers();
});

$(window).resize(function () {
    moveDescription();
});

function moveDescription() {
    let filmDescription = $("#film_description");
    let windowWidth = $(window).width();
    if (windowWidth > 1199) filmDescription.appendTo("#film_info_container");
    else filmDescription.appendTo("#film-row");
}

function cropDescriptrion() {
    let description = $("#description");
    paragraphs = description.text().trim().split("\n");
    let descLess = $("<p>" + paragraphs[0] + "</p>").prop("id", "descLess");
    let descMore = $("<div><p>" + paragraphs[1] + "</p></div>").prop("id", "descMore");
    if (paragraphs.length > 2) {
        for (var i = 2; i < paragraphs.length; i++) {
            let paragraph = $("<p>" + paragraphs[i] + "</p>")
            paragraph.appendTo(descMore)
        }
    }
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

$("#more").on("click", function () {
    if ($(this).text() === "Читать дальше...") {
        $(this).text("Свернуть");
        showMore();
    } else {
        $(this).text("Читать дальше...");
        showLess();
    }
});

$("#backward_link").on("click", function () {
    history.go(-1);
})


//actors and producers script

const showMoreActorsAndProducersBtn = $("#actorsProducersMoreBtn");
const hideActorsAndProducersBtn = $("#actorsProducersHideBtn");
const durationForShow = 400;


function hideAllActorsAndProducers() {
    let e = $(".actors-producers__item__container_show");
    $(".actors-producers__item__container_show").each(function () {
        $(this).hide();
    })
    hideActorsAndProducersBtn.hide();
}

function showMoreActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function () {
        $(this).show(durationForShow);
    })
    showMoreActorsAndProducersBtn.hide();
    hideActorsAndProducersBtn.show(durationForShow);
}

function hideActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function () {
        $(this).hide();
    })
    showMoreActorsAndProducersBtn.show(durationForShow);
    hideActorsAndProducersBtn.hide();
}

showMoreActorsAndProducersBtn.on("click", showMoreActorsAndProducers);
hideActorsAndProducersBtn.on("click", hideActorsAndProducers);
hideActorsAndProducersBtn.on("click", hideActorsAndProducers);

$("#rate_movie").click(function (){
    getGrade(document.URL.split('/').pop());
})
