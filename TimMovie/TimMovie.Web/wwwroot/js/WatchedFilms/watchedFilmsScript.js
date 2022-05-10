const windowSizeToCutText = 990;
const windowSizeToMedium = 1400;
const windowSizeToSmallFont = 660;
const smallFilmTitleLength = 30;

window.onload = function(){
    applySize();
}

window.addEventListener('resize',function(){
    applySize();
});

function applySize(){
    let width = document.documentElement.clientWidth;
    if(width < windowSizeToCutText){
        CutText();
    }
    resizeToDefault();
    if(width < windowSizeToMedium){
        if(width > windowSizeToSmallFont){

            resizeToMedium();
        }
        else{
            resizeToSmall();
        }
    }
}

function resizeToMedium(){
    document.querySelectorAll('.film-title').forEach(x => x.style='width:35%');
    document.querySelectorAll('.film-duration').forEach(x => x.style='width:20%');
}

function CutText(){
    document.querySelectorAll('.film-title-txt').forEach(d => {
        if(d.innerText.length > smallFilmTitleLength){
            d.innerText = d.innerText.substr(0,smallFilmTitleLength);
            d.innerText += '...';
        }
    })
}

function resizeToSmall(){
    document.querySelector('.films-container').querySelectorAll('span').forEach(x => x.style='font-size:12px !important');
    document.querySelector('.navigation').querySelectorAll('span').forEach(x => x.style = 'font-size:10px !important;');
    document.querySelector('.navigation-film-name').style = 'width:60%';
    document.querySelectorAll('.film-duration').forEach(x => x.style = 'width:150px');
}

function resizeToDefault(){
    document.querySelectorAll('.film-title').forEach(x => x.style='width: 40%;');
    document.querySelector('.films-container').querySelectorAll('span').forEach(x => x.style='font-size:18px !important');
    document.querySelector('.navigation').querySelectorAll('span').forEach(x => x.style = 'font-size:15px !important');
    document.querySelector('.navigation-film-name').style=' width: 48%;';
    document.querySelectorAll('.film-duration').forEach(x => x.style='width: 15%;');
}

