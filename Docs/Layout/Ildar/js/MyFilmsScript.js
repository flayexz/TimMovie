const windowSizeToCutText = 990;
const windowSizeToSmallFont = 680;
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
        if(width > windowSizeToSmallFont){
            resizeToMedium();
        }
        else{
            resizeToSmall();
        }
    }
    else{
        resizeToDefault();
    }
}


function resizeToMedium(){
    resizeToDefault();
    document.querySelectorAll('.film-grade').forEach(x => x.style='margin-left:50px !important');
    document.querySelectorAll('.film-title').forEach(x => x.style='width:20%');
    document.querySelector('.navigation-film-name').style = 'width:33%';
}


function CutText(){
    this.document.querySelectorAll('.film-title-txt').forEach(d => {
        if(d.innerText.length > smallFilmTitleLength){
            d.innerText = d.innerText.substr(0,smallFilmTitleLength);
            d.innerText += '...';
        }
    })
}

function resizeToSmall(){
        document.querySelector('.films-container').querySelectorAll('span').forEach(x => x.style='font-size:12px !important');
        document.querySelector('.navigation').querySelectorAll('span').forEach(x => x.style = 'font-size:10px !important; max-width=40px !important');
        document.querySelectorAll('.film-title').forEach(x => x.style='width:20%');
        document.querySelector('.navigation-film-name').style = 'width:33%';
        document.querySelectorAll('.film-grade').forEach(x => x.style='margin-left:10px !important');
}

function resizeToDefault(){
    document.querySelectorAll('.film-grade').forEach(x => x.style='margin-left:15px !important');
    document.querySelector('.films-container').querySelectorAll('span').forEach(x => x.style='font-size:18px !important');
    document.querySelector('.navigation').querySelectorAll('span').forEach(x => x.style = 'font-size:15px !important');
    document.querySelectorAll('.film-title').forEach(x => x.style='width:40%');
    document.querySelector('.navigation-film-name').style='width:50%';
}

