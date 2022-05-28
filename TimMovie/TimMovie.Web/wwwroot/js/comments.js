$(function () {
    let skip = 0;
    const take = 5;
    let allLoaded = false;
    let isLoading = false;
    const errorColor = "#B22222";
    const successColor = "#157347";
    const url = "/Film/GetCommentsWithPaginationView";
    window.addEventListener("scroll", tryLoadMoreComments);
    window.addEventListener("resize", tryLoadMoreComments);

    if ($(".comments-container-body-comments")[0].innerHTML.trim() === "") {
        $(".comments-container-body-comments").append("<div class=\"comments-stub h5\">Здесь пока ничего нет</div>")
    }


    window.onload = function () {
        skip = 0;
    }

    $(document).on("keypress", e => {
        let element = $(".textarea-comment")
        if (e.key === "Enter" && !e.shiftKey && element.is(":focus")) {
            event.preventDefault();
            if (element[0].value.length < 2) {
                changeButtonColorAndText("Слишком короткое сообщение", errorColor);
                return;
            }
            if (element[0].value.length > 1000) {
                changeButtonColorAndText("Cлишком длинное сообщение", errorColor);
                return;
            }
            let value = element[0].value
            element[0].value = "";
            $.post({
                url: "/Film/LeaveComment",
                data: {filmId: document.URL.split('/').pop(), content: value},
                success: function (data) {
                    $(".comments-container-body-comments").prepend(data);
                    changeButtonColorAndText("Комментарий добавлен", successColor);
                    if ($(".comments-stub")[0] !== undefined)
                        $(".comments-stub")[0].innerHTML = "";
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
        let value = element[0].value
        element[0].value = "";
        $.post({
            url: "/Film/LeaveComment",
            data: {filmId: document.URL.split('/').pop(), content: value},
            success: function (data) {
                $(".comments-container-body-comments").prepend(data);
                changeButtonColorAndText("Комментарий добавлен", successColor);
                if ($(".comments-stub")[0] !== undefined)
                    $(".comments-stub")[0].innerHTML = "";
            }
        });
    });

    function tryLoadMoreComments() {
        if (allLoaded || isLoading) {
            return;
        }
        const height = document.body.offsetHeight;
        const screenHeight = window.innerHeight;
        const scrolled = window.scrollY;
        const threshold = height - screenHeight / 4;
        const position = scrolled + screenHeight;
        if (position >= threshold) {
            isLoading = true;
            getComments();
        }
    }

    function getComments() {
        skip += take;
        $.post({
            url: url,
            data: {
                filmId: document.URL.split('/').pop(),
                take: take,
                skip: skip
            },
            success: function (data) {
                if ($(".comments-stub")[0] !== undefined) {
                    allLoaded = true;
                    return;
                }
                allLoaded = data.length < take;
                $(".comments-container-body-comments").append(data);
            }
        }).then(() => isLoading = false);
    }

    function changeButtonColorAndText(text, color) {
        let element = $(".send-comment-status");
        element.css({"color": `${color}`});
        element[0].innerText = text;
    }
});