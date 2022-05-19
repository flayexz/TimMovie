updateStatus();
setInterval(() => updateStatus(), 60000);
function updateStatus() {
    $.get({
        url: "/UserProfile/UpdateUserStatusWatchingFilm",
        success: function () {
            console.log("Заебумба");
        },
        error: function (){
            console.log("не заебумба :(");
        }
    })
}