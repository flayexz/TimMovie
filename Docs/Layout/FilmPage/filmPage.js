$(document).ready(function () {
  moveDescription();
  cropDescriptrion();
});

let isRemoved = false;

$(window).resize(function () {
  moveDescription();
  if ($(window).width() < 501 && !isRemoved) {
    $(".actor").last().hide();
    isRemoved = true;
  } else if ($(window).width() > 500) {
    $(".actor").last().show();
    isRemoved = false;
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

$("#more").on("click", function () {
  if ($(this).text() === "Читать дальше...") {
    $(this).text("Свернуть");
    showMore();
  } else {
    $(this).text("Читать дальше...");
    showLess();
  }
});
