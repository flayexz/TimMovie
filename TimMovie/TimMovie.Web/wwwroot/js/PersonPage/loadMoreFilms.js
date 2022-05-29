(function (){
    let cardContainer = $("#person-film-container");
    let amountSkip = 0;
    let amountTake = 20;
    let allLoaded = false;
    let isLoad = false;
    let currentRequest;
    let requestIsAlreadySent = false;

    function getFilmsByFilters(){
        let data = {
            amountSkip,
            amountTake
        };

        if (!requestIsAlreadySent){
            $('.loader').toggleClass('hide');
        }

        requestIsAlreadySent = true;
        currentRequest = $.get({
            url: "/Films/FilmFilters",
            data: data,
            success: function (result){
                console.log("Написал, результат пришел")
                $('.loader').toggleClass('hide');
                requestIsAlreadySent = false;

                amountSkip += amountTake;
                allLoaded = result.length < 30;

                cardContainer.append(result);
                $("img").one("load", function() {
                    prepareFilms();
                    adaptContainer();
                });
                isLoad = false;
            }
        })
    }

    function tryLoadMoreFilms() {
        if (allLoaded){
            return;
        }

        const height = document.body.offsetHeight;
        const screenHeight = window.innerHeight;

        const scrolled = window.scrollY;

        const threshold = height - screenHeight / 4;

        const position = scrolled + screenHeight;

        if (position >= threshold && !isLoad) {
            isLoad = true;
            getFilmsByFilters();
        }
    }

    $(document).ready(function (){
        tryLoadMoreFilms();

        window.addEventListener("scroll", tryLoadMoreFilms);
    });
})()

