$(document).ready(function () {
  moveDescription();
  cropDescriptrion();
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
  if(paragraphs.length > 2) {
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

$("#backward_link").on("click", function (){
  history.go(-1);
})
