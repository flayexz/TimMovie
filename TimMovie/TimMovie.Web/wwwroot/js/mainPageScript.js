$(document).ready(function () {
    $.get({
        url:"/MainPage/GetRecommendationResult",
        success: function (result) {
            console.log(result)
            if (result.length > 30) {
                $('#mainCarousels').prepend(`<h3 className="text_color pl-3 mt-5">Рекомендованные</h3> ${result}`)
                $("img").one("load", function() {
                    prepareFilms();
                    adaptContainer();
                });
            }
        }
    })
})

