(function() {
    let duration = 200;
    let containerFilm = $("#container_film");
    let currentClassForContainer = "row-cols-4";

    function displayMoreInfo() {
        $(this).find(".card-description").show(duration, "swing");
    }

    function hideMoreInfo() {
        $(this).find(".card-description").hide(duration, "swing");
    }

    function changeAmountOfCol() {
        containerFilm.removeClass(currentClassForContainer);
        let newClass;
        let width = window.innerWidth;
        if (width <= 540) {
            newClass = "row-cols-1";
        } else if (width <= 900) {
            newClass = "row-cols-2";
        } else if (width <= 1140) {
            newClass = "row-cols-3";
        } else if (width <= 1920) {
            newClass = "row-cols-4";
        }
        containerFilm.addClass(newClass);
        currentClassForContainer = newClass;
    }

    $(document).ready(function() {
        changeAmountOfCol();
        let cards = $(".card-film");
        $.each(cards, function(i, card) {
            $(card).hover(displayMoreInfo, hideMoreInfo);
        });

        $(window).resize(changeAmountOfCol);
    });
})();