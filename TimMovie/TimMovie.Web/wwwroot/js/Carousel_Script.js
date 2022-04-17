const prev = $('.prev');
const next = $('.next');
//const track = $('.track');


// var carousel
// var track 

let index = 0;



function getCardsAmount() {
    let windowWidth = window.innerWidth;
    let cardsOnSlide;
    switch (true) {
        case (windowWidth > 1400):
            cardsOnSlide = 8
            break;
        case (windowWidth > 1200):
            cardsOnSlide = 7
            break;
        case (windowWidth > 1000):
            cardsOnSlide = 6
            break;
        case (windowWidth > 800):
            cardsOnSlide = 5
            break;
        case (windowWidth > 600):
            cardsOnSlide = 4
            break;
        case (windowWidth > 449):
            cardsOnSlide = 3
            break;
        case (windowWidth > 300):
            cardsOnSlide = 2
            break;
        default:cardsOnSlide = 1
    }
    return cardsOnSlide;
}

let cardsOnSlideAmount = getCardsAmount();
let columnWidth = $('.card-container').outerWidth();

function getTransformValue() {
        const style = window.getComputedStyle(track[0])
        const matrix = style.transform
        const matrixValues = matrix.match(/matrix.*\((.+)\)/)[1].split(', ')
        return parseFloat(matrixValues[4]);
    }

function setMoveCoefficients() {
    shiftCards = cardsOnSlideAmount - 1;
    m = parseInt(16 / shiftCards) - 1;
    ost = 16 - m * shiftCards;

    if (ost < cardsOnSlideAmount) {
        shifts = m;
        ost = ost + shiftCards - cardsOnSlideAmount;
    }
    else if (ost === cardsOnSlideAmount) {
        shifts = m + 1;
        ost -= cardsOnSlideAmount
    }
    else {
        shifts = m + 1;
        ost = ost - shiftCards - 1
    }
    isNeedToHide = ost === 0;
}

//amount of full shifts
let shifts;
//How many cards will be moved
let shiftCards;
let m;
let ost;
let isNeedToHide;

setMoveCoefficients();


next.on('click', function (e) {
    carousel = $(e.currentTarget).parent().parent();
    track = carousel.find('#track')
    index++;
    console.log(index)
    carousel.find('#prev').css("display", "flex")
    if (cardsOnSlideAmount === 1)
    {
        track.css("transform", `translateX(-${index * columnWidth * 1}px)`);
        if(index === 15) next.css("display", "none");
    }
    
    else if (index < shifts)
    {
        track.css("transform", `translateX(-${index * columnWidth * shiftCards}px)`);
        if(shifts - 1 === index && isNeedToHide) next.css("display", "none");
    }
    else {
        track.css("transform", `translateX(-${-getTransformValue() + ost * columnWidth}px)`)
        carousel.find('.next').css("display", "none");
    }
});

let flag = ost === 0;

prev.on('click', function (e) {
carousel = $(e.currentTarget).parent().parent();
track = carousel.find('.track')
index--;
next.css("display", "flex");
if (index === 0) {
carousel.find('.prev').css("display", "none")
    }
    if (cardsOnSlideAmount === 1)
        track.css("transform", `translateX(-${index * columnWidth}px)`);
    else if (!flag) 
        track.css("transform", `translateX(-${index * columnWidth * shiftCards}px)`);
    else {
        track.css("transform", `translateX(-${-getTransformValue() - ost * columnWidth}px)`);
        flag = true;
    }
});

window.addEventListener('resize', () => {
    columnWidth = $('.card-container').outerWidth();
    cardsOnSlideAmount = getCardsAmount();
    setMoveCoefficients();
})