let isWatchLaterFilm

function isWatched(filmId) {
    return $.get({
        url: "/Film/IsFilmAddedToWatchLater",
        data: {filmId: filmId},
        datatype: Boolean,
        success: function (data) {
            if (data != null) {
                isWatchLaterFilm = data;
            }
        }
    });
}