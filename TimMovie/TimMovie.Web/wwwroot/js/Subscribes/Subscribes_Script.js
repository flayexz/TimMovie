$(function () {
        let element = $('#input-search-subscribe');
        let subBody = $('.subscribeCard-container');
        let allLoaded = false;
        let amountSkip = 0;
        const amountTake = 10;

        $.post({
            url: "/Search/SearchSubscribeResults",
            data: {take: amountTake},
            success: function (data) {
                subBody.empty();
                subBody.append(data);
            }
        })
        window.addEventListener("scroll", tryLoadMoreSubscribes);
        window.addEventListener("resize", tryLoadMoreSubscribes);
        element.keyup(function () {
            amountSkip = 0;
            $.post({
                url: "/Search/SearchSubscribeResults",
                data: {namePart: element.val(), take: amountTake, skip: amountSkip},
                success: function (data) {
                    subBody.empty();
                    subBody.append(data);
                }
            })
        });

        function tryLoadMoreSubscribes() {
            if (allLoaded) {
                return;
            }

            const height = document.body.offsetHeight;
            const screenHeight = window.innerHeight;
            const scrolled = window.scrollY;
            const threshold = height - screenHeight / 5;
            const position = scrolled + screenHeight;
            if (position >= threshold) {
                getSubscribes();
            }
        }

        function getSubscribes() {
            amountSkip += amountTake;
            $.post({
                url: "/Search/SearchSubscribeResults",
                data: {namePart: element.val(), take: amountTake, skip: amountSkip},
                success: function (result) {
                    allLoaded = result.length < amountTake;
                    subBody.append(result);
                }
            })
        }

    }
)