let skip = 0;
const take = 5;
const errorColor = "#B22222";
const successColor = "#157347";

window.onload = function () {
    skip = 0;
}

$(document).on("keypress", e =>{
    let element = $(".textarea-comment")
    if (e.key === "Enter" && !e.shiftKey && element.is(":focus")){
        event.preventDefault();
        if (element[0].value.length < 2) {
            changeButtonColorAndText("Слишком короткое сообщение", errorColor);
            return;
        }
        if (element[0].value.length > 1000) {
            changeButtonColorAndText("Cлишком длинное сообщение", errorColor);
            return;
        }
        $.post({
            url: "/Film/LeaveComment",
            data: {filmId: document.URL.split('/').pop(), content: element[0].value},
            success: function (data) {
                element[0].value = "";
                $(".comments-container-body-comments").prepend(data);
                changeButtonColorAndText("Комментарий добавлен", successColor);
            }
        });
    }
})
$(".leave-comment-container").on('click', '.button-comment-send', () => {
    let element = $(".textarea-comment")[0];
    if (element.value.length < 2) {
        changeButtonColorAndText("Слишком короткое сообщение", errorColor);
        return;
    }
    if (element.value.length > 1000) {
        changeButtonColorAndText("Cлишком длинное сообщение", errorColor);
        return;
    }
    $.post({
        url: "/Film/LeaveComment",
        data: {filmId: document.URL.split('/').pop(), content: element.value},
        success: function (data) {
            element.value = "";
            $(".comments-container-body-comments").prepend(data);
            changeButtonColorAndText("Комментарий добавлен", successColor);
        }
    });
});

$(".comments-container-body").on('click', '.button-show-more', () => {
    skip += 5;
    $.post({
        url: "/Film/GetCommentsWithPaginationView",
        data: {
            filmId: document.URL.split('/').pop(),
            take: take,
            skip: skip
        },
        success: function (data) {
            if (data.trim() === '')
                $(".button-show-more")[0].style.display = "none";
            else
                $(".comments-container-body-comments").append(data);
        }
    });
});

function changeButtonColorAndText(text, color){
    let element = $(".send-comment-status");
    element.css({"color": `${color}`});
    element[0].innerText = text;
}

