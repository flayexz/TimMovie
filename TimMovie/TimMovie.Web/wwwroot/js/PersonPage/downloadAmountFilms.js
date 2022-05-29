(function (){
    const userId = window.location.pathname.split("/")[2];
    const amountFilmsContainer = $(".person_amount-films");
    
    function downloadAmountFilms(){
        $.get({
            url: `/film/person/${userId}`,
            success: function (amount){
                let filmWord = inclineWord(amount, ["фильм", "фильма", "фильмов"]);
                amountFilmsContainer.text(`${amount} ${filmWord}`);
            }
        });
    }
    
    downloadAmountFilms();
})()