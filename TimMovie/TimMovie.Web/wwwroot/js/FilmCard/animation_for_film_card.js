
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
        $.each(moreInfo, function() {
            $(this).hide(durationForShow, "swing");
        });
    }
    
    function prepareFilms(){
        hideAllMoreInfo();

        let cards = $(".film-card");
        $.each(cards, function() {
            $(this).hover(displayMoreInfo, hideMoreInfo);
        });
    }

    $(document).ready(function() {
        prepareFilms()
    });