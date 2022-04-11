(function (){
    let cardContainer = $("#container_film_card");
    let numberOfLoadedCards = 0;
    let pagination = 30;
    let allLoaded = false;
    let isLoad = false;
    
    function getObjWithFilters(){
        let typeFilter = $("#sort-type").val();
        
        let genresName = []; 
        let genres = $("#genre-filter").find(".more-filters_list-item_active");
        $.each(genres, function (i, item){
            genresName.push(item.dataset.value); 
        });

        let period = $("#year-filter").find(".more-filters_list-item_active").first()[0];
        let annualPeriods = {
            firstYear: period.dataset.valueFirst,
            lastYear: period.dataset.valueLast
        }

        let countriesName = [];
        let countries = $("#country-filter").find(".more-filters_list-item_active");
        $.each(countries, function (i, item){
            countriesName.push(item.dataset.value);
        });

        let ratingObj = $("#rating-filter").find(".more-filters_list-item_active")[0];
        let rating = ratingObj === undefined ? null : ratingObj.dataset.value;
        
        return {
            typeFilter,
            genresName,
            annualPeriods,
            countriesName,
            rating
        }
    }
    
    function getFilmsByFilters(){
        let infoAboutFilters = getObjWithFilters();
        let data = {
            filters: infoAboutFilters,
            pagination,
            numberOfLoadedCards,
        };
        
        $('.loader').toggleClass('hide');
        $.post({
            url: "/Films/FilmFilters",
            data: data,
            success: function (result){
                $('.loader').toggleClass('hide');
                
                numberOfLoadedCards += pagination;
                allLoaded = result.length < 30;
                
                cardContainer.append(result);
                
                $("img").one("load", function() {
                    prepareFilms();
                    adaptСontainer();
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
    
    function loadWithNewFilter(){
        cardContainer.empty();
        numberOfLoadedCards = 0;
        getFilmsByFilters();
    }
    
    $(document).ready(function (){
        tryLoadMoreFilms();
    });

    $(".dropdown-filter__list-item").on("click", loadWithNewFilter);
    $(".more-filters__list-item").on("click", loadWithNewFilter);
    window.addEventListener("scroll", tryLoadMoreFilms);
    window.addEventListener("resize", tryLoadMoreFilms);
})()

