const interval = setInterval(() => {
    if (!document.URL.includes("Film/")) {
        clearInterval(interval);
        console.log("setInterval cleared");
    }
    else{
        updateStatus();
    }
}, 60000);

function updateStatus() {
    $.get({
        url: `/UserProfile/UpdateUserStatusWatchingFilm?filmId=${document.URL.split("/").pop()}`,
        success: function () {
            console.log("Заебумба");
        },
        error: function () {
            console.log("не заебумба :(");
        }
    })
}