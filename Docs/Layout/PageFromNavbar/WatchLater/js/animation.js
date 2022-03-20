(function() {
    let duration = 200;

    function displayMoreInfo() {
        $(this).find(".card-description").show(duration, "swing");
    }

    function hideMoreInfo() {
        $(this).find(".card-description").hide(duration, "swing");
    }

    $(document).ready(function() {
        let cards = $(".card-film");
        $.each(cards, function(i, card) {
            $(card).hover(displayMoreInfo, hideMoreInfo);
        });
    });
})();