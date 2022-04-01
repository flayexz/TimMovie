(function () {
    let navItems = $(".nav-item");
    let currentScreenSize = "xl";

    function changeNavbarButtons() {
            $.each(navItems, function (index, navItem) {
            $.each(getResizableClassName(navItem), function (index, className) {
                let width = window.innerWidth;
                currentScreenSize =
                    width <= 540 ?
                        "sm" : 
                        width <= 900 ?
                            "md" :
                            width <= 1250 ?
                                "lg" :
                                "xl";
                // if (width <= 540) {
                //     currentScreenSize = "sm";
                // } else if (width <= 900) {
                //     currentScreenSize = "md";
                // } else if (width <= 1140) {
                //     currentScreenSize = "lg";
                // } else if (width <= 1920) {
                //     currentScreenSize = "xl";
                // }
                let newClassName = getChangedClassName(className, currentScreenSize);
                navItem.classList.replace(className, newClassName);
            });
        });
        // navItems.forEach(el => {
        //     alert(el);
        //     getResizableClassName(el).forEach(className => {
        //         el.removeClass(className);
        //         let width = window.innerWidth;
        //         if (width <= 540) {
        //             currentScreenSize = "sm";
        //         } else if (width <= 900) {
        //             currentScreenSize = "md";
        //         } else if (width <= 1140) {
        //             currentScreenSize = "lg";
        //         } else if (width <= 1920) {
        //             currentScreenSize = "xl";
        //         }
        //         let newClassName = getChangedClassName(className, currentScreenSize);
        //         el.addClass(newClassName);
        //     })
        // })
    }

    /*Получает все классы элемента, которые нужны для адаптивности*/
    function getResizableClassName(el) {
        return el.className.split(/\s+/).filter(item => item.includes("-xl") ||
            item.includes("-lg") ||
            item.includes("-md") ||
            item.includes("-sm"));
    }

    /*Меняет класс на нужный, который соответствует текущему размеру экрана*/
    function getChangedClassName(origName, newScreenSize) {
        let origScreenSize = origName.split('-').pop();
        return origName.replace(origScreenSize, newScreenSize);
    }

    $(document).ready(function () {
        changeNavbarButtons();
        $(window).resize(changeNavbarButtons);
    });
})();