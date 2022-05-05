$("#mod-dialog-support-chat").toggle();
$("#support-chat-icon-close").toggle();
$(function () {
    $(".support-chat-button").click(function () {
        $("#mod-dialog-support-chat").toggle();
        $("#support-chat-icon-close").toggle();
        $("#support-chat-icon-open").toggle();
    });
    let isButtonActive = false;
    $(".chat-container-answer-textarea").keyup(function () {
        if ($(".chat-container-answer-textarea").val() === undefined || $(".chat-container-answer-textarea").val() === "") {
            $(".chat-container-answer-send").css({"color": "rgb(133,133,133)"});
            isButtonActive = false;
            $(".chat-container-answer-send").css({"cursor": "default", "background": "inherit"});
        } else {
            $(".chat-container-answer-send").css({"color": "rgb(187,13,58)"});
            isButtonActive = true;
        }
    });
    $(".chat-container-answer-send").mouseover(function () {
        if (isButtonActive) {
            $(".chat-container-answer-send").css({"cursor": "pointer", "background": "rgb(251,236,241)"});
        }
    });
    $(".chat-container-answer-send").mouseleave(function () {
        if (isButtonActive) {
            $(".chat-container-answer-send").css({"cursor": "default", "background": "inherit"});
        }
    });
})
